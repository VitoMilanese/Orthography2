using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
	[Table("Relation", Schema = "dict")]
	public class Relation
	{
		public int ID { get; set; }
		public int WordID { get; set; }
		public int RuleID { get; set; }
		public string Result { get; set; }
	}
}
