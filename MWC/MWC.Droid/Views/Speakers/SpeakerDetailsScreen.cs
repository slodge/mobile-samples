using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MWC.Android.Screens;
using MWC.BL;
using System.Net;
using System.IO;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.Speakers {
    [Activity(Label = "Speaker", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SpeakerDetailsView : BaseView<SpeakerDetailsViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_SpeakerDetails);
        }
    }
}