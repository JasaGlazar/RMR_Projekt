namespace RMR_Projekt;

public partial class Prijava : ContentPage
{
    bool is_eng_on;
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
            noAccount.Text = "No account?";
            RegisterButton.Text = "Register";
            email_entry.Placeholder = "Username";
            pass_entry.Placeholder = "Password";
        } 
        else
        {
            img_slo.BackgroundColor = Color.FromArgb("#a8ff93");
            img_eng.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            prijava_btn.Text = "Prijava";
            noAccount.Text = "Še nimate racuna?";
            RegisterButton.Text = "Registracija";
            email_entry.Placeholder = "Uporabniško ime";
            pass_entry.Placeholder = "Geslo";
        }
    }

    private void lightmode_btn_Clicked(object sender, EventArgs e)
    {
        preveri_lightmode();
    }

    private void preveri_lightmode()
    {
        if (Preferences.Get("dark", false) == false)
        {
            Preferences.Set("dark", true);
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new RMR_Projekt.Resources.Styles.Dark());
            }
        }
        else
        {
            Preferences.Set("dark", false);
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new RMR_Projekt.Resources.Styles.Light());
            }
        }
    }
}