using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
	public class IdValue
	{
		[Key]
		public int ID { get; set; }
		public string Value { get; set; }
		[NotMapped]
		public bool Disabled { get; set; }

		public IdValue()
		{
		}

		public IdValue(int id, string value)
		{
			ID = id;
			Value = value;
		}
	}
}
