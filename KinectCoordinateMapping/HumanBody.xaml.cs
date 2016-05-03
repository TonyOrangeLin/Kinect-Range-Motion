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
    /// Interaction logic for HumanBody.xaml
    /// </summary>
    
    public partial class HumanBody : Window
    {
        public SolidColorBrush greenBrush = new SolidColorBrush(Color.FromRgb(90, 255, 105));
        public SolidColorBrush redBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        public SolidColorBrush yellowBrush = new SolidColorBrush(Color.FromRgb(255, 255, 90));
        public HumanBody()
        {
            InitializeComponent();
            Reset();
        }

        public void Reset()
        {
            button2_front.Background = redBrush;

            button3_front.Background = redBrush;

            button5_front.Background = redBrush;

            button6_front.Background = redBrush;

            button12_front.Background = redBrush;

            button13_front.Background = redBrush;

            button14_front.Background = redBrush;

            button15_front.Background = redBrush;

            button17_front.Background = redBrush;

            button20_front.Background = redBrush;

            button16_front.Background = redBrush;

            button19_front.Background = redBrush;

            button18_front.Background = redBrush;

            button21_front.Background = redBrush;

            button23_front.Background = redBrush;

            button26_front.Background = redBrush;

            button22_front.Background = redBrush;

            button25_front.Background = redBrush;

            button8_back.Background = redBrush;

            button7_back.Background = redBrush;

            button17_back.Background = redBrush;

            button32_back.Background = redBrush;

            button33_back.Background = redBrush;

            button35_back.Background = redBrush;

            button39_back.Background = redBrush;

            button41_back.Background = redBrush;

            button37_back.Background = redBrush;

            button2_side.Background = redBrush;

            button5_side.Background = redBrush;

            button8_side.Background = redBrush;

            button21_side.Background = redBrush;

            button22_side.Background = redBrush;

            button23_side.Background = redBrush;

            button24_side.Background = redBrush;

            button30_side.Background = redBrush;

            button31_side.Background = redBrush;

        }
    }
}
