using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Microsoft.Kinect;
using Microsoft.Kinect.Tools;

namespace KinectCoordinateMapping.Recorder
{
    class Playback
    {
        private KStudioPlayback playback;
        private KStudioClient client;

        /// <summary> Indicates if a playback is currently in progress </summary>
        public bool isPlaying = false;

        private string lastFile = string.Empty;

        /// <summary> Recording duration </summary>
        //private TimeSpan duration = TimeSpan.FromSeconds(10);

        /// <summary> Number of playback iterations </summary>
        //private uint loopCount = 0;

        /// <summary> Delegate to use for placing a job with no arguments onto the Dispatcher </summary>
        private delegate void NoArgDelegate();

        /// <summary>
        /// Delegate to use for placing a job with a single string argument onto the Dispatcher
        /// </summary>
        /// <param name="arg">string argument</param>
        private delegate void OneArgDelegate(string arg);

        /// <summary> Current kinect sesnor status text to display </summary>
        private string kinectStatusText = string.Empty;

        /// <summary>
        /// Current record/playback status text to display
        /// </summary>
        private string recordPlayStatusText = string.Empty;

        public Playback()
        {
            
        }
        public void PlayKinect()
        {
            string filePath = this.OpenFileForPlayback();

            if (!string.IsNullOrEmpty(filePath))
            {
                this.lastFile = filePath;
                this.isPlaying = true;
                //this.RecordPlaybackStatusText = Properties.Resources.PlaybackInProgressText;
                //this.UpdateState();

                // Start running the playback asynchronously
                //PlaybackClip(filePath);
                OneArgDelegate playback = new OneArgDelegate(this.PlaybackClip);
                playback.BeginInvoke(filePath, null, null);

            }
        }


        /// <summary>
        /// Launches the OpenFileDialog window to help user find/select an event file for playback
        /// </summary>
        /// <returns>Path to the event file selected by the user</returns>
        private string OpenFileForPlayback()
        {
            string fileName = string.Empty;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = this.lastFile;
            dlg.DefaultExt = Properties.Resources.XefExtension; // Default file extension
            dlg.Filter = Properties.Resources.EventFileDescription + " " + Properties.Resources.EventFileFilter; // Filter files by extension 
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                fileName = dlg.FileName;
            }

            return fileName;
        }

        /// <summary>
        /// Plays back a .xef file to the Kinect sensor
        /// </summary>
        /// <param name="filePath">Full path to the .xef file that should be played back to the sensor</param>
        private void PlaybackClip(string filePath)
        {
            //using (KStudioClient client = KStudio.CreateClient())
            //{
            client = KStudio.CreateClient();
            client.ConnectToService();

            // Create the playback object
            playback = client.CreatePlayback(filePath);

            playback.LoopCount = 1;
            
            //playback.StepOnce();
            playback.Start();

            //while (playback.State == KStudioPlaybackState.Playing)
            //{
            //    Thread.Sleep(500);
            //}
            while (playback.State != KStudioPlaybackState.Stopped)
            {
                //if (playback.State == KStudioPlaybackState.)
                //{
                Thread.Sleep(500);
                //Console.WriteLine(playback.CurrentRelativeTime.ToString());
                //}
            }

            client.DisconnectFromService();
            //}

            // Update the UI after the background playback task has completed
            this.isPlaying = false;
            //this.Dispatcher.BeginInvoke(new NoArgDelegate(UpdateState));

        }

        public bool Pause()
        {
            if (isPlaying && playback.State == KStudioPlaybackState.Playing)
            {
                playback.Pause();
                return true;
            }
            else if (playback.State == KStudioPlaybackState.Paused)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool Stop()
        {
            if (isPlaying)
            {
                playback.Stop();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool OneStep()
        {
            if (isPlaying && playback.State == KStudioPlaybackState.Paused)
            {
                playback.StepOnce();
                return true;
            }               
            else
            {
                return false;
            }
        }

        public bool SeekTime(TimeSpan time)
        {
            if(isPlaying && playback.State == KStudioPlaybackState.Playing)
            {
                Pause();
                playback.SeekByRelativeTime(time);
                Resume();
                return true;
            }
            else if (isPlaying && playback.State == KStudioPlaybackState.Paused)
            {
                playback.SeekByRelativeTime(time);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Resume()
        {
            if (isPlaying && playback.State == KStudioPlaybackState.Paused)
            {
                playback.Resume();
                return true;
            }
            else
            {
                return false;
            }
        }

        public TimeSpan GetPlaybackDuration()
        {
            if (playback != null)
            {
                return playback.Duration;
            }
            return TimeSpan.FromSeconds(0);
        }

        public TimeSpan GetCurrent()
        {
            if (playback != null)
            {
                return playback.CurrentRelativeTime;
            }
            return TimeSpan.FromSeconds(0);
        }
    }
}
