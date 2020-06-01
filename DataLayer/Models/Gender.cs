using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
	[Table("Gender", Schema = "enums")]
	public sealed class Gender : IdValue
	{
	}
}
