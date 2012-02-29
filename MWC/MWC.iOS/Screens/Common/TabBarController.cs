using System;
using System.Linq;
using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.ExtensionMethods;
using Cirrious.MvvmCross.Touch.Interfaces;
using MWC.iOS.Interfaces;
using MWC.iOS.UI.Navigation;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views;
using MWC.Core.Mvvm.ViewModels;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using MWC.iOS.Screens.iPhone.Speakers;
using MWC.iOS.Screens.iPhone.Sessions;
using MWC.iOS.Screens.iPhone.Twitter;

namespace MWC.iOS.Screens.Common {
	public class TabBarController
        : MvxTouchTabBarViewController<MainViewModel>
        , IMWCTabBarPresenter
		, IMvxServiceConsumer<IMWCTabBarPresenterHost>
	{
        private Dictionary<Type, UINavigationController> _defaultDisplays = new Dictionary<Type, UINavigationController>();

		public TabBarController (MvxShowViewModelRequest request)
			: base(request)
		{
			this.GetService<IMWCTabBarPresenterHost>().TabBarPresenter = this;
		}

	    private int _createdSoFarCount = 0;
        private UINavigationController CreateNavigationControllerTabFor<TPrimaryType>(string title, string imageName, object creationParameters = null, params Type[] alsoSupports)
            where TPrimaryType : class, IMvxViewModel
        {
            var controller = new ViewModelAwareUINavigationController();

            if (!_defaultDisplays.ContainsKey(typeof(TPrimaryType)))
                _defaultDisplays[typeof(TPrimaryType)] = controller;

            if (alsoSupports != null)
                foreach (var viewModelType in alsoSupports)
                    controller.Add(viewModelType);

		    var screen = this.CreateViewControllerFor<TPrimaryType>(creationParameters) as UIViewController;
            screen.Title = title;
            screen.TabBarItem = new UITabBarItem(title, UIImage.FromBundle("Images/Tabs/" + imageName +".png"), _createdSoFarCount);
			controller.PushViewController(screen, false);

            _createdSoFarCount++;

            return controller;
        }

        private UIViewController CreateTabFor<TPrimaryType>(string title, string imageName, object creationParameters = null, params Type[] alsoSupports)
            where TPrimaryType : class, IMvxViewModel
        {
            var controller = new ViewModelAwareUINavigationController();

            if (!_defaultDisplays.ContainsKey(typeof(TPrimaryType)))
                _defaultDisplays[typeof(TPrimaryType)] = controller;

            if (alsoSupports != null)
                foreach (var viewModelType in alsoSupports)
                    controller.Add(viewModelType);

            var screen = this.CreateViewControllerFor<TPrimaryType>(creationParameters) as UIViewController;
            screen.Title = title;
            screen.TabBarItem = new UITabBarItem(title, UIImage.FromBundle("Images/Tabs/" + imageName + ".png"), _createdSoFarCount);
            controller.PushViewController(screen, false);

            _createdSoFarCount++;

            return controller;
        }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var viewControllers = new UIViewController[]
                                  {
                                    CreateTabFor<ScheduleViewModel>("Schedule", "schedule"),
                                    CreateTabFor<SpeakerListViewModel>("Speakers", "speakers", null, typeof(SpeakerDetailsViewModel), typeof(SessionDetailsViewModel)),
                                    CreateTabFor<SessionListViewModel>("Sessions", "sessions", null, typeof(SessionListViewModel), typeof(SpeakerDetailsViewModel), typeof(SessionDetailsViewModel)),
                                    CreateTabFor<MapsViewModel>("Map", "maps"),
                                    CreateTabFor<ExhibitorsListViewModel>("Exhibitors", "exhibitors", null, typeof(ExhibitorDetailsViewModel)),
                                    CreateTabFor<TwitterViewModel>("Twitter", "twitter", null, typeof(TweetViewModel)),
                                    CreateTabFor<MapsViewModel>("Map", "maps"),
                                    CreateTabFor<NewsListViewModel>("News", "rss", null, typeof(NewsItemViewModel)),
                                    CreateTabFor<SessionListViewModel>("Favorites", "favorites", new { listKey = SessionListViewModel.FavoritesKey() }, typeof(SessionDetailsViewModel), typeof(SpeakerDetailsViewModel)),
                                    CreateTabFor<AboutXamarinViewModel>("About Xamarin", "about"),
                                  };
			
			ViewControllers = viewControllers;

            // tell the tab bar which controllers are allowed to customize. 
            // if we don't set  it assumes all controllers are customizable. 
            // if we set to empty array, NO controllers are customizable.
            CustomizableViewControllers = new UIViewController[] { };

            // set our selected item
            SelectedViewController = ViewControllers[0];

            /*
            if (AppDelegate.IsPhone)
			{
                var speakersScreen = this.CreateViewControllerFor<SpeakerListViewModel>() as UIViewController;
                var speakerNav = CreateNavigationControllerFor(typeof(SpeakerListViewModel), typeof(SpeakerDetailsViewModel), typeof(SessionDetailsViewModel));
				speakerNav.TabBarItem = new UITabBarItem("Speakers", UIImage.FromBundle("Images/Tabs/speakers.png"), 1);
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
				twitterNav = new UINavigationController();
				twitterNav.TabBarItem = new UITabBarItem("Twitter"
											, UIImage.FromBundle("Images/Tabs/twitter.png"), 5);
				twitterNav.PushViewController ( twitterFeedScreen, false );						
				
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
            */
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
	        var currentNav = this.SelectedViewController as ViewModelAwareUINavigationController;
            if (currentNav != null)
            {
                if (currentNav.CanShow(view))
                {
                    var navigationToUse = IsInMoreController(currentNav) ? MoreNavigationController : currentNav;
                    navigationToUse.PushViewController((UIViewController)view, true);
					return true;
            	}
			}


	        UINavigationController defaultNavigation;
            if (_defaultDisplays.TryGetValue(view.ShowRequest.ViewModelType, out defaultNavigation))
            {
                var navigationToUse = IsInMoreController(defaultNavigation) ? MoreNavigationController : defaultNavigation;
                this.SelectedViewController = defaultNavigation;
                navigationToUse.PushViewController((UIViewController)view, true);
                return true;
            }

	        return false;
        }

	    private bool IsInMoreController(UIViewController toTest)
	    {
#warning there has to be a better way than this?
            return Array.IndexOf(ViewControllers, toTest) > 3;
	    }
	}
}