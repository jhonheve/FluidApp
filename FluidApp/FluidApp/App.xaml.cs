namespace FluidApp
{
    using Xamarin.Forms;

    public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
			//MainPage = new NavigationPage(new SplashView());
            MainPage = new NavigationPage(new SelfieDoc("0359318a-2f43-4ab0-944f-3fef2d9c1d23"));
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
