using System.Collections.ObjectModel;
using OrthographyMobile.Models.enums;

namespace OrthographyMobile.Models
{
	public class DataCollection
	{
		public ObservableCollection<Mode> Modes { get; private set; } = new ObservableCollection<Mode>();
		public ObservableCollection<Number> Numbers { get; private set; } = new ObservableCollection<Number>();
		public ObservableCollection<Person> Persons { get; private set; } = new ObservableCollection<Person>();
		public ObservableCollection<Gender> Genders { get; private set; } = new ObservableCollection<Gender>();
		public ObservableCollection<Language> Languages { get; private set; } = new ObservableCollection<Language>();
		public ObservableCollection<lang.Term> Terms { get; private set; } = new ObservableCollection<lang.Term>();
		public ObservableCollection<lang.Label> Labels { get; private set; } = new ObservableCollection<lang.Label>();
	}
}
