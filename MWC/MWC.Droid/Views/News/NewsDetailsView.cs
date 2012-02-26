using Android.App;
using Android.OS;
using Android.Webkit;
using Android.Widget;
using MWC.Android.Screens;
using MWC.BL;
using Android.Content.PM;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.News {
    [Activity(Label = "News", ScreenOrientation = ScreenOrientation.Portrait)]
    public class NewsDetailsView : BaseView<NewsItemViewModel> {

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_NewsDetails);

            // ugh - LoadData() method has problems when html contains a % SO USE LoadDataWithBaseURL instead even though we don't have a BaseURL
            // http://code.google.com/p/android/issues/detail?id=1733
            // http://code.google.com/p/android/issues/detail?id=4401
            FindViewById<WebView>(Resource.Id.ContentWebView).LoadDataWithBaseURL(null,
                        "<html><body>" + ViewModel.Content + "</body></html>", @"text/html", "utf-8", null);
        }
    }
}