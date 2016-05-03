using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCoordinateMapping.ProgressSquencer
{

    public enum Step {   None,
                         FrontReady, FrontHead, FrontShoulder, Iliac, Trochanter, Tuberosity, Patella, KneeJointLine, LateralmMlleoli, MedialMalleoli,
                         BackReady, ScapulaUp, Scapula, MidpointLeg, IntermalleolarLine, Tendon, 
                         SideReady, TragusAndSeventhCervicalVertebra, AcromionAndElbow, AnteriorSuperiorIliacSpineAndPosteriorSuperiorIliacSpine, TrochanterAndKneeJoint, MalleolusAndMetatarsus,
                         Done }

    public class ProgressSeq
    {
        public String[] hintName = {"無所事事",
                                    "請站在指定位置，正面朝向攝影機", "Tragus", "Acromion", "anterior superior iliac spine", "Trochanter", "right and left tibial tuberosity", "center of the right andleft patella", "lateral projection of the right and left knee joint line", "lateral malleoli",
                                    "請背朝攝影機", "inferior angle of the scapula", "acromion and 8 seventh cervical vertebra",
                                    "請側面朝向攝影機",
                                    "完成" };

        private JointType[] jointType = {  JointType.Head, JointType.Head, JointType.ShoulderLeft, JointType.ShoulderRight, JointType.HipLeft, JointType.HipRight, };

        private Step nowStep = Step.None;

        public Step NowStep
        {
            set
            {
                nowStep = value;
            }
            get
            {
                return nowStep;
            }
        }

        public ProgressSeq()
        {
            nowStep = Step.None;
            List<HintElement> hintElementList = new List<HintElement>();
            hintElementList.Add(new HintNone());
            hintElementList.Add(new HintFrontReady());
            hintElementList.Add(new HintFrontHead());
            hintElementList.Add(new HintDone());
        }
        
        public void Start()
        {
            nowStep++;
        }

        public bool CheckPointIfRoughlyCorrect(Target targetA, Target targetB, IList<Body> Bodies, CoordinateMapper coordinateMapper)
        {
            foreach (var body in Bodies)
            {
                if (body.IsTracked)
                {
                    Joint joint3 = body.Joints[jointType[(int)nowStep]];
                    ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(joint3.Position);

                    if (CheckIsIntheRange(colorPoint, targetA)&& CheckIsIntheRange(colorPoint, targetB))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckIsIntheRange(ColorSpacePoint a, Target b)
        {
            if (a.X + 100 > b.point2D().X && a.X - 100 < b.point2D().X)
            {
                if (a.Y + 100 > b.point2D().Y && a.Y - 100 < b.point2D().Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
