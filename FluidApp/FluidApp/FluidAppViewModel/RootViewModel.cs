namespace FluidApp.FluidAppViewModel
{
    using Plugin.Connectivity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;

    class RootViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public INavigation Navigation { get; internal set; }
        public ICommand NextSplashHandler { get; private set; }
        public ICommand NextWelcomeHandler { get; private set; }
        
        

        private string accessToken;
        public string AccessToken
        {
            get { return accessToken; }
            set
            {
                if (accessToken != value)
                {
                    accessToken = value;
                    OnPropertyChanged("AccessToken");
                }
            }
        }

        public RootViewModel()
        {   
            NextSplashHandler = new RelayCommandHandler(NextSplash);
            NextWelcomeHandler = new RelayCommandHandler(NextWelcome);
        }

        private async void NextSplash(object obj)
        {
            await Navigation.PushAsync(new Welcome());
        }

        private async void NextWelcome(object obj)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected && !string.IsNullOrWhiteSpace(AccessToken))
                {
                    AccessToken = string.Empty;
                    await Navigation.PushAsync(new IdentityValidation());
                }
            }
            catch (Exception ex)
            {
                var errorMessage = "Please check the WIFI connection is running correctly";
                await Application.Current.MainPage.DisplayAlert("Error", errorMessage, "Accept");
            }
        }
    }
}
