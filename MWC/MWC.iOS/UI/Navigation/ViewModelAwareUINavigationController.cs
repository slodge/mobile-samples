using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Interfaces;
using MWC.iOS.Interfaces;
using MonoTouch.UIKit;

namespace MWC.iOS.UI.Navigation
{
    public class ViewModelAwareUINavigationController 
        : UINavigationController, IViewModelAware
    {
        private readonly Dictionary<Type, bool> _viewModels = new Dictionary<Type, bool>();

        public ViewModelAwareUINavigationController()
            : base()
        {            
        }

        public void Add<TViewModel>()
            where TViewModel : IMvxViewModel
        {
            Add(typeof(TViewModel));
        }

        public void Add(Type viewModelType)
        {
            _viewModels[viewModelType] = true;
        }

        public bool CanShow(IMvxTouchView view)
        {
            return _viewModels.ContainsKey(view.ShowRequest.ViewModelType);
        }
    }
}
