namespace FluidApp
{
    using FluidApp.Entities;
    using FluidApp.Helpers;
    using Newtonsoft.Json;
    using Onfido;
    using Onfido.Entities;
    using Plugin.Media.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class UserViewModel : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Commands
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand NextDocuments { get; private set; }
        public ICommand NextSelfiePageHandler { get; private set; }
        public ICommand LoadDocumentHandler { get; private set; }
        public ICommand SelfieHandler { get; private set; }
        public ICommand FinishProcessHandler { get; private set; }
        #endregion

        #region Properties        

        private string messageDetails;
        public string MessageDetails
        {
            get { return messageDetails; }
            set
            {
                if (messageDetails != value)
                {
                    messageDetails = value;
                    OnPropertyChanged("MessageDetails");
                }
            }
        }



        public string AppId { get; set; }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;

                    OnPropertyChanged("Email");
                }
            }
        }

        private CountryApi country;
        public CountryApi Country
        {
            get
            {
                return country;
            }
            set
            {
                if (country != value)
                {
                    country = value;

                    OnPropertyChanged("Country");
                }
            }
        }

        private string identificationType;
        public string IdentificationType
        {
            get { return identificationType; }
            set
            {
                if (identificationType != value)
                {
                    identificationType = value;
                    OnPropertyChanged("IdentificationType");
                }
            }
        }

        private string idenficationFileFront;
        public string IdenficationFileFront
        {
            get { return idenficationFileFront; }
            set
            {
                if (idenficationFileFront != value)
                {
                    idenficationFileFront = value;
                    OnPropertyChanged("IdenficationFileFront");
                }
            }
        }

        private string idenficationFileBack;
        public string IdenficationFileBack
        {
            get { return idenficationFileBack; }
            set
            {
                if (idenficationFileBack != value)
                {
                    idenficationFileBack = value;
                    OnPropertyChanged("IdenficationFileBack");
                }
            }
        }

        private DateTime? dayOfBirth;
        public DateTime? DayOfBirth
        {
            get { return dayOfBirth; }
            set
            {
                if (dayOfBirth != value)
                {
                    dayOfBirth = value;
                    OnPropertyChanged("DayOfBirth");
                }
            }
        }

        public DateTime MaxDateTime { get; set; }

        private string cellPhone;
        public string CellPhone
        {
            get { return cellPhone; }
            set
            {
                if (cellPhone != value)
                {
                    cellPhone = value;
                    OnPropertyChanged("CellPhone");
                }
            }
        }

        private string selfie;
        public string Selfie
        {
            get { return selfie; }
            set
            {
                if (selfie != value)
                {
                    selfie = value;
                    OnPropertyChanged("Selfie");
                }
            }
        }

        public MediaFile DocumentSource { get; set; }

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

        private ImageSource selfieResource;
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

        public INavigation Navigation { get; internal set; }

        public List<string> DocumentTypes { get; set; }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    OnPropertyChanged("ErrorMessage");
                }
            }
        }

        private ObservableCollection<CountryApi> countries;
        public ObservableCollection<CountryApi> Countries
        {
            get
            {
                return countries;
            }
            set
            {
                if (countries != value)
                {
                    countries = value;
                    OnPropertyChanged("Countries");
                }
            }
        }

        #endregion

        #region Constructor
        public UserViewModel()
        {
            Onfido.Settings.SetApiToken("test_rKbkzSuHC8YnDNCDoBpZP1BhlevqEptU");
            Settings.SetApiVersion("v2");
            //AppId = "d2491943-f3a2-4ad7-b8b3-3126b7722c39";

            NextDocuments = new RelayCommandHandler(UploadDocuments);
            NextSelfiePageHandler = new RelayCommandHandler(SelfiePage);
            LoadDocumentHandler = new RelayCommandHandler(LoadDocument);

            DocumentTypes = new List<string>();
            DocumentTypes.Add("Driving Licence");
            DocumentTypes.Add("Passport");
            MaxDateTime = DateTime.Now.AddYears(-1);
            DayOfBirth = DateTime.Now.AddYears(-1);
            GetAllCountries();
        }
        #endregion

        #region PrivateMethod

        public List<Applicants> GetAllApplication()
        {
            var client = new Onfido.Http.OnfidoHttpClient();
            var uri = new Uri("https://api.onfido.com/v2/applicants");
            var httpResponse = client.Get(uri);
            var responseStr = httpResponse.Content.ReadAsStringAsync().Result;
            List<Applicants> applicants = new List<Applicants>();
            if (responseStr != null)
            {
                var node = responseStr.Replace("{\"applicants\":", "");
                node = node.TrimEnd('}');
                applicants = JsonConvert.DeserializeObject<List<Applicants>>(node);
            }
            return applicants;
        }

        private async void GetAllCountries()
        {
            try
            {
                var client = new Onfido.Http.OnfidoHttpClient();
                var uri = new Uri("http://restcountries.eu/rest/v2/all");
                var httpResponse = client.Get(uri);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseStr = httpResponse.Content.ReadAsStringAsync().Result;
                    var countries_api = JsonConvert.DeserializeObject<List<CountryApi>>(responseStr);
                    Countries = new ObservableCollection<CountryApi>(countries_api);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Accept");
            }
        }

        private async void UploadDocuments(object obj)
        {
            if (HasCorrectFormatDatails())
            {
                MessageDetails = string.Empty;
                if (CreateApp())
                {
                    if (string.IsNullOrWhiteSpace(MessageDetails))
                    {
                        var inst = new MainPage(IdentificationType, AppId);
                        await Navigation.PushAsync(inst);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Processing", MessageDetails, "Accept");
                    }
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Processing", MessageDetails, "Accept");
            }
        }

        public bool HasCorrectFormatDatails()
        {
            string messege = string.Empty;
            var numberExpression = @"^(\+[0-9]{9,12})$";

            if (string.IsNullOrWhiteSpace(FirstName) || FirstName.Length < 3)
            {
                messege = "The firstname doesnot have the correct format";
            }
            else if (string.IsNullOrWhiteSpace(LastName) || LastName.Length < 3)
            {
                messege = "The lastname doesnot have the correct format";
            }
            else if (Country == null || string.IsNullOrWhiteSpace(Country.alpha3Code))
            {
                messege = "The Country has not been selected";
            }
            else if (string.IsNullOrWhiteSpace(IdentificationType))
            {
                messege = "The document Type has not been selected";
            }

            else if (string.IsNullOrEmpty(CellPhone) || !Regex.Match(CellPhone, numberExpression).Success)
            {
                messege = "The Mobile does not have the correct format (+5113423234)";
            }

            var MatchEmailPattern =
                    @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                    + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                    + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                    + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

            if (!(!string.IsNullOrWhiteSpace(Email) && Regex.IsMatch(email, MatchEmailPattern)))
            {
                messege = "The Email does not have the correct format";
            }

            MessageDetails = messege;
            var isValid = string.IsNullOrEmpty(messege);
            return isValid;
        }

        private async void SelfiePage(object obj)
        {
            if (DocumentResource != null && SaveDocument())
            {
                await Navigation.PushAsync(new SelfieDoc(AppId));
            }
        }

        //private bool SaveDocument()
        //{
        //    try
        //    {
        //        var api = new Onfido.Api();
        //        var docType = DocumentType.NationalIdentityCard;
        //        if (IdentificationType == "Passport")
        //        {
        //            docType = DocumentType.Passport;
        //        }
        //        else if (IdentificationType == "Driver's License")
        //        {
        //            docType = DocumentType.Passport;
        //        }
        //        var stFile = DocumentSource.GetStream();

        //        var documentApi = api.Documents.Create(AppId, stFile, "document180430033521.jpg", docType);

        //        return (documentApi.Id != null);
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        private bool SaveDocument()
        {
            try
            {
                var docType = IdentificationType.Replace(" ", "_");

                using (var formData = new MultipartFormDataContent())
                {
                    var urli = string.Format("https://api.onfido.com/v2/applicants/{0}/documents", AppId);
                    var uri = new Uri(urli);

                    var fileContent = new StreamContent(DocumentSource.GetStream());
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "file",
                        FileName = "Document.png"
                    };
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                    formData.Add(fileContent);

                    formData.Add(new StringContent(IdentificationType), "\"type\"");

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = uri,
                        Method = HttpMethod.Post,
                        Content = formData
                    };
                    var client = new HttpClient();
                    request.Headers.Add("Authorization", string.Format("Token token={0}", Settings.GetApiToken()));
                    var response = client.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
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


        private bool CreateApp()
        {
            var api = new Onfido.Api();
            var applicant = new Onfido.Entities.Applicant
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                DateOfBirth = DayOfBirth.GetValueOrDefault(DateTime.Now.AddDays(-1)),
                Mobile = CellPhone,
                Country = Country.alpha3Code
            };

            try
            {
                Applicant app = new Onfido.Entities.Applicant();
                if (!string.IsNullOrWhiteSpace(AppId))
                {
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", string.Format("Token token={0}", Settings.GetApiToken()));
                    var stUrl = $"https://{Settings.Hostname}/{Settings.GetApiVersion()}/applicants/{AppId}";
                    var uri = new Uri(stUrl);
                    var deleteResponse = client.DeleteAsync(uri);
                    if (deleteResponse.Id == 1)
                    {
                        MessageDetails = deleteResponse.Status.ToString();
                        return false;
                    }
                }
                else
                {
                    app = api.Applicants.Create(applicant);
                    AppId = app.Id;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageDetails = ex.Message;
                return false;
            }
        }
        #endregion

    }
}
