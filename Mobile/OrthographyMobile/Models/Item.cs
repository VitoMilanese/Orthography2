using Newtonsoft.Json;

namespace OrthographyMobile.Models
{
	public class Item : BaseModel
	{
		[JsonIgnore]
		public static new string Route => "items";
		public string Text { get; set; }
		public string Description { get; set; }
	}
}