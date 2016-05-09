using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;   //For Point
using Microsoft.Kinect;  //For DepthImagePixel[]
using System.Windows.Media; //For DrawingContext dc
using System.Windows.Media.Media3D;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Shapes;
using KinectCoordinateMapping.Mark;

namespace KinectCoordinateMapping
{
    [Serializable()]
    public class Target
    {
        private double TheDeepestDetectDistance = 2.5;
        public int TargetID;
        Point3D XYZ;
        public bool TrackState = false;
        double UU, VV, AverageUU, AverageVV, CenterUU, CenterVV;// KinectHeight = 0.88;  //x, y, z, this uv,average uv, center uv

        int uvRange = 10, SearchRange = 40, CenterUVrange = 10;

        Point ColorCenter = new Point(0, 0);
        private List<double> xray = new List<double>() { };
        private List<double> yoyo = new List<double>() { };
        private List<double> zooo = new List<double>() { };
        private List<double> high = new List<double>() { };

        private readonly Pen PenGray = new Pen(Brushes.Gray, 1);

        private MarkBase markBase;

        int steadyCounter = 0;
        public bool isEnableAllScreenScan = false;
        int findPointCounter = 0;
        public bool isPing = false;
        public List<Point> expectPointList = new List<Point>();

        public Target(int myID)
        {
            TargetID = myID;
            markBase = new DotMark();
        }

        public void Setting(int XX, int YY)
        {
            this.ColorCenter.X = XX;
            this.ColorCenter.Y = YY;
            TrackState = true;
        }

        public void Setting(int XX, int YY, double UU, double VV)
        {
            this.ColorCenter.X = XX;
            this.ColorCenter.Y = YY;
            this.CenterUU = UU;
            this.CenterVV = VV;
            this.AverageUU = this.CenterUU;
            this.AverageVV = this.CenterVV;
            TrackState = true;
        }

        public void Cal(byte[] colorPixels, CameraSpacePoint[] ColorInSkel, int[] boolpixel)
        {
            int ColorCenterXsum = 0, ColorCenterYsum = 0, uuSum = 0, vvSum = 0, Cnt = 0;

            for (int i = (int)ColorCenter.X - SearchRange; i < ColorCenter.X + SearchRange; i++)
            {
                for (int j = (int)ColorCenter.Y - SearchRange; j < ColorCenter.Y + SearchRange; j++)
                {
                    if (i <= 0 || i >= 1920 || j <= 0 || j >= 1080) break;   //avoid edge prob.

                    int di = (i + j * 1920);   //di = depthpixel index                 
                    int ci = di * 4;          //ci = colorPixel index

                    UU = -0.169 * colorPixels[ci + 2] - 0.331 * colorPixels[ci + 1] + 0.5 * colorPixels[ci] + 128;
                    VV = 0.5 * colorPixels[ci + 2] - 0.419 * colorPixels[ci + 1] - 0.081 * colorPixels[ci] + 128;

                    //float depthZ = ColorInSkel[di].Z;
                    if (UU > AverageUU - uvRange && UU < AverageUU + uvRange
                     && VV > AverageVV - uvRange && VV < AverageVV + uvRange
                     && UU > CenterUU - CenterUVrange && UU < CenterUU + CenterUVrange
                     && VV > CenterVV - CenterUVrange && VV < CenterVV + CenterUVrange
                     && (boolpixel[di] == 0 || boolpixel[di] == TargetID))
                     //&& depthZ > expectZ - 0.25f && depthZ < expectZ + 0.25f)
                    {
                        boolpixel[di] = TargetID;
                        uuSum += (int)UU;
                        vvSum += (int)VV;
                        ColorCenterXsum += i;
                        ColorCenterYsum += j;
                        Cnt++;                  //顏色追蹤歸顏色追蹤，Z值追蹤歸Z值追蹤 ；即使zCnt值不夠，UV值、中心點仍繼續更新
                    }
                    else boolpixel[di] = 0;
                }
            }

            if (Cnt > 5)
            {
                AverageUU = uuSum / Cnt;
                AverageVV = vvSum / Cnt;
                Point previousCenter = ColorCenter;
                
                ColorCenter.X = ColorCenterXsum / Cnt;
                ColorCenter.Y = ColorCenterYsum / Cnt;
                
                //if( Math.Sqrt(Math.Pow((previousCenter.X - ColorCenter.X), 2) + Math.Pow((previousCenter.Y - ColorCenter.Y), 2)) < 4.5) //確認球中心是否飄移
                //{
                //    //要記錄點
                //    steadyCounter++;
                //    if (steadyCounter > 70)
                //    {
                //        steadyCounter = 0;
                //        isPing = true;
                //    }
                //}
                //else
                //{
                //    steadyCounter = 0;
                //}
                SearchRange = 40;
                TrackState = true;
                findPointCounter = 0;
            }
            else
            {
                TrackState = false;
                SearchRange = 60;
                findPointCounter++;
                if (findPointCounter > 4)
                {
                    isEnableAllScreenScan = true;
                    findPointCounter = 0;
                }
                else
                {
                    isEnableAllScreenScan = false;
                }
                if (isEnableAllScreenScan)
                {
                    //FindExpectColorPoint(colorPixels, 150, expectZ, ColorInSkel);
                    //FindExpectColorPointAllScreen(colorPixels);
                }
            }
            
        }

