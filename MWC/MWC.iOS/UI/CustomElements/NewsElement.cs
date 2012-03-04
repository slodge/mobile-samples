using System;
using Cirrious.MvvmCross.Dialog.Touch.Dialog;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MWC.BL;

namespace MWC.iOS.UI.CustomElements {
	/// <summary>
	/// Originally used MonoTouch.Dialog BadgeElement but created 
	/// this custom element to fix layout issues I was having
	/// </summary>
    public class NewsElement : BaseElement
    {

		static NSString key = new NSString ("NewsElement");
		UIImage image;
        NewsItemViewModel entry;
		
		public NewsElement (NewsItemViewModel showEntry, UIImage showImage) : base (showEntry.Title)
		{
			entry = showEntry;
			image = showImage;
		}

        protected override UITableViewCell GetCellImpl(UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key);
			
			if (cell == null) {
				cell = new NewsCell (UITableViewCellStyle.Default, key, entry.Title, image);
			} else {
				((NewsCell)cell).UpdateCell (entry.Title, image);
			}
			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
            entry.ShowDetailCommand.Execute();
		}
	}
}