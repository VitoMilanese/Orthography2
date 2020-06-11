using Newtonsoft.Json;

namespace OrthographyMobile.Models.enums
{
	public sealed class Gender : IdValue
	{
		[JsonIgnore]
		public static new string Route => "enums/genders";
	}
}
