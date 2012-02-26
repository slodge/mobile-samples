using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;
using MWC.BL;
using MWC.BL.Managers;
using MWC.DAL;

namespace MWC.Core.Mvvm.ViewModels
{
    public class ExhibitorsListViewModel : GroupedListViewModel<Exhibitor, ExhibitorListItemViewModel>
    {
        public ExhibitorsListViewModel()
        {
            //
            // Update the Exhibitors DB if it's time for an update
            //
            if (DateTime.UtcNow >= ApplicationSettings.NextExhibitorsUpdateTimeUtc)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    UpdateManager.UpdateExhibitorsFinished += delegate
                    {
                        InvokeOnMainThread(delegate
                        {
                            ApplicationSettings.NextExhibitorsUpdateTimeUtc = DateTime.UtcNow.AddHours(1);
                            BeginUpdate();
                        });
                    };
                    UpdateManager.UpdateExhibitors();
                });
            }

            BeginUpdate();
        }

        protected override IEnumerable<IGrouping<string, Exhibitor>> GetGroupedItems ()
        {
            return from s in DataManager.GetExhibitors ()
                   group s by GetGroupKey (s);
        }

        string GetGroupKey (Exhibitor item)
        {
            var name = item.Name.Trim ();
            if (name.Length == 0 || !char.IsLetter (name[0])) {
                return "#";
            }
            else {
                return char.ToLowerInvariant (name[0]).ToString ();
            }
        }

        protected override object GetItemKey (Exhibitor item)
        {
            return item.ID;
        }
    }

    public class ExhibitorListItemViewModel : GroupedListItemViewModel<Exhibitor>
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Locations { get; set; }
        public string ImageUrl { get; set; }

        public string CityAndCountry
        {
            get
            {
                return City + ", " + Country;
            }
        }

        public override void Update (Exhibitor item)
        {
            ID = item.ID;
            Name = item.Name.Trim ();
            City = item.City;
            Country = item.Country;
            Locations = item.Locations;
            ImageUrl = item.ImageUrl;

            SortKey = Name;
        }

        public IMvxCommand ShowDetailCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<ExhibitorDetailsViewModel>(new {id = ID})); }
        }
    }
}
