using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCoordinateMapping.ProgressSquencer
{
    public class HintBackReady : HintElement
    {
        public HintBackReady()
        {
            hintName = "請站在指定位置，背面朝向攝影機";
            nowStep = Step.BackReady;

        }
    }

    public class HintScapulaUp : HintElement
    {
        public HintScapulaUp()
        {
            hintName = "put right thummb on 17 third thoracic vertebra";
            nowStep = Step.ScapulaUp;
            jointType = new JointType[2] { JointType.SpineMid, JointType.SpineMid };
            rightID = 17;//只用右手判斷
            leftID = 0;
        }
    }

    public class HintScapula : HintElement
    {
        public HintScapula()
        {
            hintName = "inferior angle of the right and left scapula";
            nowStep = Step.Scapula;
            jointType = new JointType[2] { JointType.SpineMid, JointType.SpineMid };
            rightID = 7;
            leftID = 8;
        }
    }

    public class HintMidpointLeg : HintElement
    {
        public HintMidpointLeg()
        {
            hintName = "midpoint of the leg";
            nowStep = Step.MidpointLeg;
            jointType = new JointType[2] { JointType.KneeLeft, JointType.KneeRight };
            rightID = 32;
            leftID = 33;
        }
    }

    public class HintIntermalleolarLine : HintElement
    {
        public HintIntermalleolarLine()
        {
            hintName = "intermalleolar line";
            nowStep = Step.IntermalleolarLine;
            jointType = new JointType[2] { JointType.AnkleRight, JointType.AnkleLeft };
            rightID = 35;
            leftID = 39;
        }
    }

    public class HintTendon : HintElement
    {
        public HintTendon()
        {
            hintName = "bilateral calcaneal tendon.";
            nowStep = Step.Tendon;
            jointType = new JointType[2] { JointType.AnkleRight, JointType.AnkleLeft };
            rightID = 37;
            leftID = 41;
        }
    }
}
