using System;
using System.Collections.Generic;
using System.Drawing;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.iOS.Screens.Common;


namespace MWC.iOS.Screens.iPhone.Twitter {
	/// <summary>
	/// List of tweets, this MT.D-based list is used on both iPhone and iPad
	/// </summary>
	public partial class TwitterScreen : LoadingDialogViewController<TwitterViewModel> {
		//TwitterSplitView splitView;

        public TwitterScreen(MvxShowViewModelRequest request)
            : base(request, UITableViewStyle.Plain, new RootElement("Loading..."))
		{
			RefreshRequested += HandleRefreshRequested;
		}
		
        //public TwitterScreen (TwitterSplitView twitterSplitView) : this ()
        //{
        //    splitView = twitterSplitView;
        //}

		public override Source CreateSizingSource (bool unevenRows)
		{
			return new TwitterScreenSizingSource(this);
		}

		/// <summary>
		/// Implement MonoTouch.Dialog's pull-to-refresh method
		/// </summary>
		void HandleRefreshRequested (object sender, EventArgs e)
		{
            throw new NotImplementedException("Need to go via a IMvxCommand here");
			//BL.Managers.TwitterFeedManager.Update ();
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
			if (ViewModel.Items == null || ViewModel.Items.Count == 0) {
				var section = new Section ("Network unavailable") {
					new StyledStringElement ("Twitter not available. Try again later.")
				};
				Root = new RootElement ("Twitter") { section };
			} else {
				Section section;
				UI.CustomElements.TweetElement twitterElement;
				
				// create a root element and a new section (MT.D requires at least one)
				Root = new RootElement ("Twitter");
				section = new Section();
	
				// for each tweet, add a custom TweetElement to the MT.D elements collection
                foreach (var tw in ViewModel.Items)
                {
					var currentTweet = tw; 
					twitterElement = new UI.CustomElements.TweetElement (currentTweet);
					section.Add (twitterElement);
				}
				
				Root.Clear ();
				// add the section to the root
				Root.Add(section);
			}
			ReloadComplete ();
		}
	}

	/// <summary>
	/// Implement variable row height here, since when it is implemented on the TweetCell
	/// itself the variable heights are not returned after a pull-to-refresh.
	/// </summary>
	public class TwitterScreenSizingSource : DialogViewController.SizingSource
	{
		TwitterScreen twitterScreen;
		public TwitterScreenSizingSource (DialogViewController dvc) : base(dvc)
		{
			twitterScreen = (TwitterScreen)dvc;
		}
		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (twitterScreen.ViewModel.Items != null && twitterScreen.ViewModel.Items.Count > indexPath.Row) {
                var t = twitterScreen.ViewModel.Items[indexPath.Row];
				SizeF size = tableView.StringSize (t.Title
								, UIFont.FromName("Helvetica-Light",AppDelegate.Font10_5pt)
								, new SizeF (239, 140), UILineBreakMode.WordWrap);
				return 14 + 21 + 22 + size.Height + 8;
			} else 
				return 40f;
		}
	}
}