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
    public class DotMark : MarkBase
    {
        public override void Draw(Canvas canvas, DisplayStruct displayStruct, Target target, ZoomStruct zoomStruct)
        {
            int transformX;
            int transformY;

            Point tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)target.point2D().X, (int)target.point2D().Y, zoomStruct);
            transformX = (int)tempPoint.X;
            transformY = (int)tempPoint.Y;

            AddPixel.AddOnePixel(canvas, transformX, transformY);  
        }
    }
}
