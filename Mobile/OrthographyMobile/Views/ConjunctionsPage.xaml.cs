using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OrthographyMobile.ViewModels;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using OrthographyMobile.Models.enums;
using OrthographyMobile.Models.dict;

namespace OrthographyMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConjunctionsPage : ContentPage
	{
		private readonly ConjunctionsViewModel viewModel;
		private bool isShowingResult;

		public ConjunctionsPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new ConjunctionsViewModel(Dispatcher);

			SetBusy();
			viewModel.OnAnswerSubmit = Check_Clicked;
		}

		private byte orientation = 0;
		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			var vertical = width < height;
			if (vertical && orientation == 1 || !vertical && orientation == 2) return;

			if (vertical)
			{
				orientation = 1;
				Grid.SetRow(frame3, 2);
				Grid.SetRow(frame4, 2);
				Grid.SetColumn(frame2, 2);
				Grid.SetColumn(frame3, 0);
				Grid.SetColumn(frame4, 2);
				Grid.SetColumnSpan(frame1, 2);
				Grid.SetColumnSpan(frame2, 2);
				Grid.SetColumnSpan(frame3, 2);
				Grid.SetColumnSpan(frame4, 2);
			}
			else
			{
				orientation = 2;
				Grid.SetColumnSpan(frame1, 1);
				Grid.SetColumnSpan(frame2, 1);
				Grid.SetColumnSpan(frame3, 1);
				Grid.SetColumnSpan(frame4, 1);
				Grid.SetRow(frame3, 1);
				Grid.SetRow(frame4, 1);
				Grid.SetColumn(frame2, 3);
				Grid.SetColumn(frame3, 1);
				Grid.SetColumn(frame4, 2);
			}
		}

		void Random_Clicked(Object sender, EventArgs e) => SetRandomCheckState(!viewModel.RandomMode);

		void cbRandom_CheckedChanged(Object sender, CheckedChangedEventArgs e) => SetRandomCheckState(e.Value);

		private void SetRandomCheckState(bool state)
		{
			viewModel.RandomMode = state;
			cbModes.IsEnabled = !viewModel.RandomMode;
		}

		void Translate_Clicked(Object sender, EventArgs e)
		{
			var state = lblTranslation.IsVisible;
			lblTranslation.IsVisible = !state;
			lblBtnTranslation.IsVisible = state;
		}

		void ShowAnswer_Clicked(Object sender, EventArgs e)
		{
			if (lblAnswer.IsVisible) return;
			btnReset.IsVisible = false;
			btnCheck.IsVisible = false;
			lblBtnAnswer.IsVisible = false;
			lblAnswer.IsVisible = true;
			Task.Run(() =>
			{
				Task.Delay(ConjunctionsViewModel.ShowAnswerTime).Wait();
				try
				{
					Dispatcher.BeginInvokeOnMainThread(() =>
					{
						lblAnswer.IsVisible = false;
						lblBtnAnswer.IsVisible = true;
						btnReset.IsVisible = true;
						btnCheck.IsVisible = true;
					});
				}
				catch (Exception ex)
				{
					Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
				}
			});
		}

		private void SetBusy(bool on = true) => viewModel.BusyIndicator = on;

		void Reset_Clicked(Object sender, EventArgs e)
		{
			try
			{
				viewModel.GenerateWord();
			}
			catch (Exception ex)
			{
				Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
			}
		}

		void Check_Clicked(Object sender, EventArgs e)
		{
			if (isShowingResult) return;
			var input = inputAnswer?.Text?.Trim()?.ToLower()?.Replace("’", "'") ?? string.Empty;
			var answer = viewModel?.Selected?.Relation?.Result;
			if (string.IsNullOrWhiteSpace(input)) return;
			isShowingResult = true;
			if (string.IsNullOrWhiteSpace(answer)) answer = "-";
			var result = input == answer;
			lblResult.Text = result ? "Correct" : "Wrong";
			lblResult.TextColor = result ? Color.LimeGreen : Color.Red;
			inputAnswer.IsVisible = false;
			if (result) inputAnswer.Text = string.Empty;
			lblResult.IsVisible = true;
			viewModel.BusyIndicator = result;
			Task.Run(() =>
			{
				Task.Delay(ConjunctionsViewModel.ShowResultTime).Wait();
				try
				{
					Dispatcher.BeginInvokeOnMainThread(() =>
					{
						if (result)
							Reset_Clicked(btnCheck, EventArgs.Empty);
						lblResult.IsVisible = false;
						inputAnswer.IsVisible = true;
					});
					isShowingResult = false;
				}
				catch (Exception ex)
				{
					Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
				}
			});
		}

		private void SelectedChanged()
		{
			if (viewModel.IsGenerating || (viewModel.Selected?.IsBusy ?? true))
				return;
			var mode = (cbModes.SelectedItem as Mode).ID;
			var number = (cbNumbers.SelectedItem as Number).ID;
			var person = (cbPersons.SelectedItem as Person).ID;
			var gender = (cbGenders.SelectedItem as Gender).ID;
			SetBusy(true);
			Task.Run(() =>
			{
				try
				{
					Task.Delay(ConjunctionsViewModel.DispatcherAwakeTime).Wait();
					var rule = DataManager.Logic.GetRuleByDetails(mode, number, person, gender).Result;
					if (rule != null)
						UpdatePackage(rule);
					else
						UpdatePackage(mode, number, person);
				}
				catch (Exception ex)
				{
					Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
				}
				finally
				{
					SetBusy(false);
				}
			});
		}

		private void UpdatePackage(int mode, int number, int person)
		{
			var genders = DataManager.Logic.GetAvailableGenders(mode, number, person).Result;
			if (genders.Count != 0)
			{
				var rule = DataManager.Logic.GetRuleByDetails(mode, number, person, genders[0]).Result;
				if (rule != null)
					UpdatePackage(rule);
				else
					UpdatePackage(mode, number);
			}
			else
				UpdatePackage(mode, number);
		}

		private void UpdatePackage(int mode, int number)
		{
			var persons = DataManager.Logic.GetAvailablePersons(mode, number).Result;
			if (persons.Count != 0)
			{
				var genders = DataManager.Logic.GetAvailableGenders(mode, number, persons[0]).Result;
				var rule = DataManager.Logic.GetRuleByDetails(mode, number, persons[0], genders[0]).Result;
				if (rule != null)
					UpdatePackage(rule);
			}
		}

		private void UpdatePackage(Rule rule)
		{
			var package = DataManager.Logic.GetRelationByRuleAndWord(rule.ID, viewModel.Selected.Word.ID).Result;
			Dispatcher.BeginInvokeOnMainThread(() => viewModel.Selected.Package = package);
		}

		private void SelectedModeChanged()
		{
			var mode = (cbModes.SelectedItem as Mode).ID;
			if (mode == viewModel.Selected.PreviousMode) return;
			SelectedChanged();
		}

		private void SelectedNumberChanged()
		{
			var number = (cbNumbers.SelectedItem as Number).ID;
			if (number == viewModel.Selected.PreviousNumber) return;
			SelectedChanged();
		}

		private void SelectedPersonChanged()
		{
			var person = (cbPersons.SelectedItem as Person).ID;
			if (person == viewModel.Selected.PreviousPerson) return;
			SelectedChanged();
		}

		private void SelectedGenderChanged()
		{
			var gender = (cbGenders.SelectedItem as Gender).ID;
			if (gender == viewModel.Selected.PreviousGender) return;
			SelectedChanged();
		}

		void cbModes_SelectedIndexChanged(Object sender, EventArgs e)
		{
			if (Device.RuntimePlatform == Device.Android)
				SelectedModeChanged();
		}

		void cbModes_Unfocused(Object sender, FocusEventArgs e)
		{
			if (Device.RuntimePlatform == Device.iOS)
				SelectedModeChanged();
		}

		void cbNumbers_SelectedIndexChanged(Object sender, EventArgs e)
		{
			if (Device.RuntimePlatform == Device.Android)
				SelectedNumberChanged();
		}

		void cbNumbers_Unfocused(Object sender, FocusEventArgs e)
		{
			if (Device.RuntimePlatform == Device.iOS)
				SelectedNumberChanged();
		}

		void cbPersons_SelectedIndexChanged(Object sender, EventArgs e)
		{
			if (Device.RuntimePlatform == Device.Android)
				SelectedPersonChanged();
		}

		void cbPersons_Unfocused(Object sender, FocusEventArgs e)
		{
			if (Device.RuntimePlatform == Device.iOS)
				SelectedPersonChanged();
		}

		void cbGenders_SelectedIndexChanged(Object sender, EventArgs e)
		{
			if (Device.RuntimePlatform == Device.Android)
				SelectedGenderChanged();
		}

		void cbGenders_Unfocused(Object sender, FocusEventArgs e)
		{
			if (Device.RuntimePlatform == Device.iOS)
				SelectedGenderChanged();
		}
	}
}