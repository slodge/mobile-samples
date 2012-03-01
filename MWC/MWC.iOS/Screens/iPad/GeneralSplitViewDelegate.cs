using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.iPad
{
    public class GeneralSplitViewDelegate : UISplitViewControllerDelegate
    {
        public override bool ShouldHideViewController (UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
        {
            return false;
        }
	}
}