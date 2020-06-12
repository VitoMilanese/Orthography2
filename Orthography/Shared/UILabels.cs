using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orthography.Shared
{
	public static class UILabels
	{
		private const int Default_Language = 1;
		private const int Default_ID_Language_Name = 2;
		private const int Default_ID_Unknown_Label = 3;

		public static string UnknownLabel { get; private set; }
		public static Language CurrentLanguage { get; private set; }
		public static Dictionary<string, string> Labels { get; private set; }
		public static Dictionary<int, string> ModeLabels { get; private set; }
		public static Dictionary<int, string> NumberLabels { get; private set; }
		public static Dictionary<int, string> PersonLabels { get; private set; }
		public static Dictionary<int, string> GenderLabels { get; private set; }

		private static int id_languageName;
		private static int id_unknownLabel;

		public static EventHandler OnLanguageChanged { get; set; }

		static UILabels()
		{
			Labels = new Dictionary<string, string>();
			ModeLabels = new Dictionary<int, string>();
			NumberLabels = new Dictionary<int, string>();
			PersonLabels = new Dictionary<int, string>();
			GenderLabels = new Dictionary<int, string>();

			using (var db = new Db())
			{
				id_languageName = db.Labels.FirstOrDefault(p => p.Value == "ui_language_name")?.ID ?? Default_ID_Language_Name;
				id_unknownLabel = db.Labels.FirstOrDefault(p => p.Value == "unknown_label")?.ID ?? Default_ID_Unknown_Label;

				if (db.Languages.Count() > 0)
					SelectLanguage(db.Languages.FirstOrDefault(p => p.ID == Default_Language) ?? db.Languages.FirstOrDefault());
			}
		}

		public static bool SelectLanguage(int id)
		{
			using (var db = new Db())
			{
				var language = db.Languages.FirstOrDefault(p => p.ID == id);
				if (language == null) return false;
				return SelectLanguage(language);
			}
		}

		public static bool SelectLanguage(Language language)
		{
			using (var db = new Db())
				try
				{
				var languageName = db.Labels.FirstOrDefault(p => p.LanguageID == language.ID && p.TermID == id_languageName);
				CurrentLanguage = new Language
				{
					ID = language.ID,
					LanguageNameID = languageName?.ID ?? Default_ID_Language_Name,
					Label = languageName.Value ?? UnknownLabel
				};
				UnknownLabel = db.Labels.FirstOrDefault(p => p.LanguageID == language.ID && p.TermID == id_unknownLabel)?.Value ?? "unknown_label";
				Labels.Clear();
				var terms = db.Terms.ToDictionary(p => p.ID, p => p.Value);
				foreach (var label in db.Labels.Where(p => p.LanguageID == language.ID))
				{
					var term = terms.FirstOrDefault(p => p.Key == label.TermID).Value;
					if (!Labels.ContainsKey(term))
						Labels.Add(term, label.Value);
				}
				ModeLabels.Clear();
				foreach (var mode in db.Modes)
					if (!ModeLabels.ContainsKey(mode.ID))
						ModeLabels.Add(mode.ID, Labels[terms[mode.LabelID]]);
				NumberLabels.Clear();
				foreach (var number in db.Numbers)
					if (!NumberLabels.ContainsKey(number.ID))
						NumberLabels.Add(number.ID, Labels[terms[number.LabelID]]);
				PersonLabels.Clear();
				foreach (var person in db.Persons)
					if (!PersonLabels.ContainsKey(person.ID))
						PersonLabels.Add(person.ID, Labels[terms[person.LabelID]]);
				GenderLabels.Clear();
				foreach (var gender in db.Genders)
					if (!GenderLabels.ContainsKey(gender.ID))
						GenderLabels.Add(gender.ID, Labels[terms[gender.LabelID]]);
			}
			catch (Exception ex)
			{
				return false;
			}
			OnLanguageChanged?.Invoke(null, EventArgs.Empty);
			return true;
		}
	}
}
