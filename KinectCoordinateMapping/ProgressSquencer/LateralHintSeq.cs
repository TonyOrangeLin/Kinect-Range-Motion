using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCoordinateMapping.ProgressSquencer
{
    public class HintSideReady : HintElement
    {
        public HintSideReady()
        {
            hintName = "請站在指定位置，先背對攝影機，再原地向右轉九十度";
            nowStep = Step.SideReady;

        }
    }

    public class HintTragusAndSeventhCervicalVertebra : HintElement
    {
        public HintTragusAndSeventhCervicalVertebra()
        {
            hintName = "Tragus And Seventh Cervical Vertebra";
            nowStep = Step.TragusAndSeventhCervicalVertebra;
            jointType = new JointType[2] { JointType.Head, JointType.Head };
            rightID = 2;
            leftID = 8;
        }
    }

    public class HintAcromionAndElbow : HintElement
    {
        public HintAcromionAndElbow()
        {
            hintName = "Acromion And Elbow";
            nowStep = Step.AcromionAndElbow;
            rightID = 0;
            leftID = 5;
            isJudgeUpDown = true;
            // jointType = new JointType[2] { JointType.Head, JointType.Head };
        }
    }

    public class HintAnteriorSuperiorIliacSpineAndPosteriorSuperiorIliacSpine : HintElement
    {
        public HintAnteriorSuperiorIliacSpineAndPosteriorSuperiorIliacSpine()
        {
            hintName = "Anterior Superior Iliac Spine And Posterior Superior Iliac Spine";
            nowStep = Step.AnteriorSuperiorIliacSpineAndPosteriorSuperiorIliacSpine;
            rightID = 21;
            leftID = 22;
            // jointType = new JointType[2] { JointType.Head, JointType.Head };
        }
    }

    public class HintTrochanterAndKneeJoint : HintElement
    {
        public HintTrochanterAndKneeJoint()
        {
            hintName = "Trochanter And Knee Joint";
            nowStep = Step.TrochanterAndKneeJoint;
            isJudgeUpDown = true;
            rightID = 24;
            leftID = 23;
            //jointType = new JointType[2] { JointType.Head, JointType.Head };
        }
    }

    public class HintMalleolusAndMetatarsus : HintElement
    {
        public HintMalleolusAndMetatarsus()
        {
            hintName = "Malleolus And Metatarsus";
            nowStep = Step.MalleolusAndMetatarsus;
            rightID = 31;
            leftID = 30;
            jointType = new JointType[2] { JointType.AnkleRight, JointType.AnkleLeft };
        }
    }
}
