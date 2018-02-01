using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace ArcGISApp.ViewModel.DataLoader
{
    public class JSONLoader
    {
        public List<Bar> bars;

        public void LoadJSON()
        {
            var assembly = typeof(JSONLoader).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("ViewModel.DataLoader.bars_in_WA.json");

            using (var reader = new System.IO.StreamReader(stream))
            {

                var raw = reader.ReadToEnd();
                bars = JsonConvert.DeserializeObject<List<Bar>>(raw.ToString());

            }
        }
    }

    public class Bar
    {
        public string id { get; set; }
        public string name { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string hours { get; set; }
        public int beers { get; set; }

        public MapPoint getMapPoint()
        {
            // define the buoy locations
            var wgs84 = SpatialReferences.Wgs84;

            return Esri.ArcGISRuntime.Geometry.CoordinateFormatter.FromLatitudeLongitude(lat + "," + lon, wgs84);
        }
    }
}
