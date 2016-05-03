using System;   //for Math
using System.IO;
using System.Windows;
using System.Windows.Media;
using Microsoft.Kinect;
using System.Windows.Media.Imaging;
using System.Collections.Generic;  //for list
using System.Linq;   //for Math.average
//using Visifire.Charts; //for chart
using System.Windows.Controls;// for grid
using System.Windows.Input;  // for 点击事件
using System.Threading;
using System.Globalization; // For drawtext
using System.Windows.Media.Media3D; //for vector 3d

namespace KinectCoordinateMapping
{
    [Serializable()]
    public class AngleCalulator
    {
        private readonly Pen PenGreen = new Pen(Brushes.Green, 1);
        private readonly Pen PenGray = new Pen(Brushes.Gray, 1);
        private readonly Pen PenRed = new Pen(Brushes.Red, 2);
        private readonly Pen PenYellow = new Pen(Brushes.Yellow, 2);
        private readonly Pen PenOrange = new Pen(Brushes.Orange, 2);
        private readonly Pen PenDeepSkyBlue = new Pen(Brushes.DeepSkyBlue, 2);
        private readonly Pen PenDarkBlue = new Pen(Brushes.DarkBlue, 2);
        private readonly Pen PenLightGreen = new Pen(Brushes.LightGreen, 2);
        private readonly Pen PenLemonChiffon = new Pen(Brushes.LemonChiffon, 2);


        private readonly Brush brushWhite = Brushes.White;
        private readonly Brush brushRed = Brushes.Red;
        private readonly Brush brushOrange = Brushes.Orange;
        private readonly Brush brushYellow = Brushes.Yellow;
        private readonly Brush brushGreen = Brushes.Green;
        private readonly Brush brushDeepSkyBlue = Brushes.DeepSkyBlue;
        private readonly Brush brushCadetBlue = Brushes.CadetBlue;
        private readonly Brush brushPurple = Brushes.Purple;
        private readonly Brush brushGreenYellow = Brushes.GreenYellow;
        private readonly Brush brushChocolate = Brushes.Chocolate;
        private readonly Brush brushDarkGray = Brushes.DarkGray;
        private readonly Brush brushLightGreen = Brushes.LightGreen;
        private readonly Brush brushLemonChiffon = Brushes.LemonChiffon;

        public double Hor(Target TargetA, Target TargetB)
        {
            //3D
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, TargetA.point3D().Z - TargetB.point3D().Z);
            Vector3D vectorB = new Vector3D(0, 1, 0);

            //2D
            //Vector3D vectorA = new Vector3D(TargetA.point2D().X - TargetB.point2D().X, TargetA.point2D().Y - TargetB.point2D().Y, 0);
            //Vector3D vectorB = new Vector3D(1, 0, 0);

            double theta = Math.Abs(Vector3D.AngleBetween(vectorA, vectorB));
            theta = 90 - theta;
            //if (TargetA.point3D().Y < TargetB.point3D().Y) theta = -theta;

            return theta;
        }

        public double Hor(DrawingContext dc, Target TargetA, Target TargetB, bool show)
        {
            //3D
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, TargetA.point3D().Z - TargetB.point3D().Z);
            Vector3D vectorB = new Vector3D(0, 1, 0);

            //2D
            //Vector3D vectorA = new Vector3D(TargetA.point2D().X - TargetB.point2D().X, TargetA.point2D().Y - TargetB.point2D().Y, 0);
            //Vector3D vectorB = new Vector3D(1, 0, 0);

            double theta = Math.Abs(Vector3D.AngleBetween(vectorA, vectorB));
            theta = 90 - theta;
            //if (TargetA.point3D().Y < TargetB.point3D().Y) theta = -theta;

