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
    public class Record
    {
        private KStudioClient client;

        private KStudioRecording recording;

        /// <summary> Indicates if a recording is currently in progress </summary>
        public bool isRecording = false;


        private string lastFile = string.Empty;

        /// <summary> Recording duration </summary>
        private TimeSpan duration = TimeSpan.FromSeconds(10);

        /// <summary> Number of playback iterations </summary>
        private uint loopCount = 0;

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

        
        /// <summary>
        /// Records a new .xef file
        /// </summary>
        /// <param name="filePath">Full path to where the file should be saved to</param>
        private void RecordClip(string filePath)
        {
            client = KStudio.CreateClient();

            client.ConnectToService();

            // Specify which streams should be recorded
            KStudioEventStreamSelectorCollection streamCollection = new KStudioEventStreamSelectorCollection();
            streamCollection.Add(KStudioEventStreamDataTypeIds.Ir);
            streamCollection.Add(KStudioEventStreamDataTypeIds.Depth);
            streamCollection.Add(KStudioEventStreamDataTypeIds.Body);
            streamCollection.Add(KStudioEventStreamDataTypeIds.BodyIndex);
            streamCollection.Add(KStudioEventStreamDataTypeIds.UncompressedColor);
            // Create the recording object
            recording = client.CreateRecording(filePath, streamCollection);

            //recording.StartTimed(this.duration); 
            recording.Start();
            while (recording.State == KStudioRecordingState.Recording)
            {
                Thread.Sleep(500);
            }


            client.DisconnectFromService();


            // Update UI after the background recording task has completed
            this.isRecording = false;
            //this.Dispatcher.BeginInvoke(new NoArgDelegate(UpdateState));
        }

        /// <summary>
        /// Handles the user clicking on the Record button
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        public void RecordKinect()
        {
            string filePath = this.SaveRecordingAs();

            if (!string.IsNullOrEmpty(filePath))
            {
                this.lastFile = filePath;
                this.isRecording = true;
                //this.RecordPlaybackStatusText = Properties.Resources.RecordingInProgressText;
                //this.UpdateState();

                // Start running the recording asynch ronously
                OneArgDelegate recording = new OneArgDelegate(this.RecordClip);
                recording.BeginInvoke(filePath, null, null);
            }
        }

        /// <summary>
        /// Launches the SaveFileDialog window to help user create a new recording file
        /// </summary>
        /// <returns>File path to use when recording a new event file</returns>
        private string SaveRecordingAs()
        {
            string fileName = string.Empty;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "recordAndPlaybackBasics.xef";
            dlg.DefaultExt = Properties.Resources.XefExtension;
            dlg.AddExtension = true;
            dlg.Filter = Properties.Resources.EventFileDescription + " " + Properties.Resources.EventFileFilter;
            dlg.CheckPathExists = true;
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                fileName = dlg.FileName;
            }

            return fileName;
        }

        public bool StopRecord()
        {
            if (isRecording && recording.State == KStudioRecordingState.Recording)
            {
                recording.Stop();
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
