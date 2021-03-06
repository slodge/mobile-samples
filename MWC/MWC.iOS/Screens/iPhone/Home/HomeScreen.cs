using System;
using System.Collections.Generic;
using System.Drawing;
using Cirrious.MvvmCross.Binding.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.Screens.iPhone.Home {
	/// <summary>
	/// Home screen contains a masthead graphic/ad
	/// plus (iPad only) "what's on" in the next two 'timeslots'
	/// and the "favorites" list.
	/// </summary>
    public partial class HomeScreen : MvxBindingTouchViewController<ScheduleUpcomingAndFavoritesViewModel> 
    {
		UI.Controls.LoadingOverlay loadingOverlay;
		NSObject ObserverRotation;

		public HomeScreen (MvxShowViewModelRequest request) 
            : base (request, AppDelegate.IsPhone ? "HomeScreen_iPhone" : "HomeScreen_iPad", null)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			SessionTable.SeparatorColor = UIColor.Black;
			SessionTable.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine; 
			
			if (AppDelegate.IsPhone) 
            {
				MwcLogoImageView.Image = UIImage.FromBundle("/Images/Home");
				MwcLogoImageView.Frame = new RectangleF(0,0,320,480);
			} 
            else 
            {
				// IsPad
				MwcLogoImageView.Image = UIImage.FromBundle("/Images/Home-Portrait~ipad");
				
				// style the separators to be black
				SessionTable.SeparatorColor = UIColor.Black;
				SessionTable.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine; 
				UpNextTable.SeparatorColor = UIColor.Black;
				UpNextTable.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine; 
				FavoritesTable.SeparatorColor = UIColor.Black;
				FavoritesTable.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine; 

				// the extra 'clear' Views for background are a bit of a hack
				// and not needed on the phone!
				// http://forums.macrumors.com/showthread.php?t=901706
				var clearView1 = new UIView();
				clearView1.Frame = new RectangleF(0,470, 320, 200);
				clearView1.BackgroundColor = UIColor.Clear;
				SessionTable.BackgroundColor = UIColor.Clear;
				SessionTable.BackgroundView = clearView1;
				
				var clearView2 = new UIView();
				clearView2.Frame = new RectangleF(0,470+210, 320, 320);
				clearView2.BackgroundColor = UIColor.Clear;
				UpNextTable.BackgroundColor = UIColor.Clear;
				UpNextTable.BackgroundView = clearView2;
				
				var clearView3 = new UIView();
				clearView3.Frame = new RectangleF(768-320,470, 320, 420);				
				clearView3.BackgroundColor = UIColor.Clear;
				FavoritesTable.BackgroundColor = UIColor.Clear;
				FavoritesTable.BackgroundView = clearView3;	
			}

            PopulateTable();
		}

        protected void PopulateTable()
        {
            var tableSource = new MWC.iOS.AL.DaysTableSource(SessionTable);
            tableSource.SelectionChanged += (sender, args) => ViewModel.DayChosenCommand.Execute(args.AddedItems[0]);

            this.AddBindings(new Dictionary<object, string>()
		                         {
		                             {tableSource, "{'ItemsSource':{'Path':'Days'}}"}
		                         });

            SessionTable.Source = tableSource;
            SessionTable.ReloadData();

            if (AppDelegate.IsPad)
                PopulateiPadTables();
        }

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return AppDelegate.IsPad;
		}

		/// <summary>iPad only method</summary>
		void SessionClicked (object sender, MWC.iOS.AL.FavoriteClickedEventArgs args)
		{
#warning More needed here!
            //var s = new MWC.iOS.Screens.iPad.SessionPopupScreen(args.SessionClicked, this);
            //s.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
            //PresentModalViewController (s, true);
		}

		/// <summary>iPad only method</summary>
		public void SessionClosed(bool wasDirty) 
		{
			if (wasDirty)
				PopulateiPadTables();
		}

		/// <summary>iPad only method: the UpNext and Favorites tables</summary>
		void PopulateiPadTables()
		{
#warning More needed here!
            /*
			var uns = new MWC.iOS.AL.UpNextTableSource();
			UpNextTable.Source = uns;
			uns.SessionClicked += SessionClicked;
			UpNextTable.ReloadData();
			
			var fs = new MWC.iOS.AL.FavoritesTableSource();
			FavoritesTable.Source = fs;
			fs.FavoriteClicked += SessionClicked;
			FavoritesTable.ReloadData ();
             */
		}

        ///// <summary>
        ///// Show the session info, push navctrl for iPhone, in a modal overlay for iPad
        ///// </summary>
        //protected void LoadSessionDayScreen (string dayName, int day)
        //{
        //    //if (AppDelegate.IsPhone) 
        //    //{

        //    //    dayScheduleScreen = new MWC.iOS.Screens.Common.Session.SessionDayScheduleScreen (dayName, day, null);
        //    //    NavigationController.PushViewController (dayScheduleScreen, true);				
        //    //} else {
        //    //    var nvc = ParentViewController;
        //    //    var tab = nvc.ParentViewController as MWC.iOS.Screens.Common.TabBarController;
        //    //    tab.SelectedIndex = 1;
        //    //    tab.ShowSessionDay(day);
        //    //}
        //}
		
		public bool IsPortrait {
			get {
				return InterfaceOrientation == UIInterfaceOrientation.Portrait 
					|| InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown;
			}
		}

		/// <summary>
		/// Home layout changes on rotation
		/// </summary>
		protected void OnDeviceRotated (NSNotification notification)
		{
			if (AppDelegate.IsPad) {
				if (IsPortrait) {
					MwcLogoImageView.Image = UIImage.FromBundle("/Images/Home-Portrait~ipad");
					SessionTable.Frame   = new RectangleF(0,      300, 320, 320);
					UpNextTable.Frame    = new RectangleF(0,      640, 320, 300);
					FavoritesTable.Frame = new RectangleF(768-400,300, 400, 560);
				}
				else
				{	// IsLandscape
					MwcLogoImageView.Image = UIImage.FromBundle("/Images/Home-Landscape~ipad");
					SessionTable.Frame   = new RectangleF(0,   300, 320, 390);
					UpNextTable.Frame    = new RectangleF(350, 300, 320, 390);
					FavoritesTable.Frame = new RectangleF(704, 300, 320, 390);
				}
			}
		}

		/// <summary>
		/// Is called when the view is about to appear on the screen. We use this method to hide the 
		/// navigation bar.
		/// </summary>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			NavigationController.SetNavigationBarHidden (true, animated);
			
			if (AppDelegate.IsPad) {
				OnDeviceRotated(null);
				
				// We attempt to re-populate to refresh the 'Favorites' and 'Up Next' lists (which need to change over time)
				PopulateiPadTables();
			
				ObserverRotation = NSNotificationCenter.DefaultCenter.AddObserver(
					AppDelegate.NotificationOrientationDidChange, OnDeviceRotated);
				UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
			}
		}
		
		/// <summary>
		/// Is called when the another view will appear and this one will be hidden. We use this method
		/// to show the navigation bar again.
		/// </summary>
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			NavigationController.SetNavigationBarHidden (false, animated);
	
			if (AppDelegate.IsPad) {
				UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();
				NSNotificationCenter.DefaultCenter.RemoveObserver(ObserverRotation);
			}
		}
	}
}