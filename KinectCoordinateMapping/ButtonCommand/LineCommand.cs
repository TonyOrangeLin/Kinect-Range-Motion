using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KinectCoordinateMapping.ButtonCommand
{
    public class LineCommand : CommandInterface
    {
        ZoomStruct zoomStruct;
        public LineCommand(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.zoomStruct = mainWindow.zoomStruct;
        }

        public override void LeftButtonPress(int x, int y)
        {
            Point tempPoint = CoordinateTransform.TransformFromScreenToFullScreen(x, y, zoomStruct);
            x = (int)tempPoint.X;
            y = (int)tempPoint.Y;
            if (!MouseLeftPressed && TargetList.Count == 0)
            {
                    startX = x;
                    startY = y;
                    middleX = x;
                    middleY = y;
                    MouseLeftPressed = true;
                    Target target = new Target(0);
                    target.Setting(startX, startY);
                    target.RefreshTarget(mainWindow.ColorInSkeleton, mainWindow.zoomStruct.IsZoom, mainWindow.zoomStruct.ZoomOffsetX, mainWindow.zoomStruct.ZoomOffsetY, mainWindow.zoomStruct);
                    TargetList.Add(target);
            }
            else
            {
                startX = x;
                startY = y;
                middleX = x;
                middleY = y;
                MouseLeftPressed = true;
                TargetList = new List<Target>();
                Target target = new Target(0);
                target.Setting(startX, startY);
                TargetList.Add(target);
            }
        }


        public override void LeftButtonRelease(int x, int y)
        {
            Point tempPoint = CoordinateTransform.TransformFromScreenToFullScreen(x, y, zoomStruct);
            x = (int)tempPoint.X;
            y = (int)tempPoint.Y;

            endX = x;
            endY = y;

            Target target = new Target(0);
            target.Setting(endX, endY);
            target.RefreshTarget(mainWindow.ColorInSkeleton, mainWindow.zoomStruct.IsZoom, mainWindow.zoomStruct.ZoomOffsetX, mainWindow.zoomStruct.ZoomOffsetY, mainWindow.zoomStruct);
            TargetList.Add(target);

            mainWindow.groupList.Add(new Group(TargetList, mode.LenghtMode));

            TargetList = null;
            TargetList = new List<Target>();
            MouseLeftPressed = false;
        }

        public override void MouseMove(int x, int y)
        {
            Point tempPoint = CoordinateTransform.TransformFromScreenToFullScreen(x, y, zoomStruct);
            x = (int)tempPoint.X;
            y = (int)tempPoint.Y;

            middleX = x;
            middleY = y;
        }

        public override void Draw(Canvas canvas, DisplayStruct displayStruct)
        {
            for (int i = 0; i < TargetList.Count; i++)
            {
                TargetList[i].Draw(canvas, displayStruct, zoomStruct, false);
            }

            Ellipse ellipse;
            if (MouseLeftPressed && TargetList.Count == 1)
            {
                if (displayStruct.isDotDisplay)
                {
                    AddPixel.AddOnePixel(canvas, middleX, middleY);
                }
                else
                {
                    ellipse = new Ellipse
                    {
                        Fill = Brushes.Blue,
                        Width = displayStruct.ballsize,
                        Height = displayStruct.ballsize
                    };
                    Canvas.SetLeft(ellipse, middleX - ellipse.Width / 2);
                    Canvas.SetTop(ellipse, middleY - ellipse.Height / 2);
                    canvas.Children.Add(ellipse);
                }

                Point tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[0].point2D().X, (int)TargetList[0].point2D().Y, zoomStruct);
                int transformX = (int)tempPoint.X;
                int transformY = (int)tempPoint.Y;

                tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen(middleX, middleY, zoomStruct);
                int transformX2 = (int)tempPoint.X;
                int transformY2 = (int)tempPoint.Y;

                Line line = new Line();
                line.Stroke = Brushes.Aqua;
                line.StrokeThickness = displayStruct.linesize;
                line.X1 = transformX;
                line.X2 = transformX2;
                line.Y1 = transformY;
                line.Y2 = transformY2;
                line.MouseLeftButtonUp += (s, e) =>
                {
                    int mouseX = (int)e.GetPosition(mainWindow.canvas).X;
                    int mouseY = (int)e.GetPosition(mainWindow.canvas).Y;
                    LeftButtonRelease(mouseX, mouseY);
                };

                canvas.Children.Add(line);
            }

        }
    }
}
