using Foundation;
using UIKit;

namespace OrthographyMobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();

			//if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
			//{
			//	var statusBar = UIDevice.CurrentDevice.CheckSystemVersion(13, 0)
			//		? new UIView(UIApplication.SharedApplication.StatusBarFrame)
			//		: UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
			//	if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
			//	{
			//		statusBar.BackgroundColor = UIColor.Red;
			//		statusBar.TintColor = UIColor.Red;

			//	}
			//}

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
        }
    }
}
