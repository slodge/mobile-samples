using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.Common {
	/// <summary>
	/// Share a 'loading' screen between DialogViewControllers that
	/// are populated from network requests (Twitter and News)
	/// </summary>
	/// <remarks>
	/// This ViewController implements the data loading via a virtual
	/// method LoadData(), which must call StopLoadingScreen()
	/// </remarks>
    public abstract class LoadingDialogViewController<TViewModel>
        : MvxTouchDialogViewController<TViewModel>
        where TViewModel : UpdatingViewModelBase
	{
		MWC.iOS.Screens.Common.UILoadingView loadingView;
		
		/// <summary>
		/// Set pushing=true so that the UINavCtrl 'back' button is enabled
		/// </summary>
        public LoadingDialogViewController(MvxShowViewModelRequest request, UITableViewStyle style, RootElement root)
            : base(request, style, root, true)
		{
		}

		public override void ViewWillAppear (bool animated)
		{
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            RefreshLoadingVisibility();
			RefreshItems();
			base.ViewWillAppear (animated);
		}
		
		public override void ViewWillDisappear (bool animated)
		{
            ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
			StopLoadingScreen();
			base.ViewWillDisappear (animated);
		}
		
        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
			switch (propertyChangedEventArgs.PropertyName)
			{
				case "IsUpdating":
            		RefreshLoadingVisibility();
					break;
				case "Items":
				case "Groups":
					RefreshItems ();
					break;
			}
        }
		
		protected abstract void RefreshItems();		

	    private void RefreshLoadingVisibility()
	    {
            if (ViewModel.IsUpdating)
                StartLoadingScreen("loading...");
            else
                StopLoadingScreen();
	    }

	    public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return AppDelegate.IsPad;
		}

		/// <summary>
		/// Called automatically in ViewDidLoad()
		/// </summary>
		protected void StartLoadingScreen (string message)
		{
            if (loadingView != null)
                return;

			var bounds = new RectangleF(0,0,768,1004);
			if (InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft
			|| InterfaceOrientation == UIInterfaceOrientation.LandscapeRight) {
				bounds = new RectangleF(0,0,1024,748);	
			} 

			if (AppDelegate.IsPhone)
				bounds = new RectangleF(0,0,320,460);

			loadingView = new UILoadingView (message, bounds);
			// because DialogViewController is a UITableViewController,
			// we need to step OVER the UITableView, otherwise the loadingOverlay
			// sits *in* the scrolling area of the table
			View.Superview.Add (loadingView);
			View.Superview.BringSubviewToFront (loadingView);
			View.UserInteractionEnabled = false;
		}
		
		/// <summary>
		/// If a loading screen exists, it will fade it out.
		/// Your subclass MUST call this method once data has loaded (or a loading error occurred)
		/// to make the loading screen disappear and return control to the user
		/// </summary>
		protected void StopLoadingScreen ()
		{
			if (loadingView != null) {
				Debug.WriteLine ("Fade out loading...");
				loadingView.OnFinishedFadeOutAndRemove += delegate {
					if (loadingView != null) {
						Debug.WriteLine ("Disposing of loadingView object..");
						loadingView.Dispose();
						loadingView = null;
					}
				};
				loadingView.FadeOutAndRemove ();
				View.UserInteractionEnabled = true;
			}
		}
	}
}