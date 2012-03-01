using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.iOS.UI.Controls.Views;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MWC.Core.Mvvm.ViewModels;
using MWC.iOS.Interfaces;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace MWC.iOS.Screens.iPad.Sessions {
	public class SessionSpeakersMasterDetail
        : UIViewController
        , IViewModelAware
    {
		UINavigationBar navBar;
		SessionView sessionView;
		SpeakerView speakerView;

		int colWidth1 = 335;
		int colWidth2 = 433;
	
		public UIPopoverController Popover;

		public SessionSpeakersMasterDetail ()
             :base()
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

			navBar = new UINavigationBar(new RectangleF(0, 0, 768, 44));
			navBar.SetItems(new UINavigationItem[]{new UINavigationItem("Session & Speaker Info")},false);
			
			View.BackgroundColor = UIColor.LightGray;
			View.Frame = new RectangleF(0,0,768,768);

			sessionView = new SessionView();
			sessionView.Frame = new RectangleF(0,44,colWidth1,728);
			sessionView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			speakerView = new SpeakerView();
			speakerView.Frame = new RectangleF(colWidth1+1,44,colWidth2,728);
			speakerView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;

			View.AddSubview (speakerView);
			View.AddSubview (sessionView);
			View.AddSubview (navBar);
		}

        /*
		public void ShowView()
		{
			speakerId = speakerID;
			
			if (speakerId > 1) {
				sessionView.Update (speakerId, true);
				
				var session = BL.Managers.SessionManager.GetSession (speakerId);

				speakersInSession = session.Speakers;
				if (speakersInSession != null && speakersInSession.Count > 0) {
					speakerView.Update (speakersInSession[0].ID);
				} else {	// no speaker (!?)
					speakerView.Clear();
				}
			}
            else
            {
				sessionView.Clear();
				speakerView.Clear();
			}
			
			if (Popover != null) {
				Popover.Dismiss (true);
			}
		}
        */

        public void ShowView (Cirrious.MvvmCross.Touch.Interfaces.IMvxTouchView view)
        {
            if (Popover != null) {
               Popover.Dismiss (true);
            }

#warning This is very naughty code! Should really do better here - should display the view along with its controller - but this is just adapting the mwc sample for now
            var type = view.ShowRequest.ViewModelType;
            if (type == typeof(SessionDetailsViewModel))
            {
                sessionView.Update(HackLoadViewModel<SessionDetailsViewModel>(view));
                speakerView.Clear();
            }
            else if (type == typeof(SpeakerDetailsViewModel))
            {
                speakerView.Update(HackLoadViewModel<SpeakerDetailsViewModel>(view));
            }
        }

#warning This is very naughty code! Should really do better here - should display the view along with its controller - but this is just adapting the mwc sample for now
        private TViewModel HackLoadViewModel<TViewModel>(IMvxTouchView view)
            where TViewModel : class, IMvxViewModel
        {
            var typedView = view as IMvxTouchView<TViewModel>;
            var viewModel = typedView.GetService<IMvxViewModelLoader>().LoadViewModel(view.ShowRequest);
            return (TViewModel)viewModel;
        }

		protected void OnFavoriteChanged (NSNotification notification)
		{
			sessionView.UpdateFavorite();
		}

		NSObject ObserverFavoriteChanged;
		/// <summary>
		/// Keep favorite-stars in sync with changes made on other screens
		/// </summary>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			sessionView.UpdateFavorite ();

			ObserverFavoriteChanged = NSNotificationCenter.DefaultCenter.AddObserver(
					AppDelegate.NotificationFavoriteUpdated, OnFavoriteChanged);			
		}

		/// <summary>
		/// Keep favorite-stars in sync with changes made on other screens
		/// </summary>
		public override void ViewWillDisappear (bool animated)
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver(ObserverFavoriteChanged);
			sessionView.UpdateFavorite ();
			base.ViewWillDisappear (animated);
		}

		public void AddNavBarButton (UIBarButtonItem button)
		{
			button.Title = "Sessions";
			navBar.TopItem.SetLeftBarButtonItem (button, false);
		}
		
		public void RemoveNavBarButton ()
		{
			navBar.TopItem.SetLeftBarButtonItem (null, false);
		}

        #region IViewModelAware implementation

        public bool CanShow (Cirrious.MvvmCross.Touch.Interfaces.IMvxTouchView view)
        {
            var vmType = view.ShowRequest.ViewModelType;
            return (vmType == typeof(SpeakerDetailsViewModel) || vmType == typeof(SessionDetailsViewModel));
        }

        #endregion
	}
}