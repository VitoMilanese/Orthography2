using DataLayer.Models;

namespace OAPI.Web.Models
{
	public class GeneratedPackage
	{
		public Relation Relation { get; set; }
		public Word Word { get; set; }
		public Rule Rule { get; set; }
		public Number[] AvailableNumbers { get; set; }
		public Person[] AvailablePersons { get; set; }
		public Gender[] AvailableGenders { get; set; }
	}
}
