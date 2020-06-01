namespace Orthography.Models
{
	public class Word
	{
		public int ID { get; set; }
		public string Value { get; set; }
		public decimal PrepositionsMask { get; set; }
		public string Translation { get; set; }
	}
}
