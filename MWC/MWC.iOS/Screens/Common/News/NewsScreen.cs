using System;
using System.Collections.Generic;
using System.Drawing;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;
using MWC.iOS.Screens.Common;
using MWC.iOS.UI.CustomElements;

namespace MWC.iOS.Screens.Common.News {
	/// <summary>
	/// News sourced from a google search, this MT.D-based list is used on both iPhone and iPad
	/// </summary>
	public class NewsScreen : LoadingDialogViewController<NewsListViewModel> {
		static UIImage calendarImage = UIImage.FromFile (AppDelegate.ImageCalendarPad);

 		public NewsScreen (MvxShowViewModelRequest request)
            : base(request, UITableViewStyle.Plain, new RootElement("Loading..."))
 		{
			RefreshRequested += HandleRefreshRequested;
		}

		/// <summary>
		/// Implement MonoTouch.Dialog's pull-to-refresh method
		/// </summary>
		void HandleRefreshRequested (object sender, EventArgs e)
		{
            throw new NotImplementedException("Needs to go to a IMvxCommand");
		}

		void HandleUpdateStarted(object sender, EventArgs ea)
		{
			MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
		}

		void HandleUpdateFinished(object sender, EventArgs ea)
		{	
			MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			RefreshRequested -= HandleRefreshRequested;
		}
		
		// hack to keep the selection, for some reason DidLayoutSubviews is getting called twice and i don't know wh
		NSIndexPath tempIndexPath;
		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();
			if (TableView.IndexPathForSelectedRow != null) 
				tempIndexPath = TableView.IndexPathForSelectedRow;
			else if (tempIndexPath != null) {
				TableView.SelectRow (tempIndexPath, false, UITableViewScrollPosition.None);
				tempIndexPath = null;
			}
		}


		protected override void RefreshItems ()
		{
			if (ViewModel.Items.Count == 0)
            {
				var section = new Section("Network unavailable") {
					new StyledStringElement("News not available. Try again later.") 
				};
				Root = new RootElement ("News") { section };
			} 
            else 
            {
				var blogSection = new Section ();
				// creates the rows using MT.Dialog
                foreach (var post in ViewModel.Items)
                {
					var published = post.Published;
					var image = MWC.iOS.UI.CustomElements.CustomBadgeElement.MakeCalendarBadge (calendarImage
														, published.ToString ("MMM").ToUpper ()
														, published.ToString ("dd"));
					var badgeRow = new NewsElement (post, image);
	
					blogSection.Add (badgeRow);
				}
				Root = new RootElement ("News") { blogSection };
			}
			base.StopLoadingScreen();
			this.ReloadComplete ();
		}
		public override Source CreateSizingSource (bool unevenRows)
		{
			return new NewsScreenSizingSource(this);
		}
    }

	public class NewsScreenSizingSource : DialogViewController.SizingSource
	{
		NewsScreen _ns;
		public NewsScreenSizingSource (DialogViewController dvc) : base(dvc)
		{
			_ns = (NewsScreen)dvc;
		}
		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (_ns.ViewModel.Items.Count > indexPath.Row) {
                var t = _ns.ViewModel.Items[indexPath.Row];
				SizeF size = tableView.StringSize (t.Title
								, UIFont.FromName("Helvetica-Light",AppDelegate.Font16pt)
								, new SizeF (230, 400), UILineBreakMode.WordWrap);
				return size.Height + 20;
			}
			else return 40f;
		}
	}
}