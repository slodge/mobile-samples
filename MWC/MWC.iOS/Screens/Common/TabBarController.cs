using System.Collections.Generic;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Touch.Interfaces;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using MWC.Core.Mvvm.ViewModels;
using Cirrious.MvvmCross.Views;

namespace MWC.iOS.Screens.Common {
	public class TabBarController
        : MvxTouchTabBarViewController<MainViewModel>
        , IMWCTabBarPresenter
	{
		UIViewController homeScreen = null;
		
		UINavigationController homeNav, speakerNav, sessionNav;
        UIViewController speakersScreen, sessionsScreen, twitterFeedScreen, newsFeedScreen, exhibitorsScreen, favoritesScreen, mapScreen, aboutScreen;
		
		UISplitViewController speakersSplitView, sessionsSplitView, exhibitorsSplitView, twitterSplitView, newsSplitView;
		
		public TabBarController (MvxShowViewModelRequest request)
			: base(request)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// home tab
#warning IPad support removed (for now)			
			if (true /*AppDelegate.IsPhone*/) {
				homeScreen = this.CreateViewControllerFor<ScheduleViewModel>() as UIViewController;
				homeScreen.Title = "Schedule";
			} else {
				homeScreen = this.CreateViewControllerFor<ScheduleViewModel>() as UIViewController;
			}
			
			homeNav = new UINavigationController();
			homeNav.PushViewController ( homeScreen, false );			
			homeNav.Title = "Schedule";
			homeNav.TabBarItem = new UITabBarItem("Schedule"
										, UIImage.FromBundle("Images/Tabs/schedule.png"), 0);
			
			// speakers tab
			if (AppDelegate.IsPhone)
			{
			    speakersScreen = this.CreateViewControllerFor<SpeakerListViewModel>() as UIViewController;			
				speakerNav = new UINavigationController();
				speakerNav.TabBarItem = new UITabBarItem("Speakers"
											, UIImage.FromBundle("Images/Tabs/speakers.png"), 1);
				speakerNav.PushViewController ( speakersScreen, false );
			} else {
                //speakersSplitView = new MWC.iOS.Screens.iPad.Speakers.SpeakerSplitView();
                //speakersSplitView.TabBarItem = new UITabBarItem("Speakers"
                //                            , UIImage.FromBundle("Images/Tabs/speakers.png"), 1);
			}

			// sessions
			if (AppDelegate.IsPhone) {
                sessionsScreen = this.CreateViewControllerFor<SessionListViewModel>() as DialogViewController; ;
				sessionNav = new UINavigationController();
				sessionNav.TabBarItem = new UITabBarItem("Sessions"
											, UIImage.FromBundle("Images/Tabs/sessions.png"), 2);
				sessionNav.PushViewController ( sessionsScreen, false );
			} else {
                //sessionsSplitView = new MWC.iOS.Screens.iPad.Sessions.SessionSplitView();
                //sessionsSplitView.TabBarItem = new UITabBarItem("Sessions"
                //                            , UIImage.FromBundle("Images/Tabs/sessions.png"), 2);		
			}
			// maps tab
            mapScreen = this.CreateViewControllerFor<MapsViewModel>() as UIViewController; 
			mapScreen.TabBarItem = new UITabBarItem("Map"
										, UIImage.FromBundle("Images/Tabs/maps.png"), 3);
			
			if (AppDelegate.IsPhone) {
				// exhibitors
                exhibitorsScreen = this.CreateViewControllerFor<ExhibitorsListViewModel>() as UIViewController;
				exhibitorsScreen.TabBarItem = new UITabBarItem("Exhibitors"
											, UIImage.FromBundle("Images/Tabs/exhibitors.png"), 4);
				
				// twitter feed
                twitterFeedScreen = this.CreateViewControllerFor<TwitterViewModel>() as UIViewController; 
				twitterFeedScreen.TabBarItem = new UITabBarItem("Twitter"
											, UIImage.FromBundle("Images/Tabs/twitter.png"), 5);
				
				// news
                newsFeedScreen = this.CreateViewControllerFor<NewsListViewModel>() as UIViewController; ;
				newsFeedScreen.TabBarItem =  new UITabBarItem("News"
											, UIImage.FromBundle("Images/Tabs/rss.png"), 6);
			} else {
				// iPad
				// exhibitors
				
                //exhibitorsSplitView = new Screens.iPad.Exhibitors.ExhibitorSplitView();
                //exhibitorsSplitView.TabBarItem = new UITabBarItem("Exhibitors"
                //                            , UIImage.FromBundle("Images/Tabs/exhibitors.png"), 4);
				
                //// twitter feed
                //twitterSplitView = new Screens.iPad.Twitter.TwitterSplitView();
                //twitterSplitView.TabBarItem = new UITabBarItem("Twitter"
                //                            , UIImage.FromBundle("Images/Tabs/twitter.png"), 5);

                //// news
                //newsSplitView = new MWC.iOS.Screens.iPad.News.NewsSplitView();
                //newsSplitView.TabBarItem =  new UITabBarItem("News"
                //                            , UIImage.FromBundle("Images/Tabs/rss.png"), 6);
			}
		
			// favorites (only required on iPhone)
			if (AppDelegate.IsPhone) {
                favoritesScreen = this.CreateViewControllerFor<SessionListViewModel>(new { listKey = SessionListViewModel.FavoritesKey() }) as UIViewController;
				favoritesScreen.TabBarItem =  new UITabBarItem("Favorites"
											, UIImage.FromBundle("Images/Tabs/favorites.png"), 6);
			}
			// about tab
            aboutScreen = this.CreateViewControllerFor<AboutXamarinViewModel>() as UIViewController;
			aboutScreen.TabBarItem = new UITabBarItem("About Xamarin"
										, UIImage.FromBundle("Images/Tabs/about.png"), 8);
			
			UIViewController[] viewControllers;
			// create our array of controllers
			if (AppDelegate.IsPhone) {
				viewControllers = new UIViewController[] {
					homeNav,
					speakerNav,
					sessionNav,
					mapScreen,
					exhibitorsScreen,
					twitterFeedScreen,
					newsFeedScreen,
					favoritesScreen,
					aboutScreen
				};
			} else {	// IsPad
				viewControllers = new UIViewController[] {
					homeNav,
					speakersSplitView,
					sessionsSplitView,
					mapScreen,
					exhibitorsSplitView,
					twitterSplitView,
					newsSplitView,	
					// NOTE: removed Favorites
					aboutScreen
				};
			}
			
			// attach the view controllers
			ViewControllers = viewControllers;
			
			// tell the tab bar which controllers are allowed to customize. 
			// if we don't set  it assumes all controllers are customizable. 
			// if we set to empty array, NO controllers are customizable.
			CustomizableViewControllers = new UIViewController[] {};
			
			// set our selected item
			SelectedViewController = homeNav;
		}
		
        /*
		public void ShowSessionDay(int day)
		{
			// WARNING: ORDER IS IMPORTANT, call ShowDay() before setting index (which causes ViewWillAppear)
			var sv = sessionsSplitView as MWC.iOS.Screens.iPad.Sessions.SessionSplitView;
			sv.ShowDay (day);
			SelectedIndex = 2; // Sessions
		}
        */

		/// <summary>
		/// Only allow iPad application to rotate, iPhone is always portrait
		/// </summary>
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
			if (AppDelegate.IsPad)
	            return true;
			else
				return toInterfaceOrientation == UIInterfaceOrientation.Portrait;
        }

	    public bool GoBack()
	    {
	        var subNavigation = this.SelectedViewController as UINavigationController;
            if (subNavigation == null)
	            return false;

            if (subNavigation.ViewControllers.Length <= 1)
                return false;

	        subNavigation.PopViewControllerAnimated(true);
	        return true;
	    }

	    public bool ShowView(IMvxTouchView view)
	    {
            if (view is SpeakerDetailsViewModel || view is SpeakerListViewModel)
            {
                speakerNav.PushViewController((UIViewController) view, true);
                return true;
            }

	        return false;
        }
	}
}