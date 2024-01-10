namespace RMR_Projekt.Views
{
	public partial class HomePage : ContentPage
	{
        List<Izdelek> moji_alergeni = new List<Izdelek>();
        List<Izdelek> alergeni_brez = new List<Izdelek>();
        List<Izdelek> alergeni_z = new List<Izdelek>();
        public HomePage()
		{
			InitializeComponent();
			//lbl_hello.Text = "Pozdravljen " + user.username;
		}

        private async void brez_alergenov_list_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SeznamIzdelkov("Izdelki brez alergenov", alergeni_brez));
        }

        private async void z_alergeni_list_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SeznamIzdelkov("Izdelki z alergeni", alergeni_z));
        }

        private async void alergeni_list_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SeznamIzdelkov("Moji alergeni", moji_alergeni));
        }
    }
}