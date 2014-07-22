using OsmSharp.Osm.PBF.Streams;
using OsmSharp.Routing;
using OsmSharp.Routing.Osm.Interpreter;
using System.IO;

namespace OsmSharp.Sample.Routing.PBF
{
    class Program
    {
        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // THIS SAMPLE NEEDS THE OsmSharp Nuget PACKAGE.

            // create a router instance from a pbf-source.
            var router = Router.CreateLiveFrom(
                new PBFOsmStreamSource(new FileInfo("kempen.osm.pbf").OpenRead()), new OsmRoutingInterpreter());

            // hook points to the route-network.
            var resolved1 = router.Resolve(Vehicle.Car, new Math.Geo.GeoCoordinate(51.26731, 4.80143));
            var resolved2 = router.Resolve(Vehicle.Car, new Math.Geo.GeoCoordinate(51.25806, 4.77980));

            // calculate route.
            var route = router.Calculate(Vehicle.Car, resolved1, resolved2);

            // do something with the route.
            // 1: display: there is a LayerRoute that can be used.
            // 2: export: save to GPX for example.
            // 3: ...
            route.SaveAsGpx(new FileInfo("route.gpx").OpenWrite());
        }
    }
}
