using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Webkit;
using Android.Widget;
using MWC.BL;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.Twitter {
    [Activity(Label = "Tweet", ScreenOrientation = ScreenOrientation.Portrait)]
    public class TweetDetailsView : BaseView<TweetViewModel> 
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_TweetDetails);

            // ugh - LoadData() method has problems when html contains a %
            // http://code.google.com/p/android/issues/detail?id=1733
            // http://code.google.com/p/android/issues/detail?id=4401
            FindViewById<WebView>(Resource.Id.ContentWebView).LoadDataWithBaseURL(null,
                "<html><body>" + ViewModel.Content + "</body></html>", @"text/html", "utf-8", null);
        }       
    }
}