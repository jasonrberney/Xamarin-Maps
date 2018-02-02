using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using System;
using System.Collections.Generic;
using System.Text;

#if WINDOWS_UWP
using Colors = Windows.UI.Colors;
#else
using Colors = System.Drawing.Color;
#endif

namespace ArcGISApp.ViewModel.Map
{
    class AddGraphics
    {
        public AddGraphics()
        {
            // define the buoy locations
            var wgs84 = SpatialReferences.Wgs84;
            // Create initial map location and reuse the location for graphic
            MapPoint centralLocation = new MapPoint(-98.5795, 39.8283, wgs84);
            // Create overlay to where graphics are shown
            GraphicsOverlay overlay = new GraphicsOverlay();

            // Add created overlay to the MapView
            //MyMapView.GraphicsOverlays.Add(overlay);

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
    }
}
