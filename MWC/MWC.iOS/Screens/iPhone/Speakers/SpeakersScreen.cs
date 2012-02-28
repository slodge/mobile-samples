using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.Screens.iPhone.Speakers {
	/// <summary>
	/// Speakers screen. Derives from MonoTouch.Dialog's DialogViewController to do 
	/// the heavy lifting for table population. Also uses ImageLoader in SpeakerCell.cs
	/// </summary>
	public partial class SpeakersScreen : UpdateManagerLoadingDialogViewController<SpeakerListViewModel> {
		protected SpeakerDetailsScreen speakerDetailsScreen;
		IList<Speaker> speakers;
		
		/// <summary>for iPhone</summary>
		public SpeakersScreen (MvxShowViewModelRequest request) 
            : base(request)
		{
		}

		/// <summary>
		/// Populates the page with exhibitors.
		/// </summary>
		protected override void PopulateTable()
		{
			Root = 	new RootElement ("Speakers") {
					from speakerGroup in ViewModel.Groups
						select new Section (speakerGroup.Key) {
						from eachSpeaker in speakerGroup
						   select (Element) new MWC.iOS.UI.CustomElements.SpeakerElement (eachSpeaker)
			}};
			// hide search until pull-down
			TableView.ScrollToRow (NSIndexPath.FromRowSection (0,0), UITableViewScrollPosition.Top, false);
		}
		
		public override DialogViewController.Source CreateSizingSource (bool unevenRows)
		{
			return new SpeakersTableSource(this, speakers);
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

	}
	
	/// <summary>
	/// Implement index
	/// </summary>
	public class SpeakersTableSource : DialogViewController.SizingSource {
		IList<Speaker> speakerList;
		public SpeakersTableSource (DialogViewController dvc, IList<Speaker> speakers) : base(dvc)
		{
			speakerList = speakers;
		}

		public override string[] SectionIndexTitles (UITableView tableView)
		{
			var sit = from speaker in speakerList
                    group speaker by (speaker.Index) into alpha
						orderby alpha.Key
						select alpha.Key;
			return sit.ToArray();
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 60f;
		}
	}
}