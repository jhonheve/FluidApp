using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FluidApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Welcome : ContentPage
    {

        public Welcome()
        {
            InitializeComponent();
            var userViewModel= new UserViewModel();
            BindingContext = userViewModel;
            userViewModel.Navigation = Navigation;
        }
    }
}