using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
	[Table("Person", Schema = "enums")]
	public sealed class Person : IdValue
	{
	}
}
