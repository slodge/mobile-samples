﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Converters.Visibility;
using Cirrious.MvvmCross.Interfaces.Commands;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.Core.Mvvm.ViewModels
{
    public class SessionListViewModel : GroupedListViewModel<Session, SessionListItemViewModel>
    {
        public static string DayOfWeekKey(DateTime when)
        {
            return "DayOfWeek:" + when.DayOfWeek.ToString();
        }

        public static string FavoritesKey()
        {
            return "Favorites";
        }

        public static string AllKey()
        {
            return "AllKey";
        }

        public IMvxCommand ItemSelectedCommand
        {
            get { return new MvxRelayCommand<SessionListItemViewModel>((item) => item.ShowDetailCommand.Execute()); }
        }

        public SessionListViewModel(string listKey = null)
        {
            if (listKey == FavoritesKey())
            {
                InitialiseFavorites();
            }
            else if (listKey != null && listKey.StartsWith("DayOfWeek:"))
            {
                var dayOfWeekText = listKey.Substring("DayOfWeek:".Length);
                var dayOfWeek = (DayOfWeek)Enum.Parse(typeof (DayOfWeek), dayOfWeekText, false);
                InitialiseDayOfWeek(dayOfWeek);
            }
            else if (listKey == AllKey())
            {
                InitialiseAll();
            }
            else
            {
                // todo - report the error...
                InitialiseAll();
            }
            BeginUpdate();
        }

        private void InitialiseDayOfWeek(DayOfWeek dayOfWeek)
        {
            FilterDayOfWeek = dayOfWeek;
        }

        private void InitialiseFavorites()
        {
            FilterFavorites = true;
        }

        private void InitialiseAll()
        {

        }

        public bool FilterFavorites { get; private set; }
        public DayOfWeek? FilterDayOfWeek { get; private set; }

        public string Title
        {
            get
            {
                if (FilterFavorites) {
                    return "favorites";
                }
                else if (FilterDayOfWeek.HasValue) {
                    var dow = FilterDayOfWeek.Value;
                    var dt = new DateTime (2012, 1, 1);
                    while (dow != dt.DayOfWeek) {
                        dt = dt.AddDays (1);
                    }
                    return dt.ToString ("dddd").ToLower ();
                }
                else {
                    return "all";
                }
            }
        }

        public MvxVisibility NoFavoritesVisibility
        {
            get
            {
                return (FilterFavorites && Groups.Count == 0) ? MvxVisibility.Visible : MvxVisibility.Collapsed;
            }
        }

        protected override void OnUpdated ()
        {
            base.OnUpdated ();

            FirePropertyChanged ("NoFavoritesVisibility");
            FirePropertyChanged ("ListVisibility");
            FirePropertyChanged ("Title");
        }

        protected override IEnumerable<IGrouping<string, Session>> GetGroupedItems ()
        {
            var allSessions = SessionManager.GetSessions ();

            if (FilterFavorites) {
                var favs = FavoritesManager
                    .GetFavorites ()
                    .Select (f => f.SessionKey)
                    .Distinct()
                    .ToDictionary (x => x);
                return from s in allSessions
                       where favs.ContainsKey (s.Key)
                       group s by GetGroupKey (s);
            }
            else if (FilterDayOfWeek.HasValue) {
                return from s in allSessions
                       where s.Start.DayOfWeek == FilterDayOfWeek.Value
                       group s by GetGroupKey (s);
            }
            else {
                return from s in allSessions
                       group s by GetGroupKey (s);
            }
        }

        string GetGroupKey (Session s)
        {
            return s.Start.ToString ("s", System.Globalization.CultureInfo.InvariantCulture);
        }

        protected override string GetGroupTitle (string groupKey)
        {
            var time = DateTime.Parse (groupKey);

            return string.Format ("{0:dddd} {0:t}", time).ToLower ();
        }

        protected override object GetItemKey (Session item)
        {
            return item.Key;
        }
    }

    public class SessionListItemViewModel : GroupedListItemViewModel<Session>
    {
        public int ID { get; set; }
        public string SessionKey { get; set; }
        public string SpeakerNames { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public string Room { get; set; }

        public bool IsFavorite
        {
            get { return FavoritesManager.IsFavorite(SessionKey); }
            set
            {
                if (value)
                    FavoritesManager.RemoveFavoriteSession(SessionKey);
                else
                    FavoritesManager.AddFavoriteSession(new Favorite() { SessionID = ID, SessionKey = SessionKey });
                FirePropertyChanged("IsFavorite");
            }
        }

        public override void Update (Session item)
        {
            ID = item.ID;
            Title = item.Title;
            SessionKey = item.Key;
            Room = string.IsNullOrWhiteSpace (item.Room) ? "Unknown Location" : item.Room;
            Start = item.Start;
            SpeakerNames = item.SpeakerNames;
            SortKey = Start.ToString ("s", System.Globalization.CultureInfo.InvariantCulture);
        }

        public IMvxCommand ShowDetailCommand
        {
            get { return new MvxRelayCommand(() => RequestNavigate<SessionDetailsViewModel>(new { key = SessionKey } )); }
        }
    }
}
