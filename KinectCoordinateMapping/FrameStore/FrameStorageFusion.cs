using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace KinectCoordinateMapping.FrameStore
{
    public class FrameStorageFusion
    {
        public FrameStorage FuseBodyPoint(List<FrameStorage> frameList)
        {
            FrameStorage resultFrame = new FrameStorage();
            CameraSpacePoint basePoint = new CameraSpacePoint();
            ColorSpacePoint basePointColor = new ColorSpacePoint();
            if (frameList.Count == 0)
            {

            }
            else
            {
                for (int i = 0; i < frameList.Count; i++)
                {
                    if (frameList[i].NeckPoint != null)
                    {
                        basePoint = frameList[i].NeckPoint;
                        basePointColor = frameList[i].NeckPointColor;

                        resultFrame.NeckPointColor = frameList[i].NeckPointColor;
                        resultFrame.NeckPoint = frameList[i].NeckPoint;
                        resultFrame.floorVector = frameList[i].floorVector;
                        resultFrame.ColorInSkeleton = frameList[i].ColorInSkeleton;
                        resultFrame.manualTargetList = new List<Target>();
                        resultFrame.bitmapSource = frameList[i].bitmapSource;
                        resultFrame.groupList = frameList[i].groupList;

                        break;
                    }
                }
                List<Target> initList = new List<Target>();
                for (int i = 0; i < frameList.Count; i++)
                {
                    initList.AddRange(CombineToBase(basePoint, basePointColor, frameList[i]));
                }
                resultFrame.manualTargetList.AddRange(initList);
            }
            return resultFrame;
        }

        public FrameStorage FuseBodyPoint(List<FrameStorage> frameList, int startIndex, int endIndex)
        {
            FrameStorage resultFrame = new FrameStorage();
            CameraSpacePoint basePoint = new CameraSpacePoint();
            ColorSpacePoint basePointColor = new ColorSpacePoint();
            if (frameList.Count == 0)
            {

            }
            else
            {
                for (int i = startIndex; i < endIndex; i++)
                {
                    if (frameList[i].NeckPoint != null)
                    {
                        basePoint = frameList[i].NeckPoint;
                        basePointColor = frameList[i].NeckPointColor;

                        resultFrame.NeckPointColor = frameList[i].NeckPointColor;
                        resultFrame.NeckPoint = frameList[i].NeckPoint;
                        resultFrame.floorVector = frameList[i].floorVector;
                        resultFrame.ColorInSkeleton = frameList[i].ColorInSkeleton;
                        resultFrame.manualTargetList = new List<Target>();
                        resultFrame.bitmapSource = frameList[i].bitmapSource;
                        resultFrame.groupList = frameList[i].groupList;

                        break;
                    }
                }
                List<Target> initList = new List<Target>();
                for (int i = 0; i < frameList.Count; i++)
                {
                    initList.AddRange(CombineToBase(basePoint, basePointColor, frameList[i]));
                }
                resultFrame.manualTargetList.AddRange(initList);
            }
            return resultFrame;
        }

        private List<Target> CombineToBase(CameraSpacePoint basepoint, ColorSpacePoint basepointcolor, FrameStorage frame)
        {
            double distanceX = frame.NeckPoint.X - basepoint.X;
            double distanceY = frame.NeckPoint.Y - basepoint.Y;
            double distanceZ = frame.NeckPoint.Z - basepoint.Z;

            double distanceColorX = frame.NeckPointColor.X - basepointcolor.X;
            double distanceColorY = frame.NeckPointColor.Y - basepointcolor.Y;
            for (int j = 0; j < frame.manualTargetList.Count; j++)
            {
                Point3D temppoint = new Point3D();
                temppoint.X = frame.manualTargetList[j].point3D().X + distanceX;
                temppoint.Y = frame.manualTargetList[j].point3D().Y + distanceY;
                temppoint.Z = frame.manualTargetList[j].point3D().Z + distanceZ;
                frame.manualTargetList[j].setPoint3D(temppoint);

                Point temppoint2d = new Point();
                temppoint2d.X = frame.manualTargetList[j].point2D().X + distanceColorX;
                temppoint2d.Y = frame.manualTargetList[j].point2D().Y + distanceColorY;
                frame.manualTargetList[j].setPoint2D(temppoint2d);
            }
            return frame.manualTargetList;
        }
    }
}
