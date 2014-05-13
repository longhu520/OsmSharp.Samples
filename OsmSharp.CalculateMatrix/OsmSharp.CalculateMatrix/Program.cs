using OsmSharp.Collections.Tags;
using OsmSharp.Math.Geo;
using OsmSharp.Routing;
using OsmSharp.Routing.CH;
using OsmSharp.Routing.Osm.Interpreter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OsmSharp.CalculateMatrix
{
    class Program
    {
        static void Main(string[] args)
        {
            // enable OsmSharp logging.
            OsmSharp.Logging.Log.Enable();
            OsmSharp.Logging.Log.RegisterListener(new OsmSharp.WinForms.UI.Logging.ConsoleTraceListener());

            var coordinates = new List<GeoCoordinate>();

            // read coordinates.
            coordinates.Add(new GeoCoordinate(51.26580f, 4.77784f));
            coordinates.Add(new GeoCoordinate(51.27134f, 4.78863f));
            coordinates.Add(new GeoCoordinate(51.26850f, 4.80141f));
            coordinates.Add(new GeoCoordinate(51.25550f, 4.79850f));

            // reads preprocessed routing graph (use https://github.com/OsmSharp/OsmSharpDataProcessor to create this file from OpenStreetMap-data).
            OsmSharp.Logging.Log.TraceEvent("Program", Logging.TraceEventType.Information, "Deserializing routing graph...");
            TagsCollectionBase metaTags;
            var serializer = new OsmSharp.Routing.CH.Serialization.CHEdgeFlatfileSerializer();
            var graph = serializer.Deserialize(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("OsmSharp.CalculateMatrix.kempen-big.contracted.flat.routing"), out metaTags, false);

            // create router from file and define vehicle profile.
            var vehicle = Vehicle.Car;
            var router = Router.CreateCHFrom(graph, new CHRouter(), new OsmRoutingInterpreter());

            // calculate many-to-many matrix.

            // first resolve all points.
            OsmSharp.Logging.Log.TraceEvent("Program", Logging.TraceEventType.Information, "Resolving points...");
            // this steps linkgs the routes to the routing network by calculating the closest routable point.
            var routerPoints = new List<RouterPoint>();
            foreach(var coordinate in coordinates)
            {
                var routerPoint = router.Resolve(vehicle, coordinate);
                if (routerPoint != null)
                {
                    routerPoints.Add(routerPoint);
                }
            }

            // calculate many-to-many.
            OsmSharp.Logging.Log.TraceEvent("Program", Logging.TraceEventType.Information, "Calculating matrics...");
            var matrix = router.CalculateManyToManyWeight(vehicle, routerPoints.ToArray(), routerPoints.ToArray());

            for(int x = 0; x < matrix.Length; x++)
            {
                for (int y = 0; y < matrix[x].Length; y++)
                {
                    OsmSharp.Logging.Log.TraceEvent("Program", Logging.TraceEventType.Information, 
                        string.Format("[{0},{1}]:{2}",x,y,matrix[x][y]));
                }
            }

            Console.ReadLine();
        }
    }
}
