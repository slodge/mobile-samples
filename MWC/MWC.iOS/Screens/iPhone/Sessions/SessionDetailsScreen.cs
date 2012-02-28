using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;
using MWC.iOS.UI.Controls.Views;

namespace MWC.iOS.Screens.iPhone.Sessions {
	public class SessionDetailsScreen
        : MvxBindingTouchViewController<SessionDetailsViewModel>
    {
		UIScrollView scrollView;
		SessionView sessionView;
		
		public bool ShouldShowSpeakers { get; set; }

		public SessionDetailsScreen (MvxShowViewModelRequest request)
            : base(request)
		{
			ShouldShowSpeakers = true;	// by default
			
			sessionView = new SessionView();
			sessionView.Frame = new RectangleF(0,0,320,100);
			sessionView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			scrollView = new UIScrollView();
			scrollView.Frame = new RectangleF(0,0,320,370);
			Add (scrollView);
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			sessionView.Update (ViewModel, ShouldShowSpeakers);

			scrollView.Add(sessionView);
			scrollView.ContentOffset = new PointF(0,0);
			scrollView.ContentSize = sessionView.Bounds.Size.Height < 370 ? new SizeF(320,370) : sessionView.Bounds.Size;
		}
	}
}