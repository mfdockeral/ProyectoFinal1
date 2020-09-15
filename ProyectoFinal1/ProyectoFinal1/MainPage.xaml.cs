using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ProyectoFinal1
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private const string URL = @"https://jsonplaceholder.typicode.com/albums";

        #region Properties

        public ObservableCollection<Album> Albums { get; set; }

        private Album _selectedAlbum;
        public Album SelectedAlbum
        {
            get
            {
                return _selectedAlbum;
            }
            set
            {
                _selectedAlbum = value;
                //TODO: Llamar el metodo SelectAlbum para navegar al detalle
                SelectAlbum(_selectedAlbum);
            }
        }

        #endregion Properties

        private HttpClient httpClient = new HttpClient();

        public MainPage()
        {
            InitializeComponent();

            Albums = new ObservableCollection<Album>();
            //TODO: Asignar el BindingContext a la misma instancia de MainPage
            BindingContext = this;

        }

        private async void SelectAlbum(Album selectedAlbum)
        {
            if (selectedAlbum == null)
            {
                return;
            }

            //TODO: Navegar hacia la pagina AlbumDetailPage y pasar el album seleccionado

            await Navigation.PushAsync(new AlbumDetailPage(selectedAlbum.AlbumId));

            selectedAlbum = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;

            var response = await httpClient.GetAsync(URL);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                try
                {
                    var albumsResponse = JsonConvert.DeserializeObject<List<Album>>(jsonResponse);

                    if (albumsResponse.Count > 0)
                    {
                        Albums.Clear();
                        foreach (var album in albumsResponse)
                        {
                            Albums.Add(album);
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "No se pudo descargar la lista de albums", "Ok");
                }

            }
            else
            {
                await DisplayAlert("Error", "No se pudo descargar la lista de albums", "Ok");
            }

            IsBusy = false;
        }
    }
}
