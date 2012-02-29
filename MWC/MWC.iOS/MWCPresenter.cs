using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using MonoTouch.UIKit;

namespace MWC.iOS
{
    public class MWCPresenter 
        : MvxModalSupportTouchViewPresenter
	{
        public IMWCTabBarPresenter TabBarPresenter { get; set; }

        public MWCPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
			: base(applicationDelegate, window)
		{
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

