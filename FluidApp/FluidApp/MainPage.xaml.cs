using FluidApp.FluidAppViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace FluidApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage(string docType, string appId)
        {
            var documentViewModel = new DocumentViewModel(appId,docType);
            documentViewModel.LoadDocument("");
            documentViewModel.Navigation = Navigation;
            InitializeComponent();
            BindingContext = documentViewModel;
            
        }
    }
}