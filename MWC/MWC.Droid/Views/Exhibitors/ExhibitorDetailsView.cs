using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MWC.Android.Screens;
using MWC.BL;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.Exhibitors {
    [Activity(Label = "Exhibitor", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ExhibitorDetailsView : BaseView<ExhibitorDetailsViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_ExhibitorDetails);
        }
    }
}