using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal1
{
	public class RegisterResponse
	{
		public int Id { get; set; }
		public string Token { get; set; }
	}
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage
	{
        private const string URL = @"https://reqres.in/api/register";

        #region Propiedades
        //TODO: Crear las propiedades Email y Password para hacer Binding con el XAML

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        public Command Register { get; set; }

        #endregion

        public bool CanLogin { get { return !IsBusy; } }

        private HttpClient client = new HttpClient();

        public RegisterPage()
        {
            InitializeComponent();

            Register = new Command(OnRegister);
            BindingContext = this;
        }

        private async void OnRegister()
        {
            IsBusy = true;

            //TODO: Reemplazar los valores con las propiedades Email y Password
            var credentials = new
            {
                email = Email,
                password = Password
            };

            string json = JsonConvert.SerializeObject(credentials);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await this.client.PostAsync(URL, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                try
                {
                    var loginResponse = JsonConvert.DeserializeObject<RegisterResponse>(jsonResponse);

                    if (!string.IsNullOrEmpty(loginResponse.Token))
                    {
                        await Navigation.PushModalAsync(new MainTabbedPage());
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Ok");
                }

            }
            else
            {
                var x = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", x, "Ok");
            }

            IsBusy = false;
        }
    }
}