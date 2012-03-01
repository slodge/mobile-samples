using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;
using MWC.BL;
using MWC.BL.Managers;
using Cirrious.MvvmCross.Platform.Diagnostics;
using System;

namespace MWC.Core.Mvvm.ViewModels
{
    public class SpeakerDetailsViewModel : ViewModelBase
    {
        public SpeakerDetailsViewModel(string id=null, string key=null, string summaryOnly=null)
        {
            var speaker = default(Speaker);

            if (id != null)
            {
                MvxTrace.Trace("Speaker loading... " + id);
                int parsed;
                if (!int.TryParse(id, out parsed))
                {
                    throw new FormatException("Cound not parse to int value '" + id + "'");
                }
                speaker = SpeakerManager.GetSpeaker(parsed);
            }
            else if (key != null)
            {
                speaker = SpeakerManager.GetSpeakerWithKey(key);
            }

            var isSummaryOnly = !string.IsNullOrEmpty(summaryOnly);

            if (speaker != null)
            {
                Update(speaker, isSummaryOnly);
            }
        }

        public int ID { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string ImageUrl { get; set; }
        public string Bio { get; set; }

        public List<SessionDetailsViewModel> Sessions { get; set; }

        public void Update (Speaker speaker, bool summaryOnly)
        {
            ID = speaker.ID;
            Key = speaker.Key;
            Name = speaker.Name;
            Title = speaker.Title;
            Company = speaker.Company;
            ImageUrl = speaker.ImageUrl;
            Bio = CleanupPlainTextDocument (speaker.Bio);
            if (!summaryOnly)
                Sessions = speaker.Sessions.Select(x => new SessionDetailsViewModel(key: x.Key, summaryOnly:"true")).ToList();

            if (string.IsNullOrWhiteSpace (Bio)) {
                Bio = "No biographical information available.";
            }
        }

        public IMvxCommand ShowDetailCommand
        {
            get { return new MvxRelayCommand(() => this.RequestNavigate<SessionDetailsViewModel>(new { key = Key })); }
        }
    }
}
