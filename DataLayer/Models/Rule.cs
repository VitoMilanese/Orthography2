using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
	[Table("Rule", Schema = "dict")]
	public class Rule
	{
		[Key]
		public int ID { get; set; }
		public int ModeID { get; set; }
		public int NumberID { get; set; }
		public int PersonID { get; set; }
		public int GenderID { get; set; }
	}
}
