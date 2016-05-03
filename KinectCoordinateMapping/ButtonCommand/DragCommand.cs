using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCoordinateMapping.ButtonCommand
{
    public class DragCommand : CommandInterface
    {
        ZoomStruct zoomStruct;
        public DragCommand(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            zoomStruct = mainWindow.zoomStruct;
        }

        public override void LeftButtonPress(int x, int y)
        {
            if (zoomStruct.IsZoom)
            {
                MouseLeftPressed = true;
                startX = x;
                startY = y;
                middleX = x;
                middleY = y;
            }
        }

        public override void LeftButtonRelease(int x, int y)
        {
            if (MouseLeftPressed)
            {
                if (zoomStruct.IsZoom)
                {
                    MouseLeftPressed = false;
                    endX = x;
                    endY = y;

                    int distanceX = endX - startX;
                    int distanceY = endY - startY;

                    zoomStruct.ZoomOffsetX -= (int)(distanceX * zoomStruct.ZoomRatio);
                    zoomStruct.ZoomOffsetY -= (int)(distanceY * zoomStruct.ZoomRatio);
                    int tempRX = zoomStruct.ZoomOffsetX + (int)((double)1920 / zoomStruct.ZoomRatio);
                    int tempRY = zoomStruct.ZoomOffsetY + (int)((double)1080 / zoomStruct.ZoomRatio);
                    if (zoomStruct.ZoomOffsetX < 0)
                    {
                        zoomStruct.ZoomOffsetX = 0;
                    }
                    if (zoomStruct.ZoomOffsetY < 0)
                    {
                        zoomStruct.ZoomOffsetY = 0;
                    }
                    if (tempRX >= 1920)
                    {
                        zoomStruct.ZoomOffsetX = zoomStruct.ZoomOffsetX - (tempRX - 1920);
                    }
                    if (tempRY >= 1080)
                    {
                        zoomStruct.ZoomOffsetY = zoomStruct.ZoomOffsetY - (tempRY - 1080);
                    }
                }
            }
        }

        public override void MouseMove(int x, int y)
        {
            if (MouseLeftPressed)
            {
                if (zoomStruct.IsZoom)
                {
                   

                    int distanceX = x - middleX;
                    int distanceY = y - middleY;

                    zoomStruct.ZoomOffsetX -= (int)(distanceX * zoomStruct.ZoomRatio);
                    zoomStruct.ZoomOffsetY -= (int)(distanceY * zoomStruct.ZoomRatio);
                    int tempRX = zoomStruct.ZoomOffsetX + (int)((double)1920 / zoomStruct.ZoomRatio);
                    int tempRY = zoomStruct.ZoomOffsetY + (int)((double)1080 / zoomStruct.ZoomRatio);
                    if (zoomStruct.ZoomOffsetX < 0)
                    {
                        zoomStruct.ZoomOffsetX = 0;
                    }
                    if (zoomStruct.ZoomOffsetY < 0)
                    {
                        zoomStruct.ZoomOffsetY = 0;
                    }
                    if (tempRX >= 1920)
                    {
                        zoomStruct.ZoomOffsetX = zoomStruct.ZoomOffsetX - (tempRX - 1920);
                    }
                    if (tempRY >= 1080)
                    {
                        zoomStruct.ZoomOffsetY = zoomStruct.ZoomOffsetY - (tempRY - 1080);
                    }

                    middleX = x;
                    middleY = y;
                }
            }
 
        }
    }
}
