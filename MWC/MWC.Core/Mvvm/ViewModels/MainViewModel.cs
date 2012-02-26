using System;
using System.IO;
using System.Threading;
using Cirrious.MvvmCross.ExtensionMethods;
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
            Schedule = new ScheduleViewModel ();
            More = new MoreViewModel();

            BeginUpdate();
        }

        public ScheduleViewModel Schedule { get; private set; }
        public NewsListViewModel News { get; private set; }
        public TwitterViewModel Twitter { get; private set; }
        public MoreViewModel More { get; private set; }
        

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
                var text = this.GetService<IMvxResourceLoader>().GetTextResource("Assets/SeedData.xml");
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
