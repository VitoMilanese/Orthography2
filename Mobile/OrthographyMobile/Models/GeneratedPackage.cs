using OrthographyMobile.Models.dict;
using OrthographyMobile.Models.enums;

namespace OrthographyMobile.Models
{
	public class GeneratedPackage
	{
		public Relation Relation { get; set; }
		public Word Word { get; set; }
		public Rule Rule { get; set; }
		public Number[] AvailableNumbers { get; set; }
		public Person[] AvailablePersons { get; set; }
		public Gender[] AvailableGenders { get; set; }

		public GeneratedPackage()
		{
		}
	}
}
