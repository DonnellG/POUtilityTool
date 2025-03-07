using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POUtilityTool.ViewModels.Commands
{
    public class FeaturesToExcelCommand : ICommand
    {
        public FeaturesViewModel VM { get; set; }
        public FeaturesToExcelCommand(FeaturesViewModel vm)
        {
            VM = vm;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.FeaturesToExcel();
        }
    }
}
