using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using MWC.iOS.Interfaces;
using MonoTouch.UIKit;

namespace MWC.iOS
{
    public class MWCPresenter 
        : MvxModalSupportTouchViewPresenter
		, IMWCTabBarPresenterHost
	{
        public IMWCTabBarPresenter TabBarPresenter { get; set; }

        public MWCPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
			: base(applicationDelegate, window)
		{
		}
		
		protected override UINavigationController CreateNavigationController (UIViewController viewController)
		{
			var toReturn = base.CreateNavigationController (viewController);
			toReturn.NavigationBarHidden = true;
			return toReturn;
		}
		
        public override bool ShowView(Cirrious.MvvmCross.Touch.Interfaces.IMvxTouchView view)
        {
            if (TabBarPresenter != null)
            {
                if (TabBarPresenter.ShowView(view))
                    return true;
            }

            return base.ShowView(view);
        }

        public override bool GoBack()
        {
            if (TabBarPresenter != null)
            {
                return TabBarPresenter.GoBack();
            }

            return base.GoBack();
        }
	}
}

