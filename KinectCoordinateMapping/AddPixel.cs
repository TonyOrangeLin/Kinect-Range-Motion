using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KinectCoordinateMapping
{
    static class AddPixel
    {
        static public void AddOnePixel(Canvas canvas, double x, double y)
        {
            Rectangle rec = new Rectangle();
            Canvas.SetTop(rec, y);
            Canvas.SetLeft(rec, x);
            rec.Width = 1;
            rec.Height = 1;
            rec.Fill = new SolidColorBrush(Colors.Red);
            canvas.Children.Add(rec);
        }

        static public void Text(double x, double y, string text, Color color, Canvas canvasObj)
        {

            TextBlock textBlock = new TextBlock();

            textBlock.Text = text;
            textBlock.FontSize = 12;
            //textBlock.Height = 500;

            textBlock.Foreground = new SolidColorBrush(color);

            Canvas.SetLeft(textBlock, x);

            Canvas.SetTop(textBlock, y);

            canvasObj.Children.Add(textBlock);

        }
    }
}
