using KinectCoordinateMapping.Recorder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KinectCoordinateMapping
{
    /// <summary>
    /// Interaction logic for ReplayPage.xaml
    /// </summary>
    public partial class ReplayPage : Page
    {
        /// <summary>
        ///  Record class to record film using kinect studio tools
        /// </summary>
        Record record;
        /// <summary>
        ///  Playback(Replay) class to record film using kinect studio tools
        /// </summary>
        Playback playback;
        /// <summary>
        ///  Timer for set the progress bar
        /// </summary>
        DispatcherTimer timer = new DispatcherTimer();

        public ReplayPage()
        {
            InitializeComponent();
            record = new Record();
            playback = new Playback();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (playback.isPlaying)
            {
                try
                {
                    TimeSpan duration = playback.GetPlaybackDuration();
                    TimeSpan current = playback.GetCurrent();
                    double percentage = current.TotalMilliseconds / duration.TotalMilliseconds;
                    playbackProgressBar.Value = percentage * 100;
                }
                catch
                {

                }
            }
        }

        private void recordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!record.isRecording)
            {
                record.RecordKinect();
                playbackButton.IsEnabled = false;
                singleStepButton.IsEnabled = false;
                pauseButton.IsEnabled = false;
                recordButton.Content = "停止";
            }
            else
            {
                record.StopRecord();
                playbackButton.IsEnabled = true;
                singleStepButton.IsEnabled = true;
                pauseButton.IsEnabled = true;
                recordButton.Content = "錄影";
            }
        }

        private void singleStepButton_Click(object sender, RoutedEventArgs e)
        {
            playback.OneStep();
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (playback.Pause())
            {
                singleStepButton.IsEnabled = true;
                pauseButton.Content = "繼續";
            }
            else
            {
                playback.Resume();
                singleStepButton.IsEnabled = false;
                pauseButton.Content = "暫停";
            }
        }

        private void playbackButton_Click(object sender, RoutedEventArgs e)
        {
            if (!playback.isPlaying)
            {
                playback = new Playback();
                playback.PlayKinect();
                playbackButton.Content = "停止";
                recordButton.IsEnabled = false;
                pauseButton.IsEnabled = true;
                singleStepButton.IsEnabled = true;
            }
            else
            {
                playback.Stop();
                recordButton.IsEnabled = true;
                pauseButton.IsEnabled = false;
                singleStepButton.IsEnabled = false;
                playbackButton.Content = "播放";
            }
        }

        private void playbackProgressBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //playback.SeekTime(TimeSpan.FromSeconds(20));
            int mouseX = (int)e.GetPosition(playbackProgressBar).X;
            //int mouseY = (int)e.GetPosition(playbackProgressBar).Y;
            //Console.WriteLine(mouseX.ToString() + ", " + mouseY.ToString());

            double width = playbackProgressBar.Width;
            double percentage = (double)mouseX / width;
            TimeSpan timeSpan = playback.GetPlaybackDuration();
            double totalmillisecond = timeSpan.TotalMilliseconds;
            double destmillisecond = totalmillisecond * percentage;
            playback.SeekTime(TimeSpan.FromMilliseconds(destmillisecond));
        }
    }
}
