using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCoordinateMapping.ProgressSquencer
{
    public class HintFrontReady : HintElement
    {
        public HintFrontReady()
        {
            hintName = "請站在指定位置，正面朝向攝影機";
            nowStep = Step.FrontReady;

        }
    }

    public class HintFrontHead : HintElement
    {
        public HintFrontHead()
        {
            hintName = "Tragus";
            nowStep = Step.FrontShoulder;
            jointType = new JointType[2] { JointType.Head, JointType.Head };
            rightID = 3;
            leftID = 2;
        }
    }

    public class HintFrontShoulder : HintElement
    {
        public HintFrontShoulder()
        {
            hintName = "Acromion";
            nowStep = Step.FrontShoulder;
            jointType = new JointType[2] { JointType.ShoulderRight, JointType.ShoulderLeft };
            rightID = 6;
            leftID = 5;
        }
    }

    public class HintIliac : HintElement
    {
        public HintIliac()
        {
            hintName = "anterior superior iliac spine";
            nowStep = Step.Iliac;
            jointType = new JointType[2] { JointType.ShoulderRight, JointType.ShoulderLeft };
            rightID = 13;
            leftID = 12;
        }
    }

    public class HintTrochanter : HintElement
    {
        public HintTrochanter()
        {
            hintName = "anterior superior iliac spine";
            nowStep = Step.Iliac;
            jointType = new JointType[2] { JointType.ShoulderRight, JointType.ShoulderLeft };
            rightID = 15;
            leftID = 14;
        }
    }

    public class HintTuberosity : HintElement
    {
        public HintTuberosity()
        {
            hintName = "right and left tibial tuberosity;";
            nowStep = Step.Tuberosity;
            jointType = new JointType[2] { JointType.KneeRight, JointType.KneeLeft };
            rightID = 21;
            leftID = 18;
        }
    }

    public class HintPatella : HintElement
    {
        public HintPatella()
        {
            hintName = "center of the right andleft patella";
            nowStep = Step.Patella;
            jointType = new JointType[2] { JointType.KneeRight, JointType.KneeLeft };
            rightID = 20;
            leftID = 17;
        }
    }

    public class HintKneeJointLine : HintElement
    {
        public HintKneeJointLine()
        {
            hintName = "lateral projection of the right and left knee joint line";
            nowStep = Step.KneeJointLine;
            jointType = new JointType[2] { JointType.KneeRight, JointType.KneeLeft };
            rightID = 19;
            leftID = 16;
        }
    }

    public class HintLateralmMlleoli : HintElement
    {
        public HintLateralmMlleoli()
        {
            hintName = "lateral malleoli";
            nowStep = Step.LateralmMlleoli;
            jointType = new JointType[2] { JointType.AnkleRight, JointType.AnkleLeft };
            rightID = 25;
            leftID = 22;
        }
    }

    public class HintMedialMalleoli : HintElement
    {
        public HintMedialMalleoli()
        {
            hintName = "medial malleoli";
            nowStep = Step.LateralmMlleoli;
            jointType = new JointType[2] { JointType.AnkleRight, JointType.AnkleLeft };
            rightID = 26;
            leftID = 23;
        }
    }
}
