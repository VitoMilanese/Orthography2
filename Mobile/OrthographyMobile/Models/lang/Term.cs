using Newtonsoft.Json;

namespace OrthographyMobile.Models.lang
{
	public class Term : BaseModel
	{
		[JsonIgnore]
		public static new string Route => "lang/terms";
		public string Value { get; set; }
	}
}
