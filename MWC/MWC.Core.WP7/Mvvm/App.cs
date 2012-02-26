using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using MWC.Core.Mvvm.ApplicationObjects;
using MWC.Core.Mvvm.ApplicationSettings;
using MWC.Core.Mvvm.JsonFileStore;
using MWC.Core.Mvvm.ViewModelLocators;

namespace MWC.Core.Mvvm
{
    public class App
        : MvxApplication
        , IMvxServiceProducer<IMvxStartNavigation>
        , IMvxServiceProducer<IJsonStoreHelper>
        , IMvxServiceProducer<IApplicationSettings>
    {
        public App()
        {
            Title = "MWC goes MvvmCross";

            this.RegisterServiceType<IJsonStoreHelper, JsonStoreHelper>();

            this.RegisterServiceInstance<IApplicationSettings>(new ApplicationSettings.ApplicationSettings());

            AddLocator(new SingletonViewModelLocator());

            var startApplicationObject = new StartApplicationObject();
            this.RegisterServiceInstance<IMvxStartNavigation>(startApplicationObject);
        }
    }
}
