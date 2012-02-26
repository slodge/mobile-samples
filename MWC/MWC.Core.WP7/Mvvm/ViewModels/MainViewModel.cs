using System;
using System.IO;
using System.Threading;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using MWC.BL.Managers;

namespace MWC.Core.Mvvm.ViewModels
{
    public class MainViewModel 
        : ViewModelBase
        , IMvxServiceConsumer<IMvxResourceLoader>
    {
        public MainViewModel()
        {
            News = new NewsListViewModel ();
            Twitter = new TwitterViewModel ();

            Monday = new DateTimeWithCommand(this, new DateTime (2012, 2, 27));
            Tuesday = new DateTimeWithCommand(this, new DateTime (2012, 2, 28));
            Wednesday = new DateTimeWithCommand(this, new DateTime (2012, 2, 29));
            Thursday = new DateTimeWithCommand(this, new DateTime(2012, 3, 1));

            BeginUpdate();
        }

        public NewsListViewModel News { get; private set; }
        public TwitterViewModel Twitter { get; private set; }

        public class DateTimeWithCommand
        {
            private readonly MainViewModel _parent;

            public DateTimeWithCommand(MainViewModel parent, DateTime when)
            {
                _parent = parent;
                When = when;
            }

            public DateTime When { get; private set; }
            public IMvxCommand Command
            {
                get
                {
                    return new MvxRelayCommand(() => _parent.RequestNavigate<SessionListViewModel>(new { listKey = SessionListViewModel.DayOfWeekKey(When)}));
                }
            }
        }

        public DateTimeWithCommand Monday { get; private set; }
        public DateTimeWithCommand Tuesday { get; private set; }
        public DateTimeWithCommand Wednesday { get; private set; }
        public DateTimeWithCommand Thursday { get; private set; }
        public IMvxCommand FavoriteSessionsCommand
        {
            get
            {
                return new MvxRelayCommand(() => RequestNavigate<SessionListViewModel>(new { listKey = SessionListViewModel.FavoritesKey() }));
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

        public void BeginUpdate ()
        {
            UpdateDatabase();
            News.BeginUpdate ();
            Twitter.BeginUpdate ();
        }

        void UpdateDatabase()
        {
            //
            // Seed the Conference DB
            //
            var isDataSeeded = ApplicationSettings.IsDataSeeded;
            if (!isDataSeeded)
            {
                var text = this.GetService<IMvxResourceLoader>().GetTextResource("Assets\\SeedData.xml");
                UpdateManager.UpdateFromFile(text);
                ApplicationSettings.IsDataSeeded = true;
            }

            //
            // Update the Speakers & Session DB if it's time for an update
            //
            if (DateTime.UtcNow >= ApplicationSettings.NextConferenceUpdateTimeUtc)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    UpdateManager.UpdateFinished += delegate
                    {
                        InvokeOnMainThread(delegate
                        {
                            ApplicationSettings.NextConferenceUpdateTimeUtc = DateTime.UtcNow.AddHours(1);
                        });
                    };
                    UpdateManager.UpdateConference();
                });
            }
        }
    }
}
