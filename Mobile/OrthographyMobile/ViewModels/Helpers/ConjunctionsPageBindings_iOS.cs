using Xamarin.Forms;

namespace OrthographyMobile.ViewModels.Helpers
{
	public class ConjunctionsPageBindings_iOS : ConjunctionsPageBindings
	{
		public ConjunctionsPageBindings_iOS()
		{
			LblRandomMargins = new Thickness(35, 0, 0, 7);
			BtnRandomMargins = new Thickness(5, 0, 0, -4);
		}
	}
}
