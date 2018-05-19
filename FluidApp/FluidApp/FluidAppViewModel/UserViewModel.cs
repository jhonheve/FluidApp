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
            NextDocuments = new RelayCommandHandler(UploadDocuments);

            DocumentTypes = new List<string>()
            {
                "Driving Licence",
                "Passport",
                "National Identity Card"
            };

            MaxDateTime = DateTime.Now;
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
                        return;
                    }
                }
            }
            await Application.Current.MainPage.DisplayAlert("Processing", MessageDetails, "Accept");
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
            else if (DayOfBirth == null || DayOfBirth.Value.Year == MaxDateTime.Year)
            {
                messege = "The day of birth field is not a valid date";
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
