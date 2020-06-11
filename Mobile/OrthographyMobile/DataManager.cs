using OrthographyMobile.Models;
using OrthographyMobile.Models.enums;
using lang = OrthographyMobile.Models.lang;
using OrthographyMobile.Services;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace OrthographyMobile
{
	public static class DataManager
	{
		public static IDataStore<lang.Term> TermsDataStore => DependencyService.Get<IDataStore<lang.Term>>();
		public static IDataStore<lang.Label> LabelsDataStore => DependencyService.Get<IDataStore<lang.Label>>();
		public static IDataStore<Language> LanguagesDataStore => DependencyService.Get<IDataStore<Language>>();
		public static IDataStore<Number> NumbersDataStore => DependencyService.Get<IDataStore<Number>>();
		public static IDataStore<Person> PersonsDataStore => DependencyService.Get<IDataStore<Person>>();
		public static IDataStore<Gender> GendersDataStore => DependencyService.Get<IDataStore<Gender>>();
		public static IDataStore<object> LogicDataStore => DependencyService.Get<IDataStore<object>>();

		public static LogicDataStore Logic { get; private set; }
		public static DataCollection Data { get; private set; }

		public static void Init()
		{
			Data = new DataCollection();
			Logic = new LogicDataStore();
			LoadData(TermsDataStore, Data.Terms).Wait();
			LoadData(LabelsDataStore, Data.Labels).Wait();
			LoadDataAndLabels(LanguagesDataStore, Data.Languages).Wait();
			LoadDataAndLabels(NumbersDataStore, Data.Numbers).Wait();
			LoadDataAndLabels(PersonsDataStore, Data.Persons).Wait();
			LoadDataAndLabels(GendersDataStore, Data.Genders).Wait();

			LoadDataAndLabels(Logic.GetWorkingModes().Result, Data.Modes);
		}

		private static async Task LoadData<T>(IDataStore<T> from, ObservableCollection<T> to)
		{
			to.Clear();
			var items = await from.GetItemsAsync(true).ConfigureAwait(false);
			foreach (var item in items) to.Add(item);
		}

		private static async Task LoadDataAndLabels<T>(IDataStore<T> from, ObservableCollection<T> to) where T : IdValue
		{
			var items = await from.GetItemsAsync(true).ConfigureAwait(false);
			LoadDataAndLabels(items, to);
		}

		private static void LoadDataAndLabels<T>(IEnumerable<T> from, ObservableCollection<T> to) where T : IdValue
		{
			to.Clear();
			foreach (var item in from)
			{
				item.Label = Data.Labels.FirstOrDefault(p => p.ID == item.LabelID)?.Value ?? "no label";
				to.Add(item);
			}
		}
	}
}
