using Plugin.Media.Abstractions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Onfido;
using Xamarin.Forms;
using FluidApp.Helpers;
using System.Diagnostics;
using System.Windows.Input;
using System.IO;
using FluidApp.Entities;
using System.Collections.Generic;

namespace FluidApp.FluidAppViewModel
{
    public class DocumentViewModel : ManagementViewModel
    {
        public ICommand NextSelfiePageHandler { get; private set; }
        public ICommand LoadDocumentHandler { get; private set; }

        private ImageSource documentResource;
        public ImageSource DocumentResource
        {
            get { return documentResource; }
            set
            {
                if (documentResource != value)
                {
                    documentResource = value;
                    OnPropertyChanged("DocumentResource");
                }
            }
        }
        public MediaFile DocumentSource { get; set; }

        public string DocumentType { get; set; }


        public DocumentViewModel(string applicantId, string docType)
        {
            ApplicationId = applicantId;
            DocumentType = docType;
            NextSelfiePageHandler = new RelayCommandHandler(SelfiePage);
            LoadDocumentHandler = new RelayCommandHandler(LoadDocument);
        }

        private bool SaveDocument()
        {
            var urli = string.Format("https://api.onfido.com/v2/applicants/{0}/documents", ApplicationId);
            var httpContents = new List<ParameterEntity>()
                {
                    new ParameterEntity
                    {
                        httpContent = new StringContent(DocumentType), Name= "\"type\""}
                };


            var fileContent = new StreamContent(DocumentSource.GetStream());
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = "documentReview.png"
            };

            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            httpContents.Add(new ParameterEntity { httpContent = fileContent });

            var authorization = string.Format("Token token={0}", Token);
            var response = Common.SendRequest(urli, httpContents, authorization, HttpMethod.Post);
            if (response)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async void LoadDocument(object obj)
        {
            try
            {
                DocumentSource = await Common.LoadCamara("document");
                DocumentResource = ImageSource.FromStream(() =>
                {
                    var stPassport = DocumentSource.GetStream();
                    return stPassport;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Camera error: " + ex.Message);
            }
        }

        private async void SelfiePage(object obj)
        {
            if (DocumentResource != null && SaveDocument())
            {
                await Navigation.PushAsync(new SelfieDoc(ApplicationId));
            }
        }
    }
}
