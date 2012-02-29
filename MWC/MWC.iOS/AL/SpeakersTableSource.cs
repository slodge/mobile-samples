using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.iOS {
	public class SpeakersTableSource : UITableViewSource {
		IList<SpeakerDetailsViewModel> speakers;
		MWC.iOS.UI.Controls.Views.SessionView view;
		static NSString cellId = new NSString("SpeakerCell");

		public SpeakersTableSource (IList<SpeakerDetailsViewModel> speakers, MWC.iOS.UI.Controls.Views.SessionView view)
		{
			this.speakers = speakers;
			this.view = view;
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var speaker = speakers[indexPath.Row];
			var cell = tableView.DequeueReusableCell(cellId);
			if(cell == null) 
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellId);
			
			cell.TextLabel.Text = speaker.Name;
			return cell;
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 40f;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return speakers.Count;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "Speakers";
		}	

		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var speaker = speakers[indexPath.Row];
            speaker.ShowDetailCommand.Execute();
			if (AppDelegate.IsPhone) tableView.DeselectRow (indexPath, true);
		}
	}
}
