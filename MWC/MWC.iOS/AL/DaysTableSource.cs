using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MWC.iOS.UI.CustomElements;

namespace MWC.iOS.AL {
    public class DaysTableSource : MvxBindableTableViewSource
    {
		static NSString cellId = new NSString("DayCell");
		
		public DaysTableSource (UITableView tableView)
            : base(tableView)
		{
		}

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath, object item)
		{
			DayCell cell = tableView.DequeueReusableCell(cellId) as DayCell;
			
			if(cell == null)
                cell = new DayCell(cellId);

			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			return cell;
		}
		
		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (AppDelegate.IsPad)
				return 70f;
			else
				return 45f;
		}

		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			return 30f;
		}

		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "Full Schedule";
//			if (AppDelegate.IsPad) return "Full Schedule";
//			return null; // don't want a section title on the Phone
		}

		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
//			if (AppDelegate.IsPhone) return null;
			return BuildSectionHeaderView("Full Schedule");
		}
		
		/// <summary>
		/// Sharing this with all three tables on the HomeScreen
		/// </summary>
		public static UIView BuildSectionHeaderView(string caption) {
           UIView view = new UIView(new System.Drawing.RectangleF(0,0,320,20));
           UILabel label = new UILabel();
           label.BackgroundColor = UIColor.Clear;
           label.Opaque = false;
           label.TextColor = AppDelegate.ColorHeadingHome; //UIColor.FromRGB (150, 210, 254);
           label.Font = UIFont.FromName("Helvetica-Bold", 16f);
           label.Frame = new System.Drawing.RectangleF(15,0,290,20);
           label.Text = caption;
           view.AddSubview(label);
           return view;
		}
	}
}