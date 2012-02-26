using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;
using MWC.Android.Screens;
using MWC.BL;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.Exhibitors
{
    [Activity(Label = "Exhibitors", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ExhibitorsView : BaseView<ExhibitorsListViewModel>
    {
        protected override void OnViewModelSet()
        {
            Log.Debug("MWC", "EXHIBITORS OnCreate");

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.Page_Exhibitors);
        }
    }
}