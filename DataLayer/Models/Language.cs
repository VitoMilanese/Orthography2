using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Models
{
	[Table("Language", Schema = "enums")]
	public sealed class Language
	{
		[Key]
		public int ID { get; set; }
		public int LanguageNameID { get; set; }
		[NotMapped]
		public string Label { get; set; }

		public Language()
		{
		}

		public Language(int id, int languageNameID, string label = null)
		{
			ID = id;
			LanguageNameID = languageNameID;
			Label = label;
		}
	}
}
