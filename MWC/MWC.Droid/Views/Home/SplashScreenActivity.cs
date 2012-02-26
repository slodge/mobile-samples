using Android.App;
using Android.Content.PM;
using Android.OS;
using Cirrious.MvvmCross.Android.Platform;
using Cirrious.MvvmCross.Android.Views;

namespace MWC.Views.Home {

    [Activity(MainLauncher = true
        , Label = "@string/ApplicationName"
        , Theme = "@style/Theme.Splash"
        , Icon = "@drawable/icon"
        , ScreenOrientation = ScreenOrientation.Portrait
        , NoHistory = true)]
    public class SplashScreenActivity
        : MvxBaseSplashScreenActivity
    {
        public SplashScreenActivity()
            : base()
        {
        }

        protected override MvxBaseAndroidSetup CreateSetup()
        {
            return new Setup(ApplicationContext);
        }
    }
}