using System;
using System.Globalization;
using System.Threading;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Touch.Services;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace MWC.iOS {
	[Register ("AppDelegate")]
	public partial class AppDelegate 
	: MvxApplicationDelegate 
        , IMvxServiceConsumer<IMvxStartNavigation>
	{
		public const string ImageNotFavorite = "Images/star-grey45.png";
		public const string ImageIsFavorite  = "Images/star-gold45.png";
		public const string ImageCalendarPad = "Images/calendar~ipad.png";
		public const string ImageCalendarPhone = "Images/calendar~iphone.png";
		public const string ImageLocation    = "Images/building.png";

		public const string ImageEmptyExhibitors = "Images/Empty/exhibitors.png";
		public const string ImageEmptyNews     = "Images/Empty/news.png";
		public const string ImageEmptySession  = "Images/Empty/session.png";
		public const string ImageEmptySpeaker  = "Images/Empty/speaker.png";
		public const string ImageEmptyTwitter  = "Images/Empty/twitter.png";
		
		public const float Font16pt = 22f;
		public const float Font10_5pt = 14f;
		public const float Font10pt = 13f;
		public const float Font9pt = 12f;
		public const float Font7_5pt = 10f;

		public static readonly UIColor ColorNavBarTint = UIColor.FromRGB (55, 87 ,118);
		public static readonly UIColor ColorTextHome = UIColor.FromRGB (192, 205, 223);
		public static readonly UIColor ColorHeadingHome = UIColor.FromRGB (150, 210, 254);
		public static readonly UIColor ColorCellBackgroundHome = UIColor.FromRGB (36, 54, 72);
		public static readonly UIColor ColorTextLink = UIColor.FromRGB (9, 9, 238);		

		const string prefsSeedDataKey = "SeedDataLoaded";
		public const string PrefsEarliestUpdate = "EarliestUpdate";
		
		public static readonly NSString NotificationWillChangeStatusBarOrientation = new NSString("UIApplicationWillChangeStatusBarOrientationNotification");
		public static readonly NSString NotificationDidChangeStatusBarOrientation = new NSString("UIApplicationDidChangeStatusBarOrientationNotification");		
		public static readonly NSString NotificationOrientationDidChange = new NSString("UIDeviceOrientationDidChangeNotification");
		public static readonly NSString NotificationFavoriteUpdated = new NSString("NotificationFavoriteUpdated");
		// class-level declarations
		UIWindow window;
		Screens.Common.TabBarController tabBar;
		


		public static bool IsPhone {
			get {
				return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
			}
		}
		public static bool IsPad {
			get {
				return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
			}
		}
		public static bool HasRetina {
			get {
				if (MonoTouch.UIKit.UIScreen.MainScreen.RespondsToSelector(new Selector("scale")))
					return (MonoTouch.UIKit.UIScreen.MainScreen.Scale == 2.0);
				else
					return false;
			}
		}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
		
#warning for now, we just use an iPhone  MvxTouchSingleViewsPresenter
            // initialize app for single screen iPhone display
            var presenter = new MWCPresenter(this, window);
			var setup = new Setup(this, presenter);
            setup.Initialize();

            // start the app
            var start = this.GetService<IMvxStartNavigation>();
            start.Start();			
			
            window.MakeKeyAndVisible();

			return true;
		}
			
		/// <summary>
		/// When we receive a memory warning, clear the MT.D image cache
		/// </summary>
		public override void ReceiveMemoryWarning (UIApplication application)
		{
			Console.WriteLine("==== Received Memory Warning ====");
			MonoTouch.Dialog.Utilities.ImageLoader.Purge();
		}
	}
}