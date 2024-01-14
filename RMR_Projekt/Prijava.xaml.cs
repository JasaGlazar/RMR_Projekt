namespace RMR_Projekt;

public partial class Prijava : ContentPage
{
    bool is_eng_on;
	public Prijava()
	{
		InitializeComponent();
        preveri_jezik();
    }

    private async void reg_btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Registracija());
    }

    private async void prijava_btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AppShell());
    }

    private void img_slo_Clicked(object sender, EventArgs e)
    {
        img_slo.BackgroundColor = Color.FromArgb("#a8ff93");
        img_eng.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        Preferences.Set("is_eng", false);
        preveri_jezik();
    }

    private void img_eng_Clicked(object sender, EventArgs e)
    {

        img_eng.BackgroundColor = Color.FromArgb("#a8ff93");
        img_slo.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
        Preferences.Set("is_eng",true);
        preveri_jezik();
    }

    private void preveri_jezik()
    {
        if (Preferences.Get("is_eng",true) == true)
        {
            img_eng.BackgroundColor = Color.FromArgb("#a8ff93");
            img_slo.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            prijava_btn.Text = "Log in";
            log_btn.Text = "No account? Register";
            username_entry.Placeholder = "Username";
            pass_entry.Placeholder = "Password";
        } 
        else
        {
            img_slo.BackgroundColor = Color.FromArgb("#a8ff93");
            img_eng.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            prijava_btn.Text = "Prijava";
            log_btn.Text = "Še nimate racuna? Registracija";
            username_entry.Placeholder = "Uporabniško ime";
            pass_entry.Placeholder = "Geslo";
        }
    }
}