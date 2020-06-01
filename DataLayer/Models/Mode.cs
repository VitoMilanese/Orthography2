using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
	[Table("Mode", Schema = "enums")]
	public sealed class Mode : IdValue
	{
	}
}
