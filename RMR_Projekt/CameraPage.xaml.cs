using Microsoft.Maui.ApplicationModel;

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

        private void Button_Clicked(object sender, EventArgs e)
        {
			myImage.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
        }
    }
}