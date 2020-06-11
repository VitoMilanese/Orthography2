using Newtonsoft.Json;

namespace OrthographyMobile.Models.enums
{
	public sealed class Language : IdValue
	{
		[JsonIgnore]
		public static new string Route => "enums/languages";
		[JsonIgnore]
		private new int LabelID
		{
			get => base.LabelID;
			set => base.LabelID = value;
		}
		public int LanguageNameID
		{
			get => LabelID;
			set => LabelID = value;
		}

		public Language()
		{
		}

		public Language(int id, int languageNameID, string label = null)
		{
			ID = id;
			LanguageNameID = languageNameID;
			Label = label;
		}
	}
}
