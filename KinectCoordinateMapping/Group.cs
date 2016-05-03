using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace KinectCoordinateMapping
{
    [Serializable()]
    public class Group
    {
        private double number;
        public mode type;
        public List<Target> groupList;
        private AngleCalulator AngleCal = new AngleCalulator();
        private bool isReadOnly = false;

        public Group(List<Target> inputList, mode inputType)
        {
            type = inputType;
            groupList = inputList;
            Calculate();
            isReadOnly = false;
        }

        public Group(List<Target> inputList, mode inputType, bool isReadOnly, double value)
        {
            type = inputType;
            groupList = inputList;
            number = value;
            //Calculate();
            this.isReadOnly = isReadOnly;

        }

        public void Calculate()
        {
            if (isReadOnly == false)
            {
                double answer = 0;
                if (type == mode.AngleMode && groupList.Count == 3)
                {
                    answer = AngleCal.AngleBetween(groupList[0], groupList[1], groupList[2]);
                }
                else if ((type == mode.LenghtMode || type == mode.LineBisection) && groupList.Count == 2)
                {
                    answer = AngleCal.Length(groupList[0], groupList[1]);
                }
                number = answer;
            }
        }

        public double Value
        {
            set
            {
                number = value;
            }
            get
            {
                if (isReadOnly == false)
                {
                    double answer = 0;
                    if (type == mode.AngleMode && groupList.Count == 3)
                    {
                        answer = AngleCal.AngleBetween(groupList[0], groupList[1], groupList[2]);
                    }
                    else if ((type == mode.LenghtMode || type == mode.LineBisection) && groupList.Count == 2)
                    {
                        answer = AngleCal.Length(groupList[0], groupList[1]);
                    }
                    number = answer;
                }

                return number;
            }
        }

        public void Draw(Canvas canvas, DisplayStruct displayStruct, ZoomStruct zoomStruct, bool isDisplayCoord, Vector3D floorVector)
        {
            Ellipse ellipse;
            int transformX;
            int transformY;
            int transformX2;
            int transformY2;
            Point tempPoint;
            Line line;
            line = new Line();
            for (int j = 0; j < groupList.Count; j++)
            {
                groupList[j].Draw(canvas, displayStruct, zoomStruct, false);
            }

            for (int k = 0; k < groupList.Count - 1; k++)
            {
                tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)groupList[k].point2D().X, (int)groupList[k].point2D().Y, zoomStruct);
                transformX = (int)tempPoint.X;
                transformY = (int)tempPoint.Y;

                tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)groupList[k + 1].point2D().X, (int)groupList[k + 1].point2D().Y, zoomStruct);
                transformX2 = (int)tempPoint.X;
                transformY2 = (int)tempPoint.Y;

                line = new Line();
                line.X1 = transformX;
                line.X2 = transformX2;
                line.Y1 = transformY;
                line.Y2 = transformY2;
                line.Stroke = Brushes.DarkGoldenrod;
                line.StrokeThickness = displayStruct.linesize;
                canvas.Children.Add(line);

            }

            if (type == mode.AngleMode)
            {
                tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)groupList[1].point2D().X, (int)groupList[1].point2D().Y, zoomStruct);
                transformX = (int)tempPoint.X;
                transformY = (int)tempPoint.Y;
                AddPixel.Text(transformX + 5, transformY + 10, Value.ToString("f2"), Color.FromArgb(255, 00, 0xff, 0xff), canvas);
            }
            else if (type == mode.LenghtMode)
            {
                tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)(groupList[0].point2D().X + groupList[1].point2D().X) / 2, (int)groupList[0].point2D().Y + 10, zoomStruct);
                transformX = (int)tempPoint.X;
                transformY = (int)tempPoint.Y;
                AddPixel.Text(transformX, transformY + 10, Value.ToString("f2") + "cm", Color.FromArgb(255, 255, 0, 255), canvas);
            }
            else if (type == mode.LineBisection)
            {
                tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)(groupList[0].point2D().X + groupList[1].point2D().X) / 2, (int)groupList[0].point2D().Y + 10, zoomStruct);
                transformX = (int)tempPoint.X;
                transformY = (int)tempPoint.Y;
                AddPixel.Text(transformX, transformY + 10, Value.ToString("f2") + "cm", Color.FromArgb(255, 255, 0, 255), canvas);



                ellipse = new Ellipse
                {
                    Fill = Brushes.Blue,
                    Width = displayStruct.ballsize,
                    Height = displayStruct.ballsize
                };
                Canvas.SetLeft(ellipse, (line.X1 + line.X2) / 2 - ellipse.Width / 2);
                Canvas.SetTop(ellipse, (line.Y1 + line.Y2) / 2 - ellipse.Height / 2);
                canvas.Children.Add(ellipse);


                Vector originVector = new Vector(groupList[0].point2D().X - groupList[1].point2D().X, groupList[0].point2D().Y - groupList[1].point2D().Y);

                tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)(groupList[0].point2D().X + groupList[1].point2D().X) / 2, (int)(groupList[0].point2D().Y + groupList[1].point2D().Y) / 2, zoomStruct);
                transformX = (int)tempPoint.X;
                transformY = (int)tempPoint.Y;


                line = new Line();
                line.Stroke = Brushes.Yellow;
                line.StrokeThickness = displayStruct.linesize;
                line.X1 = transformX;
                line.X2 = transformX + floorVector.X * 1000;
                line.Y1 = transformY;
                line.Y2 = transformY - floorVector.Y * 1000;
                canvas.Children.Add(line);


                int result = AngleCal.DetermineLean(groupList[0].point3D() - groupList[1].point3D(), floorVector);

                if (result == -1)
                {
                    AddPixel.Text(transformX, transformY + 20, "Right", Color.FromArgb(255, 255, 0, 255), canvas);
                }
                else if (result == 1)
                {
                    AddPixel.Text(transformX, transformY + 20, "Left", Color.FromArgb(255, 255, 0, 255), canvas);
                }
                else
                {
                    AddPixel.Text(transformX, transformY + 20, "Equal", Color.FromArgb(255, 255, 0, 255), canvas);
                }

            }
        }
    }
}