        public bool FindExpectColorPointAllScreen(byte[] colorP)
        {
            int ColorCenterXsum = 0;
            int ColorCenterYsum = 0;
            int uuSum = 0;
            int vvSum = 0;
            int Cnt = 0;
            double UU = 0;
            double VV = 0;
            double AverageUU = CenterUU;
            double AverageVV = CenterVV;
            int uvRange = 10;
            Point colorCenter = new Point();
            for (int i = 0; i < 1920; i++)
            {
                for (int j = 0; j < 1080; j++)
                {

                    int di = (i + j * 1920);   //di = depthpixel index                 
                    int ci = di * 4;          //ci = colorPixel index
                    if (ci >= 1920 * 1024 * 4) break;
                    UU = -0.169 * colorP[ci + 2] - 0.331 * colorP[ci + 1] + 0.5 * colorP[ci] + 128;
                    VV = 0.5 * colorP[ci + 2] - 0.419 * colorP[ci + 1] - 0.081 * colorP[ci] + 128;
                    if (UU > AverageUU - uvRange && UU < AverageUU + uvRange && VV > AverageVV - uvRange && VV < AverageVV + uvRange)
                    {
                        uuSum += (int)UU;
                        vvSum += (int)VV;
                        ColorCenterXsum += i;
                        ColorCenterYsum += j;
                        Cnt++;
                    }
                }
            }

            if (Cnt > 5)
            {
                AverageUU = uuSum / Cnt;
                AverageVV = vvSum / Cnt;
                colorCenter.X = ColorCenterXsum / Cnt;
                colorCenter.Y = ColorCenterYsum / Cnt;
                ColorCenter = colorCenter;
                AverageUU = this.AverageUU;
                AverageVV = this.AverageVV;
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool FindExpectColorPoint(byte[] colorP, int expectsearchRange, float expectZ, CameraSpacePoint[] ColorInSkel)
        {
            int ColorCenterXsum = 0;
            int ColorCenterYsum = 0;
            int uuSum = 0;
            int vvSum = 0;
            int Cnt = 0;
            double UU = 0;
            double VV = 0;
            double AverageUU = CenterUU;
            double AverageVV = CenterVV;
            int uvRange = 10;
            Point colorCenter = new Point();
            
            for (int k = 0; k < expectPointList.Count; k++)
            {
                for (int i = (int)expectPointList[k].X - expectsearchRange; i < (int)expectPointList[k].X + expectsearchRange; i++)
                {
                    for (int j = (int)expectPointList[k].Y - (int)expectsearchRange; j < (int)expectPointList[k].Y + expectsearchRange; j++)
                    {

                        int di = (i + j * 1920);   //di = depthpixel index                 
                        int ci = di * 4;          //ci = colorPixel index

                        if (ci < 0 || ci >= 1920 * 1024 * 4)
                        {

                        }
                        else
                        {
                            float depthZ = ColorInSkel[di].Z;
                            UU = -0.169 * colorP[ci + 2] - 0.331 * colorP[ci + 1] + 0.5 * colorP[ci] + 128;
                            VV = 0.5 * colorP[ci + 2] - 0.419 * colorP[ci + 1] - 0.081 * colorP[ci] + 128;
                            if (UU > AverageUU - uvRange && UU < AverageUU + uvRange
                                && VV > AverageVV - uvRange && VV < AverageVV + uvRange)
                            // && depthZ > expectZ - 0.15f && depthZ < expectZ + 0.15f)
                            {
                                uuSum += (int)UU;
                                vvSum += (int)VV;
                                ColorCenterXsum += i;
                                ColorCenterYsum += j;
                                Cnt++;
                            }
                        }
                    }
                }

                if (Cnt > 5)
                {
                    AverageUU = uuSum / Cnt;
                    AverageVV = vvSum / Cnt;
                    colorCenter.X = ColorCenterXsum / Cnt;
                    colorCenter.Y = ColorCenterYsum / Cnt;
                    ColorCenter = colorCenter;
                    AverageUU = this.AverageUU;
                    AverageVV = this.AverageVV;
                    return true;
                }
            }
            return false;
        }

        public void SetExpectColorPoint(double X, double Y)
        {
            Point tempPoint = new Point();
            tempPoint.X = X;
            tempPoint.Y = Y;
            expectPointList.Add(tempPoint);
        }

        public void CleanExpectColorPoint()
        {
            expectPointList = new List<Point>();
        }
       
        public bool IsTracked()
        {
            return TrackState;

        }

        public Point point2D()
        {
            return ColorCenter;
        }

        public Point3D point3D()
        {
            return XYZ;
        }

        public void setPoint3D(Point3D input)
        {
            XYZ = input;
        }

        public void setPoint2D(Point input)
        {
            ColorCenter = input;
        }

        public void RefreshTarget(CameraSpacePoint[] ColorInSkel, bool isZoomIn, int offsetX, int offsetY, ZoomStruct zoomStruct)
        {
            if (TrackState)
            {
                int i = (int)ColorCenter.X;
                int j = (int)ColorCenter.Y;
                int di = (i + j * 1920);
                XYZ.X = ColorInSkel[di].X;
                XYZ.Y = ColorInSkel[di].Y;
                XYZ.Z = ColorInSkel[di].Z;
            }
        }

        public void InsertData()
        {
            xray.Add(XYZ.X); yoyo.Add(XYZ.Y); zooo.Add(XYZ.Z);
        }

        public void clearlist()
        {
            xray.Clear(); yoyo.Clear(); zooo.Clear(); high.Clear();
        }

        public void Del()
        {
            ColorCenter = new Point(0, 0);
            CenterUU = 0;
            CenterVV = 0;
            AverageUU = 0;
            AverageVV = 0;
            TrackState = false;
        }

        public void Del(Point Center, int[] boolPixels)
        {
            int SearchRange = 30;
            for (int i = (int)Center.X - SearchRange; i < Center.X + SearchRange; i++)
            {
                for (int j = (int)Center.Y - SearchRange; j < Center.Y + SearchRange; j++)
                {
                    if (i <= 0 || i >= 512 || j <= 0 || j >= 424) break;
                    int ci = (int)(i + 512 * j);
                    if (boolPixels[ci] == TargetID) boolPixels[ci] = 0;
                }
            }

            ColorCenter = new Point(0, 0);
            CenterUU = 0;
            CenterVV = 0;
            AverageUU = 0;
            AverageVV = 0;
            TrackState = false;
        }

        public void Draw(Canvas canvas, DisplayStruct displayStruct, ZoomStruct zoomStruct, bool isDisplayCoord)
        {
            if (displayStruct.isDotDisplay)
            {
                markBase = new DotMark();
                markBase.Draw(canvas, displayStruct, this, zoomStruct);
            }
            else
            {
                markBase = new BallMark();
                markBase.Draw(canvas, displayStruct, this, zoomStruct);
            }
            if (isDisplayCoord)
            {
                DrawCoordinate(canvas, zoomStruct);
            }
        }

        public void DrawCoordinate(Canvas canvas, ZoomStruct zoomStruct)
        {
            Target targetDisplay = this;
            int transformX;
            int transformY;

            Point tempPoint = CoordinateTransform.ReverseFromFullScreenToScreen((int)point2D().X, (int)point2D().Y, zoomStruct);
            transformX = (int)tempPoint.X;
            transformY = (int)tempPoint.Y;


            AddPixel.Text(transformX - 10, transformY + 15, targetDisplay.point3D().X.ToString("f2") + ", " + targetDisplay.point3D().Y.ToString("f2") + ", " + targetDisplay.point3D().Z.ToString("f2"), Color.FromArgb(255, 255, 0, 0), canvas);
        }
   
    }
}


