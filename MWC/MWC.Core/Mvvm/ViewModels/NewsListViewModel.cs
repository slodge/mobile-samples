﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Converters.Visibility;
using Cirrious.MvvmCross.Interfaces.Commands;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.Core.Mvvm.ViewModels
{
    public class NewsListViewModel : UpdatingItemsViewModelBase<NewsItemViewModel>
    {
        public void BeginUpdate ()
        {
            IsUpdating = true;

            ThreadPool.QueueUserWorkItem (delegate {
                var entries = NewsManager.Get ();
                PopulateData (entries);

                NewsManager.UpdateFinished += HandleUpdateFinished;
                NewsManager.Update ();
            });
        }

        public IMvxCommand RefreshCommand
        {
            get
            {
                return new MvxRelayCommand(BeginUpdate);
            }
        }

        void HandleUpdateFinished (object sender, EventArgs e)
        {
            NewsManager.UpdateFinished -= HandleUpdateFinished;
            var entries = NewsManager.Get ();
            IsUpdating = false;
            PopulateData (entries);
        }

        void PopulateData (IEnumerable<RSSEntry> entries)
        {
            InvokeOnMainThread (delegate {
                //
                // Set all the news items
                //
                Items = new ObservableCollection<NewsItemViewModel> (
                    from e in entries
                    select new NewsItemViewModel (e));

#warning bugbug - this is wrong here - we shouldn't really need to set IsUpdating false here
                IsUpdating = Items.Count == 0;

                /*
                 * TODO - get this functionality in the Views
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
            });
        }
    }
}
