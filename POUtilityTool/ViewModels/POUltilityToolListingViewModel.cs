using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;

namespace POUtilityTool.ViewModels
{
    public class POUltilityToolListingViewModel : ViewModelBase
    {
        private POUtilityToolViewModel pOUtilityToolViewModel;

        private readonly ObservableCollection<POUtilityToolListingItemsViewModel> _pOUtilityToolListingItemsViewModel;
        public IEnumerable<POUtilityToolListingItemsViewModel> POUtilityToolListingItemsViewModel => _pOUtilityToolListingItemsViewModel;
        private POUtilityToolListingItemsViewModel selectedListingItem;

        public POUtilityToolListingItemsViewModel SelectedListingItem
        {
            get { return selectedListingItem; }
            set
            {
                selectedListingItem = value;
                OnPropertyChanged("SelectedListingItem");
                SwitchInfoViews();
                
            }
        }


        public POUltilityToolListingViewModel(POUtilityToolViewModel pOUtilityToolViewModel)
        {
            this.pOUtilityToolViewModel = pOUtilityToolViewModel;

            _pOUtilityToolListingItemsViewModel = new ObservableCollection<POUtilityToolListingItemsViewModel>();
            _pOUtilityToolListingItemsViewModel.Add(new POUtilityToolListingItemsViewModel("Features"));
            _pOUtilityToolListingItemsViewModel.Add(new POUtilityToolListingItemsViewModel("Work Items"));
            //_pOUtilityToolListingItemsViewModel.Add(new POUtilityToolListingItemsViewModel("Current Sprint"));
            //_pOUtilityToolListingItemsViewModel.Add(new POUtilityToolListingItemsViewModel("Future Sprint"));

        }

        //Methods
        private void SwitchInfoViews()
        {
            pOUtilityToolViewModel.DevOpsInfoVis = Visibility.Collapsed;
            pOUtilityToolViewModel.FeatureVis = Visibility.Collapsed;
            pOUtilityToolViewModel.WorkItemsVis = Visibility.Collapsed;

            if (SelectedListingItem != null)
            {
                pOUtilityToolViewModel.DevOpsInfoVis = selectedListingItem.CategoryName == "DevOps" ? Visibility.Visible : Visibility.Collapsed;
                pOUtilityToolViewModel.FeatureVis = selectedListingItem.CategoryName == "Features" ? Visibility.Visible : Visibility.Collapsed;
                pOUtilityToolViewModel.WorkItemsVis = selectedListingItem.CategoryName == "Work Items" ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
