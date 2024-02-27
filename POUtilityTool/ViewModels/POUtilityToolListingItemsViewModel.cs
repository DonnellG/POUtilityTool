using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POUtilityTool.ViewModels
{
    public class POUtilityToolListingItemsViewModel
    {
        public string CategoryName { get; }
        public ICommand CheckSubjectChannelsCommand { get; }
        public ICommand OverwriteExcelCommand { get; }
        public ICommand UpdateExcelCommand { get; }
        public ICommand GatherInformationCommand { get; }
        public ICommand GenerateProgressCardInformation { get; }

        public POUtilityToolListingItemsViewModel(string categoryName)
        {
            CategoryName = categoryName;
        }

    }
}
