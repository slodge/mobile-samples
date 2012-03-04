using System;
using System.Drawing;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Utilities;
using Cirrious.MvvmCross.Views;
using MWC.Core.Mvvm.Converters;
using MWC.Core.Mvvm.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.iPhone.Twitter {
	/// <summary>
	/// Displays tweet: name, icon, tweet text
	/// </summary>
    public class TweetDetailsScreen : MvxBindingTouchViewController<TweetViewModel>
        , IImageUpdated
    {
		UILabel date, user;
		UnderlineLabel handle;
		UIButton handleButton;
		UIImageView image;
		UIWebView webView;
		EmptyOverlay emptyOverlay;
		
		public TweetDetailsScreen (MvxShowViewModelRequest request) 
            : base(request)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

                        View.BackgroundColor = UIColor.White;

         user = new UILabel () {
             TextAlignment = UITextAlignment.Left,
             Font = UIFont.FromName("Helvetica-Light",AppDelegate.Font16pt),
             BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
         };
         handle = new UnderlineLabel () {
             TextAlignment = UITextAlignment.Left,
             Font = UIFont.FromName("Helvetica-Light",AppDelegate.Font9pt),
             TextColor = AppDelegate.ColorTextLink,
             
             BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
         };
         handleButton = UIButton.FromType (UIButtonType.Custom);
         handleButton.TouchUpInside += (sender, e) => {
                var url = new NSUrl(ViewModel.Url);
             var urlRequest = new NSUrlRequest(url);
             if (AppDelegate.IsPhone)
                 NavigationController.PushViewController (new WebViewController (urlRequest), true);
             else
                 PresentModalViewController (new WebViewController(urlRequest), true);
         };
         date = new UILabel () {
             TextAlignment = UITextAlignment.Left,
             Font = UIFont.FromName("Helvetica-Light",AppDelegate.Font9pt),
             TextColor = UIColor.DarkGray,
             BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
         };

         image = new UIImageView();
         
         webView = new UIWebView();
         webView.Delegate = new WebViewDelegate(this);
         try { // iOS5 only
             webView.ScrollView.ScrollEnabled = false; 
             webView.ScrollView.Bounces = false;
         } catch {}

         View.AddSubview (user);
         View.AddSubview (handle);
         View.AddSubview (handleButton);
         View.AddSubview (image);
         View.AddSubview (date);
         View.AddSubview (webView);
         


            LayoutSubviews();
            Update();
        }

		public void Update()
		{
		    var converters = new AllConverters();
            handle.Text = ViewModel.Username;
            user.Text = ViewModel.RealName;
            date.Text = ViewModel.PublishedAgo;

            var u = new Uri(this.ViewModel.ImageUrl);
			var img = ImageLoader.DefaultRequestImage(u,this);
			if(img != null)
				image.Image = MWC.iOS.UI.CustomElements.TweetCell.RemoveSharpEdges (img);

			var css = "<style>" +
				"body {background-color:#ffffff; }" +
				"body,b,i,p,h2 {font-family:Helvetica-Light;}" +
				"</style>";

            webView.LoadHtmlString(css + ViewModel.Content, new NSUrl(NSBundle.MainBundle.BundlePath, true));
		}
		
		public void UpdatedImage (Uri uri)
		{
			Console.WriteLine ("UPDATED:" + uri.AbsoluteUri);
			var img = ImageLoader.DefaultRequestImage (uri, this);
			if (img != null)
				image.Image = MWC.iOS.UI.CustomElements.TweetCell.RemoveSharpEdges (img);
		}

		void LayoutSubviews ()
		{
            if (EmptyOverlay.ShowIfRequired(ref emptyOverlay, ViewModel, this.View
						, "No tweet selected", EmptyOverlayType.Twitter)) 
				return;
			
			image.Frame   = new RectangleF(8,   8,  48, 48);
			user.Frame    = new RectangleF(69, 14, 239, 24);
			handle.Frame  = new RectangleF(69, 39, 239, 20); //14
			handleButton.Frame = new RectangleF (69, 14, 239, 40); // over the two display fields
			date.Frame    = new RectangleF(69, 55, 80,  15); 
			webView.Frame = new RectangleF(0,  75, 320, 440 - 75);
		}
		
		class WebViewDelegate : UIWebViewDelegate {
			private TweetDetailsScreen tweetScreen;
			public WebViewDelegate (TweetDetailsScreen tds)
			{
				tweetScreen = tds;
			}
		
			/// <summary>
			/// Allow links inside tweets to be viewed in a UIWebView
			/// </summary>
			public override bool ShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
			{
				if (navigationType == UIWebViewNavigationType.LinkClicked) {
					if (AppDelegate.IsPhone)
						tweetScreen.NavigationController.PushViewController (new WebViewController (request), true);
					else
						tweetScreen.PresentModalViewController (new WebViewController(request), true);
					return false;
				}
				return true;
			}
		}
	}
}