using System;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.UIKit;

namespace MWC.iOS
{
	public class SinglePanePresenter : MvxTouchSingleViewsPresenter
	{
		UIWindow window;
		UINavigationController navigationController;
		
		public SinglePanePresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
			: base(applicationDelegate, window)
		{
			this.window = window;
		}
		
		protected override MonoTouch.UIKit.UINavigationController CreateNavigationController 
			(MonoTouch.UIKit.UIViewController viewController)
		{
			navigationController = base.CreateNavigationController (viewController);
			navigationController.NavigationBarHidden = true;
			
			return navigationController;
		}		
	}
}

