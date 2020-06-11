using Newtonsoft.Json;

namespace OrthographyMobile.Models.dict
{
	public class Rule : BaseModel
	{
		[JsonIgnore]
		public static new string Route => "dict/rules";
		public int ModeID { get; set; }
		public int NumberID { get; set; }
		public int PersonID { get; set; }
		public int GenderID { get; set; }
	}
}
