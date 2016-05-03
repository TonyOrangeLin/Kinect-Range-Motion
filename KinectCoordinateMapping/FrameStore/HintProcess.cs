using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace KinectCoordinateMapping.FrameStore
{
    public class HintProcess
    {
        private bool isSoundHint;
        private bool isColorHint;
        private Reminder reminderWindow;
        public HumanBody humanBodyWindow;
        private TTSEngine ttsEngine;
        private DispatcherTimer timer = new DispatcherTimer();
        private bool hintFlag;
        private int hintCount;
        public HintProcess()
        {
            isSoundHint = true;
            isColorHint = true;
            hintFlag = false;
            hintCount = 0;
            reminderWindow = new Reminder();
            humanBodyWindow = new HumanBody();
            ttsEngine = new TTSEngine();
            if (isColorHint)
            {
                humanBodyWindow.Show();
                reminderWindow.Show();
            } 
        }

        public void TTSHint(String text)
        {
            ttsEngine.SpeakText(text);
        }

        public HintProcess(bool sound, bool color)
        {
            isColorHint = color;
            hintFlag = false;
            hintCount = 0;
            reminderWindow = new Reminder();
            humanBodyWindow = new HumanBody();
            ttsEngine = new TTSEngine();

            
            if (isColorHint)
            {
                reminderWindow.Show();
                humanBodyWindow.Show();
            }

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        public void HintGreen()
        {
            if (isColorHint)
            {
                reminderWindow.backgroundGrid.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                hintFlag = true;
                hintCount = 0;
            }
        }

        public void HintYellow()
        {
            if (isColorHint)
            {
                reminderWindow.backgroundGrid.Background = new SolidColorBrush(Color.FromRgb(255, 255, 0));
                hintFlag = true;
                hintCount = 0;
            }
        }

        public void HintRed()
        {
            if (isColorHint)
            {
                reminderWindow.backgroundGrid.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                hintFlag = true;
                hintCount = 0;
            }
        }

        public void Reset()
        {
            
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (hintFlag)
            {
                hintCount++;
                if (hintCount >= 3)
                {
                    hintFlag = false;
                    reminderWindow.backgroundGrid.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }

            }
        }
    }
}
