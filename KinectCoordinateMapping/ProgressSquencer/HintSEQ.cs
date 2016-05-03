using KinectCoordinateMapping.ProgressSquencer;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCoordinateMapping
{
    public class HintElement
    {
        protected String hintName;
        protected Step nowStep = Step.None;
        protected JointType[] jointType;
        protected int rightID;
        protected int leftID;
        protected bool isJudgeUpDown = false;//如果是TRUE , RIGHT ID代表的是下方的ID  LEFT ID
        public String HintSentence
        {
            get
            {
                return hintName;
            }
            set
            {
                hintName = value;
            }
        }

        public Step NowStep
        {
            get
            {
                return nowStep;
            }
            set
            {
                nowStep = value;
            }
        }

        public JointType[] JointTypeList
        {
            get
            {
                return jointType;
            }
            set
            {
                jointType = value;
            }
        }

        public bool CheckPointIfRoughlyCorrect(Target targetA, Target targetB, IList<Body> Bodies, CoordinateMapper coordinateMapper)
        {
            foreach (var body in Bodies)
            {
                if (body.IsTracked)
                {
                    Joint joint3 = body.Joints[jointType[(int)nowStep]];
                    ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(joint3.Position);

                    if (CheckIsIntheRange(colorPoint, targetA) && CheckIsIntheRange(colorPoint, targetB))
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

        public void TagTargetID(List<Target> pingList)
        {
            if (isJudgeUpDown)
            {
                if (pingList[0].point2D().Y > pingList[1].point2D().Y)
                {
                    //0在右邊 1在左邊
                    pingList[0].TargetID = rightID;
                    pingList[1].TargetID = leftID;
                }
                else
                {
                    pingList[0].TargetID = leftID;
                    pingList[1].TargetID = rightID;
                }
            }
            else
            { 
                if (pingList[0].point2D().X > pingList[1].point2D().X)
                {
                    //0在右邊 1在左邊
                    pingList[0].TargetID = rightID;
                    pingList[1].TargetID = leftID;
                }
                else
                {
                    pingList[0].TargetID = leftID;
                    pingList[1].TargetID = rightID;
                }
            }
        }
    }

    public class HintNone : HintElement
    {
        public HintNone()
        {
            hintName = "無所事事";
            nowStep = Step.None;

        }
    }

    public class HintDone : HintElement
    {
        public HintDone()
        {
            hintName = "完成";
            nowStep = Step.Done;

        }
    }
}
