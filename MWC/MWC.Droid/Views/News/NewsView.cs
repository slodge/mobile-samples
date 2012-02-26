using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MWC.Android.Screens;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.News {
    [Activity(Label = "News", ScreenOrientation = ScreenOrientation.Portrait)]
    public class NewsView : BaseView<NewsListViewModel>
    {
        protected override void OnViewModelSet()
        {
            // set our layout to be the home screen
            SetContentView(Resource.Layout.Page_News);
        }
    }
}