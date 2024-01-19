

namespace RMR_Projekt.Views
{
	public partial class ProfilePage : ContentPage
	{
        public bool is_eng_on = false;
        public bool is_dark_on = false;
		public ProfilePage()
		{
			InitializeComponent();
            BindingContext = new ViewModels.ProfilePageViewModel(Navigation);
		}

        private void img_slo_Clicked(object sender, EventArgs e)
        {
            img_slo.BackgroundColor = Color.FromArgb("#a8ff93");
            img_eng.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            Preferences.Set("is_eng",false);
            is_eng_on = false;
        }

        private void img_eng_Clicked(object sender, EventArgs e)
        {

            img_eng.BackgroundColor = Color.FromArgb("#a8ff93");
            img_slo.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            Preferences.Set("is_eng", true);
            is_eng_on = true;
        }


        private void dark_light_switch_Toggled(object sender, ToggledEventArgs e)
        {
            if (is_dark_on == false)
            {
                light_mode_btn.Source = ImageSource.FromFile("dark.png");
                edit_ProfilePic_btn.Source = ImageSource.FromFile("beli_edit.png");
                is_dark_on = true;
                Preferences.Set("dark",true);
                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                if (mergedDictionaries != null)
                {
                    mergedDictionaries.Clear();
                    mergedDictionaries.Add(new RMR_Projekt.Resources.Styles.Dark());
                }
            }
            else
            {
                light_mode_btn.Source = ImageSource.FromFile("light.png");
                edit_ProfilePic_btn.Source = ImageSource.FromFile("edit.png");
                is_dark_on = false;
                Preferences.Set("dark", false);
                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                if (mergedDictionaries != null)
                {
                    mergedDictionaries.Clear();
                    mergedDictionaries.Add(new RMR_Projekt.Resources.Styles.Light());
                }
            }
        }

        private async void logout_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Prijava());
        }
    }
}