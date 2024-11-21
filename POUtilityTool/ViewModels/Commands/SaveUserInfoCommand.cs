using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using POUtilityTool.Model;
using POUtilityTool.ViewModels.Helpers;

namespace POUtilityTool.ViewModels.Commands
{
    public class SaveUserInfoCommand : ICommand
    {
        public UserInfoViewModel VM { get; set; }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public SaveUserInfoCommand(UserInfoViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object? parameter)
        {
            //UserInfoViewModel viewModel = parameter as UserInfoViewModel;
            //if (viewModel == null) { return false; }
            //UserInfo userInfo = viewModel.UserInfo;
            UserInfo userInfo = parameter as UserInfo;
            if (userInfo == null) {return false;}
            if (string.IsNullOrEmpty(userInfo.PersonAccessToken)) {return false;}
            if (string.IsNullOrEmpty(userInfo.ProjectName)) { return false; }
            if (string.IsNullOrEmpty(userInfo.OrgName)) { return false; }
            if (string.IsNullOrEmpty(userInfo.AreaPath)) { return false; }
            //if (string.IsNullOrEmpty(userInfo.ExcelFilePath)) { return false; }
            //if (string.IsNullOrEmpty(userInfo.PPTFilePath)) { return false; }
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.SaveUserInfo();
        }
    }
}
