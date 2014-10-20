using OsmSharp.Osm.Xml.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OsmSharp.Osm.OverpassAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            // query to find all bicyle stations within the city of Antwerp.
            var query = "node(51.18881185379808,4.370841979980469,51.244939510768184,4.454612731933594)[\"amenity\"=\"bicycle_rental\"];out;";

            // create and execute the webrequest.
            var request = WebRequest.Create(
                "http://overpass.osm.rambler.ru/cgi/interpreter?data=" + query);
            request.Method = "GET";
            var requestStream = request.GetResponse().GetResponseStream(); // get the data.

            // use an XML data processor source.
            var source = new XmlOsmStreamSource(requestStream);

            // pull the data from the stream and deserialize XML.
            var osmEntities = new List<OsmGeo>(source);
            var outputCsv = new List<string[]>();
            for(int idx = 0; idx < osmEntities.Count; idx++)
            {
                var node = osmEntities[idx] as Node;
                var refId = string.Empty;
                var name = string.Empty;
                if (node != null && 
                    node.Tags.TryGetValue("ref", out refId) &&
                    node.Tags.TryGetValue("name", out name))
                {
                    var latitude = node.Coordinate.Latitude;
                    var longitude = node.Coordinate.Longitude;

                    outputCsv.Add(new string[] { latitude.ToInvariantString(), longitude.ToInvariantString(), refId, name });
                } 
                if (!node.Tags.TryGetValue("ref", out refId))
                {
                    Console.WriteLine("Node {0} has no ref.", node.Id.Value);
                }
                if (!node.Tags.TryGetValue("name", out name))
                {
                    Console.WriteLine("Node {0} has no name.", node.Id.Value);
                }
            }
            OsmSharp.IO.DelimitedFiles.DelimitedFileHandler.WriteDelimitedFile(null, outputCsv.ToArray(),
                new StreamWriter(new FileInfo("bicylestations.csv").OpenWrite()), OsmSharp.IO.DelimitedFiles.DelimiterType.DotCommaSeperated);
        }
    }
}
