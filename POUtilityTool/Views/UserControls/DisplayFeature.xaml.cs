using POUtilityTool.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POUtilityTool.Views.UserControls
{
    /// <summary>
    /// Interaction logic for DisplayFeature.xaml
    /// </summary>
    public partial class DisplayFeature : UserControl
    {
        public Feature Feature //dependency property propdp
        {
            get { return (Feature)GetValue(FeatureProperty); }
            set { SetValue(FeatureProperty, value); }
        }
        public static readonly DependencyProperty FeatureProperty =
            DependencyProperty.Register("Feature",typeof (Feature), typeof(DisplayFeature), new PropertyMetadata(null, SetValues));
        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DisplayFeature featureUserControl = d as DisplayFeature;
            if (featureUserControl != null)
            {
                featureUserControl.DataContext = featureUserControl.Feature;
            }
        }
        public DisplayFeature()
        {
            InitializeComponent();
        }
    }
}
