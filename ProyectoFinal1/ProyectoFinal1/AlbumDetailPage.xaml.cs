using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal1
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AlbumDetailPage : ContentPage
	{
        private const string URL = @"https://jsonplaceholder.typicode.com/photos?albumId={0}";
        private int AlbumId;
        private HttpClient httpClient = new HttpClient();

        #region Properties

        public ObservableCollection<Photo> Photos { get; set; }

        #endregion Properties

        public AlbumDetailPage(int albumId)
        {
            InitializeComponent();

            AlbumId = albumId;
            Photos = new ObservableCollection<Photo>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //TODO: Implementar el codigo para descargar la lista de fotos de un album
            // Usar la constante URL declarada al inicio de la clase y reemplazar {0}
            // Por la variable albumId

            var urlFinal = string.Format(URL, AlbumId);

            IsBusy = true;

            var response = await httpClient.GetAsync(urlFinal);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                try
                {
                    var fotosRespuesta = JsonConvert.DeserializeObject<List<Photo>>(jsonResponse);

                    if (fotosRespuesta.Count > 0)
                    {
                        Photos.Clear();
                        foreach (var album in fotosRespuesta)
                        {
                            Photos.Add(album);
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