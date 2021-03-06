using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.Common.About {
	/// <summary>
	/// This screen REPLACES the old XIB version
	/// </summary>
	public class AboutXamarinScreen : MvxBindingTouchViewController<AboutXamarinViewModel>
	{
		protected string basedir;
		UIWebView webView;

		public AboutXamarinScreen (MvxShowViewModelRequest request)
            : base (request)
		{
			Title = "About Xamarin";
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			webView = new UIWebView();

			webView.ShouldStartLoad = 
			delegate (UIWebView webViewParam, NSUrlRequest request, UIWebViewNavigationType navigationType) {
				// view links in a new 'webbrowser' window like about, session & twitter
				if (navigationType == UIWebViewNavigationType.LinkClicked) {
					UIApplication.SharedApplication.OpenUrl (request.Url);
					return false;
				}
				return true;
			};

			Add (webView);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			webView.Frame = new RectangleF (0, 0, View.Bounds.Width, View.Bounds.Height);
			webView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			
			NSUrl url = null;
			
			if(AppDelegate.IsPad)
				url = NSUrl.FromFilename("Images/About/iPad/index.html");
			else
				url = NSUrl.FromFilename("Images/About/iPhone/index.html");
			var request = new NSUrlRequest(url);
			webView.LoadRequest(request);
		}
	}
}