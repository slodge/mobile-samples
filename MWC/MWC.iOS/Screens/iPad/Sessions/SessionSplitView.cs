using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using MWC.iOS.Screens.iPhone.Sessions;
using Cirrious.MvvmCross.Touch.Interfaces;

namespace MWC.iOS.Screens.iPad.Sessions {
	public class SessionSplitView
        : GeneralSplitView
    {
		public SessionSplitView (UIViewController masterView, params Type[] supportedViewModelTypes)
            : base(masterView, new SessionSpeakersMasterDetail(), supportedViewModelTypes)
		{
			Delegate = new SessionSplitViewDelegate();
		}
		
		/// <summary>
		/// On 'view will appear', if we were showing a particular day last time,
		/// we're going to revert to the entire schedule this time
		/// </summary>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

        public override void ShowDetail(UIViewController view)
        {
            var masterDetail = this.ViewControllers[1] as SessionSpeakersMasterDetail;
            masterDetail.ShowView(view as IMvxTouchView);
        }

		public bool IsPortrait {
			get {
				return InterfaceOrientation == UIInterfaceOrientation.Portrait 
					|| InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown;
			}
		}
	}

 	public class SessionSplitViewDelegate : UISplitViewControllerDelegate
    {
        public override bool ShouldHideViewController (UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
        {
            return inOrientation == UIInterfaceOrientation.Portrait
                || inOrientation == UIInterfaceOrientation.PortraitUpsideDown;
        }

		public override void WillHideViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem barButtonItem, UIPopoverController pc)
		{
			SessionSpeakersMasterDetail dvc = svc.ViewControllers[1] as SessionSpeakersMasterDetail;
			
			if (dvc != null) {
				dvc.AddNavBarButton (barButtonItem);
				dvc.Popover = pc;
			} else Console.WriteLine ("SessionSplitViewController dvc == null (hide)");
		}
		
		public override void WillShowViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
		{
			SessionSpeakersMasterDetail dvc = svc.ViewControllers[1] as SessionSpeakersMasterDetail;
			
			if (dvc != null) {
				dvc.RemoveNavBarButton ();
				dvc.Popover = null;
			} else Console.WriteLine ("SessionSplitViewController dvc == null (show)");
		}
	}
}