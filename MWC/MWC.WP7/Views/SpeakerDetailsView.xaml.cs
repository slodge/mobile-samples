using System;
using System.Linq;
using System.Windows;
using Cirrious.MvvmCross.WindowsPhone.Views;
using MWC.Core.Mvvm.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MWC.WP7.Utilities;

namespace MWC.WP7.Views
{
    public class BaseSpeakerDetailsView : MvxPhonePage<SpeakerDetailsViewModel>
    {
        
    }

    public partial class SpeakerDetailsView : BaseSpeakerDetailsView
    {
        public SpeakerDetailsView ()
        {
            InitializeComponent ();
        }

#warning Pinning disabled
        private void HandlePinClick (object sender, EventArgs e)
        {
            MessageBox.Show("Pinning disabled");
            return;

            var vm = (SpeakerDetailsViewModel)DataContext;

            var uri = "/SpeakerDetails.xaml?key=" + vm.Key;

            var imageUri = default (Uri);
            try {
                imageUri = SpeakerImage.SaveAsTile ("Speaker-" + vm.ID);
            }
            catch (Exception) {
            }            

            var foundTile = ShellTile.ActiveTiles.FirstOrDefault (x => x.NavigationUri.ToString ().Contains (uri));

            if (foundTile != null) {
                foundTile.Delete ();
            }

            var tile = new StandardTileData {
                Title = vm.Name,
                BackContent = string.Format ("{0} at {1}", vm.Title, vm.Company),
                BackTitle = vm.Name,
                BackgroundImage = (imageUri != null) ? imageUri : new Uri ("/Background.png", UriKind.RelativeOrAbsolute),
            };

            ShellTile.Create (new Uri (uri, UriKind.Relative), tile);
        }
    }
}
