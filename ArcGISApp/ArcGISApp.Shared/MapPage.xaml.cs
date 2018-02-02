using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
//using Esri.ArcGISRuntime.UI.Controls;
using System;
using System.Reflection;
using Xamarin.Forms;
//using Windows.UI;

#if WINDOWS_UWP
using Colors = Windows.UI.Colors;
#else
using Colors = System.Drawing.Color;
#endif

namespace ArcGISApp
{
	public partial class MapPage : ContentPage
    {
        private BasemapSelector basemapSelector;
        private GraphicsOverlay overlay;
        public MapPage()
        {
            InitializeComponent();
            Initialize();
            basemapSelector = new BasemapSelector(MyMapView);
            layerLoad(MyMapView);
        }
        // String array to store titles for the viewpoints specified above.
        private string[] titles = new string[]
        {
            "Topo",
            "Streets",
            "Imagery",
            "Ocean"
        };

        // Map initialization logic is contained in MapViewModel.cs
        private void Initialize()
        {
            // Create new Map with basemap
            //Map myMap = new Map(BasemapType.ImageryWithLabels, -33.867886, -63.985, 16);

            // Assign the map to the MapView
            //MyMapView.Map = myMap;

            // define the buoy locations
            var wgs84 = SpatialReferences.Wgs84;
            // Create initial map location and reuse the location for graphic
            MapPoint centralLocation = new MapPoint(-98.5795, 39.8283, wgs84);
            // Create overlay to where graphics are shown
            GraphicsOverlay overlay = new GraphicsOverlay();

            // Add created overlay to the MapView
            MyMapView.GraphicsOverlays.Add(overlay);

            // Create a simple marker symbol
            SimpleMarkerSymbol simpleSymbol = new SimpleMarkerSymbol()
            {
                Color = Colors.Red,
                //Color = Xamarin.Forms.Color.Aqua;
                Size = 50,
                Style = SimpleMarkerSymbolStyle.Circle
            };

            // Add a new graphic with a central point that was created earlier
            Graphic graphicWithSymbol = new Graphic(centralLocation, simpleSymbol);
            overlay.Graphics.Add(graphicWithSymbol);
        }

        public async void layerLoad(MapView myMapView)
        {
            overlay = new GraphicsOverlay();
            myMapView.GraphicsOverlays.Add(overlay);

            Assembly currentAssembly = null;

#if WINDOWS_UWP
            // Get current assembly that contains the image
            currentAssembly = GetType().GetTypeInfo().Assembly;
#else
            // Get current assembly that contains the image
            currentAssembly = Assembly.GetExecutingAssembly();
#endif

            // Get image as a stream from the resources
            // Picture is defined as EmbeddedResource and DoNotCopy
            var beerMug = currentAssembly.GetManifestResourceStream("ArcGISApp.images.beer_mug.png");

            PictureMarkerSymbol beer = await PictureMarkerSymbol.CreateAsync(beerMug);
            ArcGISApp.JSONLoader loader = new ArcGISApp.JSONLoader();

            if(loader.bars.Count > 0)
            {
                MapPoint mp;
                Graphic graphic;
                foreach (Bar b in loader.bars)
                {
                    mp = b.getMapPoint();
                    graphic = new Graphic(mp, beer);
                    overlay.Graphics.Add(graphic);
                }
            }
        }

        private async void OnChangeBasemapButtonClicked(object sender, EventArgs e)
        {
            // Show sheet and get title from the selection
            var selectedBasemap = await DisplayActionSheet("Select basemap", "Cancel", null, titles);

            basemapSelector.ChangeBasemap(selectedBasemap);
        }
    }
}
