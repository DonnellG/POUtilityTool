using POUtilityTool.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace POUtilityTool.ViewModels
{
    public class POUtilityToolViewModel : ViewModelBase
    {
        //ViewModels
        public POUltilityToolListingViewModel POUltilityToolListingViewModel { get; }
        public POUltilityToolDetailsViewModel POUltilityToolDetailsViewModel { get; }
        public DevOpsInfoViewModel DevOpsInfoViewModel { get; set; }
        public FeaturesViewModel FeaturesViewModel { get; set; }
        public WorkItemsViewModel WorkItemsViewModel { get; set; }
        public ShowDevOpsInfoCommand ShowDevOpsInfoCommand { get; set; }

        //Controlling Visibility
        private Visibility devOpsInfoVis;

        public Visibility DevOpsInfoVis
        {
            get { return devOpsInfoVis; }
            set
            {
                devOpsInfoVis = value;
                OnPropertyChanged("DevOpsInfoVis");
            }
        }

        private Visibility featureVis;

        public Visibility FeatureVis
        {
            get { return featureVis; }
            set
            {
                featureVis = value;
                OnPropertyChanged("FeatureVis");
            }
        }

        private Visibility workItemsVis;

        public Visibility WorkItemsVis
        {
            get { return workItemsVis; }
            set
            {
                workItemsVis = value;
                OnPropertyChanged("WorkItemsVis");
            }
        }


        public POUtilityToolViewModel()
        {
            //Init Models
            POUltilityToolListingViewModel = new POUltilityToolListingViewModel(this);
            POUltilityToolDetailsViewModel = new POUltilityToolDetailsViewModel();
            DevOpsInfoViewModel = new DevOpsInfoViewModel();
            FeaturesViewModel = new FeaturesViewModel();
            WorkItemsViewModel = new WorkItemsViewModel();
            ShowDevOpsInfoCommand = new ShowDevOpsInfoCommand(this);

            //Visibility Init
            DevOpsInfoVis = Visibility.Visible;
            FeatureVis = Visibility.Collapsed;
            WorkItemsVis = Visibility.Collapsed;
        }

        public void ShowDevOpsInfo()
        {
            POUltilityToolListingViewModel.SelectedListingItem = null;

            DevOpsInfoVis = Visibility.Visible;
            FeatureVis = Visibility.Collapsed;
            WorkItemsVis = Visibility.Collapsed;
        }
    }
}
