using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KinectCoordinateMapping.ButtonCommand
{
    public class CommandInterface
    {
        protected bool MouseLeftPressed = false;
        protected int startX;
        protected int startY;
        protected int middleX;
        protected int middleY;
        protected int endX;
        protected int endY;
        protected double zoomRatio = 3.0;
        public List<Target> TargetList = new List<Target>();
        protected MainWindow mainWindow;

        public CommandInterface()
        {
            TargetList = new List<Target>();
        }
        public virtual void LeftButtonPress(int x, int y)
        {

        }

        public virtual void LeftButtonRelease(int x, int y)
        {

        }

        public virtual void MouseMove(int x, int y)
        {

        }

        public virtual void Draw(Canvas canvas, DisplayStruct displayStruct)
        {

        }

        
    }
}
