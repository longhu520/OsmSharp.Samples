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
using OsmSharp.Math.Geo;
using OsmSharp.Routing;
using System.Threading.Tasks;
using OsmSharp.Routing.CH.Serialization.Sorted;
using System.Reflection;
using OsmSharp.Routing.Osm.Interpreter;
using OsmSharp.Routing.CH;
using OsmSharp.Routing.Graph.Router;
using OsmSharp.Routing.CH.PreProcessing;

namespace Android.Routing.Offline
{
    public static class RouterFacade
    {
        /// <summary>
        /// Holds the graph.
        /// </summary>
        private static IBasicRouterDataSource<CHEdgeData> _graph;

        public static void Initialize()
        {
            var routingSerializer = new CHEdgeDataDataSourceSerializer();
            _graph = routingSerializer.Deserialize(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Android.Routing.Offline.kempen-big.contracted.mobile.routing"));
        }

        /// <summary>
        /// Calculate a simple route.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Route Calculate(GeoCoordinate from, GeoCoordinate to)
        {
            try
            {
                lock(_graph)
                {
                    var router = Router.CreateCHFrom(_graph, new CHRouter(), new OsmRoutingInterpreter());

                    var fromResolved = router.Resolve(Vehicle.Car, from);
                    var toResolved = router.Resolve(Vehicle.Car, to);

                    if(fromResolved != null && toResolved !=null)
                    {
                        return router.Calculate(Vehicle.Car, fromResolved, toResolved);
                    }
                }
            }
            catch(Exception ex)
            {
                OsmSharp.Logging.Log.TraceEvent("Router", OsmSharp.Logging.TraceEventType.Critical, "Unhandled exception occured: {0}", ex.ToString());
            }
            return null;
        }
    }
}