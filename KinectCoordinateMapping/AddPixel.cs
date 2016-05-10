using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        
        static public void DrawCurve(int startX, int startY, int endX, int endY, Brush brushes, Canvas canvas)
        {
            if (endX > startX)
            {

            }
            else
            {
                int temp = endX;
                endX = startX;
                startX = temp;
            }
            if (endY > startY)
            {

            }
            else
            {
                int temp = endY;
                endY = startY;
                startY = temp;
            }
            Path tempPath = new System.Windows.Shapes.Path();
            tempPath.Fill = brushes;
            PathGeometry pG = new PathGeometry();
            PathFigureCollection pfC = new PathFigureCollection();
            PathFigure PF = new PathFigure();

            ArcSegment arc = new ArcSegment(new Point(startX, startY), new Size(endX - startX, endY - startY), 180, true, SweepDirection.Clockwise, false);
            PF.Segments = new PathSegmentCollection();
            PF.Segments.Add(arc);
            PF.StartPoint = new Point(startX, startY);
            pfC.Add(PF);
            pG.Figures = pfC;
            tempPath.Data = pG;
            canvas.Children.Add(tempPath);
        }

        static public void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3, Brush brush, Canvas canvas)
        {
            Polygon myPolygon = new Polygon();
            myPolygon.Stroke = Brushes.Black;
            myPolygon.Fill = brush;

            myPolygon.StrokeThickness = 2;
            myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
            myPolygon.VerticalAlignment = VerticalAlignment.Center;
            Point Point1 = new Point(x1, y1);
            Point Point2 = new Point(x2, y2);
            Point Point3 = new Point(x3, y3);
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(Point1);
            myPointCollection.Add(Point2);
            myPointCollection.Add(Point3);
            myPolygon.Points = myPointCollection;

            canvas.Children.Add(myPolygon);
        }
    }
}
