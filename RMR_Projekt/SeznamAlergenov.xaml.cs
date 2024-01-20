using Firebase.Auth;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RMR_Projekt.Data;
using System.Formats.Asn1;
using System.Text;
using ZXing.Aztec.Internal;

namespace RMR_Projekt;

public partial class SeznamAlergenov : ContentPage
{
    private List<Izdelek> moji_alergeni;
    private string firebaseApiKey = "AIzaSyDjdAnxx_FBJ9nZzENatjoRKtH7K02sGNE";

    public SeznamAlergenov(string title, List<Izdelek> seznam_izdelkov)
	{
		InitializeComponent();
      //list_izdelkov.ItemsSource = seznam_izdelkov.ToList();
		moji_alergeni = seznam_izdelkov;
		list_izdelkov.ItemsSource = moji_alergeni;
		Title = title;

        LoadUserPreferences();
    }
    public SeznamAlergenov()
	{
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var selectedAllergens = moji_alergeni.Where(item => item.IsSelected).Select(item => item.ime.ToLower()).ToList();

        var currentUser = Preferences.Get("PrijavaToken", " ");

        if(!string.IsNullOrEmpty(currentUser) )
        {
            string userEmail = GetUserEmailFromIdToken(currentUser);

            if (!string.IsNullOrEmpty(userEmail))
            {
                await UpdateUserPreferences(userEmail, selectedAllergens);
            }
            else
            {
                await Console.Out.WriteLineAsync("Napaka pri pridobitvi Email naslova");
            }
        }
        else
        {
            await Console.Out.WriteLineAsync("Prijava token ne obstaja");
        }

    }

    private async Task UpdateUserPreferences(string userEmail, List<string> selectedAllergens)
    {
        try
        {
            var firebaseUrl = "https://rmr-projekt-a8434-default-rtdb.europe-west1.firebasedatabase.app/";
            var endpoint = "PrijavljenUporabnik.json";
            //var firebaseApiKey = "AIzaSyDjdAnxx_FBJ9nZzENatjoRKtH7K02sGNE";
            

            using (var httpClient = new HttpClient())
            {
                // Fetch all users
                var fetchUrl = $"{firebaseUrl}{endpoint}?auth={firebaseApiKey}";
                var fetchResponse = await httpClient.GetAsync(fetchUrl);
                var usersData = fetchResponse.IsSuccessStatusCode ? JsonConvert.DeserializeObject<Dictionary<string, JObject>>(await fetchResponse.Content.ReadAsStringAsync()) : null;

                //string idk = "lol";
                //await Console.Out.WriteLineAsync("lol");

                // If usersData is null or empty, create a new user
                if (usersData == null || !usersData.Any())
                {
                    await CreateUser(httpClient, fetchUrl, userEmail, selectedAllergens);
                    return;
                }

                // Find the user with the matching email
                var userKey = usersData?.FirstOrDefault(kvp => kvp.Value["username"]?.Value<string>() == userEmail).Key;

                if (userKey != null)
                {
                    // Prepare the data to be updated
                    var updatedPreferences = usersData[userKey];
                    updatedPreferences["alergeni"] = JToken.FromObject(selectedAllergens);

                    // Update the user data
                    var jsonContent = JsonConvert.SerializeObject(updatedPreferences);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    var updateUrl = $"{firebaseUrl}PrijavljenUporabnik/{Uri.EscapeDataString(userKey)}.json?auth={firebaseApiKey}";
                    var updateResponse = await httpClient.PutAsync(updateUrl, httpContent);

                    if (updateResponse.IsSuccessStatusCode)
                    {
                        await Console.Out.WriteLineAsync("Alergeni uspešno posodobljeni");
                        await App.Current.MainPage.DisplayAlert("Uspešno", "Alergeni uspešno posodobljeni", "OK");
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync("Napaka pri posodobitvi alergenov");
                        await App.Current.MainPage.DisplayAlert("Napaka", "Napaka pri posodobitvi alergenov", "OK");
                    }
                }
                else
                {
                    // Create a new user
                    await CreateUser(httpClient, fetchUrl, userEmail, selectedAllergens);
                }
            }
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync($"Napaka pri posodobitvi alergenov: {ex.Message}");
        }

    }

    private string GetUserEmailFromIdToken(string currentUser)
    {
        try
        {
            var tokenObj = JsonConvert.DeserializeObject<JObject>(currentUser);

            if (tokenObj != null && tokenObj.TryGetValue("User", out var userObj))
            {
                var user = userObj as JObject;
                if (user != null && user.TryGetValue("email", out var userEmail))
                {
                    return userEmail.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while parsing the token");
        }

        return null;
    }


    private async Task CreateUser(HttpClient httpClient, string url, string userEmail, List<string> selectedAllergens)
    {
        var newUser = new JObject
        {
            ["Jezik"] = "defaultLanguage",
            ["ProduktiBrez"] = new JArray { "placeholder" },
            ["ProduktiZ"] = new JArray { "placeholder"},
            ["Theme"] = "defaultTheme",
            ["username"] = userEmail,
            ["alergeni"] = JToken.FromObject(selectedAllergens)
        };

        var jsonContent = JsonConvert.SerializeObject(newUser);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var createResponse = await httpClient.PostAsync(url, httpContent);

        if (createResponse.IsSuccessStatusCode)
        {
            await Console.Out.WriteLineAsync("Alergeni uspešno posodobljeni");
        }
        else
        {
            await Console.Out.WriteLineAsync("Napaka pri posodobitvi alergenov");
        }
    }


    /*private async Task<JObject> FetchUserData(string userEmail)
    {
        var firebaseUrl = "https://rmr-projekt-a8434-default-rtdb.europe-west1.firebasedatabase.app/";
        var endpoint = "PrijavljenUporabnik.json";
        
        using (var httpClient = new HttpClient())
        {
            var fetchUrl = $"{firebaseUrl}{endpoint}?auth={firebaseApiKey}";
            var fetchResponse = await httpClient.GetAsync(fetchUrl);
            var usersData = fetchResponse.IsSuccessStatusCode ? JsonConvert.DeserializeObject<Dictionary<string, JObject>>(await fetchResponse.Content.ReadAsStringAsync()) : null;

            // Find the user with the matching email
            var userKey = usersData?.FirstOrDefault(kvp => kvp.Value["username"]?.Value<string>() == userEmail).Key;

            return userKey != null ? usersData[userKey] : null;
        }
    }*/

    private async void LoadUserPreferences()
    {
        var userAllergens = await PrijavljenUporabnikFirebase.VrniAlergene();

        foreach (var allergen in moji_alergeni)
        {
            allergen.IsSelected = userAllergens.Contains(allergen.ime.ToLower());
        }

        list_izdelkov.ItemsSource = moji_alergeni;
    }
}