using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using MWC.Core.Mvvm.ViewModels;
using MWC.WP7.Views;
using Microsoft.Phone.Controls;

namespace MWC.WP7
{
    public class Setup
        : MvxBaseWindowsPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame)
            : base(rootFrame)
        {
        }

        protected override MvxApplication CreateApp()
        {
            var app = new MWC.Core.Mvvm.App();
            return app;
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            var views = from type in this.GetType().Assembly.GetTypes()
                        where type.Namespace != null
                        where type.Namespace.EndsWith("Views")
                        where !type.Name.StartsWith("Base")
                        let viewModelPropertyInfo = type.GetProperty("ViewModel")
                        where viewModelPropertyInfo != null
                        let viewModelType = viewModelPropertyInfo.PropertyType
                        select new {type,viewModelType};

            return views.ToDictionary(x => x.viewModelType, x => x.type);
            /*
            return new Dictionary<Type, Type>()
                       {
                            { typeof(MainViewModel), typeof(MainView)},
                            { typeof(SessionListViewModel), typeof(SessionListView)},
                            { typeof(SessionDetailsViewModel), typeof(SessionDetailsView)},
                            { typeof(SpeakerDetailsViewModel), typeof(SpeakerDetailsView)},
                            { typeof(TweetViewModel), typeof(TweetDetailsView)},
                            { typeof(AboutXamarinViewModel), typeof(AboutXamarinView)},
                       };
             */
        }
    }
}
