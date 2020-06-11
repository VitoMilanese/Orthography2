using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using OrthographyMobile.ViewModels;
using Xamarin.Forms;

namespace OrthographyMobile.Views
{
	public partial class PrepositionsPage : ContentPage
	{
		private readonly Color ColorChecked = Color.Yellow;
		private readonly Color ColorUnchecked = Color.White;
		private readonly Color ColorCorrect = Color.Green;
		private readonly Color ColorWrong = Color.Red;

		PrepositionsViewModel viewModel;

		private bool isEditing;
		private Button[] buttons { get; }
		private Dictionary<Button, bool> states { get; }

		public PrepositionsPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new PrepositionsViewModel();
			SetBusy();

			buttons = new[] { btn_0, btn_1, btn_2, btn_3, btn_4, btn_5, btn_6, btn_7, btn_8 };
			states = new Dictionary<Button, bool>();
			foreach (var btn in buttons)
			{
				btn.Clicked += btnState_Clicked;
				states.Add(btn, false);
				btn.TextColor = Color.White;
			}

			isEditing = true;
		}

		//protected override void OnApplyTemplate()
		//{
		//	base.OnApplyTemplate();
		//	var mdPage = Application.Current.MainPage as MasterDetailPage;
		//	var navPage = mdPage.Detail as NavigationPage;
		//	navPage.BarBackgroundColor = Color.Red;
		//	//NavigationPage.BarBackgroundColorProperty
		//}

		void Translate_Clicked(Object sender, EventArgs e)
		{
			var state = lblTranslation.IsVisible;
			lblTranslation.IsVisible = !state;
			lblBtnTranslation.IsVisible = state;
		}

		private void SetBusy(bool on = true) => viewModel.BusyIndicator = on;

		void btnState_Clicked(Object sender, EventArgs e)
		{
			if (!isEditing) return;
			var btn = sender as Button;
			if (btn == null || !states.ContainsKey(btn)) return;
			states[btn] = !states[btn];
			btn.BorderColor = states[btn] ? ColorChecked : ColorUnchecked;
			btn.BorderWidth = states[btn] ? 3 : 1;
		}

		private void Next_Clicked(Object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				try
				{
					Dispatcher.BeginInvokeOnMainThread(() => viewModel.GenerateWord());
					Task.Delay(PrepositionsViewModel.DispatcherAwakeTime).Wait();
					while (viewModel.IsGenerating) ;
				}
				catch (Exception ex)
				{
					Debugger.Log(0, $"Debug_{GetType()}", ex.Message);
				}
				finally
				{
					Dispatcher.BeginInvokeOnMainThread(() =>
					{
						Reset_Clicked(sender, e);
						btnNext.IsVisible = false;
						btnReset.IsVisible = true;
						btnCheck.IsVisible = true;
						isEditing = true;
					});
				}
			});
		}

		private void Reset_Clicked(Object sender, EventArgs e)
		{
			foreach (var btn in buttons)
			{
				states[btn] = false;
				btn.BackgroundColor = Color.Transparent;
				btn.BorderColor = ColorUnchecked;
				btn.BorderWidth = 1;
			}
		}

		private void Check_Clicked(Object sender, EventArgs e)
		{
			isEditing = false;
			btnReset.IsVisible = false;
			btnCheck.IsVisible = false;
			btnNext.IsVisible = true;

			var resultValue = true;
			var bits = BitmaskToArray((int)viewModel.Selected.PrepositionsMask, 9);
			for (var i = 0; i < buttons.Length; ++i)
			{
				var f = states[buttons[i]] == bits[i];
				resultValue &= f;
				if (f)
					buttons[i].BackgroundColor = bits[i] ? ColorCorrect : Color.Transparent;
				else
				{
					buttons[i].BackgroundColor = bits[i] ? Color.FromRgba(0, 1, 0, 0.1) : ColorWrong;
					buttons[i].BorderColor = bits[i] ? ColorCorrect : buttons[i].BorderColor;
					buttons[i].BorderWidth = bits[i] ? 3 : buttons[i].BorderWidth;
				}
			}
		}

		private static int ArrayToBitmask(bool[] array)
		{
			var bitmask = 0;
			for (var i = array.Length - 1; i >= 0; --i)
			{
				bitmask |= array[i] ? 1 : 0;
				if (i > 0) bitmask <<= 1;
			}
			return bitmask;
		}

		private static bool[] BitmaskToArray(int bitmask, int arraySize)
		{
			var res = new bool[arraySize];
			for (var i = 0; i < arraySize; ++i)
			{
				res[i] = bitmask % 2 > 0;
				bitmask >>= 1;
			}
			return res;
		}
	}
}
