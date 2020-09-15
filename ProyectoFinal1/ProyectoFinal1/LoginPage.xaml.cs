using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal1
{

	public class LoginResponse
	{
		public string Token { get; set; }
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        private const string URL = @"https://reqres.in/api/login";

        #region Propiedades
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
                this.OnPropertyChanged();
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
                this.OnPropertyChanged();
            }
        }

        public bool CanLogin { get { return !IsBusy; } }

        public Command Login { get; set; }
        public Command Register { get; set; }
        #endregion Propiedades

        private HttpClient client = new HttpClient();

        public LoginPage()
        {
            InitializeComponent();

            Login = new Command(OnLogin);
            Register = new Command(OnRegister);
            BindingContext = this;
        }

        private async void OnRegister(object obj)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        private async void OnLogin()
        {
            IsBusy = true;

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
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);

                    if (!string.IsNullOrEmpty(loginResponse.Token))
                    {
                        await Navigation.PushModalAsync(new MainTabbedPage());
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Credenciales Invalidas", "Ok");
                }

            }
            else
            {
                await DisplayAlert("Error", "Credenciales Invalidas", "Ok");
            }

            IsBusy = false;
        }
    }
}