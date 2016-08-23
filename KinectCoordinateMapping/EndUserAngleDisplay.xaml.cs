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
using System.Windows.Shapes;

namespace KinectCoordinateMapping
{
    /// <summary>
    /// Interaction logic for EndUserAngleDisplay.xaml
    /// </summary>
    public partial class EndUserAngleDisplay : Window
    {
        public EndUserAngleDisplay()
        {
            InitializeComponent();
        }

        public void UpdateData(double angle)
        {
            angleTextBlock.Text = angle.ToString("f2"); 
        }
    }

    
}
