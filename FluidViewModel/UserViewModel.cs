using Onfido;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FluidViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Properties        
        //public ICommand NextWelcome { get; private set; }

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
        #endregion

        #region Constructor
        public UserViewModel()
        {
            Onfido.Settings.SetApiToken("test_rKbkzSuHC8YnDNCDoBpZP1BhlevqEptU");
            Settings.SetApiVersion("v2");
            //NextWelcome = new RelayCommandHandler(GetApplication);
        }
        #endregion

        #region PrivateMethod
        private void GetApplication(object obj)
        {   
            var api = new Onfido.Api();
            var apps = api.Applicants.All();
            var app = apps.Where(a => a.Email == Email).FirstOrDefault();
            if (app != null)
            {
                AppId= app.Id;
                firstName = app.FirstName;
                lastName = app.LastName;
            }
        }
        #endregion

    }
}
