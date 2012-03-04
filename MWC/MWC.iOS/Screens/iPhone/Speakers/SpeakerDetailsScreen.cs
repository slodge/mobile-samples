using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Utilities;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.Screens.iPhone.Speakers {
	/// <summary>
	/// Displays personal information about the speaker
	/// </summary>
	public class SpeakerDetailsScreen 
        : MvxBindingTouchViewController<SpeakerDetailsViewModel>
        , IImageUpdated {

		UILabel nameLabel, titleLabel, companyLabel;
		UITextView bioTextView;
		UIImageView image;
		UIToolbar toolbar;
		UIScrollView scrollView;
		UITableView sessionTable;		
		int y = 0;
		const int ImageSpace = 80;
		
		public bool ShouldShowSessions { get; set; }

        public SpeakerDetailsScreen(MvxShowViewModelRequest request)
            : base(request)
		{
			ShouldShowSessions = true;
		}

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            View.BackgroundColor = UIColor.White;
         
         if (AppDelegate.IsPad) {
             toolbar = new UIToolbar (new RectangleF(0,0,UIScreen.MainScreen.Bounds.Width, 40));
             
             View.AddSubview (toolbar);
             y = 40;
         }

         nameLabel = new UILabel () {
             TextAlignment = UITextAlignment.Left,
             Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt),
             BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
         };
         titleLabel = new UILabel () {
             TextAlignment = UITextAlignment.Left,
             Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt),
             TextColor = UIColor.DarkGray,
             BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
         };
         companyLabel = new UILabel () {
             TextAlignment = UITextAlignment.Left,
             Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10pt),
             TextColor = UIColor.DarkGray,
             BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
         };
          bioTextView = new UITextView () {
             TextAlignment = UITextAlignment.Left,
             Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt),
             BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
             ScrollEnabled = true,
             Editable = false
         };
         image = new UIImageView();

         
         scrollView = new UIScrollView();

         scrollView.AddSubview (nameLabel);
         scrollView.AddSubview (titleLabel);
         scrollView.AddSubview (companyLabel);
         scrollView.AddSubview (bioTextView);
         scrollView.AddSubview (image);  

         Add (scrollView);
        }
        
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			// this shouldn't be null, but it gets that way when the data
			// "shifts" underneath it. need to reload the screen or prevent
			// selection via loading overlay - neither great UIs :-(
			LayoutSubviews ();
			Update ();
		}

		void LayoutSubviews ()
		{
			var full = View.Bounds;
			var bigFrame = full;
			
			scrollView.Frame = full;

			bigFrame.X = ImageSpace+13+17;
			bigFrame.Y = y + 27; // 15 -> 13
			bigFrame.Height = 26;
			bigFrame.Width -= (ImageSpace+13+17);
			nameLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.X = ImageSpace+13+17;
			smallFrame.Y = y + 27+26;
			smallFrame.Height = 15; // 12 -> 15
			smallFrame.Width -= (ImageSpace+13+17);
			titleLabel.Frame = smallFrame;
			
			smallFrame.Y += y + 17;
			companyLabel.Frame = smallFrame;

			image.Frame = new RectangleF(13, y + 15, 80, 80);
			
			bioTextView.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt);
			
			if (!String.IsNullOrEmpty(ViewModel.Bio)) {
				var f = new SizeF (full.Width - 13 * 2, 4000);
                SizeF size = bioTextView.StringSize(ViewModel.Bio
									, this.bioTextView.Font
									, f);
				bioTextView.Frame = new RectangleF(5
									, y + 115
									, f.Width
									, size.Height + 120); // doesn't seem to measure properly... CR/LF issues?
			
				bioTextView.ScrollEnabled = true;
				
				scrollView.ContentSize = new SizeF(320, bioTextView.Frame.Y + bioTextView.Frame.Height + 20);
			} else {
				bioTextView.ScrollEnabled = false;
				bioTextView.Frame = new RectangleF(5, y + 115, 310, 30);;
			}
			

			float bottomOfTheseControls = bioTextView.Frame.Y + bioTextView.Frame.Height;

            if (ShouldShowSessions && ViewModel.Sessions != null && ViewModel.Sessions.Count > 0)
            {
				RectangleF frame;
				//if (AppDelegate.IsPhone) {
					frame = new RectangleF(5
									, bottomOfTheseControls
									, 310
                                    , ViewModel.Sessions.Count * 40 + 50); // plus 40 for header
				//}

				if (sessionTable == null) {
					sessionTable = new UITableView(frame, UITableViewStyle.Grouped);
					sessionTable.BackgroundColor = UIColor.White;
					var whiteView = new UIView();
					whiteView.BackgroundColor = UIColor.White;
					sessionTable.BackgroundView = whiteView;
					sessionTable.ScrollEnabled = false;
					scrollView.AddSubview (sessionTable);
				}
				sessionTable.Frame = frame;
                sessionTable.Source = new SessionsTableSource(ViewModel.Sessions);

				scrollView.ContentSize = new SizeF(320, bottomOfTheseControls + sessionTable.Frame.Height + 20);

			} else { // there are NO sessions, remove the table if it exists
				if (sessionTable != null) {
					sessionTable.RemoveFromSuperview ();
					sessionTable.Dispose ();
					sessionTable = null;
				}
				if (AppDelegate.IsPhone)
					scrollView.ContentSize = new SizeF(320, bottomOfTheseControls);
			}
		}

		public void SelectSession (SessionDetailsViewModel session)
		{
            session.ShowDetailCommand.Execute();
#warning How to do ShouldShowSpeakers?
            //var request = base.Create
            //var sds = new MWC.iOS.Screens.iPhone.Sessions.SessionDetailsScreen (session.ID);
            //sds.ShouldShowSpeakers = false;
            //sds.Title = "Session";
            //NavigationController.PushViewController(sds, true);
		}

		void Update()
		{
			nameLabel.Text = ViewModel.Name;
            titleLabel.Text = ViewModel.Title;
            companyLabel.Text = ViewModel.Company;

            if (!String.IsNullOrEmpty(ViewModel.Bio))
            {
                bioTextView.Text = ViewModel.Bio;
				bioTextView.TextColor = UIColor.Black;
			} else {
				bioTextView.TextColor = UIColor.Gray;
				bioTextView.Text = "No background information available.";
			}
            if (ViewModel.ImageUrl != "http://www.mobileworldcongress.com")
            {
                var u = new Uri(ViewModel.ImageUrl);
				image.Image = ImageLoader.DefaultRequestImage (u, this);
			}
		}

		public void UpdatedImage (Uri uri)
		{
			image.Image = ImageLoader.DefaultRequestImage (uri, this);
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return AppDelegate.IsPad;
        }

	}
}