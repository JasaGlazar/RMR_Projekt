using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_Projekt.ViewModels
{
    internal class PrijavaViewModel : INotifyPropertyChanged
    {
        public string WebApiKey = "AIzaSyDjdAnxx_FBJ9nZzENatjoRKtH7K02sGNE";

        public INavigation _navigation;
        public Command PrijavaBtn { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }
        private string geslo;
        public string Geslo
        {
            get => geslo;
            set
            {
                geslo = value;
                RaisePropertyChanged("Geslo");
            }
        }

        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public PrijavaViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            PrijavaBtn = new Command(PrijavaBtnTappedAsync);

        }

        private async void PrijavaBtnTappedAsync(object obj)
        {
            FirebaseAuthProvider firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));

            Preferences.Remove("PrijavaToken");

            try
            {
                FirebaseAuthLink auth = await firebaseAuthProvider.SignInWithEmailAndPasswordAsync(Email, Geslo);
                FirebaseAuthLink vsebina = await auth.GetFreshAuthAsync();

                var serializiranaVsebina = JsonConvert.SerializeObject(vsebina);
                Preferences.Set("PrijavaToken", serializiranaVsebina);

                //Tu je error za ponovno prijavo
                //await this._navigation.PushModalAsync(neke);

                Application.Current.MainPage = new NavigationPage(new AppShell());
            }
            catch (FirebaseAuthException ex)
            {
                string errorMessage = "Napaka pri prijavi";

                if (ex.Reason == AuthErrorReason.WrongPassword)
                {
                    errorMessage = "Nepravilni email ali geslo";
                }
                else if (ex.Reason == AuthErrorReason.UserNotFound)
                {
                    errorMessage = "Uporabnik ne obstaja. Prosim registrirajte se najprej";
                }
                else if (ex.Reason == AuthErrorReason.InvalidEmailAddress)
                {
                    errorMessage = "Email ni veljaven";
                }
                else if (ex.ResponseData.Contains("INVALID_LOGIN_CREDENTIALS", StringComparison.OrdinalIgnoreCase))
                {
                    errorMessage = "Nepravilni email ali geslo";
                }
                else if (ex.Reason == AuthErrorReason.MissingPassword)
                {
                    errorMessage = "Vnesite geslo";
                }
                else if (ex.Reason == AuthErrorReason.UserDisabled)
                {
                    errorMessage = "Vaš račun je onemogočen";
                }

                await App.Current.MainPage.DisplayAlert("Napaka", errorMessage, "OK");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Opozorilo", "Nepričakovana napaka", "OK");
                throw;
            }
        }
    }
}
