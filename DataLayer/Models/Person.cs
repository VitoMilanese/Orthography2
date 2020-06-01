using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models.enums
{
	[Table("Person", Schema = "enums")]
	public sealed class Person : IdValue
	{
	}
}
