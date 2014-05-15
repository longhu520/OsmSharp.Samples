using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using OsmSharp.Android.UI;
using OsmSharp.Math.Geo;
using OsmSharp.UI.Map;
using System.Reflection;
using OsmSharp.UI.Map.Layers;
using OsmSharp.UI.Renderer.Scene;

namespace Android.Vectors
{
    [Activity(Label = "Android.Vectors", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
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

            // initialize OsmSharp native handlers.
            Native.Initialize();

            // initialize map.
            var map = new Map();

            // add a preprocessed vector data file.
            var sceneStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Android.Vectors.default.map");
            map.AddLayer(new LayerScene(Scene2D.Deserialize(sceneStream, true)));

            // define the mapview.
            _mapView = new MapView(this, new MapViewSurface(this));
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
            _mapView.Dispose();
            GC.Collect();
        }
    }
}

