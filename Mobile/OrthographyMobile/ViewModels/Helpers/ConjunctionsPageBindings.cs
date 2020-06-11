using Xamarin.Forms;

namespace OrthographyMobile.ViewModels.Helpers
{
	public abstract class ConjunctionsPageBindings : BaseViewModel
	{
		public Thickness BtnRandomMargins { get; set; }
		public Thickness LblRandomMargins { get; set; }

		public void UpdateProperties()
		{
			OnPropertyChanged("LblRandomMargins");
			OnPropertyChanged("BtnRandomMargins");
		}
	}
}
