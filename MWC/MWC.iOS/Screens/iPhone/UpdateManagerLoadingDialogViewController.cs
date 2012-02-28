using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Dialog;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.Screens.iPhone {

    /// <summary>
    /// Base class for loading screens: Home, Speakers, Sessions
    /// </summary>
    /// <remarks>
    /// This ViewController implements the data loading via a virtual
    /// method LoadData(), which must call StopLoadingScreen()
    /// </remarks>
    public partial class UpdateManagerLoadingDialogViewController<TViewModel> 
        : MvxTouchDialogViewController<TViewModel>
        where TViewModel : UpdatingViewModelBase
    {
        UI.Controls.LoadingOverlay loadingOverlay;

        /// <summary>
        /// Set pushing=true so that the UINavCtrl 'back' button is enabled
        /// </summary>
        public UpdateManagerLoadingDialogViewController(MvxShowViewModelRequest request)
            : base(request, UITableViewStyle.Plain, null, true)
        {
        }


        public override void ViewDidLoad()
        {
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            RefreshLoadingVisibility();
        }

        public override void ViewDidUnload()
        {
            ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            base.ViewDidUnload();
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            RefreshLoadingVisibility();
        }

        private void RefreshLoadingVisibility()
        {
            if (ViewModel.IsUpdating)
                StartLoadingScreen();
            else
                StopLoadingScreen();
        }

		private void StartLoadingScreen()
		{
            if (loadingOverlay == null)
            {
                var bounds = new RectangleF(0, 0, 768, 1004);
                if (InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft
                || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight)
                {
                    bounds = new RectangleF(0, 0, 1024, 748);
                }

                loadingOverlay = new MWC.iOS.UI.Controls.LoadingOverlay(bounds);
                // because DialogViewController is a UITableViewController,
                // we need to step OVER the UITableView, otherwise the loadingOverlay
                // sits *in* the scrolling area of the table
                View.Superview.Add(loadingOverlay);
                View.Superview.BringSubviewToFront(loadingOverlay);
            }
            Console.WriteLine("Waiting for updates to finish before displaying table.");		    
		}

        private void StopLoadingScreen()
        {
            if (loadingOverlay != null)
            {
                loadingOverlay.Hide();
                loadingOverlay.RemoveFromSuperview();
                loadingOverlay = null;
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }


#warning AlwaysRefresh might actually be useful?
        /// <summary>
        /// Whether the table will be reloaded on ViewWillAppear.
        /// </summary>
        protected bool AlwaysRefresh { get; set; }
    }
}