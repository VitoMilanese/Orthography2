using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
	public class IdValue
	{
		[Key]
		public int ID { get; set; }
		public int LabelID { get; set; }
		[NotMapped]
		public bool Disabled { get; set; }

		public IdValue()
		{
		}

		public IdValue(int id, int labelID)
		{
			ID = id;
			LabelID = labelID;
		}
	}
}
