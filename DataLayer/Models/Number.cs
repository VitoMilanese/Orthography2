using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
	[Table("Number", Schema = "enums")]
	public sealed class Number : IdValue
	{
	}
}
