using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Touch.Views;
using MWC.Core.Mvvm;
using MWC.Core.Mvvm.Converters;
using MWC.Core.Mvvm.ViewModels;
using MWC.iOS.Interfaces;
using MWC.iOS.Screens.Common;
using MWC.iOS.Screens.Common.About;
using MWC.iOS.Screens.Common.Map;
using MWC.iOS.Screens.iPhone.Exhibitors;
using MWC.iOS.Screens.iPhone.Home;
using MWC.iOS.Screens.iPhone.Sessions;
using MWC.iOS.Screens.iPhone.Twitter;
using MWC.iOS.Screens.iPhone.Speakers;
using MWC.iOS.Screens.Common.News;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ExtensionMethods;

namespace MWC.iOS
{
   public class Setup
        : MvxTouchDialogBindingSetup
		, IMvxServiceProducer<IMWCTabBarPresenterHost>
    {
		private readonly IMWCTabBarPresenterHost _presenter;
		
        public Setup(MvxApplicationDelegate applicationDelegate, MWCPresenter presenter)
            : base(applicationDelegate, presenter)
        {
			_presenter = presenter;
        }

        #region Overrides of MvxBaseSetup
				
        protected override MvxApplication CreateApp()
        {
            var app = new App();
            return app;
        }
		
		protected override void InitializeLastChance ()
		{
			this.RegisterServiceInstance<IMWCTabBarPresenterHost>(_presenter);
			base.InitializeLastChance ();
		}

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            //var views = from type in this.GetType().Assembly.GetTypes()
            //            where type.Namespace != null
            //            where type.Namespace.Contains(".Screens")
            //            where type.Name.EndsWith("Screen")
            //            where !type.Name.StartsWith("Update")
            //            let viewModelPropertyInfo = type.GetProperty("ViewModel")
            //            where viewModelPropertyInfo != null
            //            let viewModelType = viewModelPropertyInfo.PropertyType
            //            select new { type, viewModelType };

            //var dictionary = views.ToDictionary(x => x.viewModelType, x => x.type);

            return new Dictionary<Type, Type>()
                       {
                            { typeof(MainViewModel), typeof(TabBarController)},
                            { typeof(ScheduleViewModel), typeof(HomeScreen)},
                            { typeof(SessionListViewModel), typeof(SessionsScreen)},
                            { typeof(SessionDetailsViewModel), typeof(SessionDetailsScreen)},
                            { typeof(AboutXamarinViewModel), typeof(AboutXamarinScreen)},
                            { typeof(MapsViewModel), typeof(MapScreen)},
                            { typeof(ExhibitorDetailsViewModel), typeof(ExhibitorDetailsScreen)},
                            { typeof(ExhibitorsListViewModel), typeof(ExhibitorsScreen)},
                            { typeof(SpeakerListViewModel), typeof(SpeakersScreen)},
                            { typeof(SpeakerDetailsViewModel), typeof(SpeakerDetailsScreen)},
                            { typeof(TweetViewModel), typeof(TweetDetailsScreen)},
                            { typeof(TwitterViewModel), typeof(TwitterScreen)},
                            { typeof(NewsListViewModel), typeof(NewsScreen)},
                            { typeof(NewsItemViewModel), typeof(NewsDetailsScreen)},
                            //{ typeof(PullToRefreshViewModel), typeof(PullToRefreshView)},
                       };
        }
		
		protected override void FillValueConverters(Cirrious.MvvmCross.Binding.Interfaces.Binders.IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            var filler = new MvxInstanceBasedValueConverterRegistryFiller(registry);
            filler.AddFieldConverters(typeof(AllConverters));
        }

        #endregion
    }

}

