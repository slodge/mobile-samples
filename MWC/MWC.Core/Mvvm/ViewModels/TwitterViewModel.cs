using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Cirrious.MvvmCross.Converters.Visibility;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.Core.Mvvm.ViewModels
{
    public class TwitterViewModel : ViewModelBase
    {
        private ObservableCollection<TweetViewModel> _items;
        public ObservableCollection<TweetViewModel> Items
        {
            get { return _items; }
            private set
            {
                _items = value; FirePropertyChanged("Items");
            }
        }

        private bool _isUpdating;
        public bool IsUpdating
        {
            get { return _isUpdating; }
            private set
            {
                _isUpdating = value; FirePropertyChanged("IsUpdating");
            }
        }

        public void BeginUpdate ()
        {
            IsUpdating = true;

            ThreadPool.QueueUserWorkItem (delegate {
                var entries = TwitterFeedManager.GetTweets ();
                PopulateData (entries);

                TwitterFeedManager.UpdateFinished += HandleUpdateFinished;
                TwitterFeedManager.Update ();
            });
        }

        void HandleUpdateFinished (object sender, EventArgs e)
        {
            TwitterFeedManager.UpdateFinished -= HandleUpdateFinished;
            var entries = TwitterFeedManager.GetTweets ();
            PopulateData(entries);
            IsUpdating = false;
        }

        void PopulateData (IEnumerable<Tweet> entries)
        {
            InvokeOnMainThread (delegate {
                //
                // Set all the tweets
                //
                Items = new ObservableCollection<TweetViewModel> (
                    from e in entries
                    select new TweetViewModel (e));

#warning bugbug - this is wrong here - we shouldn't really need to set IsUpdating false here
                IsUpdating = Items.Count == 0;
                
                /*
                 * REMOVED AS THIS IS NOT A VIEW MODEL CONCERN...
                 * Need to work this out purely in the view(s) somehow...
                 * 
                //
                // Add some margin to the last item to get it out of the
                // way of the AppBar
                //
                if (Items.Count > 0) {
                    var last = Items.Last();
                    if (last != null) {
                        var m = last.Margin;
                        m.Bottom = 12 * 6;
                        last.Margin = m;
                    }
                }
                 */
            });
        }
    }
}
