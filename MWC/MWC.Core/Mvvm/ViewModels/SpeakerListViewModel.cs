﻿using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.Core.Mvvm.ViewModels
{
    public class SpeakerListViewModel : GroupedListViewModel<Speaker, SpeakerListItemViewModel>
    {
        public SpeakerListViewModel()
        {
            BeginUpdate();
        }

        protected override IEnumerable<IGrouping<string, Speaker>> GetGroupedItems ()
        {
            return from s in SpeakerManager.GetSpeakers ()
                   group s by GetGroupKey (s);
        }

        string GetGroupKey (Speaker s)
        {
            return s.Name.Length > 0 ? char.ToLowerInvariant (s.Name[0]).ToString () : "?";
        }

        protected override object GetItemKey (Speaker item)
        {
            return item.Key;
        }
    }

    public class SpeakerListItemViewModel : GroupedListItemViewModel<Speaker>
    {
        public int ID { get; set; }
        public string SpeakerKey { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string ImageUrl { get; set; }

        public string TitleAndCompany
        {
            get
            {
                if (string.IsNullOrEmpty (Title)) {
                    return Company;
                }
                else {
                    return string.Format ("{0}, {1}", Title, Company);
                }
            }
        }

        public override void Update (Speaker speaker)
        {
            SortKey = speaker.Name;

            ID = speaker.ID;
            SpeakerKey = speaker.Key;
            Name = speaker.Name;
            Title = speaker.Title;
            Company = speaker.Company;
            ImageUrl = speaker.ImageUrl;        
        }

        public IMvxCommand ShowDetailCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<SpeakerDetailsViewModel>(new { key = SpeakerKey })); }
        }
    }
}
