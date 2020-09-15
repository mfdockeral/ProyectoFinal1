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
	public partial class UsuariosPage : ContentPage
	{
        private const string URL = @"https://reqres.in/api/users?page=1";

        #region Properties

        public ObservableCollection<User> Users { get; set; }

        #endregion Properties

        private HttpClient httpClient = new HttpClient();

        public UsuariosPage()
        {
            InitializeComponent();

            Users = new ObservableCollection<User>();
            // TODO: Agregar el BindingContext de la instancia de esta pagina.
            BindingContext = this;
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
                    var usersResponse = JsonConvert.DeserializeObject<UserResponse>(jsonResponse);

                    if (usersResponse.data.Count > 0)
                    {
                        Users.Clear();
                        foreach (var user in usersResponse.data)
                        {
                            Users.Add(user);
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "No se pudo descargar la lista de usuarios", "Ok");
                }

            }
            else
            {
                await DisplayAlert("Error", "No se pudo descargar la lista de usuarios", "Ok");
            }

            IsBusy = false;
        }
    }

    public class User
    {
        public int id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string avatar { get; set; }
    }

    public class Ad
    {
        public string company { get; set; }
        public string url { get; set; }
        public string text { get; set; }
    }

    public class UserResponse
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public List<User> data { get; set; }
        public Ad ad { get; set; }
    }
}
