using Microsoft.Maui.ApplicationModel;
using Newtonsoft.Json;
using System.Text;

namespace RMR_Projekt.Views
{
	public partial class CameraPage : ContentPage
	{
		public CameraPage()
		{
			InitializeComponent();
		}

        private void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
			if (cameraView.Cameras.Count > 0)
			{
				cameraView.Camera = cameraView.Cameras.First();
				MainThread.BeginInvokeOnMainThread(async () => { 
					await cameraView.StopCameraAsync();
					await cameraView.StartCameraAsync();
				});
			}
        }



        private async void ApiCall(string BarcodeNumber)
        {
            string apiPath = "https://world.openfoodfacts.org/api/v2/product/";
            //var number = barcodeResult.Text.Trim();
            var numberTest = BarcodeNumber;


            string json = await GetJsonAsync($"{apiPath}{numberTest}.json");


            if (!string.IsNullOrEmpty(json))
            {
                // Deserialize the JSON into a ProductInfo object
                ProductInfo productInfo = JsonConvert.DeserializeObject<ProductInfo>(json);

                var allergens = productInfo.product.allergens_hierarchy;

                List<string> modifiedAllergens = new List<string>();

                foreach (var allergen in allergens)
                {
                    // Find the position of ":" and remove everything before it
                    int colonIndex = allergen.IndexOf(':');
                    string modifiedAllergen = colonIndex != -1 ? allergen.Substring(colonIndex + 1) : allergen;

                    // Add the modified allergen to the new list
                    modifiedAllergens.Add(modifiedAllergen);
                }

                // Isto se naredi za sestavine produkta
                // Posodib json pa classe da bodo shranejvali hranilne vrednosti za graf -> Borba za èetrtek

                // Save the modified allergens list back to product.allergens_hierarchy
                productInfo.product.allergens_hierarchy = modifiedAllergens;
                product_image.Source = productInfo.product.image_url;
                product_name.Text = "Nutella";
                alergeni_list.Clear();
                foreach(string product in productInfo.product.allergens_hierarchy.ToList())
                {
                    Label l = new Label();
                    l.Text = product;
                    alergeni_list.Add(l);
                }

            }
            else
            {
                // Handle the case where JSON is not available or invalid
                Console.WriteLine("Failed to fetch valid JSON data.");
            }

        }

        private async void FirebasePOST(ProductInfo productInfo)
        {
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

      /*  private async void Button_Clicked(object sender, EventArgs e)
        {
            string apiPath = "https://world.openfoodfacts.org/api/v2/product/";
            //var number = barcodeResult.Text.Trim();
            var numberTest = "8000500357729";


            string json = await GetJsonAsync($"{apiPath}{numberTest}.json");


            if (!string.IsNullOrEmpty(json))
            {
                // Deserialize the JSON into a ProductInfo object
                ProductInfo productInfo = JsonConvert.DeserializeObject<ProductInfo>(json);

                var allergens = productInfo.product.allergens_hierarchy;

                List<string> modifiedAllergens = new List<string>();

                foreach (var allergen in allergens)
                {
                    // Find the position of ":" and remove everything before it
                    int colonIndex = allergen.IndexOf(':');
                    string modifiedAllergen = colonIndex != -1 ? allergen.Substring(colonIndex + 1) : allergen;

                    // Add the modified allergen to the new list
                    modifiedAllergens.Add(modifiedAllergen);
                }

                // Isto se naredi za sestavine produkta
                // Posodib json pa classe da bodo shranejvali hranilne vrednosti za graf -> Borba za èetrtek

                // Save the modified allergens list back to product.allergens_hierarchy
                productInfo.product.allergens_hierarchy = modifiedAllergens;


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
            else
            {
                // Handle the case where JSON is not available or invalid
                Console.WriteLine("Failed to fetch valid JSON data.");
            }

        }
      */

        private async Task<string> GetJsonAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        // Handle unsuccessful response here if needed
                        Console.WriteLine($"Error: {response.StatusCode}");
                        return string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions here if needed
                    Console.WriteLine($"Exception: {ex.Message}");
                    return string.Empty;
                }
            }
        }


        private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
        {
			MainThread.BeginInvokeOnMainThread(() =>
			{
                ApiCall(args.Result[0].Text);
            });
        }

        public void preveri_barvo_alergenov()
        {
            if (Preferences.Get("is_dark", false) == true)
            {

            }
            else
            {

            }
        }
    }

    // Define classes to represent the structure of the JSON data
    public class ProductInfo
    {
        public string Code { get; set; }
        public product product { get; set; }
    }

    public class product
    {
        public List<string> allergens_hierarchy { get; set; }
        public string image_url { get; set; }
        public List<string> ingredients_hierarchy { get; set; }

    }



}