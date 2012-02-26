using System;
using System.Collections.Generic;
using System.Windows;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.Core.Mvvm.ViewModels
{
    public class SessionDetailsViewModel : ViewModelBase
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Room { get; set; }
        public string Overview { get; set; }
        public List<string> SpeakerKeys { get; set; }

        public bool IsFavorite { get; set; }
        public Visibility FavoriteVisibility { get; set; }

        public SessionDetailsViewModel (string id = null, string key = null)
        {
            SpeakerKeys = new List<string> ();

            var session = default(Session);
            if (id != null)
            {
                session = SessionManager.GetSession(int.Parse(id));
            }
            else if (key != null)
            {
                session = SessionManager.GetSessionWithKey(key);
            }

            if (session != null)
                Update(session);
        }

        public string TimeSpanText
        {
            get
            {
                return string.Format ("{0:dddd} {0:t} - {1:t}", Start, End);
            }
        }

        public void Update (Session session)
        {
            ID = session.ID;
            Key = session.Key;
            Title = session.Title;
            Start = session.Start;
            End = session.End;            
            Room = session.Room;
            Overview = CleanupPlainTextDocument (session.Overview);
            if (session.SpeakerKeys != null) {
                SpeakerKeys = new List<string> (session.SpeakerKeys);
            }
            else {
                SpeakerKeys.Clear ();
            }

            if (string.IsNullOrWhiteSpace (Overview)) {
                Overview = "No overview available.";
            }

            UpdateIsFavorite ();
        }

        public void UpdateIsFavorite ()
        {
            IsFavorite = FavoritesManager.IsFavorite (Key);
            FavoriteVisibility = IsFavorite ? Visibility.Visible : Visibility.Collapsed;

            FirePropertyChanged ("IsFavorite");
            FirePropertyChanged ("FavoriteVisibility");
        }
    }
}
