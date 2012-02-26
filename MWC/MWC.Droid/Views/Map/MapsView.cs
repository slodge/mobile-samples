using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Android.Views;
using MWC.Android.Screens;
using Android.GoogleMaps;
using Android.Views;
using Android.Content.PM;
using MWC.Core.Mvvm.ViewModelLocators;
using MWC.Core.Mvvm.ViewModels;

namespace MWC.Views.Map {
    [Activity(Label = "Map", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MapsView
        : MvxBindingMapActivityView<MapsViewModel>
    {
        MyLocationOverlay myLocationOverlay;

        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.Page_Maps);

            var map = FindViewById<MapView>(Resource.Id.map);

            map.Clickable = true;
            map.Traffic = false;
            map.Satellite = true;

            map.SetBuiltInZoomControls(true);
            map.Controller.SetZoom(15);
            map.Controller.SetCenter(new GeoPoint((int)(Constants.MapPinLatitude * 1e6), (int)(Constants.MapPinLongitude * 1e6)));
            
            AddMyLocationOverlay(map);
            AddPinOverlay(map);

            var animateButton = FindViewById<Button>(Resource.Id.animateButton);

            animateButton.Click += (sender, e) => {
                map.Controller.AnimateTo(
                    new GeoPoint((int)(Constants.MapPinLatitude * 1e6), (int)(Constants.MapPinLongitude * 1e6)), () => {
                        map.Controller.SetZoom(15);
                        var toast = Toast.MakeText(this, Constants.MapPinTitle, ToastLength.Short);
                        toast.Show();
                    });

            };
        }

        void AddPinOverlay(MapView map)
        {
            var pin = Resources.GetDrawable(Resource.Drawable.pin_map);
            var pinOverlay = new MapPinOverlay(pin);
            map.Overlays.Add(pinOverlay);
        }

        void AddMyLocationOverlay(MapView map)
        {
            myLocationOverlay = new MyLocationOverlay(this, map);
            map.Overlays.Add(myLocationOverlay);

            myLocationOverlay.RunOnFirstFix(() => {
                map.Controller.AnimateTo(myLocationOverlay.MyLocation);

                RunOnUiThread(() => {
                    var toast = Toast.MakeText(this, "Located", ToastLength.Short);
                    toast.Show();
                });
            });
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            BaseView<MapsViewModel>.FillMenu(menu);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (BaseView<MapsViewModel>.OnOptionsItemSelected(ViewModel, item))
                return true;
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            base.OnResume();
            myLocationOverlay.EnableMyLocation();
        }

        protected override void OnPause()
        {
            base.OnPause();
            myLocationOverlay.DisableMyLocation();
        }

        protected override bool IsRouteDisplayed
        {
            get { return false; }
        }
    }
}
