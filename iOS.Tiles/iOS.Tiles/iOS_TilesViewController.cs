using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using OsmSharp.iOS.UI;
using OsmSharp.UI.Map.Layers;
using OsmSharp.UI.Map;
using OsmSharp.Math.Geo;

namespace iOS.Tiles
{
    public partial class iOS_TilesViewController : UIViewController
    {
        /// <summary>
        /// Holds the mapview.
        /// </summary>
        private MapView _mapView;

        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public iOS_TilesViewController()
			: base(UserInterfaceIdiomIsPhone ? "iOS_TilesViewController_iPhone" : "iOS_TilesViewController_iPad", null)
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
			
            // initialize OsmSharp native hooks.
            Native.Initialize();

            // initialize map.
            var map = new Map();

            // add tile Layer.
            // WARNING: Always look at usage policies!
            // WARNING: Don't use my tiles, it's a free account and will shutdown when overused!
            map.AddLayer(new LayerTile("http://a.tiles.mapbox.com/v3/osmsharp.i8ckml0l/{0}/{1}/{2}.png"));

            // define the mapview.
            _mapView = new MapView();
            _mapView.Map = map;
            _mapView.MapMaxZoomLevel = 18; // limit min/max zoom.
            _mapView.MapMinZoomLevel = 0;
            _mapView.MapTilt = 0;
            _mapView.MapCenter = new GeoCoordinate(21.38, -157.782);
            _mapView.MapZoom = 12;
            _mapView.MapAllowTilt = false;

            View = _mapView;
        }
    }
}

