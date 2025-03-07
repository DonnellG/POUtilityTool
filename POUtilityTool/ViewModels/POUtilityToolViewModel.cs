using POUtilityTool.ViewModels.Commands;
using POUtilityTool.ViewModels.Helpers;
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
        public QueryHelper QueryHelper { get; set; }
        public ExcelHelper ExcelHelper { get; set; }
        public POUltilityToolListingViewModel POUltilityToolListingViewModel { get; }
        public POUltilityToolDetailsViewModel POUltilityToolDetailsViewModel { get; }
        public UserInfoViewModel UserInfoViewModel { get; set; }
        public FeaturesViewModel FeaturesViewModel { get; set; }
        public WorkItemsViewModel WorkItemsViewModel { get; set; }
        public ShowUserInfoCommand ShowUserInfoCommand { get; set; }

        //Controlling Visibility
        private Visibility userInfoVis;

        public Visibility UserInfoVis
        {
            get { return userInfoVis; }
            set
            {
                userInfoVis = value;
                OnPropertyChanged("UserInfoVis");
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
            QueryHelper = new QueryHelper();  //I need to use the same helper for each Vis... maybe??
            ExcelHelper = new ExcelHelper();

            //Init Models
            POUltilityToolListingViewModel = new POUltilityToolListingViewModel(this);
            POUltilityToolDetailsViewModel = new POUltilityToolDetailsViewModel();
            UserInfoViewModel = new UserInfoViewModel(this);
            FeaturesViewModel = new FeaturesViewModel(this);
            WorkItemsViewModel = new WorkItemsViewModel(this);

            ShowUserInfoCommand = new ShowUserInfoCommand(this);

            //Visibility Init
            UserInfoVis = Visibility.Visible;
            FeatureVis = Visibility.Collapsed;
            WorkItemsVis = Visibility.Collapsed;
        }

        public void ShowUserInfo()
        {
            POUltilityToolListingViewModel.SelectedListingItem = null;

            UserInfoVis = Visibility.Visible;
            FeatureVis = Visibility.Collapsed;
            WorkItemsVis = Visibility.Collapsed;
        }
    }
}