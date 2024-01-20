using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMR_Projekt.Views;

namespace RMR_Projekt.Data
{
    static class PrijavljenUporabnikFirebase
    {
        private static string firebaseApiKey = "AIzaSyDjdAnxx_FBJ9nZzENatjoRKtH7K02sGNE";

        private static string loggedUserData = Preferences.Get("PrijavaToken", " ");

        private static async Task<JObject> FetchUserData(string userEmail)
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
        }


        public static string GetUserEmailFromIdToken(string currentUser)
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

        public static async Task<List<string>> VrniAlergene()
        {
            var currentUser = Preferences.Get("PrijavaToken", " ");

            if(!string.IsNullOrEmpty(currentUser))
            {
                string UserEmail = GetUserEmailFromIdToken(currentUser);
                if (!string.IsNullOrEmpty(UserEmail))
                {
                    var userData = await FetchUserData(UserEmail);
                    if (userData != null)
                    {
                        var userAllergens = userData["alergeni"].ToObject<List<string>>();

                        return userAllergens;
                    }
                }
            }

            return new List<string>();
        }

        public static async Task<List<string>> VrniProdukteBrezAlergenov()
        {
            var currentUser = Preferences.Get("PrijavaToken", " ");

            if (!string.IsNullOrEmpty(currentUser))
            {
                string UserEmail = GetUserEmailFromIdToken(currentUser);
                if (!string.IsNullOrEmpty(UserEmail))
                {
                    var userData = await FetchUserData(UserEmail);
                    if (userData != null)
                    {
                        var userProduktiBrezAlergenov = userData["ProduktiBrez"].ToObject<List<string>>();

                        return userProduktiBrezAlergenov;
                    }
                }
            }

            return new List<string>();
        }


        public static async Task DodajProduktZAlergeni(ProductInfo productInfo)
        {
                string userEmail = GetUserEmailFromIdToken(loggedUserData);
                if (!string.IsNullOrEmpty(userEmail))
                {
                    var userData = await FetchUserData(userEmail);
                    if (userData != null)
                    {
                        // Get the current list of products
                        var currentProducts = userData["ProduktiZ"].ToObject<List<string>>();

                        // Add the new product code if it's not already in the list
                        if (!currentProducts.Contains(productInfo.Code))
                        {
                            currentProducts.Add(productInfo.Code);
                        }

                        // Update the ProduktiZ property in the database
                        userData["ProduktiZ"] = JToken.FromObject(currentProducts);
                        await UpdateUserData(userEmail, userData);
                    }
                }
        }

        private static async Task UpdateUserData(string userEmail, JObject userData)
        {
            var firebaseUrl = "https://rmr-projekt-a8434-default-rtdb.europe-west1.firebasedatabase.app/";
            var endpoint = $"PrijavljenUporabnik/{Uri.EscapeDataString(userEmail)}.json";

            using (var httpClient = new HttpClient())
            {
                var updateUrl = $"{firebaseUrl}{endpoint}?auth={firebaseApiKey}";
                var jsonContent = JsonConvert.SerializeObject(userData);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                await httpClient.PutAsync(updateUrl, httpContent);
            }
        }

    }
}
