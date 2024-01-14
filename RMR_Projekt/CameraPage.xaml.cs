using Microsoft.Maui.ApplicationModel;
using Newtonsoft.Json;

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
			var number = barcodeResult.Text.Trim();
			var numberTest = "product/8000500357729";


            string json = await GetJsonAsync($"{apiPath}{numberTest}.json");


            if (!string.IsNullOrEmpty(json))
            {
                // Deserialize the JSON into a ProductInfo object
                ProductInfo productInfo = JsonConvert.DeserializeObject<ProductInfo>(json);

                // Extract information from the ProductInfo object
                string productNumber = productInfo.Code;
                List<string> ingredients = productInfo.Product.IngredientsText?.Split(',').Select(i => i.Trim()).ToList() ?? new List<string>();
                List<string> allergens = productInfo.Product.AllergensTags ?? new List<string>();
                string imageSource = productInfo.Product.ImageUrl;

                // Now you can use the extracted information as needed
                Console.WriteLine($"Product Number: {productNumber}");
                Console.WriteLine($"Ingredients: {string.Join(", ", ingredients)}");
                Console.WriteLine($"Allergens: {string.Join(", ", allergens)}");
                Console.WriteLine($"Image Source: {imageSource}");
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
        public ProductDetails Product { get; set; }
    }

    public class ProductDetails
    {
        public string IngredientsText { get; set; }
        public List<string> AllergensTags { get; set; }
        public string ImageUrl { get; set; }
    }




}