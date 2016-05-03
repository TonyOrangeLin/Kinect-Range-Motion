using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KinectCoordinateMapping.Mark
{
    public class BallMark : MarkBase
    {
        public override void Draw(Canvas canvas, DisplayStruct displayStruct, Target target, ZoomStruct zoomStruct)
        {
            int transformX;
            int transformY;

            Point tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)target.point2D().X, (int)target.point2D().Y, zoomStruct);
            transformX = (int)tempPoint.X;
            transformY = (int)tempPoint.Y;

            if (target.IsTracked())
            {
                Ellipse ellipse = new Ellipse
                {
                    Fill = Brushes.Red,
                    Width = displayStruct.ballsize,
                    Height = displayStruct.ballsize
                };
                Canvas.SetLeft(ellipse, transformX - ellipse.Width / 2);
                Canvas.SetTop(ellipse, transformY - ellipse.Height / 2);
                canvas.Children.Add(ellipse);
            }
            else if (!target.IsTracked())
            {
                Ellipse ellipse = new Ellipse
                {
                    Fill = Brushes.Blue,
                    Width = displayStruct.ballsize,
                    Height = displayStruct.ballsize
                };
                Canvas.SetLeft(ellipse, transformX - ellipse.Width / 2);
                Canvas.SetTop(ellipse, transformY - ellipse.Height / 2);
                canvas.Children.Add(ellipse);
            }
        }
    }
}
