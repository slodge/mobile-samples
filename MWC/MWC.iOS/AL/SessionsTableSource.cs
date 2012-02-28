using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS {
	public class SessionsTableSource : UITableViewSource {
        IList<SessionDetailsViewModel> sessions;
		static NSString cellId = new NSString("SessionCell");

		public SessionsTableSource (IList<SessionDetailsViewModel> sessions)
		{
			this.sessions = sessions;
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var speaker = sessions[indexPath.Row];
			var cell = tableView.DequeueReusableCell(cellId);
			if(cell == null) 
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellId);
			
			cell.TextLabel.Text = speaker.Title;
			return cell;
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 40f;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return sessions.Count;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "Sessions";
		}	

		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var session = sessions[indexPath.Row];
		    session.ShowDetailCommand.Execute();
		}
	}
}
