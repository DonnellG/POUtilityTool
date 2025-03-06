using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POUtilityTool.ViewModels.Commands
{
    public class PPTBrowseFileCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public UserInfoViewModel VM { get; set; }

        public PPTBrowseFileCommand(UserInfoViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.PPTBrowseFile();
        }
    }
}
