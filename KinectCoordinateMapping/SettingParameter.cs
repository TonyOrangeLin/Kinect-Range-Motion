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
        public enum AROMmode { None, KneeExtensionFlexion, ShoulderFlexion, ShoulderAbduction, HipKneeFlexion }
        public double startAngle { get; set; }
        public double nowAngle { get; set; }

        public double appointAngle { get; set; }

        public double PROMAngle { get; set; }

        public double AROMAngle { get; set; }

        public double timeInApointRange { get; set; }
        public int successCount { get; set; }

        public int totalCount { get; set; }

        public int limitCount { get; set; }

        public bool isNearAppointPoint { get; set; } 

        public int nearAppointPointCount { get; set; }

        public double toleranceRange { get; set; }
    }
}
