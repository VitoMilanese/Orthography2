namespace DataLayer.Models
{
	public class IdValue
	{
		public int ID { get; set; }
		public string Value { get; set; }

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
