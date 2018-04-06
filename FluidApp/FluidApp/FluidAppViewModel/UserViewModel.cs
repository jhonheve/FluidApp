using Newtonsoft.Json;
using Onfido;
using Onfido.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FluidApp
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand NextWelcome { get; private set; }
        public ICommand NextDocuments { get; private set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Properties        
        //public ICommand NextWelcome { get; private set; }

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

        public string AppId { get; private set; }

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

        private string country;
        public string Country
        {
            get { return country; }
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

        private DateTime dayOfBirth;
        public DateTime DayOfBirth
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

        public INavigation Navigation { get; internal set; }

        public List<string> DocumentTypes { get; set; }
        #endregion

        #region Constructor
        public UserViewModel()
        {
            Onfido.Settings.SetApiToken("test_rKbkzSuHC8YnDNCDoBpZP1BhlevqEptU");
            Settings.SetApiVersion("v2");
            NextWelcome = new RelayCommandHandler(GetApplication);
            NextDocuments = new RelayCommandHandler(UploadDocuments);
            DocumentTypes = new List<string>();
            DocumentTypes.Add("Government ID");
            DocumentTypes.Add("Driver's License");
            DocumentTypes.Add("Passport");
            DayOfBirth = DateTime.Now;
        }
        #endregion

        #region PrivateMethod

        private async void GetApplication(object obj)
        {
            if (!string.IsNullOrWhiteSpace(Email))
            {
                try
                {
                    var api = new Onfido.Api();
                    var apps = GetAllApplication();
                    var app = apps.FirstOrDefault(a => a.Email == Email);
                    if (app != null)
                    {
                        AppId = app.Id;
                        firstName = app.FirstName;
                        lastName = app.LastName;
                        CellPhone = app.Mobile;
                        DayOfBirth = app.DateOfBirth.Value;
                    }
                }
                finally
                {
                    await Navigation.PushAsync(new IdentityValidation(this));
                }
            }
        }

        private List<Applicants> GetAllApplication()
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

        private async void UploadDocuments(object obj)
        {
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(LastName))
            {
                if (CreateApp())
                {
                    await Navigation.PushAsync(new MainPage(this));
                }
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
                DateOfBirth = DayOfBirth,
                Mobile = CellPhone
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
                return false;
            }
        }

        #endregion

    }

    public class Applicants
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("created_at")]
        public DateTime? CreatedAt;

        [JsonProperty("href")]
        public string HRef;

        [JsonProperty("title")]
        public string Title;

        [JsonProperty("first_name")]
        public string FirstName;

        [JsonProperty("middle_name")]
        public string MiddleName;

        [JsonProperty("last_name")]
        public string LastName;

        [JsonProperty("gender")]
        public string Gender;

        [JsonProperty("dob")]
        public DateTime? DateOfBirth;

        [JsonProperty("telephone")]
        public string Telephone;

        [JsonProperty("mobile")]
        public string Mobile;

        [JsonProperty("email")]
        public string Email;

        [JsonProperty("country")]
        public string Country;

    }
}
