using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace KinectCoordinateMapping
{
    class SettingParameter
    {
        public enum mode { None, KneeExtensionFlexion, ShoulderFlexion, ShoulderAbduction, HipKneeFlexion }
        public CameraSpacePoint startPoint { get; set; }
        public CameraSpacePoint nowPoint { get; set; }

        public double appointAngle { get; set; }

        public double PROMAngle { get; set; }

        public double AROMAngle { get; set; }

        public double timeInApointRange { get; set; }
        public int successCount { get; set; }

        public int totalCount { get; set; }
    }
}
