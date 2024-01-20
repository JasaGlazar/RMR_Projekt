using RMR_Projekt.Data;

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
            pozdrav();
            NaloziProfilno();
            if(Preferences.Get("dark",false) == true)
            {
                dark_light_switch.IsToggled= true;
            } 
            else
            {
                dark_light_switch.IsToggled = false;
            }

            if (Preferences.Get("is_eng",false) == true)
            {
                img_eng.BackgroundColor = Color.FromArgb("#a8ff93");
                img_slo.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
            } 
            else
            {
                {
                    img_slo.BackgroundColor = Color.FromArgb("#a8ff93");
                    img_eng.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                }

		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NaloziProfilno();
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

        private async void edit_ProfilePic_btn_Clicked(object sender, EventArgs e)
        {
            var mediaFile = await MediaPicker.PickPhotoAsync();
            if (mediaFile != null)
            {
                // Get the stream of the selected image
                var stream = await mediaFile.OpenReadAsync();

                // Generate a unique file name
                string uniqueFileName = Guid.NewGuid().ToString("N") + ".jpg";

                // Save the stream to a file in the app's local storage
                var filePath = Path.Combine(FileSystem.AppDataDirectory, uniqueFileName);
                using (var fileStream = File.OpenWrite(filePath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                // Set the source of the Image control
                Preferences.Remove("ProfilePic");
                Preferences.Set("ProfilePic", filePath);
                profile_picture.Source = " ";
                profile_picture.Source = filePath;
            }
        }

        public void pozdrav()
        {
            var currentUser = Preferences.Get("PrijavaToken", " ");
            string currentEmail = PrijavljenUporabnikFirebase.GetUserEmailFromIdToken(currentUser);

            string[] emailParts = currentEmail.Split('@');

            if (emailParts.Length > 0)
            {
                string firstName = emailParts[0];
                string capitalizedFirstName = char.ToUpper(firstName[0]) + firstName.Substring(1);

                username_lbl.Text = capitalizedFirstName;
            }
        }

        private void NaloziProfilno()
        {
            profile_picture.Source = Preferences.Get("ProfilePic"," ");
        }
    }
}