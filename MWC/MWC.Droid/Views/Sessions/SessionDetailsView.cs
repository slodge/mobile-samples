using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using MWC.BL;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.Sessions {
    [Activity(Label = "Session", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SessionDetailsView : BaseView<SessionDetailsViewModel> {

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_SessionDetails);
        }
    }
}