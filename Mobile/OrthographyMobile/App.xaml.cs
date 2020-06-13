using Xamarin.Essentials;
using Xamarin.Forms;
using OrthographyMobile.Services;

namespace OrthographyMobile
{
	public partial class App : Application
	{
		//TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
		//To debug on Android emulators run the web backend against .NET Core not IIS
		//If using other emulators besides stock Google images you may need to adjust the IP address
		public static string AzureBackendUrl =
			DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000"
			//: "http://192.168.1.111:51975";
			//: "http://192.168.1.111:81";
			//: "http://vhanych.com";
			: "http://vitomilanese-001-site1.gtempurl.com";
		public static bool UseMockDataStore = false;

		public App()
		{
			InitializeComponent();

			// [enums]
			DependencyService.Register<NumbersDataStore>();
			DependencyService.Register<PersonsDataStore>();
			DependencyService.Register<GendersDataStore>();
			DependencyService.Register<LanguagesDataStore>();
			// [lang]
			DependencyService.Register<TermsDataStore>();
			DependencyService.Register<LabelsDataStore>();

			MainPage = new AppShell();
		}

		protected override void OnStart()
		{
			DataManager.Init();
			base.OnStart();
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
