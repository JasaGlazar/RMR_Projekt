using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_Projekt.ViewModels
{
    internal class RegistracijaViewModel : INotifyPropertyChanged
    {
        public string webApiKey = "AIzaSyDjdAnxx_FBJ9nZzENatjoRKtH7K02sGNE";

        private INavigation _navigation;

        public Command RegistracijaUporabnika { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string email;

        private string geslo;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }
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

        public RegistracijaViewModel(INavigation navigation)
        {
            _navigation = navigation;

            RegistracijaUporabnika = new Command(RegistracijaUporabnikaTappedAsync);
        }

        private async void RegistracijaUporabnikaTappedAsync(object obj)
        {
            try
            {
                FirebaseAuthProvider firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                FirebaseAuthLink auth = await firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(Email, Geslo);

                string token = auth.FirebaseToken;
                if (token != null)
                {
                    await App.Current.MainPage.DisplayAlert("Obvestilo", "Uporabnik uspešno registriran", "OK");
                    await this._navigation.PopAsync();
                }
            }
            catch (FirebaseAuthException ex)
            {
                string errorMessage = "Napaka pri registraciji";

                if (ex.Reason == AuthErrorReason.EmailExists)
                {
                    errorMessage = "Uporabniški račun s tem Email naslovom že obstaja";
                }
                else if (ex.Reason == AuthErrorReason.WeakPassword)
                {
                    errorMessage = "Geslo je prešibko. Dolgo mora biti vsaj 6 znakov";
                }
                else if (ex.Reason == AuthErrorReason.InvalidEmailAddress)
                {
                    errorMessage = "Email ni veljaven";
                }
                else if (ex.Reason == AuthErrorReason.MissingPassword)
                {
                    errorMessage = "Vnesite geslo";
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
