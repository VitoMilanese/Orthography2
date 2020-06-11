using Newtonsoft.Json;

namespace OrthographyMobile.Models.lang
{
	public class Label : BaseModel
	{
		[JsonIgnore]
		public static new string Route => "lang/labels";
		public int LanguageID { get; set; }
		public int TermID { get; set; }
		public string Value { get; set; }
	}
}
