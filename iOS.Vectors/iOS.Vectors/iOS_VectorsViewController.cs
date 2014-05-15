using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using OsmSharp.iOS.UI;
using OsmSharp.UI.Map;
using OsmSharp.UI.Map.Layers;
using System.Reflection;
using OsmSharp.UI.Renderer.Scene;
using OsmSharp.Math.Geo;

namespace iOS.Vectors
{
    public partial class iOS_VectorsViewController : UIViewController
    {
        /// <summary>
        /// Holds the mapview.
        /// </summary>
        private MapView _mapView;

        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public iOS_VectorsViewController()
			: base(UserInterfaceIdiomIsPhone ? "iOS_VectorsViewController_iPhone" : "iOS_VectorsViewController_iPad", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            // initialize OsmSharp native handlers.
            Native.Initialize();

            // initialize map.
            var map = new Map();

            // add a preprocessed vector data file.
            var sceneStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"iOS.Vectors.default.map");
            map.AddLayer(new LayerScene(Scene2D.Deserialize(sceneStream, true)));

            // define the mapview.
            _mapView = new MapView();
            _mapView.Map = map;
            _mapView.MapMaxZoomLevel = 17; // limit min/max zoom, the vector data in this sample covers only a small area.
            _mapView.MapMinZoomLevel = 12;
            _mapView.MapTilt = 0;
            _mapView.MapCenter = new GeoCoordinate(51.26361, 4.78620);
            _mapView.MapZoom = 16;
            _mapView.MapAllowTilt = false;

            View = _mapView;
        }
    }
}

