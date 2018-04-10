using FluidApp.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FluidApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelfieDoc : ContentPage
	{
		public SelfieDoc ()
		{
			InitializeComponent ();
		}

        public SelfieDoc(UserViewModel userViewModel)
        {
            BindingContext = userViewModel;
            InitializeComponent();
        }

        //private async void Button_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        imgSelfieFile = await Common.LoadCamara("selfie");
        //        selfieFile.Source = ImageSource.FromStream(() =>
        //        {
        //            var stSelfie = imgSelfieFile.GetStream();
        //            return stSelfie;

        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("Camera fail", "The camara has not loaded correctly", "Ok");
        //        Debug.WriteLine("Camera error: " + ex.Message);
        //    }
        //}

        //private void Next_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var api = new Onfido.Api();
        //        var documents = new Onfido.Resources.Documents();                
        //        var document2 = api.Documents.Create(app.Id, pictureSelfie.GetStream(), "selfie.png", Onfido.Entities.DocumentType.Unknown);

        //        var checks = new Onfido.Resources.Checks();
        //        var check = new Check
        //        {
        //            Type = CheckType.Express,
        //            Reports = new List<Report>
        //            {
        //                new Report { Name = "identity", Variant="kyc"}
        //            }
        //        };

        //        var new_check = checks.Create(app.Id, check);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}