using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectCoordinateMapping
{
    public class ZoomStruct
    {
        private bool isZoom;
        private double zoomRatio;
        private int zoomOffsetX;
        private int zoomOffsetY;

        public ZoomStruct()
        {
            isZoom = false;
            zoomRatio = 1.0;
            zoomOffsetX = 0;
            zoomOffsetY = 0;
        }

        public bool IsZoom
        {
            get
            {
                return isZoom;
            }
            set
            {
                isZoom = value;
            }
        }

        public double ZoomRatio
        {
            get
            {
                return zoomRatio;
            }
            set
            {
                zoomRatio = value;
            }
        }

        public int ZoomOffsetX
        {
            get
            {
                return zoomOffsetX;
            }
            set
            {
                zoomOffsetX = value;
            }
        }

        public int ZoomOffsetY
        {
            get
            {
                return zoomOffsetY;
            }
            set
            {
                zoomOffsetY = value;
            }
        }
    }
}
