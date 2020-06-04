using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orthography.Shared
{
	public static class UILabels
	{
		private const int Default_Language = 1;
		private const int Default_ID_Language_Name = 35;
		private const int Default_ID_Unknown_Label = 36;

		public static string UnknownLabel { get; private set; }
		public static Language CurrentLanguage { get; private set; }
		public static Dictionary<string, string> Labels { get; private set; }
		public static Dictionary<int, string> ModeLabels { get; private set; }
		public static Dictionary<int, string> NumberLabels { get; private set; }
		public static Dictionary<int, string> PersonLabels { get; private set; }
		public static Dictionary<int, string> GenderLabels { get; private set; }

		private static int id_languageName;
		private static int id_unknownLabel;

		static UILabels()
		{
			Labels = new Dictionary<string, string>();
			ModeLabels = new Dictionary<int, string>();
			NumberLabels = new Dictionary<int, string>();
			PersonLabels = new Dictionary<int, string>();
			GenderLabels = new Dictionary<int, string>();
			
			id_languageName = Db.Context.Labels.FirstOrDefault(p => p.Value == "ui_language_name")?.ID ?? Default_ID_Language_Name;
			id_unknownLabel = Db.Context.Labels.FirstOrDefault(p => p.Value == "unknown_label")?.ID ?? Default_ID_Unknown_Label;

			if (Db.Context.Languages.Count() > 0)
				SelectLanguage(Db.Context.Languages.FirstOrDefault(p => p.ID == Default_Language) ?? Db.Context.Languages.FirstOrDefault());
		}

		public static bool SelectLanguage(int id)
		{
			var language = Db.Context.Languages.FirstOrDefault(p => p.ID == id);
			if (language == null) return false;
			return SelectLanguage(language);
		}

		public static bool SelectLanguage(Language language)
		{
			try
			{
				var languageName = Db.Context.Labels.FirstOrDefault(p => p.LanguageID == language.ID && p.TermID == id_languageName);
				CurrentLanguage = new Language
				{
					ID = language.ID,
					LanguageNameID = languageName?.ID ?? Default_ID_Language_Name,
					Label = languageName.Value ?? UnknownLabel
				};
				UnknownLabel = Db.Context.Labels.FirstOrDefault(p => p.LanguageID == language.ID && p.TermID == id_unknownLabel)?.Value ?? "unknown_label";
				Labels.Clear();
				var terms = Db.Context.Terms.ToDictionary(p => p.ID, p => p.Value);
				foreach (var label in Db.Context.Labels.Where(p => p.LanguageID == language.ID))
				{
					var term = terms.FirstOrDefault(p => p.Key == label.TermID).Value;
					if (!Labels.ContainsKey(term))
						Labels.Add(term, label.Value);
				}
				ModeLabels.Clear();
				foreach (var mode in Db.Context.Modes)
					if (!ModeLabels.ContainsKey(mode.ID))
						ModeLabels.Add(mode.ID, Labels[terms[mode.LabelID]]);
				NumberLabels.Clear();
				foreach (var number in Db.Context.Numbers)
					if (!NumberLabels.ContainsKey(number.ID))
						NumberLabels.Add(number.ID, Labels[terms[number.LabelID]]);
				PersonLabels.Clear();
				foreach (var person in Db.Context.Persons)
					if (!PersonLabels.ContainsKey(person.ID))
						PersonLabels.Add(person.ID, Labels[terms[person.LabelID]]);
				GenderLabels.Clear();
				foreach (var gender in Db.Context.Genders)
					if (!GenderLabels.ContainsKey(gender.ID))
						GenderLabels.Add(gender.ID, Labels[terms[gender.LabelID]]);
			}
			catch (Exception ex)
			{
				return false;
			}
			return true;
		}
	}
}
