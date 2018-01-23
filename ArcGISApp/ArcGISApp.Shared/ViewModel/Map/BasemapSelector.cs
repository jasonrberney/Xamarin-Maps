using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Esri.ArcGISRuntime.Xamarin.Forms;
using Esri.ArcGISRuntime.Mapping;

namespace ArcGISApp
{
    public class BasemapSelector
    {
        private MapView MyMapView;
        public BasemapSelector(MapView myMapView)
        {
            MyMapView = myMapView;
        }
        public string[] titles = new string[]
        {
            "Topo",
            "Streets",
            "Imagery",
            "Ocean"
        };

        public void ChangeBasemap(string selectedBasemap)
        {

            // If selected cancel do nothing
            if (selectedBasemap == "Cancel") return;

            switch (selectedBasemap)
            {
                case "Topo":

                    // Set the basemap to Topographic
                    MyMapView.Map.Basemap = Basemap.CreateTopographic();
                    break;

                case "Streets":

                    // Set the basemap to Streets
                    MyMapView.Map.Basemap = Basemap.CreateStreets();
                    break;

                case "Imagery":

                    // Set the basemap to Imagery
                    MyMapView.Map.Basemap = Basemap.CreateImagery();
                    break;

                case "Ocean":

                    // Set the basemap to Imagery
                    MyMapView.Map.Basemap = Basemap.CreateOceans();
                    break;

                default:
                    break;
            }
        }
    }
}
