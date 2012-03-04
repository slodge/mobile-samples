using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Dialog.Touch.Dialog;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.ViewModels;
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

		
		/// <summary>for iPhone</summary>
		public SpeakersScreen (MvxShowViewModelRequest request) 
            : base(request)
		{
		}


		protected override void RefreshItems ()
		{
			Root = 	new RootElement ("Speakers") {
					from speakerGroup in ViewModel.Groups
						select new Section (speakerGroup.Key) {
						from eachSpeaker in speakerGroup
						   select (Element) new MWC.iOS.UI.CustomElements.SpeakerElement (eachSpeaker)
			}};
			
			if (ViewModel.Groups.Count > 0)
			{
				// hide search until pull-down
				TableView.ScrollToRow (NSIndexPath.FromRowSection (0,0), UITableViewScrollPosition.Top, false);
			}
		}
					
		public override DialogViewController.Source CreateSizingSource (bool unevenRows)
		{
			return new SpeakersTableSource(this, ViewModel);
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
		SpeakerListViewModel speakerList;
		public SpeakersTableSource (DialogViewController dvc, SpeakerListViewModel speakers) : base(dvc)
		{
			speakerList = speakers;
		}

		public override string[] SectionIndexTitles (UITableView tableView)
		{
			return speakerList.Groups.Select(x => x.Key).ToArray();
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 60f;
		}
	}
}