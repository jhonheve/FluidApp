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
		public IdentityValidation(UserViewModel userViewModel)
		{
            BindingContext = userViewModel;
            InitializeComponent();
        }        
    }
}