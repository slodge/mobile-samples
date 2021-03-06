using System;
using Cirrious.MvvmCross.Dialog.Touch.Dialog;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MWC.BL;
using MWC.SAL;

namespace MWC.iOS.UI.CustomElements {
	/// <summary>
	/// Renders a Tweet
	/// </summary>
	/// <remarks>
	/// Originally implemented IElementSizing.GetHeight in this class, however
	/// the variable height was not returned after pull-to-refresh (MT.D bug?)
	/// This was fixed by moving the implementation to TwitterScreenSizingSource
	/// </remarks>
    public class TweetElement : BaseElement
    {
		static NSString cellId = new NSString ("TweetElement");

        TweetViewModel tweet;
		//MWC.iOS.Screens.iPad.Twitter.TwitterSplitView splitView;

		/// <summary>
		/// for iPhone
		/// </summary>
		public TweetElement (TweetViewModel showTweet) : base (showTweet.Author)
		{
			tweet = showTweet;
		}

        /*
		/// <summary>
		/// for iPad (SplitViewController)
		/// </summary>
		public TweetElement (Tweet showTweet, MWC.iOS.Screens.iPad.Twitter.TwitterSplitView twitterSplitView) : base (showTweet.Author)
		{
			tweet = showTweet;
			splitView = twitterSplitView;	// could be null, in current implementation
		}
         */

		protected override UITableViewCell GetCellImpl (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (cellId);
			if (cell == null)
				cell = new TweetCell (UITableViewCellStyle.Subtitle, cellId, tweet);
			else
				((TweetCell)cell).UpdateCell (tweet);

			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
            tweet.ShowDetailCommand.Execute();
		}
	}
}