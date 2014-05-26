using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OsmSharp.Android.UI;
using OsmSharp.UI.Map;
using OsmSharp.UI.Map.Layers;
using OsmSharp.Math.Geo;

namespace Android.Demo
{
    [Activity(Label = "Online Tiles Demo")]
    public class TilesActivity : Activity
    {
        /// <summary>
        /// Holds the mapview.
        /// </summary>
        private MapView _mapView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // hide title bar.
            this.RequestWindowFeature(global::Android.Views.WindowFeatures.NoTitle);

            // initialize map.
            var map = new Map();

            // add tile Layer.
            // WARNING: Always look at usage policies!
            // WARNING: Don't use my tiles, it's a free account and will shutdown when overused!
            map.AddLayer(new LayerTile("http://a.tiles.mapbox.com/v3/osmsharp.i8ckml0l/{0}/{1}/{2}.png"));

            // define the mapview.
            _mapView = new MapView(this, new MapViewSurface(this));
            _mapView.Map = map;
            _mapView.MapMaxZoomLevel = 18; // limit min/max zoom because MBTiles sample only contains a small portion of a map.
            _mapView.MapMinZoomLevel = 0;
            _mapView.MapTilt = 0;
            _mapView.MapCenter = new GeoCoordinate(21.38, -157.782);
            _mapView.MapZoom = 12;
            _mapView.MapAllowTilt = false;

            // set the map view as the default content view.
            SetContentView(_mapView);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // dispose of all resources.
            // the mapview is completely destroyed in this sample, read about the Android Activity Lifecycle here:
            // http://docs.xamarin.com/guides/android/application_fundamentals/activity_lifecycle/
            //_mapView.Map.Close();

            //_mapView.Close();
            _mapView.Dispose();
        }
    }
}