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
            var userViewModel = new UserViewModel();
            userViewModel.Navigation = Navigation;
            BindingContext = userViewModel;
        }

        public IdentityValidation(UserViewModel userViewModel)
		{
            InitializeComponent();
            userViewModel = new UserViewModel();
            userViewModel.Navigation = Navigation;
            BindingContext = userViewModel;    
        }
    }
}