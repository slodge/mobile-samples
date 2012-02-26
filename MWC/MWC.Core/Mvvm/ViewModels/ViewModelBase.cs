using System.Collections.Generic;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Services.Tasks;
using Cirrious.MvvmCross.ViewModels;
using MWC.Core.Mvvm.ApplicationSettings;

namespace MWC.Core.Mvvm.ViewModels
{
    public abstract class ViewModelBase 
        : MvxViewModel
        , IMvxServiceConsumer<IApplicationSettings>
        , IMvxServiceConsumer<IMvxWebBrowserTask>
    {
        private IApplicationSettings _applicationSettings;
        protected IApplicationSettings ApplicationSettings
        {
            get
            {
                if (_applicationSettings == null)
                    _applicationSettings = this.GetService<IApplicationSettings>();
                return _applicationSettings;
            }
        }

        public IMvxCommand ShowWebPageCommand
        {
            get { return new MvxRelayCommand<string>(ShowWebPage); }
        }

        protected void ShowWebPage(string url)
        {
            this.GetService<IMvxWebBrowserTask>().ShowWebPage(url);
        }

        protected string CleanupPlainTextDocument (string doc)
        {
            if (string.IsNullOrWhiteSpace (doc)) {
                return "";
            }

            //
            // We want to make sure that there are no more than 1
            // blank continuous lines
            //
            var oldLines = doc.Split ('\n');
            var newLines = new List<string> ();

            var prevWasBlank = true;
            foreach (var line in oldLines) {
                var blank = string.IsNullOrWhiteSpace (line);

                if (!blank || !prevWasBlank) {
                    newLines.Add (line.TrimEnd ());
                }

                prevWasBlank = blank;
            }

            return string.Join ("\r\n", newLines);
        }

        #region Common Commands

        public IMvxCommand ShowTopPageCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<MainViewModel>(true)); }
        }

        public IMvxCommand FavoriteSessionsCommand
        {
            get
            {
                return new MvxRelayCommand(() => RequestNavigate<SessionListViewModel>(new { listKey = SessionListViewModel.FavoritesKey() }));
            }
        }

        public IMvxCommand ShowMapCommand
        {
            get
            {
                return new MvxRelayCommand(() => RequestNavigate<MapsViewModel>());
            }
        }

        public IMvxCommand AllSessionsCommand
        {
            get
            {
                return new MvxRelayCommand(() => RequestNavigate<SessionListViewModel>(new { listKey = SessionListViewModel.AllKey() }));
            }
        }

        public IMvxCommand ShowSpeakersCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<SpeakerListViewModel>()); }
        }

        public IMvxCommand ShowExhibitorsCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<ExhibitorsListViewModel>()); }
        }

        public IMvxCommand ShowAboutXamarinCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<AboutXamarinViewModel>()); }
        }

        public IMvxCommand ShowNewsCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<NewsListViewModel>()); }
        }

        public IMvxCommand ShowTwitterCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<TwitterViewModel>()); }
        }

        #endregion
    }
}
