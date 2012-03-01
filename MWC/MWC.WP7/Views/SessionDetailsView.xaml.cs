using System;
using System.Linq;
using System.Windows;
using Cirrious.MvvmCross.WindowsPhone.Views;
using MWC.Core.Mvvm.ViewModels;
using MWC.BL.Managers;
using MWC.BL;
using Microsoft.Phone.Shell;

namespace MWC.WP7.Views
{
    public class BaseSessionDetailsView : MvxPhonePage<SessionDetailsViewModel>
    {        
    }

    public partial class SessionDetailsView : BaseSessionDetailsView
    {
        public SessionDetailsView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo (System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo (e);

            UpdateFavoriteButtonIcon (ViewModel.IsFavorite);
        }

        void UpdateFavoriteButtonIcon (bool fav)
        {
            if (fav) {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IconUri = new Uri ("/Images/appbar.favs.removefrom.rest.png", UriKind.RelativeOrAbsolute);
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).Text = "remove fav";
            }
            else {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IconUri = new Uri ("/Images/appbar.favs.addto.rest.png", UriKind.RelativeOrAbsolute);
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).Text = "add fav";
            }
        }

        private void HandleFavoriteClick(object sender, EventArgs e)
        {
#warning Must be possible to do this better...
            ViewModel.FavoriteSessionsCommand.Execute();
            UpdateFavoriteButtonIcon(ViewModel.IsFavorite);
            /*
            if (FavoritesManager.IsFavorite (ViewModel.Key)) {
                FavoritesManager.RemoveFavoriteSession(ViewModel.Key);
                UpdateFavoriteButtonIcon (false);
            }
            else {
                FavoritesManager.AddFavoriteSession (new Favorite {
                    SessionKey = ViewModel.Key,
                });
                UpdateFavoriteButtonIcon (true);
            }
             */

        }

#warning Pinning disabled!
        private void HandlePinClick (object sender, EventArgs e)
        {
            MessageBox.Show("Pinning disabled");
            return;

            var vm = (SessionDetailsViewModel)DataContext;

            var uri = "/SessionDetails.xaml?key=" + vm.Key;

            var foundTile = ShellTile.ActiveTiles.FirstOrDefault (x => x.NavigationUri.ToString ().Contains (uri));

            if (foundTile != null) {
                foundTile.Delete ();
            }

            ShellTile.Create (new Uri (uri, UriKind.Relative), new StandardTileData {
                Title = vm.Title,
                BackContent = string.Format ("{0:ddd} {0:t} {1}", vm.Start, vm.Room),
                BackTitle = vm.Title,
                BackgroundImage = new Uri ("/Background.png", UriKind.RelativeOrAbsolute),
            });
        }
    }
}
