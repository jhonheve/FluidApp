using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FluidApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //if (CrossMedia.Current.IsTakePhotoSupported)
            //{
            //    var img = await CrossMedia.Current.PickPhotoAsync();
            //    if (img!=null)
            //    {
            //        LoadedPicture.Source = ImageSource.FromStream(() =>
            //        {
            //            var stream = img.GetStream();
            //            img.Dispose();
            //            return stream;
            //        });
            //    }
            //}

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                var storePicture = new StoreCameraMediaOptions()
                {
                    SaveToAlbum = true,
                    Name = "DocumentRequested.jpg",
                    AllowCropping = true,
                    DefaultCamera = CameraDevice.Rear,
                    SaveMetaData=true
                };

                //if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                //{
                //    DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                //    return;
                //}

                //var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                //{

                //    SaveToAlbum = true,
                //    CompressionQuality = 75,
                //    PhotoSize = PhotoSize.MaxWidthHeight,
                //    MaxWidthHeight = 2000,
                //    DefaultCamera = CameraDevice.Front
                //});

                var photo = await CrossMedia.Current.TakePhotoAsync(storePicture);
                LoadedPicture.Source = ImageSource.FromStream(() =>
                {
                    var stream = photo.GetStream();
                    photo.Dispose();
                    return stream;

                });
            }
            catch (Exception ex)
            {

            }
        }
    }
}
