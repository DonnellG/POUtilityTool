using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POUtilityTool.ViewModels
{
    public class WorkItemsViewModel : ViewModelBase
    {
        public POUtilityToolViewModel POUtilityToolViewModel { get; set; }
        public WorkItemsViewModel(POUtilityToolViewModel pOUtilityToolViewModel)
        {
            POUtilityToolViewModel = pOUtilityToolViewModel;
        }
    }
}
