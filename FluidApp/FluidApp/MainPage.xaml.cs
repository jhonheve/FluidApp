using FluidApp.Helpers;
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

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                pictureDocument = await Common.LoadCamara("document");
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

            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
