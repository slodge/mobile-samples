using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.iOS.Screens.Common.News;

namespace MWC.iOS.Screens.iPad.News
{
	public class NewsSplitView : UISplitViewController
	{
		NewsScreen newsList;
		
		NewsDetailsScreen newsDetailView;
		
		public NewsSplitView ()
		{
			View.Bounds = new RectangleF(0,0,UIScreen.MainScreen.Bounds.Width,UIScreen.MainScreen.Bounds.Height);
			Delegate = new SplitViewDelegate();
			
			newsList = new NewsScreen(this);
			
			newsDetailView = new NewsDetailsScreen(null);
			
			this.ViewControllers = new UIViewController[]
				{newsList, newsDetailView};
		}
		
		public void ShowNews (int newsID, UIViewController newsView)
		{
			this.ViewControllers = new UIViewController[]
				{newsList, newsView};

		}
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }
	}

 	public class SplitViewDelegate : UISplitViewControllerDelegate
    {
#warning Put back in
        //public override bool ShouldHideViewController (UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
        //{
        //    return false;
        //}
	}
}