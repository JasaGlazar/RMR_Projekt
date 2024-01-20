namespace RMR_Projekt;

public partial class Registracija : ContentPage
{
	public Registracija()
	{
        InitializeComponent();
        preveri_jezik();
        BindingContext = new ViewModels.RegistracijaViewModel(Navigation);
	}

    private async void log_btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Prijava());
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Prijava());

    }

    private void preveri_jezik()
    {
        if (Preferences.Get("is_eng", true) == true)
        {
            LoginButton.Text = "Log in";
            AccountYes.Text = "No account? ";
            naprej_btn.Text = "Register";
            username_entry.Placeholder = "Username";
            pass_entry.Placeholder = "Password";
            confirm_pass_entry.Placeholder = "Confirm password";
        }
        else
        {
            LoginButton.Text = "Prijava";
            AccountYes.Text = "Še nimate racuna? ";
            naprej_btn.Text = "Registracija";
            username_entry.Placeholder = "Uporabniško ime";
            pass_entry.Placeholder = "Geslo";
            confirm_pass_entry.Placeholder = "Potrdite geslo";
        }
    }
}