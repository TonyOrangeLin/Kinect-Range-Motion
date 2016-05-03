using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace KinectCoordinateMapping.FrameStore
{
    [Serializable()]
    public class FrameStorage 
    {
        public CameraSpacePoint[] ColorInSkeleton;
        [field: NonSerialized()]
        public BitmapSource bitmapSource;
        public Vector4 floorVector;
        public List<Group> groupList;
        public List<Target> manualTargetList;
        public CameraSpacePoint NeckPoint;//暫時用來判斷人的位置
        public ColorSpacePoint NeckPointColor;
        public FrameStorage()
        {
            floorVector = new Vector4();
            groupList = new List<Group>();
            manualTargetList = new List<Target>();
            NeckPoint = new CameraSpacePoint();
            NeckPointColor = new ColorSpacePoint();
        }

        public FrameStorage(Vector4 a, BitmapSource b, CameraSpacePoint[] c, List<Group> d)
        {
            floorVector = a;
            bitmapSource = b;
            ColorInSkeleton = c;
            groupList = d;
            manualTargetList = new List<Target>();
        }

        public FrameStorage(Vector4 a, BitmapSource b, CameraSpacePoint[] c, List<Group> d, List<Target> e, CameraSpacePoint f, ColorSpacePoint g)
        {
            floorVector = a;
            bitmapSource = b;
            ColorInSkeleton = c;
            groupList = d;
            manualTargetList = e;
            NeckPoint = f;
            NeckPointColor = g;
        }
    }
}
