using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using OrthographyMobile.ViewModels.Helpers;

namespace OrthographyMobile.ViewModels
{
	public class ConjunctionsViewModel : BaseViewModel
	{
		public const int ShowAnswerTime = 1500;
		public const int ShowResultTime = 750;
		public const int DispatcherAwakeTime = 200;

		private IDispatcher Dispatcher { get; }

		public ConjunctionsPageBindings UI { get; private set; }

		public bool IsGenerating { get; private set; }

		private GeneratedPackageToken selected;
		public GeneratedPackageToken Selected
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

		private bool m_randomMode;
		public bool RandomMode
		{
			get => m_randomMode;
			set => SetProperty(ref m_randomMode, value);
		}

		public EventHandler OnAnswerSubmit { get; set; }

		public Command AnswerSubmitCommand { get; set; }

		public ConjunctionsViewModel(IDispatcher dispatcher)
		{
			Dispatcher = dispatcher;
			Selected = new GeneratedPackageToken();
			if (Device.RuntimePlatform == Device.iOS)
				UI = new ConjunctionsPageBindings_iOS();
			else
				UI = new ConjunctionsPageBindings_Droid();
			UI.UpdateProperties();
			Title = "Browse";
			AnswerSubmitCommand = new Command(() => OnAnswerSubmit?.Invoke(this, EventArgs.Empty));
			GenerateWord(true);
		}

		public void GenerateWord() => GenerateWord(false);

		private void GenerateWord(bool ignoreBusy)
		{
			if (IsGenerating) return;
			IsGenerating = true;
			if (!ignoreBusy)
			{
				if (IsBusy) return;
				else IsBusy = true;
			}
			Task.Run(() =>
			{
				try
				{
					BusyIndicator = true;
					Task.Delay(DispatcherAwakeTime).Wait();

					var exclId = Selected.Relation != null ? Selected.Relation.ID : int.MinValue;
					var exclMode = !RandomMode && Selected.Mode != null ? Selected.Mode.ID : int.MinValue;

					var random = Logic.GetRandomRelationDetailed(exclId, exclMode).Result;

					Dispatcher.BeginInvokeOnMainThread(() => Selected.Package = random);
					Task.Delay(DispatcherAwakeTime).Wait();
					while (Selected.IsBusy) ;
				}
				catch (Exception ex)
				{
					Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
				}
				finally
				{
					BusyIndicator = false;
					if (!ignoreBusy) IsBusy = false;
					IsGenerating = false;
				}
			});
		}
	}
}