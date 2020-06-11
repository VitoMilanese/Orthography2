using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OrthographyMobile.Models;
using OrthographyMobile.Services;
using OrthographyMobile.Models.enums;
using System.Collections.ObjectModel;

namespace OrthographyMobile.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public LogicDataStore Logic => DataManager.Logic;
		public DataCollection Data => DataManager.Data;
		public ObservableCollection<Mode> Modes { get; private set; }
		public ObservableCollection<Number> Numbers { get; private set; }
		public ObservableCollection<Person> Persons { get; private set; }
		public ObservableCollection<Gender> Genders { get; private set; }

		bool isBusy = false;
		public bool IsBusy
		{
			get { return isBusy; }
			set { SetProperty(ref isBusy, value); }
		}

		string title = string.Empty;
		public string Title
		{
			get { return title; }
			set { SetProperty(ref title, value); }
		}

		public BaseViewModel()
		{
			Modes = Data.Modes;
			Numbers = Data.Numbers;
			Persons = Data.Persons;
			Genders = Data.Genders;
		}

		protected bool SetProperty<T>(ref T backingStore, T value,
			[CallerMemberName] string propertyName = "",
			Action onChanged = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return false;

			backingStore = value;
			onChanged?.Invoke();
			OnPropertyChanged(propertyName);
			return true;
		}

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			var changed = PropertyChanged;
			if (changed == null)
				return;

			changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
