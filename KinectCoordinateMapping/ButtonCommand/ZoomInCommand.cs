using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCoordinateMapping.ButtonCommand
{
    class ZoomInCommand : CommandInterface
    {
        ZoomStruct zoomStruct;
        public ZoomInCommand(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.zoomStruct = mainWindow.zoomStruct;
        }

        public override void LeftButtonPress(int x, int y)
        {

        }

        public override void LeftButtonRelease(int x, int y)
        {
            zoomStruct.IsZoom = true;
            zoomStruct.ZoomRatio += (double)0.01;
            if (zoomStruct.ZoomRatio > 3.0)
            {
                zoomStruct.ZoomRatio = 3.0;
            }

            zoomStruct.ZoomOffsetX = 0;
            zoomStruct.ZoomOffsetY = 0;
            //int screenSizeWidth = ((int)(1920 / (4.0 - zoomRatio))) / 2;
            //int screenSizeHeight = ((int)(1080 / (4.0 - zoomRatio))) / 2;
            //zoomStruct.ZoomOffsetX = (int)(x * (zoomRatio + (double)0.1) - screenSizeWidth);
            //zoomStruct.ZoomOffsetY = (int)(y * (zoomRatio + (double)0.1) - screenSizeHeight);
            //int tempRX = (int)(x * (zoomRatio + (double)0.1) + screenSizeWidth);
            //int tempRY = (int)(y * (zoomRatio + (double)0.1) + screenSizeHeight);
            //if (zoomStruct.ZoomOffsetX < 0)
            //{
            //    zoomStruct.ZoomOffsetX = 0;
            //}
            //if (zoomStruct.ZoomOffsetY < 0)
            //{
            //    zoomStruct.ZoomOffsetY = 0;
            //}
            //if (tempRX >= 1920)
            //{
            //    zoomStruct.ZoomOffsetX = zoomStruct.ZoomOffsetX - (tempRX - 1920);
            //}
            //if (tempRY >= 1080)
            //{
            //    zoomStruct.ZoomOffsetY = zoomStruct.ZoomOffsetY - (tempRY - 1080);
            //}

            //for (int i = 0; i < mainWindow.groupList.Count; i++)
            //{
            //    for (int j = 0; j < mainWindow.groupList[i].groupList.Count; j++)
            //    {
            //        mainWindow.groupList[i].groupList[j].RefreshZoomIn();
            //    }
            //}

            TargetList = new List<Target>();

        }

        public override void MouseMove(int x, int y)
        {

        }
    }
}
