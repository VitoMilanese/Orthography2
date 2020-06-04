using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
	[Table("Term", Schema = "lang")]
	public class Term
	{
		[Key]
		public int ID { get; set; }
		public string Value { get; set; }
	}
}
