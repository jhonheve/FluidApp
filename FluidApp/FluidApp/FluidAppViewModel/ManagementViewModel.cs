using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FluidApp.FluidAppViewModel
{
    public class ManagementViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; internal set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Token { get; set; }
        public string ApplicationId { get; set; }

        public ManagementViewModel()
        {
            Token = "test_rKbkzSuHC8YnDNCDoBpZP1BhlevqEptU";
            Onfido.Settings.SetApiToken(Token);
            Onfido.Settings.SetApiVersion("v2");
        }

    }
}
