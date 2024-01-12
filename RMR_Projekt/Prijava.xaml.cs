namespace RMR_Projekt;

public partial class Prijava : ContentPage
{
	public Prijava()
	{
		InitializeComponent();
    }

    private async void reg_btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Registracija());
    }

    private async void prijava_btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AppShell());
    }
}