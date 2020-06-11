using System;
using System.Diagnostics;
using System.Threading.Tasks;
using OrthographyMobile.Models.dict;

namespace OrthographyMobile.ViewModels
{
	public class PrepositionsViewModel : BaseViewModel
	{
		public const int DispatcherAwakeTime = 200;

		public bool IsGenerating { get; private set; }

		private Word selected;
		public Word Selected
		{
			get => selected;
			private set => SetProperty(ref selected, value);
		}

		private bool busyIndicator = true;
		public bool BusyIndicator
		{
			get => busyIndicator;
			set => SetProperty(ref busyIndicator, value);
		}

		public PrepositionsViewModel()
		{
			if (IsBusy) return;
			Selected = new Word();
			GenerateWord();
		}

		/// <summary>
		/// Must be called before accessing the data and recalled when is needed to update data
		/// </summary>
		/// <returns></returns>
		public void LoadData()
		{
			if (IsBusy) return;
			IsBusy = true;
			try
			{
				GenerateWord();
			}
			catch (Exception ex)
			{
				Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public void GenerateWord()
		{
			if (IsGenerating) return;
			if (IsBusy) return; else IsBusy = true;
			IsGenerating = true;
			Task.Run(() =>
			{
				try
				{
					BusyIndicator = true;
					Task.Delay(DispatcherAwakeTime).Wait();

					var exclId = Selected?.ID ?? 0;
					var word = Logic.GetRandomWordWithPreposition(exclId).Result;

					Selected = word;
				}
				catch (Exception ex)
				{
					Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
				}
				finally
				{
					BusyIndicator = false;
					IsBusy = false;
					IsGenerating = false;
				}
			});
		}
	}
}
