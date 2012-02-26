using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.Widget;
using MWC.BL;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.Speakers {
    [Activity(Label = "Speakers", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SpeakerListView 
        : BaseView<SpeakerListViewModel>
    {
        protected override void OnViewModelSet()
        {
            MWCApp.LogDebug("SPEAKERS OnViewModelSet");

            SetContentView(Resource.Layout.Page_Speakers);
        }
    }
}