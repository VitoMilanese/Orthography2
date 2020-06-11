using Newtonsoft.Json;

namespace OrthographyMobile.Models.enums
{
	public sealed class Person : IdValue
	{
		[JsonIgnore]
		public static new string Route => "enums/persons";
	}
}
