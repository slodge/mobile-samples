using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Converters.Visibility;
using Cirrious.MvvmCross.Interfaces.Commands;
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
        public string SpeakerNames { get; set; }
        public IList<Speaker> Speakers { get; set; }

        public bool IsFavorite
        {
            get { return FavoritesManager.IsFavorite(Key); }
            set
            {
                if (value)
                    FavoritesManager.RemoveFavoriteSession(Key);
                else
                    FavoritesManager.AddFavoriteSession(new Favorite() { SessionID = ID, SessionKey = Key });
                FirePropertyChanged("IsFavorite");
            }
        }

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
            SpeakerNames = session.SpeakerNames;
            Overview = CleanupPlainTextDocument(session.Overview);
            Speakers = session.Speakers;

            if (session.SpeakerKeys != null) {
                SpeakerKeys = new List<string> (session.SpeakerKeys);
            }
            else {
                SpeakerKeys.Clear ();
            }

            if (string.IsNullOrWhiteSpace (Overview)) {
                Overview = "No overview available.";
            }
        }

        public IMvxCommand ShowDetailCommand
        {
            get { return new MvxRelayCommand(() => this.RequestNavigate<SessionDetailsViewModel>(new { id = ID })); }
        }
    }
}
