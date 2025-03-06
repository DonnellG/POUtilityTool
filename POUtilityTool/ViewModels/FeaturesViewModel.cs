using POUtilityTool.Model;
using POUtilityTool.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POUtilityTool.ViewModels
{
    public class FeaturesViewModel : ViewModelBase
    {
        public POUtilityToolViewModel POUtilityToolViewModel { get; set; }
        public ObservableCollection<Feature> Features { get; set; }
        public UpdateRanksCommand UpdateRanksCommand { get; set; }
        public FeaturesToExcelCommand FeaturesToExcelCommand { get; set; }
        public GenerateFeaturesCommand GenerateFeaturesCommand { get; set; }
        public FeaturesViewModel(POUtilityToolViewModel pOUtilityToolViewModel)
        {
            POUtilityToolViewModel = pOUtilityToolViewModel;

            UpdateRanksCommand = new UpdateRanksCommand(this);
            GenerateFeaturesCommand = new GenerateFeaturesCommand(this);
            FeaturesToExcelCommand = new FeaturesToExcelCommand(this);

            Features = new ObservableCollection<Feature>();
        }
        public void UpdateRanks()
        {
            POUtilityToolViewModel.QueryHelper.UpdateFeatureRanksByStack(POUtilityToolViewModel.UserInfoViewModel.Project,
                                                                         POUtilityToolViewModel.UserInfoViewModel.AreaPath);
        }
        public async Task GenerateFeaturesAsync()
        {
            ObservableCollection<Feature> generatedFeatures = await POUtilityToolViewModel.QueryHelper.GenerateFeatures(POUtilityToolViewModel.UserInfoViewModel.Project,
                                                                                                                        POUtilityToolViewModel.UserInfoViewModel.AreaPath);
            Features.Clear();
            foreach (Feature feature in generatedFeatures)
            {
                Features.Add(feature);
            }
        }
        public void FeaturesToExcel()
        {
            
        }
    }
}
