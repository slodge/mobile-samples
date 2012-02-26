using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Android.Views;
using MWC.Android.Screens;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.Home {
    /// <summary>
    /// http://docs.xamarin.com/android/tutorials/User_Interface/tab_layout
    /// </summary>
    /// <remarks>
    /// Icon design guidelines
    /// http://developer.android.com/guide/practices/ui_guidelines/icon_design_tab.html
    /// </remarks>
    [Activity(Label = "Home"
            , Theme = "@android:style/Theme.NoTitleBar"
            , ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainView : MvxBindingTabActivityView<MainViewModel> 
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Main_TabBar);

            TabHost.TabSpec spec;     // Resusable TabSpec for each tab
            Intent intent;            // Reusable Intent for each tab

            // Initialize a TabSpec for each tab and add it to the TabHost
            intent = base.CreateIntentFor<ScheduleViewModel>();

            spec = TabHost.NewTabSpec("home");
            spec.SetIndicator("Schedule", Resources.GetDrawable(Resource.Drawable.tab_schedule));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // ------------
            intent = base.CreateIntentFor<SpeakerListViewModel>();

            spec = TabHost.NewTabSpec("speakers");
            spec.SetIndicator("Speakers", Resources.GetDrawable(Resource.Drawable.tab_speakers));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // ------------
            intent = base.CreateIntentFor<SessionListViewModel>();

            spec = TabHost.NewTabSpec("sessions");
            spec.SetIndicator("Sessions", Resources.GetDrawable(Resource.Drawable.tab_sessions));
            spec.SetContent(intent);
            TabHost.AddTab(spec);


            //  Note that map is not a View - no harm in using normal screens when no data is needed!
            // ------------
            intent = base.CreateIntentFor<MapsViewModel>();
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("map");
            spec.SetIndicator("Map", Resources.GetDrawable(Resource.Drawable.tab_maps));
            spec.SetContent(intent);
            TabHost.AddTab(spec);


            // ------------
            intent = base.CreateIntentFor<MoreViewModel>();
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("more");
            spec.SetIndicator("", Resources.GetDrawable(global::Android.Resource.Drawable.IcMenuMore));   // android.R.drawable.ic_menu_more
            spec.SetContent(intent);
            TabHost.AddTab(spec);
        }
    }
}