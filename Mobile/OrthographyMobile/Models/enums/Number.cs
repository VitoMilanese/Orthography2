using Newtonsoft.Json;

namespace OrthographyMobile.Models.enums
{
	public sealed class Number : IdValue
	{
		[JsonIgnore]
		public static new string Route => "enums/numbers";
	}
}
