using Newtonsoft.Json;

namespace OrthographyMobile.Models.enums
{
	public sealed class Mode : IdValue
	{
		[JsonIgnore]
		public static new string Route => "enums/modes";
	}
}
