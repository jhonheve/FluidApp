using Onfido;
using Onfido.Entities;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FluidApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private MediaFile pictureDocument;
        private MediaFile pictureSelfie;

        public string DocumentType { get; set; }
        public string Country { get; set; }

        public MainPage(UserViewModel userViewModel)
        {
            BindingContext = userViewModel;
            InitializeComponent();
        }

        public MainPage(string documentType, string country)
        {
            DocumentType = documentType;
            Country = country;
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                pictureSelfie = await LoadCamara("selfie");
                selfieFile.Source = ImageSource.FromStream(() =>
                {
                    var stSelfie = pictureSelfie.GetStream();
                    return stSelfie;

                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Camera fail", "The camara has not loaded correctly", "Ok");
                Debug.WriteLine("Camera error: " + ex.Message);
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                pictureDocument = await LoadCamara("document");
                LoadedPicture.Source = ImageSource.FromStream(() =>
                {
                    var stPassport = pictureDocument.GetStream();                   
                    return stPassport;
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Camera fail", "The camara has not loaded correctly", "Ok");
                Debug.WriteLine("Camera error: " + ex.Message);
            }
        }

        private static async Task<MediaFile> LoadCamara(string fileName)
        {
            Location location = new Location();
            location.Latitude = 52.0271723411551;
            location.Longitude = -0.764563267810425;
            location.Altitude = 84;
            location.HorizontalAccuracy = 165;
            location.Speed = -1;
            location.Direction = -1;

            var storePicture = new StoreCameraMediaOptions()
            {
                SaveToAlbum = false,
                Name = $"{fileName}{DateTime.Now.ToString("yyMMddhhmmss")}.jpg",
                AllowCropping = true,
                DefaultCamera = CameraDevice.Front,
                SaveMetaData = true,
                Location = location
            };
            var photo = await CrossMedia.Current.TakePhotoAsync(storePicture);
            return photo;
        }

        private void Next_Clicked(object sender, EventArgs e)
        {
            try
            {               
                var api = new Onfido.Api();
                var applicant = new Onfido.Entities.Applicant
                {
                    FirstName = "J_123_oh_nVsertergara",
                    LastName = "Smith",
                    Email = "adfqtZte__123_rtasaa@gmddail.com"
                };
                var app = api.Applicants.Create(applicant);

                var documents = new Onfido.Resources.Documents();
                var document = api.Documents.Create(app.Id, pictureDocument.GetStream(), "passport.png", Onfido.Entities.DocumentType.Passport);
                var document2 = api.Documents.Create(app.Id, pictureSelfie.GetStream(), "selfie.png", Onfido.Entities.DocumentType.Unknown);

                var checks = new Onfido.Resources.Checks();
                var check = new Check
                {
                    Type = CheckType.Express,
                    Reports = new List<Report>
                    {
                        new Report { Name = "identity", Variant="kyc"}
                    }
                };

                var new_check = checks.Create(app.Id, check);

            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
