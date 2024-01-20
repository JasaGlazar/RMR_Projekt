using Newtonsoft.Json;
using RMR_Projekt.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_Projekt.Data
{
    internal static class ProductInfoFirebase
    {
        private static string firebaseApiKey = "AIzaSyDjdAnxx_FBJ9nZzENatjoRKtH7K02sGNE";

        public static async Task<List<ProductInfo>> VrniProductInfo()
        {
            var firebaseUrl = "https://rmr-projekt-a8434-default-rtdb.europe-west1.firebasedatabase.app/";
            var endpoint = "ProductInfo.json";

            using (var httpClient = new HttpClient())
            {
                var fetchUrl = $"{firebaseUrl}{endpoint}?auth={firebaseApiKey}";
                var fetchResponse = await httpClient.GetAsync(fetchUrl);
                var productData = fetchResponse.IsSuccessStatusCode ? JsonConvert.DeserializeObject<Dictionary<string, ProductInfo>>(await fetchResponse.Content.ReadAsStringAsync()) : null;

                return productData?.Values.ToList() ?? new List<ProductInfo>();
            }
        }

        public static async void POSTProductInfo(ProductInfo productInfo)
        {
            // Fetch all existing ProductInfo from the database
            var existingProducts = await VrniProductInfo();

            // Check if the product already exists in the database
            if (existingProducts.Any(p => p.Code == productInfo.Code))
            {
                Console.WriteLine("Product already exists in the database.");
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                // Convert ProductInfo to JSON
                string contentJson = JsonConvert.SerializeObject(productInfo);

                // Specify the Firebase Realtime Database URL and the node to post to
                string firebaseUrl = "https://rmr-projekt-a8434-default-rtdb.europe-west1.firebasedatabase.app/";
                string firebaseNode = "ProductInfo/";

                // Build the URL for the POST request
                string requestUrl = $"{firebaseUrl}{firebaseNode}.json";

                // Create a StringContent with the JSON data
                StringContent content = new StringContent(contentJson, Encoding.UTF8, "application/json");

                // Send the POST request
                var response = await client.PostAsync(requestUrl, content);

                // Check if the request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Successfully posted the data
                    Console.WriteLine("Data posted successfully!");
                }
                else
                {
                    // Handle the error
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }
    }
}
