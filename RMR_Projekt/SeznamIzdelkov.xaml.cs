namespace RMR_Projekt;

public partial class SeznamIzdelkov : ContentPage
{
    private List<Izdelek> moji_alergeni;

    public SeznamIzdelkov(string title, List<Izdelek> seznam_izdelkov)
	{
		InitializeComponent();
       // list_izdelkov.ItemsSource = seznam_izdelkov.ToList();
		moji_alergeni = seznam_izdelkov;
		list_izdelkov.ItemsSource = moji_alergeni;
		Title = title;
	}
	public SeznamIzdelkov() 
	{
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var selectedAllergens = moji_alergeni.Where(item => item.IsSelected).Select(item => item.ime.ToLower()).ToList();

        var currentUser = Preferences.Get("PrijavaToken", " ");


    }
}