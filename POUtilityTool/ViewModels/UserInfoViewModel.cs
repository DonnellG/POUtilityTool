using Microsoft.Win32;
using POUtilityTool.Model;
using POUtilityTool.ViewModels.Commands;
using POUtilityTool.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POUtilityTool.ViewModels
{
    public class UserInfoViewModel : ViewModelBase
    {
        public POUtilityToolViewModel POUtilityToolViewModel { get; set; }
        public QueryHelper QueryHelper { get; set; }
        private UserInfo userInfo;

        public UserInfo UserInfo
        {
            get { return userInfo; }
            set
            {
                userInfo = value;
                OnPropertyChanged("UserInfo");
            }
        }

        private string personalAccessToken;

        public string PersonalAccessToken
        {
            get { return personalAccessToken; }
            set
            {
                personalAccessToken = value;
                UserInfo = UpdateUserInfo("PersonalAccessToken", personalAccessToken);
                OnPropertyChanged("PersonalAccessToken");
            }
        }
        private string orgName;

        public string OrgName
        {
            get { return orgName; }
            set
            {
                orgName = value;
                UserInfo = UpdateUserInfo("OrgName", orgName);
                OnPropertyChanged("OrgName");
            }
        }
        private string project;

        public string Project
        {
            get { return project; }
            set
            {
                project = value;
                UserInfo = UpdateUserInfo("ProjectName", project);
                OnPropertyChanged("Project");
            }
        }

        private string areaPath;

        public string AreaPath
        {
            get { return areaPath; }
            set
            {
                areaPath = value;
                UserInfo = UpdateUserInfo("AreaPath", areaPath);
                OnPropertyChanged("AreaPath");
            }
        }
        private string excelFilePath;

        public string ExcelFilePath
        {
            get { return excelFilePath; }
            set
            {
                excelFilePath = value;
                UserInfo = UpdateUserInfo("ExcelFilePath", excelFilePath);
                OnPropertyChanged("ExcelFilePath");
            }
        }
        private string pptFilePath;

        public string PPTFilePath
        {
            get { return pptFilePath; }
            set
            {
                pptFilePath = value;
                UserInfo = UpdateUserInfo("PPTFilePath", pptFilePath);
                OnPropertyChanged("PPTFilePath");
            }
        }
        private string authenticationResponse;

        public string AuthenticationResponse
        {
            get { return authenticationResponse; }
            set
            {
                authenticationResponse = value;
                OnPropertyChanged("AuthenticationResponse");
            }
        }

        //Commands
        public SaveUserInfoCommand SaveUserInfoCommand { get; set; }
        public ExcelBrowseFileCommand ExcelBrowseFileCommand { get; set; }
        public PPTBrowseFileCommand PPTBrowseFileCommand { get; set; }

        //*******************Constructor******************
        public UserInfoViewModel()
        {
            SaveUserInfoCommand = new SaveUserInfoCommand(this);
            ExcelBrowseFileCommand = new ExcelBrowseFileCommand(this);
            PPTBrowseFileCommand = new PPTBrowseFileCommand(this);

            //Initial Values
            UserInfo = new UserInfo();
            personalAccessToken = "";
            orgName = "Graitec";
            project = "Proj CANAM Steel";
            areaPath = "Bucket 3";
            excelFilePath = @"C:\Users\DonnellGrantham\Graitec\Canam Steel Corp (EXTERNAL) - General\PO Technical Documents\Testing\ONYX Completion Test.xlsx";
            pptFilePath = @"C:\Users\DonnellGrantham\Graitec\Canam Steel Corp (EXTERNAL) - General\PO Technical Documents\Testing\Progress Cards Test.pptx";


            AuthenticationResponse = "Donnell Is Right";

        }

        //Methods
        public string BrowseFile(string title, string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = title;
            openFileDialog.Filter = filter;
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "";
        }
        public void ExcelBrowseFile()
        {
            ExcelFilePath = BrowseFile("Select an Excel File", "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls|All Files (*.*)|*.*");
        }
        public void PPTBrowseFile()
        {
            PPTFilePath = BrowseFile("Select a PPT File", "PowerPoint Files (*.ppt;*.pptx;*.pps;*.ppsx)|*.ppt;*.pptx;*.pps;*.ppsx|All Files (*.*)|*.*");
        }
        private UserInfo UpdateUserInfo(string propName, string value)
        {
            QueryHelper = new QueryHelper();
            UserInfo newUserInfo = new UserInfo
            {
                PersonAccessToken = propName == "PersonAccessToken" ? value : this.PersonalAccessToken,
                OrgName = propName == "OrgName" ? value : this.OrgName,
                ProjectName = propName == "ProjectName" ? value : this.Project,
                AreaPath = propName == "AreaPath" ? value : this.AreaPath,
                ExcelFilePath = propName == "ExcelFilePath" ? value : this.ExcelFilePath,
                PPTFilePath = propName == "PPTFilePath" ? value : pptFilePath
            };

            return newUserInfo;
        }

        public async void SaveUserInfo()
        {
            QueryHelper.PAT = personalAccessToken;
            QueryHelper.OrgName = orgName;
            AuthenticationResponse = await QueryHelper.CheckAuthentication();
        }
    }
}
