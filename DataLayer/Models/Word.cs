using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
	[Table("Word", Schema = "dict")]
	public class Word
	{
		[Key]
		public int ID { get; set; }
		public string Value { get; set; }
		public decimal PrepositionsMask { get; set; }
		public string Translation { get; set; }
	}
}
