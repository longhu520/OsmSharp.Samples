using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using OsmSharp.iOS.UI;
using System.Reflection;
using OsmSharp.UI.Map.Layers;
using OsmSharp.UI.Map;
using OsmSharp.Math.Geo;

namespace iOS.MBTiles
{
    public partial class iOS_MBTilesViewController : UIViewController
    {
        /// <summary>
        /// Holds the mapview.
        /// </summary>
        private MapView _mapView;

        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public iOS_MBTilesViewController()
			: base(UserInterfaceIdiomIsPhone ? "iOS_MBTilesViewController_iPhone" : "iOS_MBTilesViewController_iPad", null)
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

            // add MBTiles Layer.
            // any stream will do or any path on the device to a MBTiles SQLite database
            // in this case the data is taken from the resource stream, written to disk and then opened.
            map.AddLayer(new LayerMBTile(SQLiteConnection.CreateFrom(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(@"iOS.MBTiles.kempen.mbtiles"), "map")));

            // define the mapview.
            _mapView = new MapView();
            _mapView.Map = map;
            _mapView.MapMaxZoomLevel = 17; // limit min/max zoom because MBTiles sample only contains a small portion of a map.
            _mapView.MapMinZoomLevel = 12;
            _mapView.MapTilt = 0;
            _mapView.MapCenter = new GeoCoordinate(51.26361, 4.78620);
            _mapView.MapZoom = 16;
            _mapView.MapAllowTilt = false;

            View = _mapView;
        }
    }
}

