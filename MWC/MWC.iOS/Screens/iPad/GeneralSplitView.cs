using System;
using System.Linq;
using System.Collections.Generic;
using Cirrious.MvvmCross.Touch.Interfaces;
using MWC.iOS.Interfaces;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.iPad
{
    public class GeneralSplitView
        : IntelligentSplitViewController
        , IViewModelAware
    {
        private Dictionary<Type, Type> _viewModelTypes;

        UIViewController _masterView;
        UIViewController _detailsView;

        public GeneralSplitView(UIViewController masterView, UIViewController detailsView = null, params Type[] supportedViewModelTypes)
        {
            //View.Bounds = new RectangleF(0,0,UIScreen.MainScreen.Bounds.Width,UIScreen.MainScreen.Bounds.Height);
            Delegate = new GeneralSplitViewDelegate();
            _detailsView = detailsView ?? new UIViewController();
            _masterView = masterView;
            _viewModelTypes = supportedViewModelTypes.ToDictionary(x => x);
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.ViewControllers = new UIViewController[] { _masterView, _detailsView };
        }
		
        public virtual void ShowDetail(UIViewController view)
        {
            _detailsView = view;
            this.ViewControllers = new UIViewController[] { _masterView, _detailsView };
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
            _masterView = view;
            this.ViewControllers = new UIViewController[] { _masterView, _detailsView };
        }
    }
}