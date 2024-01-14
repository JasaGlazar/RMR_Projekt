namespace RMR_Projekt;

public partial class Registracija : ContentPage
{
	public Registracija()
	{
        InitializeComponent();
        BindingContext = new ViewModels.RegistracijaViewModel(Navigation);
	}

    private async void log_btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Prijava());
    }

}