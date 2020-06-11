using Newtonsoft.Json;

namespace OrthographyMobile.Models.dict
{
	public class Word : BaseModel
	{
		[JsonIgnore]
		public static new string Route => "dict/words";
		public string Value { get; set; }
		public decimal PrepositionsMask { get; set; }
		public string Translation { get; set; }
	}
}
