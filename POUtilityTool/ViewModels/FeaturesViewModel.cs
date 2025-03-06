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
        public ObservableCollection<Feature> Features { get; set; }
        public FeaturesToExcelCommand FeaturesToExcelCommand { get; set; }
        public GenerateFeaturesCommand GenerateFeaturesCommand { get; set; }
        public FeaturesViewModel()
        {
            GenerateFeaturesCommand = new GenerateFeaturesCommand(this);
            FeaturesToExcelCommand = new FeaturesToExcelCommand(this);

            Features = new ObservableCollection<Feature>();
            Feature donnelljr = new Feature("Donnell Jr.");
            donnelljr.Id = 18372;
            donnelljr.spikeClosedPercentage = 83;
            donnelljr.technicalStoryClosedPercentage = 28;
            donnelljr.userStoryClosedPercentage = 30;
            Features.Add(donnelljr);
            Features.Add(new Feature("Kristina"));
            Features.Add(new Feature("Quentin"));
            Features.Add(new Feature("Donnell Sr."));
            Features.Add(new Feature("Angelina"));
            Features.Add(new Feature("Deandre"));
        }
        public void GenerateFeatures()
        {

        }

        public void FeaturesToExcel()
        {
            
        }
    }
}
