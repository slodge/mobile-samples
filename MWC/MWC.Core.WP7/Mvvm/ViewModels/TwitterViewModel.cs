﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.Core.Mvvm.ViewModels
{
    public class TwitterViewModel : ViewModelBase
    {
        public ObservableCollection<TweetViewModel> Items { get; private set; }

        public bool IsUpdating { get; set; }
        public Visibility ListVisibility { get; set; }
        public Visibility NoDataVisibility { get; set; }

        public Visibility UpdatingVisibility
        {
            get
            {
                return (IsUpdating || Items == null || Items.Count == 0) ? Visibility.Visible : Visibility.Collapsed;
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

            FirePropertyChanged ("IsUpdating");
            FirePropertyChanged ("UpdatingVisibility");
        }

        void HandleUpdateFinished (object sender, EventArgs e)
        {
            TwitterFeedManager.UpdateFinished -= HandleUpdateFinished;
            var entries = TwitterFeedManager.GetTweets ();
            IsUpdating = false;
            PopulateData (entries);
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

                //
                // Update the properties
                //
                FirePropertyChanged ("Items");

                ListVisibility = Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                NoDataVisibility = Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

                FirePropertyChanged ("ListVisibility");
                FirePropertyChanged ("NoDataVisibility");
                FirePropertyChanged ("IsUpdating");
                FirePropertyChanged ("UpdatingVisibility");
            });
        }
    }
}