using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.iPhone.Sessions {
	/// <summary>
	/// Speakers screen. Derives from MonoTouch.Dialog's DialogViewController to do 
	/// the heavy lifting for table population.
	/// </summary>
	public partial class SessionsScreen : UpdateManagerLoadingDialogViewController<SessionListViewModel> {
		public IList<BL.Session> Sessions;
		
		/// <summary>for iPhone</summary>
        public SessionsScreen(MvxShowViewModelRequest request)
            : base(request)
		{
#warning How to enable search for iPad?
            //EnableSearch = true; // requires SessionElement to implement Matches()
        }
		

		/// <summary>
		/// Populates the page with sessions, grouped by time slot
		/// </summary>
		protected override void PopulateTable()
		{
			Sessions = BL.Managers.SessionManager.GetSessions ();
			
			Root = 	new RootElement ("Sessions") {
					from session in ViewModel.Groups
						select new Section (session.Key) {
						from eachSession in session.Items
						   select (Element) new MWC.iOS.UI.CustomElements.SessionElement (eachSession)
			}};
			// hide search until pull-down
			TableView.ScrollToRow (NSIndexPath.FromRowSection (0,0), UITableViewScrollPosition.Top, false);
		}	
		
		/// <summary>
		/// Used by iPad, to control popover list in SplitView
		/// </summary>
		public void ShowAll() {
			PopulateTable ();
		}
		
		/*
		/// <summary>
		/// Used by iPad, to filter popover list in SplitView
		/// </summary>
		public void FitlerByDay (int day) {
			Sessions = BL.Managers.SessionManager.GetSessions (day);
		
			Root = 	new RootElement ("") {
					from s in this.Sessions
						group s by s.Start.Ticks into g
						orderby g.Key
						select new Section (new DateTime (g.Key).ToString("dddd HH:mm") ) {
						from hs in g
						   select (Element) new MWC.iOS.UI.CustomElements.SessionElement (hs, splitView)
			}};
			// start again at the top
			lastScrollY = NSIndexPath.FromRowSection(0,0);
			TableView.ScrollToRow (lastScrollY, UITableViewScrollPosition.Top, false);
		}
		*/

		// scroll back to the point where you last were in the list
		NSIndexPath lastScrollY;
		public override void ViewWillDisappear (bool animated)
		{
			lastScrollY = TableView.IndexPathForSelectedRow;
			if (AppDelegate.IsPad && ObserverFavoriteChanged != null) 
				NSNotificationCenter.DefaultCenter.RemoveObserver(ObserverFavoriteChanged);
			base.ViewWillDisappear (animated);
		}

		/// <summary>
		/// Keep scroll-position in sync when coming back from detailscreen
		/// Keep favorite-stars in sync with changes made on other screens
		/// </summary>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (lastScrollY != null)
				TableView.ScrollToRow (lastScrollY, UITableViewScrollPosition.Middle, false);
			
			// sync the favorite stars if they change in other views (also SessionDayScheduleScreen)
			OnFavoriteChanged(null);
			
			if (AppDelegate.IsPad) {
				ObserverFavoriteChanged = NSNotificationCenter.DefaultCenter.AddObserver(
					AppDelegate.NotificationFavoriteUpdated, OnFavoriteChanged);
			}
		}
		protected void OnFavoriteChanged (NSNotification notification)
		{
			foreach (var sv in TableView.Subviews) {
				var cell = sv as MWC.iOS.UI.CustomElements.SessionCell;
				if (cell != null) {
					cell.UpdateFavorite();
				}	
			}
		}
		NSObject ObserverFavoriteChanged;
	}
}