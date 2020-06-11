using Newtonsoft.Json;

namespace OrthographyMobile.Models.dict
{
	public class Relation : BaseModel
	{
		[JsonIgnore]
		public static new string Route => "dict/relations";
		public int WordID { get; set; }
		public int RuleID { get; set; }
		public string Result { get; set; }
	}
}
