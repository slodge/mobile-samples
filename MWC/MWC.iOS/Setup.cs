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
            return new Dictionary<Type, Type>()
                       {
                            { typeof(MainViewModel), typeof(TabBarController)},
                            //{ typeof(SimpleTextPropertyViewModel), typeof(SimpleTextPropertyView)},
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

