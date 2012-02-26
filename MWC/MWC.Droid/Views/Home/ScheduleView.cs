using Android.App;
using Cirrious.MvvmCross.Binding.Android.Views;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.Home {
    [Activity]
    public class ScheduleView : MvxBindingActivityView<ScheduleViewModel> 
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_Schedule);
        }
    }
}