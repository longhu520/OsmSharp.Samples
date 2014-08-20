using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using OsmSharp.Android.UI;
using OsmSharp.UI.Map.Layers;
using OsmSharp.UI.Map;
using OsmSharp.Math.Geo;
using OsmSharp.UI;

namespace Android.Routing.Offline
{
    [Activity(Label = "Android.Routing.Offline", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {   /// <summary>
        /// Holds the mapview.
        /// </summary>
        private MapView _mapView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // enable the logging.
            OsmSharp.Logging.Log.Enable();
            OsmSharp.Logging.Log.RegisterListener(new OsmSharp.Android.UI.Log.LogTraceListener());

            // hide title bar.
            this.RequestWindowFeature(global::Android.Views.WindowFeatures.NoTitle);

            // initialize OsmSharp native handlers.
            Native.Initialize();

            // initialize router.
            RouterFacade.Initialize();

            // initialize map.
            var map = new Map();

            // add tile Layer.
            // WARNING: Always look at usage policies!
            // WARNING: Don't use my tiles, it's a free account and will shutdown when overused!
            map.AddLayer(new LayerTile(@"http://otile1.mqcdn.com/tiles/1.0.0/osm/{0}/{1}/{2}.png"));

            // define route layer.
            _routeLayer = new LayerRoute(map.Projection);
            map.AddLayer(_routeLayer);

            // define the mapview.
            var surface = new MapViewSurface(this);
            _mapView = new MapView(this, surface);
            _mapView.Map = map;
            _mapView.MapMaxZoomLevel = 18;
            _mapView.MapMinZoomLevel = 0;
            _mapView.MapTilt = 0;
            _mapView.MapCenter = new GeoCoordinate(51.2631, 4.7872);
            _mapView.MapZoom = 16;
            _mapView.MapAllowTilt = false;

            _mapView.MapTapEvent += _mapView_MapTapEvent;

            // set the map view as the default content view.
            SetContentView(_mapView);
        }

        /// <summary>
        /// Holds the previous coordinate.
        /// </summary>
        private GeoCoordinate _previous = null;

        /// <summary>
        /// Holds the route layer.
        /// </summary>
        private LayerRoute _routeLayer;

        /// <summary>
        /// Handles the tap event.
        /// </summary>
        /// <param name="coordinate"></param>
        async void _mapView_MapTapEvent(GeoCoordinate coordinate)
        {
            if (_previous != null)
            {
                var route = RouterFacade.Calculate(_previous, coordinate);
                _routeLayer.Clear();
                if (route != null)
                {
                    _routeLayer.AddRoute(route);

                    // TODO: issue #120.
                    (_mapView as IMapView).Invalidate();
                }
            }
            _previous = coordinate;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // dispose of all resources.
            // the mapview is completely destroyed in this sample, read about the Android Activity Lifecycle here:
            // http://docs.xamarin.com/guides/android/application_fundamentals/activity_lifecycle/
            _mapView.Dispose();
        }
    }
}

