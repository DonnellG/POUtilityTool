using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POUtilityTool.ViewModels
{
    class POUtilityToolViewModel : ViewModelBase
    {
        public POUltilityToolListingViewModel POUltilityToolListingViewModel { get; }
        public POUltilityToolDetailsViewModel POUltilityToolDetailsViewModel { get; }
        
        public ICommand EditUserDetailsCommand { get; }

        public POUtilityToolViewModel()
        {
            POUltilityToolListingViewModel = new POUltilityToolListingViewModel();
            POUltilityToolDetailsViewModel = new POUltilityToolDetailsViewModel();
        }

    }
}
