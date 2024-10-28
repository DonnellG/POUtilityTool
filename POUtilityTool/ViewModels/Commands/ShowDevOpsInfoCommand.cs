using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POUtilityTool.ViewModels.Commands
{
    public class ShowDevOpsInfoCommand : ICommand
    {
        public POUtilityToolViewModel VM { get; set; }
        public event EventHandler? CanExecuteChanged;

        public ShowDevOpsInfoCommand(POUtilityToolViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.ShowDevOpsInfo();
        }
    }
}
