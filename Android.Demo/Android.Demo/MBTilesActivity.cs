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
using OsmSharp.Android.UI.Data.SQLite;
using OsmSharp.UI.Map.Layers;
using System.Reflection;
using OsmSharp.Math.Geo;

namespace Android.Demo
{
    [Activity(Label = "Offline Tiles Demo")]
    public class MBTilesActivity : Activity
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

            // add MBTiles Layer.
            // any stream will do or any path on the device to a MBTiles SQLite databas
            // in this case the data is taken from the resource stream, written to disk and then opened.
            map.AddLayer(new LayerMBTile(SQLiteConnection.CreateFrom(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Android.Demo.kempen.mbtiles"), "map")));

            // define the mapview.
            _mapView = new MapView(this, new MapViewSurface(this));
            _mapView.Map = map;
            _mapView.MapMaxZoomLevel = 17; // limit min/max zoom because MBTiles sample only contains a small portion of a map.
            _mapView.MapMinZoomLevel = 12;
            _mapView.MapTilt = 0;
            _mapView.MapCenter = new GeoCoordinate(51.26361, 4.78620);
            _mapView.MapZoom = 16;
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