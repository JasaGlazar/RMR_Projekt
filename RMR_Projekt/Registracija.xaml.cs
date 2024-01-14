namespace RMR_Projekt;

public partial class Registracija : ContentPage
{
	public Registracija()
	{
		InitializeComponent();
        preveri_jezik();
	}

    private async void log_btn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Prijava());
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
        Preferences.Set("is_eng", true);
        preveri_jezik();
    }

    private void preveri_jezik()
    {
        if (Preferences.Get("is_eng", true) == true)
        {
            img_eng.BackgroundColor = Color.FromArgb("#a8ff93");
            img_slo.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            naprej_btn.Text = "Next";
            log_btn.Text = "Already have na account? Log in";
            username_entry.Placeholder = "Username";
            pass_entry.Placeholder = "Password";
            confirm_pass_entry.Placeholder = "Confirm password";
        }
        else
        {
            img_slo.BackgroundColor = Color.FromArgb("#a8ff93");
            img_eng.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            naprej_btn.Text = "Naprej";
            log_btn.Text = "Ze imate racun? Prijava";
            username_entry.Placeholder = "Uporabnisko ime";
            pass_entry.Placeholder = "Geslo";
            confirm_pass_entry.Placeholder = "Potrdite geslo";
        }
    }

}