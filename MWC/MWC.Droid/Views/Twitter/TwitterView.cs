using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MWC.Android.Screens;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.Twitter {
    [Activity(Label = "Twitter", ScreenOrientation = ScreenOrientation.Portrait)]
    public class TwitterView : BaseView<TwitterViewModel>
    {
        protected override void OnViewModelSet()
        {
            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.Page_Twitter);
        }
    }
}