using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KinectCoordinateMapping.ButtonCommand
{
    public class AngleCommand : CommandInterface
    {
        Target tempTarget;
        DisplayStruct displayStruct;
        ZoomStruct zoomStruct;
        public AngleCommand(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.displayStruct = mainWindow.displayStruct;
            this.zoomStruct = mainWindow.zoomStruct;
        }

        public override void LeftButtonPress(int x, int y)
        {
            //startX = x;
            //startY = y;
            //middleX = x;
            //middleY = y;
            //MouseLeftPressed = true;
            //Target tempTarget;
            //if (TargetList.Count == 2)
            //{
            //    tempTarget = new Target(0);
            //    tempTarget.Setting(startX, startY);
            //    tempTarget.RefreshTarget(mainWindow.ColorInSkeleton, mainWindow.zoomStruct.IsZoom, mainWindow.zoomStruct.ZoomOffsetX, mainWindow.zoomStruct.ZoomOffsetY, mainWindow.zoomStruct);
            //}
            //else
            //{
            //    Target target = new Target(0);
            //    target.Setting(startX, startY);
            //    TargetList.Add(target);
            //    target.RefreshTarget(mainWindow.ColorInSkeleton, mainWindow.zoomStruct.IsZoom, mainWindow.zoomStruct.ZoomOffsetX, mainWindow.zoomStruct.ZoomOffsetY, mainWindow.zoomStruct);
            //}
        }

        public override void LeftButtonRelease(int x, int y)
        {
            Point tempPoint = CoordinateTransform.TransformFromScreenToFullScreen(x, y, zoomStruct);
            x = (int)tempPoint.X;
            y = (int)tempPoint.Y;

            if (TargetList.Count == 0 || TargetList.Count == 1 || TargetList.Count == 2)
            {
                Target target = new Target(0);
                target.Setting(x, y);
                target.RefreshTarget(mainWindow.ColorInSkeleton, mainWindow.zoomStruct.IsZoom, mainWindow.zoomStruct.ZoomOffsetX, mainWindow.zoomStruct.ZoomOffsetY, mainWindow.zoomStruct);
                TargetList.Add(target);  
            }
            else if (TargetList.Count == 3)
            {
                endX = x;
                endY = y;

                int left = (int)TargetList[1].point2D().X - 20;
                int right = (int)TargetList[1].point2D().X + 20;
                int up = (int)TargetList[1].point2D().Y - 20;
                int down = (int)TargetList[1].point2D().Y + 20;

                int left2 = (int)TargetList[0].point2D().X - 20;
                int right2 = (int)TargetList[0].point2D().X + 20;
                int up2 = (int)TargetList[0].point2D().Y - 20;
                int down2 = (int)TargetList[0].point2D().Y + 20;

                if (endX <= right && endX >= left && endY >= up && endY <= down)
                {
                    //tempTarget = new Target(0);
                    //tempTarget.Setting(endX, endY);
                    //tempTarget.RefreshTarget(mainWindow.ColorInSkeleton, mainWindow.zoomStruct.IsZoom, mainWindow.zoomStruct.ZoomOffsetX, mainWindow.zoomStruct.ZoomOffsetY, mainWindow.zoomStruct);
                    //TargetList.Add(tempTarget);
                    //TargetList.RemoveAt(2);
                    mainWindow.groupList.Add(new Group(TargetList, mode.AngleMode));
                    TargetList = new List<Target>();
                }
                else if (endX <= right2 && endX >= left2 && endY >= up2 && endY <= down2)
                {
                    tempTarget = TargetList[1];
                    TargetList.RemoveAt(1);
                    TargetList.Insert(0, tempTarget);

                    mainWindow.groupList.Add(new Group(TargetList, mode.AngleMode));
                    TargetList = new List<Target>();
                }
                else
                {
                    TargetList.RemoveAt(2);
                }
            }
            
            
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

            //Ellipse ellipse;
            if (TargetList.Count == 1)
            {
                //if (displayStruct.isDotDisplay)
                //{

                //    AddPixel.AddOnePixel(canvas, middleX, middleY);
                //}
                //else
                //{
                //    ellipse = new Ellipse
                //    {
                //        Fill = Brushes.Blue,
                //        Width = displayStruct.ballsize,
                //        Height = displayStruct.ballsize
                //    };
                //    Canvas.SetLeft(ellipse, middleX - ellipse.Width / 2);
                //    Canvas.SetTop(ellipse, middleY - ellipse.Height / 2);
                //    canvas.Children.Add(ellipse);
                //}
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

                //if ( line.X1 > line.X2)
                //{
                //    line.X1 -= 5;
                //    line.X2 += 5;
                //}
                //else if(line.X1 <= line.X2)
                //{
                //    line.X1 += 5;
                //    line.X2 -= 5;
                //}

                //if (line.Y1 > line.Y2)
                //{
                //    line.Y1 -= 5;
                //    line.Y2 += 5;
                //}
                //else if (line.Y1 <= line.Y2)
                //{
                //    line.Y1 += 5;
                //    line.Y2 -= 5;
                //}
                canvas.Children.Add(line);
                
            }
            if (TargetList.Count == 2)
            {
                Point tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[0].point2D().X, (int)TargetList[0].point2D().Y, zoomStruct);
                int transformX = (int)tempPoint.X;
                int transformY = (int)tempPoint.Y;

                tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                int transformX2 = (int)tempPoint.X;
                int transformY2 = (int)tempPoint.Y;

                Line line = new Line();
                line.X1 = transformX;
                line.X2 = transformX2;
                line.Y1 = transformY;
                line.Y2 = transformY2;
                line.Stroke = Brushes.Aqua;
                line.StrokeThickness = displayStruct.linesize;
                line.MouseLeftButtonUp += (s, e) =>
                {
                    int mouseX = (int)e.GetPosition(mainWindow.canvas).X;
                    int mouseY = (int)e.GetPosition(mainWindow.canvas).Y;
                    LeftButtonRelease(mouseX, mouseY);
                };
                canvas.Children.Add(line);

                //if (tempTarget != null)
                //{
                    
                //    //if (displayStruct.isDotDisplay)
                //    //{
                //    //    AddPixel.AddOnePixel(canvas, tempTarget.point2D().X, tempTarget.point2D().Y);
                //    //}
                //    //else
                //    //{
                //    //    ellipse = new Ellipse
                //    //    {
                //    //        Fill = Brushes.Blue,
                //    //        Width = displayStruct.ballsize,
                //    //        Height = displayStruct.ballsize
                //    //    };
                //    //    Canvas.SetLeft(ellipse, tempTarget.point2D().X - ellipse.Width / 2);
                //    //    Canvas.SetTop(ellipse, tempTarget.point2D().Y - ellipse.Height / 2);
                //    //    canvas.Children.Add(ellipse);
                //    //}

                //    //if (MouseLeftPressed)
                //    //{
                //    //    ellipse = new Ellipse
                //    //    {
                //    //        Fill = Brushes.Blue,
                //    //        Width = displayStruct.ballsize,
                //    //        Height = displayStruct.ballsize
                //    //    };
                //    //    Canvas.SetLeft(ellipse, middleX - ellipse.Width / 2);
                //    //    Canvas.SetTop(ellipse, middleY - ellipse.Height / 2);
                //    //    if (displayStruct.isDotDisplay)
                //    //    {
                //    //        AddPixel.AddOnePixel(canvas, middleX, middleY);
                //    //    }
                //    //    else
                //    //    {
                //    //        canvas.Children.Add(ellipse);
                //    //    }

                //    //    line = new Line();
                //    //    line.Stroke = Brushes.Aqua;
                //    //    line.StrokeThickness = displayStruct.linesize;
                //    //    line.X1 = tempTarget.point2D().X;
                //    //    line.X2 = middleX;
                //    //    line.Y1 = tempTarget.point2D().Y;
                //    //    line.Y2 = middleY;
                //    //    canvas.Children.Add(line);
                //    //}
                //}


            }
            if (TargetList.Count == 3)
            {
                Point tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[0].point2D().X, (int)TargetList[0].point2D().Y, zoomStruct);
                int transformX = (int)tempPoint.X;
                int transformY = (int)tempPoint.Y;

                tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                int transformX2 = (int)tempPoint.X;
                int transformY2 = (int)tempPoint.Y;

                Line line = new Line();
                line.X1 = transformX;
                line.X2 = transformX2;
                line.Y1 = transformY;
                line.Y2 = transformY2;
                line.Stroke = Brushes.Aqua;
                line.StrokeThickness = displayStruct.linesize;
                line.MouseLeftButtonUp += (s, e) =>
                {
                    int mouseX = (int)e.GetPosition(mainWindow.canvas).X;
                    int mouseY = (int)e.GetPosition(mainWindow.canvas).Y;
                    LeftButtonRelease(mouseX, mouseY);
                };
                canvas.Children.Add(line);


                tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[2].point2D().X, (int)TargetList[2].point2D().Y, zoomStruct);
                int transformX4 = (int)tempPoint.X;
                int transformY4 = (int)tempPoint.Y;

                tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen(middleX, middleY, zoomStruct);
                int transformX3 = (int)tempPoint.X;
                int transformY3 = (int)tempPoint.Y;


                line = new Line();
                line.X1 = transformX3;
                line.X2 = transformX4;
                line.Y1 = transformY3;
                line.Y2 = transformY4;
                line.MouseLeftButtonUp += (s, e) =>
                {
                    int mouseX = (int)e.GetPosition(mainWindow.canvas).X;
                    int mouseY = (int)e.GetPosition(mainWindow.canvas).Y;
                    LeftButtonRelease(mouseX, mouseY);
                };
                //if (line.X1 > line.X2)
                //{
                //    line.X1 -= 5;
                //    line.X2 += 5;
                //}
                //else if (line.X1 <= line.X2)
                //{
                //    line.X1 += 5;
                //    line.X2 -= 5;
                //}

                //if (line.Y1 > line.Y2)
                //{
                //    line.Y1 -= 5;
                //    line.Y2 += 5;
                //}
                //else if (line.Y1 <= line.Y2)
                //{
                //    line.Y1 += 5;
                //    line.Y2 -= 5;
                //}

                line.Stroke = Brushes.Aqua;
                line.StrokeThickness = displayStruct.linesize;
                canvas.Children.Add(line);

            }
        }
    }
}
