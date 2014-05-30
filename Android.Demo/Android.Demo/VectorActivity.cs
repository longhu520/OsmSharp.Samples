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
using System.Reflection;
using OsmSharp.UI.Map.Layers;
using OsmSharp.UI.Renderer.Scene;
using OsmSharp.Math.Geo;

namespace Android.Demo
{
    [Activity(Label = "My Activity")]
    public class VectorActivity : Activity
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

            // add a preprocessed vector data file.
            var sceneStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Android.Demo.default.map");
            map.AddLayer(new LayerScene(Scene2D.Deserialize(sceneStream, true)));

            // define the mapview.
            var mapViewSurface = new MapViewSurface(this);
            mapViewSurface.MapScaleFactor = 2;
            _mapView = new MapView(this, mapViewSurface);
            _mapView.Map = map;
            _mapView.MapMaxZoomLevel = 17; // limit min/max zoom, the vector data in this sample covers only a small area.
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
            _mapView.Map.Close();

            _mapView.Close();
            _mapView.Dispose();
            _mapView = null;
        }
    }
}