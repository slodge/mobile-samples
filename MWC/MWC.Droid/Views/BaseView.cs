using Android.Content;
using Android.Views;
using Cirrious.MvvmCross.Android.Views;
using Cirrious.MvvmCross.Binding.Android.Views;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using MWC.Android.Screens;
using MWC.Core.Mvvm.ViewModels;
using MWC.Views.News;

namespace  MWC.Views {
    public abstract class BaseView<TViewModel> 
        : MvxBindingActivityView<TViewModel>
        where TViewModel : ViewModelBase
    {        
        /// <summary>
        /// http://mgroves.com/monodroid-creating-an-options-menu/ 
        /// </summary>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            FillMenu(menu);

            return true;
        }

        public static void FillMenu(IMenu menu)
        {
            var item = menu.Add(Menu.None, 1, 1, new Java.Lang.String("Schedule"));
            //item.SetIcon(Resource.Drawable.calendar);

            item = menu.Add(Menu.None, 2, 2, new Java.Lang.String("Speakers"));
                // HACK: todo - add 'using' statement around Java.Lang.Strings for GC (as per novell hint)
            //item.SetIcon(Resource.Drawable.microphone);

            item = menu.Add(Menu.None, 3, 3, new Java.Lang.String("Sessions"));
            //item.SetIcon(Resource.Drawable.bullhorn);

            item = menu.Add(Menu.None, 4, 4, new Java.Lang.String("Map"));
            //item.SetIcon(Resource.Drawable.map);

            item = menu.Add(Menu.None, 5, 5, new Java.Lang.String("Favorites"));
            //item.SetIcon(Resource.Drawable.star);

            item = menu.Add(Menu.None, 6, 6, new Java.Lang.String("News"));
            //item.SetIcon(Resource.Drawable.star);

            item = menu.Add(Menu.None, 7, 7, new Java.Lang.String("Twitter"));
            //item.SetIcon(Resource.Drawable.star);

            item = menu.Add(Menu.None, 8, 8, new Java.Lang.String("Exhibitors"));
            //item.SetIcon(Resource.Drawable.star);

            item = menu.Add(Menu.None, 9, 9, new Java.Lang.String("About Xamarin"));
            //item.SetIcon(Resource.Drawable.star);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (OnOptionsItemSelected(ViewModel, item))
                return true;
            return base.OnOptionsItemSelected(item);
        }

        public static bool OnOptionsItemSelected(ViewModelBase viewModel, IMenuItem item)
        {
            switch (item.TitleFormatted.ToString())
            {
                case "Schedule":
                    viewModel.ShowTopPageCommand.Execute();
                    return true;

                case "Speakers":
                    viewModel.ShowSpeakersCommand.Execute();
                    return true;

                case "Sessions":
                    viewModel.AllSessionsCommand.Execute();
                    return true;

                case "Favorites":
                    viewModel.FavoriteSessionsCommand.Execute();
                    return true;

                case "News":
                    viewModel.ShowNewsCommand.Execute();
                    return true;

                case "Twitter":
                    viewModel.ShowTwitterCommand.Execute();
                    return true;

                case "Exhibitors":
                    viewModel.ShowExhibitorsCommand.Execute();
                    return true;

                case "About Xamarin":
                    viewModel.ShowAboutXamarinCommand.Execute();
                    return true;

                case "Map":
                    viewModel.ShowMapCommand.Execute();
                    return true;
            }
            return false;
        }
    }
}