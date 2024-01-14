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

        private async void Button_Clicked(object sender, EventArgs e)
        {

			string apiPath = "https://world.openfoodfacts.org/api/v2/product/";
			//var number = barcodeResult.Text.Trim();
			var numberTest = "5010477348678";


            string json = await GetJsonAsync($"{apiPath}{numberTest}.json");


            if (!string.IsNullOrEmpty(json))
            {
                // Deserialize the JSON into a ProductInfo object
                ProductInfo productInfo = JsonConvert.DeserializeObject<ProductInfo>(json);

                /*
                     V productInfo mamo zaj podatke ki smo jih pridobili s skeniranjem, na podlagi tega se morajo izpisati alergeni

                 */
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
				barcodeResult.Text = $"{args.Result[0].Text}";
            });
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