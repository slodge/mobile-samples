using System;
using System.Drawing;
using MonoTouch.UIKit;
using MWC.iOS.Screens.iPhone.Exhibitors;

namespace MWC.iOS.Screens.iPad.Exhibitors
{
    public class GeneralSplitViewDelegate : UISplitViewControllerDelegate
    {
        public override bool ShouldHideViewController (UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
        {
            return false;
        }
	}
}