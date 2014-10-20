
using OsmSharp.Osm.PBF.Streams;
using OsmSharp.Routing.Osm.Interpreter;
using OsmSharp.Routing.TSP.Genetic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsmSharp.Routing.TSP
{
    class Program
    {
        static void Main(string[] args)
        {
            OsmSharp.Logging.Log.Enable();
            OsmSharp.Logging.Log.RegisterListener(new OsmSharp.WinForms.UI.Logging.ConsoleTraceListener());
            // read the source file.
            var lines = OsmSharp.IO.DelimitedFiles.DelimitedFileHandler.ReadDelimitedFile(null,
                new FileInfo("bicylestations.csv").OpenRead(), OsmSharp.IO.DelimitedFiles.DelimiterType.DotCommaSeperated, true);

            // create router.
            var interpreter = new OsmRoutingInterpreter();
            var router = Router.CreateLiveFrom(new PBFOsmStreamSource(new FileInfo("antwerp.osm.pbf").OpenRead()), interpreter);

            // resolve all points.
            var resolvedPoints = new List<RouterPoint>();
            foreach(var line in lines)
            {
                var latitude = double.Parse(line[0], System.Globalization.CultureInfo.InvariantCulture);
                var longitude = double.Parse(line[1], System.Globalization.CultureInfo.InvariantCulture);
                var refId = double.Parse(line[2], System.Globalization.CultureInfo.InvariantCulture);

                var resolved = router.Resolve(Vehicle.Bicycle, new Math.Geo.GeoCoordinate(latitude, longitude));
                if(resolved != null && router.CheckConnectivity(Vehicle.Bicycle, resolved, 100))
                { // point exists and is connected.
                    resolvedPoints.Add(resolved);
                }
                else
                { // report that the point could not be resolved.
                    Console.WriteLine("Point with ref {0} could not be resolved!", refId);
                }
            }

            // calculate TSP.
            var tspSolver = new RouterTSPWrapper<RouterTSP>(
                new RouterTSPAEXGenetic(1000, 200), router, interpreter);
            var route = tspSolver.CalculateTSP(Vehicle.Bicycle, resolvedPoints.ToArray());
            route.SaveAsGpx(new FileInfo("output.gpx").OpenWrite());
        }
    }
}
