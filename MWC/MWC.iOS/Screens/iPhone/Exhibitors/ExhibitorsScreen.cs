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

namespace MWC.iOS.Screens.iPhone.Exhibitors {
	/// <summary>
	/// Exhibitors screen. Derives from MonoTouch.Dialog's DialogViewController to do 
	/// the heavy lifting for table population.
	/// </summary>
	/// <remarks>
	/// This class initially inherited from UpdateManagerLoadingDialogViewController
	/// but when we split the data download into two parts, the methods from that
	/// baseclass we duplicated here (due to different eventhandlers)
	/// </remarks>
    public partial class ExhibitorsScreen : UpdateManagerLoadingDialogViewController<ExhibitorsListViewModel>
	{
		protected ExhibitorDetailsScreen exhibitorsDetailsScreen;
		
		/// <summary>
		/// Set pushing=true so that the UINavCtrl 'back' button is enabled
		/// </summary>
        public ExhibitorsScreen(MvxShowViewModelRequest request)
            : base(request)
		{
			EnableSearch = true; // requires ExhibitorElement to implement Matches()
		}
		

		/// <summary>
		/// Populates the page with exhibitors.
		/// </summary>
		protected void PopulateTable()
		{
			Root = 	new RootElement ("Exhibitors") {
					from exhibitorGroup in ViewModel.Groups
				    select new Section (exhibitorGroup.Key) {
					    from eachExhibitor in exhibitorGroup.Items
						    select (Element)new MWC.iOS.UI.CustomElements.ExhibitorElement(eachExhibitor)
			}};
			
			if (ViewModel.Groups.Count > 0)
			{
				// hide search until pull-down
				TableView.ScrollToRow (NSIndexPath.FromRowSection (0,0), UITableViewScrollPosition.Top, false);
			}
		}

		public override DialogViewController.Source CreateSizingSource (bool unevenRows)
		{
			return new ExhibitorsTableSource (this, ViewModel);
		}

		#region UpdatemanagerLoadingDialogViewController copied here, for Exhibitor-specific behaviour
		UI.Controls.LoadingOverlay loadingOverlay;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			BL.Managers.UpdateManager.UpdateExhibitorsStarted += HandleUpdateStarted;
			BL.Managers.UpdateManager.UpdateExhibitorsFinished += HandleUpdateFinished;
		}
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if(BL.Managers.UpdateManager.IsUpdatingExhibitors) {
				if (loadingOverlay == null) {
					loadingOverlay = new MWC.iOS.UI.Controls.LoadingOverlay (View.Frame);
					// because DialogViewController is a UITableViewController,
					// we need to step OVER the UITableView, otherwise the loadingOverlay
					// sits *in* the scrolling area of the table
					if (View.Superview != null) {
						// TODO: see when Superview is null
						View.Superview.Add (loadingOverlay); 
						View.Superview.BringSubviewToFront (loadingOverlay);
					}
				}
				Console.WriteLine("Waiting for updates to finish before displaying table.");
			} else {
				loadingOverlay = null;
				if (Root == null || Root.Count == 0) {
					Console.WriteLine("Not updating, populating table.");
					PopulateTable();
				} else Console.WriteLine ("Exhibitors already populated");
			}
		}
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			BL.Managers.UpdateManager.UpdateExhibitorsFinished -= HandleUpdateFinished; 
		}
		void HandleUpdateStarted(object sender, EventArgs e)
		{
			Console.WriteLine("Updates starting, need to create overlay.");
			InvokeOnMainThread ( () => {
				if (loadingOverlay == null) {
					loadingOverlay = new MWC.iOS.UI.Controls.LoadingOverlay (TableView.Frame);
					// because DialogViewController is a UITableViewController,
					// we need to step OVER the UITableView, otherwise the loadingOverlay
					// sits *in* the scrolling area of the table
					if (View.Superview != null) { //TODO: see when this is null
						View.Superview.Add (loadingOverlay); 
						View.Superview.BringSubviewToFront (loadingOverlay);
					}
				}
			});
		}
		void HandleUpdateFinished(object sender, EventArgs e)
		{
			Console.WriteLine("Updates finished, going to populate table.");
			InvokeOnMainThread ( () => {
				PopulateTable ();
				if (loadingOverlay != null)
					loadingOverlay.Hide ();
				loadingOverlay = null;
			});
		}
		#endregion
	}

	/// <summary>
	/// Implement index-slider down right side of tableview
	/// </summary>
	public class ExhibitorsTableSource : DialogViewController.SizingSource
    {
	    private readonly ExhibitorsListViewModel exhibitorList;

		public ExhibitorsTableSource (DialogViewController dvc, ExhibitorsListViewModel viewModel) : base(dvc)
		{
            exhibitorList = viewModel;
		}

		public override string[] SectionIndexTitles (UITableView tableView)
		{
			return exhibitorList.Groups.Select(x => x.Key).ToArray();
		}

//		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
//		{
//			return 65f;
//		}
	}
}