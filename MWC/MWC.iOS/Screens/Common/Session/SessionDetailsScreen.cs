using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.iOS.Screens.Common.Session {
	/// <summary>
	/// Display session info (name, time, location) using UIKit controls and XIB file
	/// </summary>
    public partial class SessionDetailsScreen : MvxBindingTouchViewController<SessionDetailsViewModel>
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public SessionDetailsScreen (MvxShowViewModelRequest request)
			: base (request, UserInterfaceIdiomIsPhone ? "SessionDetailsScreen_iPhone" : "SessionDetailsScreen_iPad", null)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			int width = 245;
			if (!UserInterfaceIdiomIsPhone)
				width = 700;

			Title = "Session Detail";
			TitleLabel.Text = ViewModel.Title;
            SpeakerLabel.Text = ViewModel.SpeakerNames;
            TimeLabel.Text = ViewModel.Start.ToString("dddd") + " " +
                                ViewModel.Start.ToString("H:mm") + " - " +
                                ViewModel.End.ToString("H:mm");
            LocationLabel.Text = ViewModel.Room;
            OverviewLabel.Text = ViewModel.Overview;

            SizeF titleSize = TitleLabel.StringSize(ViewModel.Title
							, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
							, new SizeF (245, 400), UILineBreakMode.WordWrap);
			TitleLabel.Font = UIFont.FromName("Helvetica-Light", AppDelegate.Font16pt);
			TitleLabel.TextColor = UIColor.Black;
			TitleLabel.Frame = new RectangleF(13, 15, width, titleSize.Height);
			TitleLabel.Lines = 0;
			TitleLabel.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt);

            SizeF speakerSize = TitleLabel.StringSize(ViewModel.SpeakerNames
							, UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt)
							, new SizeF (245, 400), UILineBreakMode.WordWrap);
			SpeakerLabel.Font = UIFont.FromName("Helvetica-LightOblique", AppDelegate.Font10pt);
			SpeakerLabel.Frame = new RectangleF(13
													, 15 + 13 + titleSize.Height
													, width, speakerSize.Height);
			TimeLabel.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font7_5pt);
			TimeLabel.Frame = new RectangleF(13
													, 15 + titleSize.Height + 13 + speakerSize.Height + 5
													, width, 10);
			
			LocationLabel.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font7_5pt);
			LocationLabel.Frame = new RectangleF(13
													, 15 + titleSize.Height + 13 + speakerSize.Height + 7 + 12
													, width, 10);

			float overviewLabelWidth = 310;
			var overviewLabelY = 15 + titleSize.Height + 13 + speakerSize.Height + TimeLabel.Frame.Height + LocationLabel.Frame.Height + 20;
			float overviewLabelHeight = (UserInterfaceIdiomIsPhone?360:854) - overviewLabelY;
			OverviewLabel.Editable = false;
			OverviewLabel.Font = UIFont.FromName("Helvetica-Light", AppDelegate.Font10_5pt);
			if (AppDelegate.IsPhone) {
				// going to scroll the whole thing!
				OverviewLabel.ScrollEnabled = false;
			
				SizeF overviewSize = OverviewLabel.StringSize (
                                  ViewModel.Overview
								, UIFont.FromName("Helvetica-Light", AppDelegate.Font10_5pt)
								, new SizeF(overviewLabelWidth, 2500) // just width wasn't working...
								, UILineBreakMode.WordWrap);

				overviewLabelHeight = overviewSize.Height + 30;
				
				ScrollView.ContentSize = new SizeF(320, overviewLabelY + overviewLabelHeight + 10);
			}
			
			OverviewLabel.Frame = new RectangleF(5
													, overviewLabelY
													, UserInterfaceIdiomIsPhone?overviewLabelWidth:700
													, overviewLabelHeight);


			FavoriteButton.TouchUpInside += (sender, e) => {
				ToggleFavorite ();
			};

            if (FavoritesManager.IsFavorite(ViewModel.Key))
				FavoriteButton.SetImage (new UIImage(AppDelegate.ImageIsFavorite), UIControlState.Normal);
			else
				FavoriteButton.SetImage (new UIImage(AppDelegate.ImageNotFavorite), UIControlState.Normal);
		}

		bool ToggleFavorite ()
        {
#warning this really should go to: ViewModel IMvxCommand
            if (FavoritesManager.IsFavorite(ViewModel.Key))
            {
				FavoriteButton.SetImage (new UIImage(AppDelegate.ImageNotFavorite), UIControlState.Normal);
                FavoritesManager.RemoveFavoriteSession(ViewModel.Key);
				return false;
			} else {
				FavoriteButton.SetImage (new UIImage(AppDelegate.ImageIsFavorite), UIControlState.Normal);
                var fav = new Favorite { SessionID = ViewModel.ID, SessionKey = ViewModel.Key };
				FavoritesManager.AddFavoriteSession (fav);
				return true;
			}
		}
	}
}