namespace RMR_Projekt;

public partial class Prijava : ContentPage
{
	public Prijava()
	{
		InitializeComponent();
        if(Preferences.Get("dark",false) == true){
            error_lbl.Text = "dela";
        }
        BindingContext = new ViewModels.PrijavaViewModel(Navigation);
    }

    private async void reg_btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Registracija());
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new Registracija());
    }
}