namespace FluidApp
{
    using FluidApp.FluidAppViewModel;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelfieDoc : ContentPage
    {
        public SelfieDoc(string documentType, string applicationId)
        {
            var selfieViewModel = new SelfieViewModel(documentType, applicationId);
            selfieViewModel.LoadSelfie(null);
            selfieViewModel.Navigation = Navigation;
            BindingContext = selfieViewModel;
            InitializeComponent();           
        }

        public SelfieDoc(string applicationId)
        {
            var selfieViewModel = new SelfieViewModel("", applicationId);
            selfieViewModel.LoadSelfie(null);
            selfieViewModel.Navigation = Navigation;
            BindingContext = selfieViewModel;
            InitializeComponent();
           
        }
    }
}