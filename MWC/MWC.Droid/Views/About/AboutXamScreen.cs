using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Webkit;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.About
{
    [Activity(Label = "About Xamarin", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AboutXamView : BaseView<AboutXamarinViewModel>
    {
        protected override void OnViewModelSet()
        {
            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.Page_AboutXamarin);

            var web = FindViewById<WebView>(Resource.Id.AboutWebView);
            web.LoadUrl("file:///android_asset/About/index.html");
        }
    }
}