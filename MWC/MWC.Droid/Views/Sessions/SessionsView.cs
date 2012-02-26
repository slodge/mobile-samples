using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Android.Views;
using MWC.Android.Screens;
using MWC.BL;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.Sessions 
{
    [Activity(Label = "Sessions", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SessionsView : BaseView<SessionListViewModel>
    {
        ListView sessionListView = null;

        protected override void OnViewModelSet()
        {
            Log.Debug("MWC", "SESSIONS OnViewModelSet");

            // set our layout to be the home screen
            SetContentView(Resource.Layout.Page_Sessions);

            //Find our controls
            sessionListView = FindViewById<MvxBindableListView>(Resource.Id.SessionList);
            sessionListView.Adapter = new SessionListAdapter(this);
            /*
            dayID = Intent.GetIntExtra("DayID", -1);

            //Find our controls
            sessionListView = FindViewById<ListView>(Resource.Id.SessionList);
            titleTextView = FindViewById<TextView>(Resource.Id.TitleTextView);
            
            // wire up task click handler
            if (sessionListView != null) {
                sessionListView.ItemClick += (object sender, ItemEventArgs e) => {
                    var sessionDetails = new Intent(this, typeof(SessionDetailsScreen));
                    var session = sessionTimeslotListAdapter[e.Position];
                    sessionDetails.PutExtra("SessionID", session.ID);
                    StartActivity(sessionDetails);
                };
            }
             */
        }

        public class SessionListAdapter : MvxBindableListAdapter
        {
            public SessionListAdapter(Context context) 
                : base(context)
            {
            }

            protected override global::Android.Views.View GetBindableView(global::Android.Views.View convertView, object source)
            {
                if (source is SessionListItemViewModel)
                    return base.GetBindableView(convertView, source, Resource.Layout.ListItem_Session);
                else
                    return base.GetBindableView(convertView, source, Resource.Layout.ListItem_SessionTimeslot);
            }
        }

        // scroll back to the point where you last were in the list
        int lastScrollY = -1;
        protected override void OnPause()
        {
            base.OnPause();
            if (sessionListView.FirstVisiblePosition < 5)
                lastScrollY = 0;
            else
                lastScrollY = (sessionListView.FirstVisiblePosition + sessionListView.LastVisiblePosition) / 2;
        }
        protected override void OnResume()
        {
            base.OnResume();
            sessionListView.SetSelectionFromTop(lastScrollY, 200);
        }
    }
}