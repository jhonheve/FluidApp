using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FluidApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Welcome : ContentPage
	{
        
        public Welcome ()
		{
			InitializeComponent ();
		}

        private async void Next_Clicked(object sender, EventArgs e)
        {   
            await Navigation.PushAsync(new IdentityValidation());
        }
    }
}