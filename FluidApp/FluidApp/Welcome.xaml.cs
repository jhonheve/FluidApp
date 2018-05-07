using FluidApp.FluidAppViewModel;
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
            var rootViewModel = new RootViewModel();
            BindingContext = rootViewModel;
            rootViewModel.Navigation = Navigation;
            InitializeComponent();
        }

        public Welcome(UserViewModel userViewModel)
        {
            InitializeComponent();
            BindingContext = userViewModel;
        }
    }
}