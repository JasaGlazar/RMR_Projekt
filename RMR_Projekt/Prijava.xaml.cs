namespace RMR_Projekt;

public partial class Prijava : ContentPage
{
    bool is_eng_on;
	public Prijava()
	{
		InitializeComponent();
        preveri_jezik();
        BindingContext = new ViewModels.PrijavaViewModel(Navigation);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async void reg_btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Registracija());
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        pass_entry.IsEnabled = false;
        pass_entry.IsEnabled = true;
        await Navigation.PushModalAsync(new Registracija());
    }
    private void preveri_jezik()
    {
        if (Preferences.Get("is_eng",true) == true)
        {
            prijava_btn.Text = "Log in";
            noAccount.Text = "No account? ";
            RegisterButton.Text = "Register";
            email_entry.Placeholder = "Username";
            pass_entry.Placeholder = "Password";
        } 
        else
        {
            prijava_btn.Text = "Prijava";
            noAccount.Text = "�e nimate racuna? ";
            RegisterButton.Text = "Registracija";
            email_entry.Placeholder = "Uporabni�ko ime";
            pass_entry.Placeholder = "Geslo";
        }
    }



}