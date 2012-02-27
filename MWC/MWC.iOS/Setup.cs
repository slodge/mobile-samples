using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Services;
using Cirrious.MvvmCross.Binding.Binders;
using MWC.Core.Mvvm;
using MWC.Core.Mvvm.Converters;
using MWC.Core.Mvvm.ViewModels;
using MWC.iOS.Screens.Common;
using MWC.iOS.Screens.iPhone.Home;
using MWC.iOS.Screens.iPhone.Sessions;

namespace MWC.iOS
{
   public class Setup
        : MvxTouchDialogBindingSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        #region Overrides of MvxBaseSetup
		
		
        protected override MvxApplication CreateApp()
        {
            var app = new App();
            return app;
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
                            { typeof(SessionListItemViewModel), typeof(SessionsScreen)},
                            { typeof(SessionDetailsViewModel), typeof(SessionDetailsScreen)},
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

