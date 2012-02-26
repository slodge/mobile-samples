using System.Windows;
using System.Windows.Media;
using Cirrious.MvvmCross.WindowsPhone.Views;
using MWC.Core.Mvvm.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace MWC.WP7.Views
{
    public class BaseTweetDetailsView : MvxPhonePage<TweetViewModel>
    {        
    }

    public partial class TweetDetailsView : BaseTweetDetailsView
    {
        public TweetDetailsView ()
        {
            InitializeComponent ();
        }

        protected override void OnNavigatedTo (System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo (e);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New) 
            {
                if (!string.IsNullOrEmpty (ViewModel.Content)) {

                    //
                    // Adapt to the theme
                    //
                    var bgColor = "black";
                    var color = "white";
                    if ((Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == Visibility.Visible) {
                        bgColor = "white";
                        color = "black";
                    }

                    var accentColor = (Color)Application.Current.Resources["PhoneAccentColor"];
                    var linkColor = "#" + accentColor.ToString ().Substring(3);
                    
                    //
                    // Show the text
                    //
                    var html = string.Format ("<html><head><style>body{{background-color:{0};color:{1};}} a{{color:{2};}}</style></head><body>{3}</body></html>",
                        bgColor, color, linkColor, ViewModel.Content);

                    Browser.NavigateToString (html);
                }
            }
        }

        private void Browser_Navigating (object sender, NavigatingEventArgs e)
        {
            e.Cancel = true;

            var task = new WebBrowserTask {
                Uri = e.Uri,
            };
            task.Show ();
        }

        private void Browser_Navigated (object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Browser.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
