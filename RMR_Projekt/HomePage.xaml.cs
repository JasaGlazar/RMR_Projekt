using RMR_Projekt.Data;

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
            izberi_jezik_buttons();
            pozdrav();
            dodaj();
            //lbl_hello.Text = "Pozdravljen " + user.username;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            izberi_jezik_buttons();
            pozdrav();
        }

        private void brez_alergenov_list_Clicked(object sender, EventArgs e)
        {
            izberi_jezik("Izdelki brez alergenov","Products without alergens",alergeni_brez);
        }

        private void z_alergeni_list_Clicked(object sender, EventArgs e)
        {
            izberi_jezik("Izdelki z alergeni","Products with alergens", alergeni_z);
        }

        private void alergeni_list_Clicked(object sender, EventArgs e)
        {
            izberi_jezik("Moji alergeni","My alergens", moji_alergeni);
        }

        private void dodaj()
        {
            moji_alergeni.Add(new Izdelek("https://upload.wikimedia.org/wikipedia/commons/a/a5/Glass_of_Milk_%2833657535532%29.jpg","Milk"));
            moji_alergeni.Add(new Izdelek("https://i0.wp.com/post.healthline.com/wp-content/uploads/2019/12/bread-varieties-group-still-life-1296x728-header-1296x728.jpg?w=1155&h=1528", "Gluten"));
            moji_alergeni.Add(new Izdelek("https://be-still-farms.com/cdn/shop/articles/wooden-spoon-with-soy-seeds-close-up_1500x.jpg?v=1689344872", "Soybeans"));
            moji_alergeni.Add(new Izdelek("https://www.licious.in/blog/wp-content/uploads/2021/01/eggs-online-2-750x750.jpg", "Eggs"));
            moji_alergeni.Add(new Izdelek("https://media.npr.org/assets/img/2019/09/27/nuts-1_custom-61cfca772f4f991e157977ffe42febcb8c23d7dc.jpg", "Nuts"));
            moji_alergeni.Add(new Izdelek("https://hips.hearstapps.com/hmg-prod/images/two-full-sea-bass-fish-on-ice-with-lemon-garnish-royalty-free-image-1638224995.jpg", "Fish"));
            moji_alergeni.Add(new Izdelek("https://leitesculinaria.com/wp-content/uploads/2021/04/homemade-yellow-mustard-fp.jpg", "Mustard"));
            moji_alergeni.Add(new Izdelek("https://www.thespruceeats.com/thmb/h5_OYBELY8-WXAbSF2RLJ3evrJg=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/what-is-celery-5199268-hero-01-cb9c645dfb614f0a8eef5b0c316ce16d.jpg", "Celery"));
            moji_alergeni.Add(new Izdelek("https://belmontpeanuts.com/cdn/shop/products/Sea-Salt-scaled.jpg?v=1695127532&width=1946", "Peanuts"));
            moji_alergeni.Add(new Izdelek("https://www.datocms-assets.com/20941/1628519726-sulphites2.png?auto=compress&fm=jpg&w=850", "Sulphur dioxide and sulphites"));
            moji_alergeni.Add(new Izdelek("https://domf5oio6qrcr.cloudfront.net/medialibrary/6195/conversions/17f1f1f9-da43-405a-b3b4-4d1fcb5cec51-thumb.jpg", "Sesame seeds"));
            moji_alergeni.Add(new Izdelek("https://thumbs.dreamstime.com/z/crustacean-vector-crab-prawns-ocean-lobster-crawfish-crayfish-seafood-illustration-crustaceans-set-sea-animals-shrimp-137353529.jpg", "Crustaceans"));
            moji_alergeni.Add(new Izdelek("https://i.pinimg.com/564x/ed/c0/06/edc006388218e2c6caff7a75a3263e34.jpg", "Molluscs"));
        }

        private async void izberi_jezik(string slo,string eng,List<Izdelek> seznam)
        {
            if(Preferences.Get("is_eng",false) == true)
            {
                await Navigation.PushAsync(new SeznamIzdelkov(eng, seznam));
            } else
            {
                await Navigation.PushAsync(new SeznamIzdelkov(slo, seznam));
            }
        }

        private void izberi_jezik_buttons()
        {
            if (Preferences.Get("is_eng", false) == true)
            {
                alergeni_list.Text = "MY ALERGENS";
                z_alergeni_list.Text = "Products with alergens".ToUpper();
                brez_alergenov_list.Text = "Products without alergens".ToUpper();
            }
            else
            {
                alergeni_list.Text = "MOJI ALERGENI";
                z_alergeni_list.Text = "IZDELKI Z ALERGENI";
                brez_alergenov_list.Text = "IZDELKI BREZ ALERGENOV";
            }
        }

        private void pozdrav()
        {
            var currentUser = Preferences.Get("PrijavaToken", " ");
            string currentEmail = PrijavljenUporabnikFirebase.GetUserEmailFromIdToken(currentUser);

            string[] emailParts = currentEmail.Split('@');

            if (emailParts.Length > 0)
            {
                string firstName = emailParts[0];
                string capitalizedFirstName = char.ToUpper(firstName[0]) + firstName.Substring(1);

                if (Preferences.Get("is_eng", false))
                {
                    lbl_hello.Text = "Hello " + capitalizedFirstName;
                }
                else
                {
                    lbl_hello.Text = "Pozdravljeni " + capitalizedFirstName;
                }
            }
        }
    }
}