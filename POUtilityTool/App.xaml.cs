using POUtilityTool.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace POUtilityTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow() 
            {
                //DataContext = new POUtilityToolViewModel()
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }

}
