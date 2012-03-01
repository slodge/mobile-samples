using System;
using System.Linq;
using System.Collections.Generic;
using Cirrious.MvvmCross.Touch.Interfaces;
using MWC.iOS.Interfaces;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.iPad.Exhibitors
{
    public sealed class GeneralSplitView 
        : UISplitViewController
        , IViewModelAware
    {
        private Dictionary<Type, Type> _viewModelTypes;

        public GeneralSplitView(UIViewController masterView, UIViewController detailsView = null, params Type[] supportedViewModelTypes)
        {
            //View.Bounds = new RectangleF(0,0,UIScreen.MainScreen.Bounds.Width,UIScreen.MainScreen.Bounds.Height);
            Delegate = new GeneralSplitViewDelegate();
            _viewModelTypes = supportedViewModelTypes.ToDictionary(x => x);

            var detailsPlaceholder = detailsView ?? new UIViewController();

            this.ViewControllers = new UIViewController[] { masterView, detailsPlaceholder };
        }
		
        public void ShowDetail(UIViewController view)
        {
            this.ViewControllers = new UIViewController[] { this.ViewControllers[0], view };
        }

        public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

        public bool CanShow(IMvxTouchView view)
        {
            return _viewModelTypes.ContainsKey(view.ShowRequest.ViewModelType);
        }

        public void PushMasterView(UIViewController view)
        {
            this.ViewControllers = new UIViewController[] { view, this.ViewControllers[0] };
        }
    }
}