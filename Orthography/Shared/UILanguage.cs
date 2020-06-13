using System.Collections.Generic;
using System.Linq;

namespace Orthography.Shared
{
	public static class UILanguage
	{
		public const int DefaultLanguage = 1;
		private const int Default_ID_Language_Name = 2;
		private const int Default_ID_Unknown_Label = 3;
		private static int id_languageName;
		private static int id_unknownLabel;

		public static Dictionary<int, string> LanguageNames { get; private set; }
		public static Dictionary<int, string> UnknownLabels { get; private set; }
		public static Dictionary<int, Dictionary<int, string>> ModeLabels { get; private set; }
		public static Dictionary<int, Dictionary<int, string>> NumberLabels { get; private set; }
		public static Dictionary<int, Dictionary<int, string>> PersonLabels { get; private set; }
		public static Dictionary<int, Dictionary<int, string>> GenderLabels { get; private set; }
		public static Dictionary<int, Dictionary<string, string>> Labels { get; private set; }

		static UILanguage()
		{
			LanguageNames = new Dictionary<int, string>();
			UnknownLabels = new Dictionary<int, string>();
			ModeLabels = new Dictionary<int, Dictionary<int, string>>();
			NumberLabels = new Dictionary<int, Dictionary<int, string>>();
			PersonLabels = new Dictionary<int, Dictionary<int, string>>();
			GenderLabels = new Dictionary<int, Dictionary<int, string>>();
			Labels = new Dictionary<int, Dictionary<string, string>>();

			using (var db = new Db())
			{
				foreach(var language in db.Languages.ToList())
				{
					ModeLabels.Add(language.ID, new Dictionary<int, string>());
					NumberLabels.Add(language.ID, new Dictionary<int, string>());
					PersonLabels.Add(language.ID, new Dictionary<int, string>());
					GenderLabels.Add(language.ID, new Dictionary<int, string>());
					Labels.Add(language.ID, new Dictionary<string, string>());

					var terms = db.Terms.ToDictionary(p => p.ID, p => p.Value);
					foreach (var label in db.Labels.Where(p => p.LanguageID == language.ID))
					{
						var term = terms.FirstOrDefault(p => p.Key == label.TermID).Value;
						if (!Labels[language.ID].ContainsKey(term))
							Labels[language.ID].Add(term, label.Value);
					}

					foreach (var mode in db.Modes)
						if (!ModeLabels[language.ID].ContainsKey(mode.ID))
							ModeLabels[language.ID].Add(mode.ID, Labels[language.ID][terms[mode.LabelID]]);
					foreach (var number in db.Numbers)
						if (!NumberLabels[language.ID].ContainsKey(number.ID))
							NumberLabels[language.ID].Add(number.ID, Labels[language.ID][terms[number.LabelID]]);
					foreach (var person in db.Persons)
						if (!PersonLabels[language.ID].ContainsKey(person.ID))
							PersonLabels[language.ID].Add(person.ID, Labels[language.ID][terms[person.LabelID]]);
					foreach (var gender in db.Genders)
						if (!GenderLabels[language.ID].ContainsKey(gender.ID))
							GenderLabels[language.ID].Add(gender.ID, Labels[language.ID][terms[gender.LabelID]]);

					id_languageName = db.Labels.FirstOrDefault(p => p.Value == "ui_language_name")?.ID ?? Default_ID_Language_Name;
					var languageNames = db.Labels.FirstOrDefault(p => p.LanguageID == language.ID && p.TermID == id_languageName)?.Value ?? "unknown_language";
					LanguageNames.Add(language.ID, languageNames);

					id_unknownLabel = db.Labels.FirstOrDefault(p => p.Value == "unknown_label")?.ID ?? Default_ID_Unknown_Label;
					var unknownLabel = db.Labels.FirstOrDefault(p => p.LanguageID == language.ID && p.TermID == id_unknownLabel)?.Value ?? "unknown_label";
					UnknownLabels.Add(language.ID, unknownLabel);
				}
			}
		}

		public static string Language(int id) => LanguageNames[id];
		public static string UnknownLabel(int language = DefaultLanguage) => UnknownLabels[language];
		public static string Mode(int id, int language = DefaultLanguage) => ModeLabels[language][id];
		public static string Number(int id, int language = DefaultLanguage) => NumberLabels[language][id];
		public static string Person(int id, int language = DefaultLanguage) => PersonLabels[language][id];
		public static string Gender(int id, int language = DefaultLanguage) => GenderLabels[language][id];
		public static string Label(string key, int language = DefaultLanguage) => Labels[language][key];
	}
}
