using Newtonsoft.Json;

namespace OrthographyMobile.Models
{
	public abstract class BaseModel
	{
		[JsonIgnore]
		public string Route => "basemodel";
		public int ID { get; set; }
	}
}
