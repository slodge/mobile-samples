using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MWC.Android.Screens;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.Home
{
    [Activity(Label = "More", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MoreView : BaseView<MoreViewModel>
    {
        protected override void OnViewModelSet()
        {
            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.Page_More);
        }
    }
}
