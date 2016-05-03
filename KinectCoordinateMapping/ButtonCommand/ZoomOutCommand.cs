using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCoordinateMapping.ButtonCommand
{
    class ZoomOutCommand : CommandInterface
    {
        ZoomStruct zoomStruct;
        public ZoomOutCommand(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.zoomStruct = mainWindow.zoomStruct;
        }

        public override void LeftButtonPress(int x, int y)
        {

        }

        public override void LeftButtonRelease(int x, int y)
        {
            zoomStruct.ZoomRatio -= (double)0.01;
            if (zoomStruct.ZoomRatio < 1.0)
            {
                zoomStruct.ZoomRatio = 1.0;
            }
            zoomStruct.ZoomOffsetX = 0;
            zoomStruct.ZoomOffsetY = 0;
            TargetList = new List<Target>();
        }

        public override void MouseMove(int x, int y)
        {
        }
    }
}
