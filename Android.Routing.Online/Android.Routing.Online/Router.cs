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
using ServiceStack;
using OsmSharp.Service.Routing.Core.Primitives;
using OsmSharp.Routing;
using OsmSharp.Service.Routing.Core;
using System.Threading.Tasks;

namespace Android.Routing.Online
{
    public static class Router
    {
        /// <summary>
        /// Calculate a simple route.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static async Task<Route> Calculate(GeoCoordinate from, GeoCoordinate to)
        {
            // REMARK1: this sample needs ServiceStack.Client.Pcl
            // REMARK2: this sample could need a fix for a bug related to System.IO.Compression: https://github.com/ServiceStack/Hello#issues
            // REMARK3: to have a strongly type client to the OsmSharp.Service.Routing service the package OsmSharp.Service.Routing needs to be installed.

            try
            {
                // create Json client.
                var client = new JsonServiceClient("http://build.osmsharp.com:666/");
                client.Timeout = new TimeSpan(0, 5, 0);

                // creates the array of the routing hook.
                var hooks = new RoutingHook[2];

                // create the array of geocoordinates.
                hooks[0] = new RoutingHook()
                {
                    Id = 0,
                    Latitude = (float)from.Latitude,
                    Longitude = (float)from.Longitude,
                    Tags = new RoutingHookTag[0]
                };
                hooks[1] = new RoutingHook()
                {
                    Id = 1,
                    Latitude = (float)to.Latitude,
                    Longitude = (float)to.Longitude,
                    Tags = new RoutingHookTag[0]
                };

                // set the request.
                var routingResponse = client.Get<RoutingResponse>(
                            new RoutingOperation()
                            {
                                Vehicle = Vehicle.Car.UniqueName,
                                Hooks = hooks,
                                Type = RoutingOperationType.Regular
                            });
                return routingResponse.Route;
            }
            catch(Exception ex)
            {
                OsmSharp.Logging.Log.TraceEvent("Router", OsmSharp.Logging.TraceEventType.Critical, "Unhandled exception occured: {0}", ex.ToString());
            }
            return null;
        }
    }
}