using KinectCoordinateMapping.FrameStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace KinectCoordinateMapping.ProfileDocument
{
    public class CalculateFrontResult
    {
        private AngleCalulator AngleCal = new AngleCalulator();
        public List<Double> CalculateResult(List<Double> Data, FrameStorage storage, Canvas canvas, ZoomStruct zoomStruct, DisplayStruct displayStruct)
        {
            FrameStorage frontFrame = storage;
            Target targetA = new Target(0);
            Target targetB = new Target(0);
            Target targetC = new Target(0);
            Target targetD = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 2)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 3)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0)
            {
                Data[1] = AngleCal.Hor(targetA, targetB);//A1


                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                Target horTarget = new Target(0);
                horTarget.Setting((int)targetA.point2D().X, (int)targetB.point2D().Y);
                formTargetList.Add(horTarget);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[1]));
            }

            targetA = new Target(0);
            targetB = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 5)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 6)
                {
                    targetB = frontFrame.manualTargetList[i];
                }

            }

            if (targetA.TargetID != 0 && targetB.TargetID != 0)
            {
                Data[2] = AngleCal.Hor(targetA, targetB);//A1

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                Target horTarget = new Target(0);
                horTarget.Setting((int)targetA.point2D().X, (int)targetB.point2D().Y);
                formTargetList.Add(horTarget);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[2]));
            }
            targetA = new Target(0);
            targetB = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 12)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 13)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0)
            {
                Data[3] = AngleCal.Hor(targetA, targetB);//A3

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                Target horTarget = new Target(0);
                horTarget.Setting((int)targetA.point2D().X, (int)targetB.point2D().Y);
                formTargetList.Add(horTarget);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[3]));
            }
            targetA = new Target(0);
            targetB = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 5)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 6)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 12)
                {
                    targetC = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 13)
                {
                    targetD = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0 && targetC.TargetID != 0 && targetD.TargetID != 0)
            {
                Data[4] = AngleCal.twoLines(targetB, targetA, targetD, targetC);//A4
            }
            targetA = new Target(0);
            targetB = new Target(0);
            targetC = new Target(0);
            targetD = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 14)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 16)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 22)
                {
                    targetC = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0 && targetC.TargetID != 0)
            {
                Data[5] = AngleCal.threePts(targetA, targetB, targetC);

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                formTargetList.Add(targetC);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[5]));
            }
            targetA = new Target(0);
            targetB = new Target(0);
            targetC = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 15)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 19)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 25)
                {
                    targetC = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0 && targetC.TargetID != 0)
            {
                Data[6] = AngleCal.threePts(targetA, targetB, targetC);
                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                formTargetList.Add(targetC);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[6]));
            }
            targetA = new Target(0);
            targetB = new Target(0);
            targetC = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 12)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 23)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 13)
                {
                    targetC = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 26)
                {
                    targetD = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0 && targetC.TargetID != 0 && targetD.TargetID != 0)
            {
                double temp1 = AngleCal.Length(targetA, targetB);
                double temp2 = AngleCal.Length(targetC, targetD);


                Data[7] = temp2 - temp1;

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                frontFrame.groupList.Add(new Group(formTargetList, mode.LenghtMode, true, temp1));


                formTargetList = new List<Target>();
                formTargetList.Add(targetC);
                formTargetList.Add(targetD);
                frontFrame.groupList.Add(new Group(formTargetList, mode.LenghtMode, true, temp2));
            }
            targetA = new Target(0);
            targetB = new Target(0);
            targetC = new Target(0);
            targetD = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 18)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 21)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0)
            {
                Data[8] = AngleCal.Hor(targetA, targetB);

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                Target horTarget = new Target(0);
                horTarget.Setting((int)targetA.point2D().X, (int)targetB.point2D().Y);
                formTargetList.Add(horTarget);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[8]));

            }
            targetA = new Target(0);
            targetB = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 12)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 17)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 18)
                {
                    targetC = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0 && targetC.TargetID != 0)
            {
                Data[9] = AngleCal.threePts(targetA, targetB, targetC);

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                formTargetList.Add(targetC);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[9]));

            }
            targetA = new Target(0);
            targetB = new Target(0);
            targetC = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 13)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 20)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 21)
                {
                    targetC = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0 && targetC.TargetID != 0)
            {
                Data[10] = AngleCal.threePts(targetA, targetB, targetC);

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                formTargetList.Add(targetC);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[10]));
            }
            targetA = new Target(0);
            targetB = new Target(0);
            targetC = new Target(0);

            

            return Data;
        }

        public List<Double> CalculateBackResult(List<Double> Data, FrameStorage storage)
        {
            FrameStorage frontFrame = storage;
            Target targetA = new Target(0);
            Target targetB = new Target(0);
            Target targetC = new Target(0);
            Target targetD = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 32)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 35)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 37)
                {
                    targetC = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0 && targetC.TargetID != 0)
            {
                Data[19] = AngleCal.threePts(targetA, targetB, targetC);

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                formTargetList.Add(targetC);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[19]));
            }
            targetA = new Target(0);
            targetB = new Target(0);
            targetC = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 33)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 39)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 41)
                {
                    targetC = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0 && targetC.TargetID != 0)
            {
                Data[20] = AngleCal.threePts(targetA, targetB, targetC);

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                formTargetList.Add(targetC);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[20]));
            }
            targetA = new Target(0);
            targetB = new Target(0);
            targetC = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 7)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 8)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 17)
                {
                    targetC = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0 && targetC.TargetID != 0)
            {
                Data[21] = AngleCal.threePts(targetA, targetB, targetC);  //A21
                Data[22] = AngleCal.threePts(targetC, targetA, targetB);  //A22
                Data[23] = AngleCal.threePts(targetB, targetC, targetA);  //A23

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                formTargetList.Add(targetC);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[21]));

                formTargetList = new List<Target>();
                formTargetList.Add(targetC);
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[22]));

                formTargetList = new List<Target>();
                formTargetList.Add(targetB);
                formTargetList.Add(targetC);
                formTargetList.Add(targetA);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[23]));

            }
            targetA = new Target(0);
            targetB = new Target(0);
            targetC = new Target(0);

            return Data;
        }

        public List<Double> CalculateSideResult(List<Double> Data, FrameStorage storage)
        {
            FrameStorage frontFrame = storage;
            Target targetA = new Target(0);
            Target targetB = new Target(0);
            Target targetC = new Target(0);
            Target targetD = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 2)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 8)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0)
            {
                Data[11] = AngleCal.Hor(targetA, targetB);//A11

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                Target horTarget = new Target(0);
                horTarget.Setting((int)targetA.point2D().X, (int)targetB.point2D().Y);
                formTargetList.Add(horTarget);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[11]));

            }
            targetA = new Target(0);
            targetB = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 5)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 2)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0)
            {
                Data[12] = AngleCal.Ver(targetA, targetB);//A12
            }
            targetA = new Target(0);
            targetB = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 5)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 23)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0)
            {
                Data[13] = AngleCal.Ver(targetA, targetB);//A13
            }
            targetA = new Target(0);
            targetB = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 5)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 23)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 30)
                {
                    targetC = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0 && targetC.TargetID != 0)
            {
                Data[14] = AngleCal.threePts(targetA, targetB, targetC);//A14

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                formTargetList.Add(targetC);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[14]));
            }
            targetA = new Target(0);
            targetB = new Target(0);
            targetC = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 5)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 30)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0)
            {
                Data[15] = AngleCal.Ver(targetA, targetB);//A15
            }
            targetA = new Target(0);
            targetB = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 21)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 22)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0)
            {
                Data[16] = AngleCal.Hor(targetA, targetB);//A15

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                Target horTarget = new Target(0);
                horTarget.Setting((int)targetA.point2D().X, (int)targetB.point2D().Y);
                formTargetList.Add(horTarget);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[16]));
            }
            targetA = new Target(0);
            targetB = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 23)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 24)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 30)
                {
                    targetC = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0 && targetC.TargetID != 0)
            {
                Data[17] = AngleCal.threePts(targetA, targetB, targetC);//A17

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                formTargetList.Add(targetC);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[17]));
            }
            targetA = new Target(0);
            targetB = new Target(0);
            targetC = new Target(0);

            for (int i = 0; i < frontFrame.manualTargetList.Count; i++)
            {
                if (frontFrame.manualTargetList[i].TargetID == 24)
                {
                    targetA = frontFrame.manualTargetList[i];
                }
                if (frontFrame.manualTargetList[i].TargetID == 30)
                {
                    targetB = frontFrame.manualTargetList[i];
                }
            }
            if (targetA.TargetID != 0 && targetB.TargetID != 0)
            {
                Data[18] = AngleCal.Hor(targetA, targetB);//A18

                List<Target> formTargetList = new List<Target>();
                formTargetList.Add(targetA);
                formTargetList.Add(targetB);
                Target horTarget = new Target(0);
                horTarget.Setting((int)targetA.point2D().X, (int)targetB.point2D().Y);
                formTargetList.Add(horTarget);
                frontFrame.groupList.Add(new Group(formTargetList, mode.AngleMode, true, Data[18]));
            }
            targetA = new Target(0);
            targetB = new Target(0);

            return Data;
        }
    }
}
