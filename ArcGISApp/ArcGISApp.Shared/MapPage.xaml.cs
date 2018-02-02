using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
using Esri.ArcGISRuntime.Location;
using Newtonsoft.Json;
//using Esri.ArcGISRuntime.UI.Controls;
using System;
using System.Collections.Generic;
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

            MyMapView.GeoViewTapped += OnMapViewTapped;

            // Starts location display with auto pan mode set to Off
            MyMapView.LocationDisplay.AutoPanMode = LocationDisplayAutoPanMode.Off;

            //TODO Remove this IsStarted check https://github.com/Esri/arcgis-runtime-samples-xamarin/issues/182
            if (!MyMapView.LocationDisplay.IsEnabled)
            {
                MyMapView.LocationDisplay.IsEnabled = true;
            }
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

                    Dictionary<string, object> dict = b.getAttributes();

                    foreach (KeyValuePair<string, object> obj in dict)
                    {
                        graphic.Attributes.Add(obj);
                    }

                    overlay.Graphics.Add(graphic);
                }
            }
        }

        private async void OnMapViewTapped(object sender, Esri.ArcGISRuntime.Xamarin.Forms.GeoViewInputEventArgs e)
        {
            var tolerance = 10d; // Use larger tolerance for touch
            var maximumResults = 1; // Only return one graphic  
            var onlyReturnPopups = false; // Don't return only popups

            // Use the following method to identify graphics in a specific graphics overlay
            IdentifyGraphicsOverlayResult identifyResults = await MyMapView.IdentifyGraphicsOverlayAsync(
                 overlay,
                 e.Position,
                 tolerance,
                 onlyReturnPopups,
                 maximumResults
                 );

            // Check if we got results
            if (identifyResults.Graphics.Count > 0)
            {
                // Make sure that the UI changes are done in the UI thread
                Device.BeginInvokeOnMainThread(async () => {
                    var json = JsonConvert.SerializeObject(identifyResults.Graphics[0].Attributes, Formatting.Indented);
                    await DisplayAlert("", json, "OK");
                });
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
