using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KinectCoordinateMapping
{
    public static class CoordinateTransform
    {
        static public Point TransformFromScreenToFullScreen(int x, int y, ZoomStruct zoomStruct)
        {
            double TransformX;
            double TransformY;

            TransformX = ((1920.0 / zoomStruct.ZoomRatio) / 640.0 * x) + zoomStruct.ZoomOffsetX;
            TransformY = ((1080.0 / zoomStruct.ZoomRatio) / 360.0 * y) + zoomStruct.ZoomOffsetY;

            Point newPoint = new Point(TransformX, TransformY);

            return newPoint;
        }

        static public Point ReverseFromFullScreenToScreen(int x, int y, ZoomStruct zoomStruct)
        {
            double TransformX;
            double TransformY;

            TransformX =((x - zoomStruct.ZoomOffsetX) * 640.0  * (zoomStruct.ZoomRatio / 1920.0));
            TransformY =((y - zoomStruct.ZoomOffsetY) * 360.0 * (zoomStruct.ZoomRatio / 1080.0));

            Point newPoint = new Point(TransformX, TransformY);

            return newPoint;
        }
    }
}
