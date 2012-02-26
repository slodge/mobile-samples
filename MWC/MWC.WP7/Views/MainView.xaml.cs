using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Cirrious.MvvmCross.WindowsPhone.Views;
using MWC.BL.Managers;
using MWC.Core.Mvvm.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace MWC.WP7.Views
{
    public partial class MainView : BaseMainView
    {
        public MainView()
        {
            InitializeComponent();

            Language = XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.Name);
        }

        /*
        private void HandleSessionTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = (ListBoxItem)sender;

            var args = "";

            if (item.DataContext != null && item.DataContext is DateTime)
            {
                args = "?dayOfWeek=" + ((DateTime)item.DataContext).DayOfWeek;
            }
            else if (item.DataContext != null && item.DataContext is string)
            {
                args = "?" + Uri.EscapeDataString((string)item.DataContext);
            }

            NavigationService.Navigate(new Uri("/SessionList.xaml" + args, UriKind.RelativeOrAbsolute));
        }
        */

#warning Ideally this would be in the ViewModel too...
        private void HandleMap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var task = new BingMapsTask
            {
                Center = new GeoCoordinate(41.374377, 2.152226),
                SearchTerm = "Fira de Barcelona",
            };
            task.Show();
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var headerText = ((PanoramaItem)((Panorama)sender).SelectedItem).Header.ToString();
            ApplicationBar.IsVisible = (headerText == "twitter" || headerText == "news");
        }

        private void HandleRefresh(object sender, EventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            var headerText = ((PanoramaItem)MainPanorama.SelectedItem).Header.ToString();

            if (headerText == "twitter")
            {
                vm.Twitter.BeginUpdate();
            }
            else if (headerText == "news")
            {
                vm.News.BeginUpdate();
            }
        }
    }

    public class BaseMainView : MvxPhonePage<MainViewModel>
    {

    }
}