using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
	[Table("String", Schema = "lang")]
	public class Label
	{
		[Key]
		public int ID { get; set; }
		public int LanguageID { get; set; }
		public int TermID { get; set; }
		public string Value { get; set; }
	}
}
