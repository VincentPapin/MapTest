using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using Newtonsoft.Json;


namespace MapTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string urlWS =
            "http://api.aviationstack.com/v1/flights?access_key=5ac8339e195b4c8bb3fb500cd6c8f728";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MapView_OnLoaded(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // choose your provider here
            mapView.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            mapView.MinZoom = 2;
            mapView.MaxZoom = 17;
            // whole world zoom
            mapView.Zoom = 15;
            // lets the map use the mousewheel to zoom
            mapView.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            mapView.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            mapView.DragButton = MouseButton.Left;
            // mapView.SetPositionByKeywords("1 square des bruyères 49450 Saint André de la Marche, France");
        }

        private void BtnSearch_OnClick(object sender, RoutedEventArgs e)
        {
            // string saisieKeys = tbSearch.Text;
            // MapView_SetPosition(saisieKeys);
            //Déclarartion du ListeVol
            ListeVol listeVol = new ListeVol();

            listeVol = GetListVoltWS(urlWS).Result;
            AfficheVol(listeVol);
        }

        private void MapView_SetPosition(string keys)
        {
            mapView.SetPositionByKeywords(keys);
            mapView.Zoom = 12;
        }

        // public async void CallMethod()
        // {
        //     Task<ListeVol> task = GetListVoltWS(urlWS);
        //     ListeVol listeVol = await task;
        // }

        static async Task<ListeVol> GetListVoltWS(string url)
        {
            using (var webClient = new WebClient())
            {
                ListeVol listeVol = new ListeVol();
                string rawData = webClient.DownloadString(url);
                listeVol = JsonConvert.DeserializeObject<ListeVol>(rawData);

                return listeVol;
            }
        }

        private void AfficheVol(ListeVol listeVol)
        {
            mapView.MapProvider = GMapProviders.GoogleMap;
            mapView.Manager.Mode = AccessMode.ServerOnly;
            mapView.SetPositionByKeywords("Paris, France");


            foreach (Vol vol in listeVol.data)
            {
                if (vol.live is not null)
                {
                    var point = new PointLatLng(vol.live.latitude, vol.live.longitude);

                    GMapMarker marker = new GMapMarker(point);

                    marker.Shape = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Stroke = Brushes.Blue,
                        StrokeThickness = 1.5
                    };

                    marker.ZIndex = 55;
                    mapView.Markers.Add(marker);
                }
            }
        }
    }
}