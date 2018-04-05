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
    public partial class IdentityValidation : ContentPage
	{
		public IdentityValidation()
		{
			InitializeComponent();
            IdentityTypes.Items.Add("Government ID");
            IdentityTypes.Items.Add("Driver's License");
            IdentityTypes.Items.Add("Passport");
        }

        private async void Next_Clicked(object sender, System.EventArgs e)
        {
            var firstName = txtFirstName.Text;
            var lastName = txtLastName.Text;
            var country = txtCountry.Text;
            var identityType = IdentityTypes.SelectedItem.ToString();
            await Navigation.PushAsync(new MainPage(identityType, country));
        }
    }
}