            if (show)       //show angle text
            {
                dc.DrawText(new FormattedText(theta.ToString("f0"),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                25, brushDeepSkyBlue),
                new Point(TargetB.point2D().X - 35, TargetB.point2D().Y - 35));

                dc.DrawLine(PenDeepSkyBlue, TargetA.point2D(), TargetB.point2D());    //show angle line 
                dc.DrawLine(PenDeepSkyBlue, new Point(TargetA.point2D().X, TargetB.point2D().Y), TargetB.point2D());
            }
            return theta;
        }

        public double Hor(Target TargetA, Target TargetB, Canvas canvas, bool show)
        {
            //3D
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, TargetA.point3D().Z - TargetB.point3D().Z);
            Vector3D vectorB = new Vector3D(0, 1, 0);

            //2D
            //Vector3D vectorA = new Vector3D(TargetA.point2D().X - TargetB.point2D().X, TargetA.point2D().Y - TargetB.point2D().Y, 0);
            //Vector3D vectorB = new Vector3D(1, 0, 0);

            double theta = Math.Abs(Vector3D.AngleBetween(vectorA, vectorB));
            theta = 90 - theta;
            //if (TargetA.point3D().Y < TargetB.point3D().Y) theta = -theta;

            if (show)       //show angle text
            {
               // canvas.DrawText(new FormattedText(theta.ToString("f0"),
               //// CultureInfo.GetCultureInfo("en-us"),
               // FlowDirection.LeftToRight,
               // new Typeface("Verdana"),
               // 25, brushDeepSkyBlue),
               // new Point(TargetB.point2D().X - 35, TargetB.point2D().Y - 35));

               // canvas.DrawLine(PenDeepSkyBlue, TargetA.point2D(), TargetB.point2D());    //show angle line 
               // canvas.DrawLine(PenDeepSkyBlue, new Point(TargetA.point2D().X, TargetB.point2D().Y), TargetB.point2D());
            }
            return theta;
        }
        public double Ver(Target TargetA, Target TargetB)
        {
            //3D
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, 0);// TargetA.point3D().Z - TargetB.point3D().Z);
            Vector3D vectorB = new Vector3D(0, 1, 0);

            //2D
            //Vector3D vectorA = new Vector3D(TargetA.point2D().X - TargetB.point2D().X, TargetA.point2D().Y - TargetB.point2D().Y, 0);
            //Vector3D vectorB = new Vector3D(0, TargetA.point2D().Y - TargetB.point2D().Y, 0);

            double theta = Math.Abs(Vector3D.AngleBetween(vectorA, vectorB));
            if (TargetA.point3D().X < TargetB.point3D().X) theta = -theta;
            return theta;
        }

        public double Ver(DrawingContext dc, Target TargetA, Target TargetB, bool show)
        {
            //3D
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, 0);// TargetA.point3D().Z - TargetB.point3D().Z);
            Vector3D vectorB = new Vector3D(0, 1, 0);

            //2D
            //Vector3D vectorA = new Vector3D(TargetA.point2D().X - TargetB.point2D().X, TargetA.point2D().Y - TargetB.point2D().Y, 0);
            //Vector3D vectorB = new Vector3D(0, TargetA.point2D().Y - TargetB.point2D().Y, 0);

            double theta = Math.Abs(Vector3D.AngleBetween(vectorA, vectorB));
            if (TargetA.point3D().X < TargetB.point3D().X) theta = -theta;

            if (show)       //show angle text
            {
                dc.DrawText(new FormattedText(theta.ToString("f0"),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                25, brushLemonChiffon),
                new Point(TargetB.point2D().X - 35, TargetB.point2D().Y - 35));

                dc.DrawLine(PenLemonChiffon, TargetA.point2D(), TargetB.point2D());    //show angle line 
                //dc.DrawLine(PenLemonChiffon, new Point(TargetB.point2D().X, TargetA.point2D().Y), new Point(TargetB.point2D().X, TargetB.point2D().Y));  //show Vertical line
            }
            return theta;
        }

        public double twoLines(Target TargetA, Target TargetB, Target TargetC, Target TargetD)
        {
            //3D
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, TargetA.point3D().Z - TargetB.point3D().Z);
            Vector3D vectorB = new Vector3D(TargetC.point3D().X - TargetD.point3D().X, TargetC.point3D().Y - TargetD.point3D().Y, TargetC.point3D().Z - TargetD.point3D().Z);

            //2D
            //Vector3D vectorA = new Vector3D(TargetA.point2D().X - TargetB.point2D().X, TargetA.point2D().Y - TargetB.point2D().Y, 0);
            //Vector3D vectorB = new Vector3D(TargetC.point2D().X - TargetD.point2D().X, TargetC.point2D().Y - TargetD.point2D().Y, 0);

            double theta = Math.Abs(Vector3D.AngleBetween(vectorA, vectorB));

            return theta;
        }

        public double twoLines(DrawingContext dc, Target TargetA, Target TargetB, Target TargetC, Target TargetD, bool show)
        {
            //3D
            //Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, TargetA.point3D().Z - TargetB.point3D().Z);
            //Vector3D vectorB = new Vector3D(TargetC.point3D().X - TargetD.point3D().X, TargetC.point3D().Y - TargetD.point3D().Y, TargetC.point3D().Z - TargetD.point3D().Z);

            //2D
            Vector3D vectorA = new Vector3D(TargetA.point2D().X - TargetB.point2D().X, TargetA.point2D().Y - TargetB.point2D().Y, 0);
            Vector3D vectorB = new Vector3D(TargetC.point2D().X - TargetD.point2D().X, TargetC.point2D().Y - TargetD.point2D().Y, 0);

            double theta = Math.Abs(Vector3D.AngleBetween(vectorA, vectorB));


            if (show)
            {
                dc.DrawText(new FormattedText(theta.ToString("f0"),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                25, brushOrange),
                new Point((TargetA.point2D().X + TargetB.point2D().X) / 2, (TargetA.point2D().Y + TargetC.point2D().Y - 40) / 2));

                dc.DrawLine(PenOrange, TargetA.point2D(), TargetB.point2D());    //show angle line 
                dc.DrawLine(PenOrange, TargetC.point2D(), TargetD.point2D());
            }
            return theta;
        }

        public double threePts(Target TargetA, Target TargetB, Target TargetC)
        {
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, TargetA.point3D().Z - TargetB.point3D().Z);
            Vector3D vectorB = new Vector3D(TargetB.point3D().X - TargetC.point3D().X, TargetB.point3D().Y - TargetC.point3D().Y, TargetB.point3D().Z - TargetC.point3D().Z);

            //Vector3D vectorA = new Vector3D(TargetA.point2D().X - TargetB.point2D().X, TargetA.point2D().Y - TargetB.point2D().Y, 0);
            //Vector3D vectorB = new Vector3D(TargetB.point2D().X - TargetC.point2D().X, TargetB.point2D().Y - TargetC.point2D().Y, 0);


            double theta = Math.Abs(180 - Vector3D.AngleBetween(vectorA, vectorB));

            return theta;
        }

        public double threePts(DrawingContext dc, Target TargetA, Target TargetB, Target TargetC, bool show)
        {
            //Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, TargetA.point3D().Z - TargetB.point3D().Z);
            //Vector3D vectorB = new Vector3D(TargetB.point3D().X - TargetC.point3D().X, TargetB.point3D().Y - TargetC.point3D().Y, TargetB.point3D().Z - TargetC.point3D().Z);

            Vector3D vectorA = new Vector3D(TargetA.point2D().X - TargetB.point2D().X, TargetA.point2D().Y - TargetB.point2D().Y, 0);
            Vector3D vectorB = new Vector3D(TargetB.point2D().X - TargetC.point2D().X, TargetB.point2D().Y - TargetC.point2D().Y, 0);


            double theta = Math.Abs(180 - Vector3D.AngleBetween(vectorA, vectorB));


            if (show)       //show angle text
            {
                dc.DrawText(new FormattedText(theta.ToString("f0"),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                25, brushYellow),
                new Point(TargetB.point2D().X - 55, TargetB.point2D().Y - 15));

                dc.DrawLine(PenYellow, TargetA.point2D(), TargetB.point2D());    //show angle line 
                dc.DrawLine(PenYellow, TargetB.point2D(), TargetC.point2D());
            }
            return theta;
        }


        public double Length(DrawingContext dc, Target TargetA, Target TargetB, double calibrateRatio)
        {
            double AA = Math.Ceiling(Math.Sqrt((Math.Pow(TargetA.point3D().X - TargetB.point3D().X, 2) + Math.Pow(TargetA.point3D().Y - TargetB.point3D().Y, 2) + Math.Pow(TargetA.point3D().Z - TargetB.point3D().Z, 2))) * 100);
            //double AA = Math.Ceiling(Math.Sqrt((Math.Pow(TargetA.point3D().X - TargetB.point3D().X, 2) + Math.Pow(TargetA.point3D().Y - TargetB.point3D().Y, 2) + Math.Pow(TargetA.point3D().Z - TargetB.point3D().Z, 2))));
            AA = AA * calibrateRatio;
            dc.DrawLine(PenYellow, TargetA.point2D(), TargetB.point2D());
            dc.DrawText(new FormattedText(AA.ToString("f0"),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                25, brushYellow),
                new Point(TargetB.point2D().X - 55, TargetB.point2D().Y - 15));
            return AA;

        }
        public double Length(DrawingContext dc, Target TargetA, Target TargetB)
        {
            double AA = Math.Ceiling(Math.Sqrt((Math.Pow(TargetA.point3D().X - TargetB.point3D().X, 2) + Math.Pow(TargetA.point3D().Y - TargetB.point3D().Y, 2) + Math.Pow(TargetA.point3D().Z - TargetB.point3D().Z, 2))) * 100);
            dc.DrawLine(PenYellow, TargetA.point2D(), TargetB.point2D());
            dc.DrawText(new FormattedText(AA.ToString("f0"),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                25, brushYellow),
                new Point(TargetB.point2D().X - 55, TargetB.point2D().Y - 15));
            return AA;

        }

        public VerticalData VerticalDistance(DrawingContext dc, Target TargetA, Target TargetB, double FootCenter3DX, double FootCenter2DX, bool show)
        {
            double AC = Math.Ceiling(Math.Sqrt(Math.Pow(TargetA.point3D().X - FootCenter3DX, 2)) * 100);
            double BC = Math.Ceiling(Math.Sqrt(Math.Pow(TargetB.point3D().X - FootCenter3DX, 2)) * 100);
            double CC = (TargetA.point3D().Y - TargetB.point3D().Y) * 100;
            VerticalData abc = new VerticalData();
            abc.Right = AC;
            abc.Left = BC;
            abc.Mid = CC;

            if (show)       //show angle text
            {
                dc.DrawText(new FormattedText(AC.ToString("f0"), //AC
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                25, brushLightGreen),
                new Point(TargetA.point2D().X - 35, TargetA.point2D().Y - 25));

                dc.DrawText(new FormattedText(BC.ToString("f0"), //BC
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                25, brushLightGreen),
                new Point(TargetB.point2D().X, TargetB.point2D().Y - 25));

                dc.DrawText(new FormattedText(CC.ToString("f0"), //CC
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                25, brushLightGreen),
                new Point(FootCenter2DX - 10, TargetB.point2D().Y / 2 + TargetA.point2D().Y / 2));

                dc.DrawLine(PenLightGreen, TargetA.point2D(), new Point(FootCenter2DX, TargetA.point2D().Y)); //AC 
                dc.DrawLine(PenLightGreen, TargetB.point2D(), new Point(FootCenter2DX, TargetB.point2D().Y)); //BC 
                dc.DrawLine(PenLightGreen, new Point(FootCenter2DX, TargetA.point2D().Y), new Point(FootCenter2DX, TargetB.point2D().Y));  //CC

            }
            return abc;
        }


        public double SkelTilt2D(Point TargetA, Point TargetB)
        {
            Vector vectorA = new Vector(TargetA.X - TargetB.X, TargetA.Y - TargetB.Y);
            Vector vectorB = new Vector(0, -1);
            return Vector.AngleBetween(vectorA, vectorB);
        }

        //public double SkelTilt(Joint TargetA, Joint TargetB)  //Joint ver.
        //{
        //    Vector3D vectorA = new Vector3D(TargetA.Position.X - TargetB.Position.X, TargetA.Position.Y - TargetB.Position.Y, TargetA.Position.Z - TargetB.Position.Z);
        //    Vector3D vectorB = new Vector3D(0, -1, 0);

        //    return Vector3D.AngleBetween(vectorA, vectorB);
        //}

        public double SkelSpin(Joint TargetA, Joint TargetB)
        {
            Vector3D vectorA = new Vector3D(TargetA.Position.X - TargetB.Position.X, 0//TargetA.Position.Y - TargetB.Position.Y
                , TargetA.Position.Z - TargetB.Position.Z);

            Vector3D vectorB = new Vector3D(-1, 0, 0);

            return Vector3D.AngleBetween(vectorA, vectorB);
        }

        public double TrunkSpin(DrawingContext dc, Target TargetA, Target TargetB)
        {
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, 0, TargetA.point3D().Z - TargetB.point3D().Z);
            Vector3D vectorB = new Vector3D(-1, 0, 0);

            double theta = Vector3D.AngleBetween(vectorA, vectorB);

            if (TargetA.point3D().Z > TargetB.point3D().Z)
                theta = -theta;

            dc.DrawText(new FormattedText(theta.ToString("f0"),
            CultureInfo.GetCultureInfo("en-us"),
            FlowDirection.LeftToRight,
            new Typeface("Verdana"),
            25, brushGreenYellow),
            new Point(TargetB.point2D().X - 35, TargetB.point2D().Y - 35));

            return theta;

        }

        public double TrunkTilt(Target TargetA, Target TargetB)
        {
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, 0);
            Vector3D vectorB = new Vector3D(-1, 0, 0);


            return Vector3D.AngleBetween(vectorA, vectorB);
        }

        public double HeadSpin(Target TargetA, Target TargetB)
        {
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, 0, TargetA.point3D().Z - TargetB.point3D().Z);
            Vector3D vectorB = new Vector3D(0, 0, -1);
            double theta = Vector3D.AngleBetween(vectorA, vectorB);

            if ((TargetA.point3D().X > TargetB.point3D().X))
                theta = -theta;

            return theta;
        }

        public double HeadTilt(Target TargetA, Target TargetB)
        {
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, 0);
            Vector3D vectorB = new Vector3D(0, -1, 0);  //待驗證

            return Vector3D.AngleBetween(vectorA, vectorB);
        }

        public double HeadTiltFace(Vector vec)
        {
            Vector vectorB = new Vector(0, 1);
            double theta = Vector.AngleBetween(vec, vectorB);

            return theta;
            //if (theta >= 0)
            //    return 180 - theta;
            //else
            //    return -theta - 180;
        }

        public double HeadSpinFace(Vector vec)
        {
            Vector vectorB = new Vector(0, -1);
            double theta = Vector.AngleBetween(vec, vectorB);
            if (theta >= 0)
                return 180 - theta;
            else
                return -theta - 180;
        }

        public double Length(Target TargetA, Target TargetB)
        {
            double AA = Math.Ceiling(Math.Sqrt((Math.Pow(TargetA.point3D().X - TargetB.point3D().X, 2) + Math.Pow(TargetA.point3D().Y - TargetB.point3D().Y, 2) + Math.Pow(TargetA.point3D().Z - TargetB.point3D().Z, 2))) * 100);
            
            return AA;

        }

        public double AngleBetween(Target TargetA, Target TargetB, Target TargetC)
        {
            //3D
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, TargetA.point3D().Z - TargetB.point3D().Z);
            Vector3D vectorB = new Vector3D(TargetC.point3D().X - TargetB.point3D().X, TargetC.point3D().Y - TargetB.point3D().Y, TargetC.point3D().Z - TargetB.point3D().Z);

            //2D
            //Vector3D vectorA = new Vector3D(TargetA.point2D().X - TargetB.point2D().X, TargetA.point2D().Y - TargetB.point2D().Y, 0);
            //Vector3D vectorB = new Vector3D(1, 0, 0);

            //double theta = Math.Abs(Vector3D.AngleBetween(vectorA, vectorB));
            double theta = Vector3D.AngleBetween(vectorA, vectorB);
            //theta = 90 - theta;
            //if (TargetA.point3D().Y < TargetB.point3D().Y) theta = -theta;

            
            return theta;
        }

        public double AngleBetween(DrawingContext dc, Target TargetA, Target TargetB, Target TargetC, bool show)
        {
            //3D
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, TargetA.point3D().Z - TargetB.point3D().Z);
            Vector3D vectorB = new Vector3D(TargetC.point3D().X - TargetB.point3D().X, TargetC.point3D().Y - TargetB.point3D().Y, TargetC.point3D().Z - TargetB.point3D().Z);

            //2D
            //Vector3D vectorA = new Vector3D(TargetA.point2D().X - TargetB.point2D().X, TargetA.point2D().Y - TargetB.point2D().Y, 0);
            //Vector3D vectorB = new Vector3D(1, 0, 0);

            //double theta = Math.Abs(Vector3D.AngleBetween(vectorA, vectorB));
            double theta = Vector3D.AngleBetween(vectorA, vectorB);
            //theta = 90 - theta;
            //if (TargetA.point3D().Y < TargetB.point3D().Y) theta = -theta;

            if (show)       //show angle text
            {
                dc.DrawText(new FormattedText(theta.ToString("f0"),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                25, brushDeepSkyBlue),
                new Point(TargetB.point2D().X - 35, TargetB.point2D().Y - 35));

                dc.DrawLine(PenDeepSkyBlue, TargetB.point2D(), TargetA.point2D());    //show angle line 
                dc.DrawLine(PenDeepSkyBlue, TargetB.point2D(), TargetC.point2D());
            }
            return theta;
        }

        public Vector3D TwoPointVector(Target TargetA, Target TargetB, DrawingContext dc, bool show)
        {
            Vector3D vectorC = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, TargetA.point3D().Z - TargetB.point3D().Z);
            if (show)
            {
                dc.DrawText(new FormattedText(vectorC.X.ToString("f2") + ", " + vectorC.Y.ToString("f2") + ", " + vectorC.Z.ToString("f2"),
                   CultureInfo.GetCultureInfo("en-us"),
                   FlowDirection.LeftToRight,
                   new Typeface("Verdana"),
                   25, brushDeepSkyBlue),
                   new Point(TargetB.point2D().X + 20, TargetB.point2D().Y + 20));
            }
            return vectorC;
            //dc.DrawLine(PenDeepSkyBlue, TargetB.point2D(), TargetA.point2D());    //show angle line 
            //dc.DrawLine(PenDeepSkyBlue, TargetB.point2D(), TargetC.point2D());
        }

        public Vector3D CrossProduct(Target TargetA, Target TargetB, Target TargetC)
        {
            Vector3D vectorA = new Vector3D(TargetA.point3D().X - TargetB.point3D().X, TargetA.point3D().Y - TargetB.point3D().Y, TargetA.point3D().Z - TargetB.point3D().Z);
            Vector3D vectorB = new Vector3D(TargetC.point3D().X - TargetB.point3D().X, TargetC.point3D().Y - TargetB.point3D().Y, TargetC.point3D().Z - TargetB.point3D().Z);
            return Vector3D.CrossProduct(vectorA, vectorB);
        }

        public int DetermineLean(Vector3D lineVector, Vector3D floorVector)
        {
            Vector line = new Vector(lineVector.X, lineVector.Y);
            if (line.X < 0)
            {
                line.Negate();
            }
            Vector floor = new Vector(floorVector.X, floorVector.Y);

            double angle = Vector.AngleBetween(line, floor);
            int result;
            if (angle > 90)
            {
                //右邊高
                result = 1;
            }
            else if (angle < 90)
            {
                //左邊高
                result = -1;
            }
            else
            {
                //一樣高
                result = 0;
            }
            return result;
        }

    }
    public class VerticalData
    {
        public double Right;
        public double Left;
        public double Mid;
    }

}
