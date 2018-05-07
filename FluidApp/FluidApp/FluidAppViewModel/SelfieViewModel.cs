using FluidApp.Entities;
using FluidApp.Helpers;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FluidApp.FluidAppViewModel
{
    public class SelfieViewModel : ManagementViewModel
    {


        private ImageSource selfieResource;

        public ICommand SelfieHandler { get; private set; }
        public ICommand FinishProcessHandler { get; private set; }

        public MediaFile SelfieSource { get; set; }

        public ImageSource SelfieResource
        {
            get { return selfieResource; }
            set
            {
                if (selfieResource != value)
                {
                    selfieResource = value;
                    OnPropertyChanged("SelfieResource");
                }
            }
        }
        public string DocumentType { get; set; }

        public SelfieViewModel(string documentType, string applicationId)
        {
            ApplicationId = applicationId;
            DocumentType = documentType;
            SelfieHandler = new RelayCommandHandler(LoadSelfie);
            FinishProcessHandler = new RelayCommandHandler(FinishProcess);

        }

        public async void LoadSelfie(object obj)
        {
            SelfieSource = await Common.LoadCamara("Selfie");
            SelfieResource = ImageSource.FromStream(() =>
            {
                var stBits = SelfieSource.GetStream();
                return stBits;
            });
        }

        private void FinishProcess(object obj)
        {
            if (SelfieResource != null && SaveSelfie())
            {
                CheckRequest();
            }
        }

        private async void CheckRequest2()
        {
            try
            {
                Onfido.Settings.SetApiToken(Token);
                Onfido.Settings.SetApiVersion("v2");

                //var checks = new Onfido.Resources.Checks();
                //var check = new Onfido.Entities.Check
                //{
                //    Type = Onfido.Entities.CheckType.Express,
                //    Reports = new List<Onfido.Entities.Report>
                //    {
                //        new Onfido.Entities.Report { Name = "identity", Variant="kyc"}
                //    }
                //};

                var checks = new Onfido.Resources.Checks();
                var check = new Onfido.Entities.Check
                {
                    Type = Onfido.Entities.CheckType.Standard,
                    Reports = new List<Onfido.Entities.Report>
                    {
                        new Onfido.Entities.Report { Name = "identity" },
                        new Onfido.Entities.Report { Name = "document" }
                    }
                };

                var new_check = checks.Create(ApplicationId, check);


                //var new_check = checks.Create(ApplicationId, check);
                var msg = "Please wait until yuo receive our confirmation\nThank you. We will contact you soon";
                await Application.Current.MainPage.DisplayAlert("Processing", msg, "Accept");
                await Navigation.PushAsync(new SplashView());
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private async void CheckRequest()
        {
            try
            {
                var uri = $"https://api.onfido.com/v2/applicants/{ApplicationId}/checks";
                var httpContents = new List<ParameterEntity>()
                {
                    new ParameterEntity
                    {
                        httpContent = new StringContent("standard"), Name = "\"type\""
                    },
                    new ParameterEntity
                    {
                        httpContent = new StringContent("identity"), Name = "\"reports[][name]\""
                    },
                     new ParameterEntity
                    {
                        httpContent = new StringContent("kyc"), Name = "\"reports[][variant]\""
                    }
                };

                var authorization = string.Format("Token token={0}", Token);
                var response = Common.SendRequest(uri, httpContents, authorization, HttpMethod.Post);
                var msg = string.Empty;
                if (response)
                {
                    msg = "Please wait until you receive our confirmation\nThank you. We will contact you soon";
                }
                else
                {
                    msg = "There was an error validating the data.\nPlease conctact the administrator";
                }

                await Application.Current.MainPage.DisplayAlert("Processing", msg, "Accept");
                var splashView = new SplashView();
                await Navigation.PushAsync(splashView);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private bool SaveSelfie()
        {
            var urlSelfie = "https://api.onfido.com/v2/live_photos";
            var httpContents = new List<ParameterEntity>()
            {
                new ParameterEntity
                {
                    httpContent = new StringContent(ApplicationId), Name = "\"applicant_id\""
                },
                new ParameterEntity
                {
                    httpContent = new StringContent("false"), Name = "\"advanced_validation\""
                }

            };

            var fileContent = new StreamContent(SelfieSource.GetStream());
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = "selfieReview.jpg"
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");

            httpContents.Add(new ParameterEntity { httpContent = fileContent });

            var authorization = string.Format("Token token={0}", Token);
            var response = Common.SendRequest(urlSelfie, httpContents, authorization, HttpMethod.Post);
            if (response)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
