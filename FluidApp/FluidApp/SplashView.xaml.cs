namespace FluidApp
{
    using FluidApp.FluidAppViewModel;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SplashView : ContentPage
	{
		public SplashView ()
		{
            var rootViewModel = new RootViewModel();
            BindingContext = rootViewModel;
            rootViewModel.Navigation = Navigation;
            InitializeComponent();
		}
	}
}