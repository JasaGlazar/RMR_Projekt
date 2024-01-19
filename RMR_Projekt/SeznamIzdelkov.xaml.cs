namespace RMR_Projekt;

public partial class SeznamIzdelkov : ContentPage
{
	public SeznamIzdelkov(string title, List<Izdelek> seznam_izdelkov)
	{
		InitializeComponent();
        list_izdelkov.ItemsSource = seznam_izdelkov.ToList();
		Title = title;
	}
	public SeznamIzdelkov() 
	{
        InitializeComponent();
    }
}