using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Binding.Android;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using MWC.Android.Screens;
using MWC.Bindings;
using MWC.Core.Mvvm;
using MWC.Core.Mvvm.Converters;
using MWC.Core.Mvvm.ViewModels;
using MWC.Views.Home;

namespace MWC
{
    public class Setup
        : MvxBaseAndroidBindingSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override MvxApplication CreateApp()
        {
            return new App();
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            var views = from type in this.GetType().Assembly.GetTypes()
                        where type.Name.EndsWith("View")
                        where type.Namespace != null
                        where type.Namespace.Contains("Views")
                        where !type.Name.StartsWith("Base")
                        let viewModelPropertyInfo = type.GetProperty("ViewModel")
                        where viewModelPropertyInfo != null
                        let viewModelType = viewModelPropertyInfo.PropertyType
                        select new { type, viewModelType };

            return views.ToDictionary(x => x.viewModelType, x => x.type);
            // return new Dictionary<Type, Type>()
            //           {
            //               {typeof(MainViewModel),typeof(MainView)},
            //               {typeof(ScheduleViewModel),typeof(ScheduleView)},
            //           };
        }

        public override string ExecutableNamespace
        {
            get { return "MWC"; }
        }

        public override Assembly ExecutableAssembly
        {
            get { return GetType().Assembly; }
        }

        protected override void FillTargetFactories(Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterFactory(new MvxCustomBindingFactory<Button>("IsFavorite", (button) => new MvxFavoritesButtonBinding(button)));
        }

        protected override void FillValueConverters(Cirrious.MvvmCross.Binding.Interfaces.Binders.IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            var filler = new MvxInstanceBasedValueConverterRegistryFiller(registry);
            filler.AddFieldConverters(typeof(AllConverters));
        }
    }
}