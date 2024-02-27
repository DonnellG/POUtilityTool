using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POUtilityTool.ViewModels
{
    class POUltilityToolListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<POUtilityToolListingItemsViewModel> _pOUtilityToolListingItemsViewModel;
        public IEnumerable<POUtilityToolListingItemsViewModel> POUtilityToolListingItemsViewModel => _pOUtilityToolListingItemsViewModel;

        public POUltilityToolListingViewModel()
        {
            _pOUtilityToolListingItemsViewModel = new ObservableCollection<POUtilityToolListingItemsViewModel>();
            _pOUtilityToolListingItemsViewModel.Add(new POUtilityToolListingItemsViewModel("Work Items"));
            _pOUtilityToolListingItemsViewModel.Add(new POUtilityToolListingItemsViewModel("Features"));
            _pOUtilityToolListingItemsViewModel.Add(new POUtilityToolListingItemsViewModel("Current Sprint"));
            _pOUtilityToolListingItemsViewModel.Add(new POUtilityToolListingItemsViewModel("Future Sprint"));

        }
    }
}
