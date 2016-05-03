using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace KinectCoordinateMapping.FrameStore
{
    public class TTSEngine
    {
        SpeechSynthesizer reader;
        String status;

        public TTSEngine()
        {
            reader = new SpeechSynthesizer();
           
        }

        public void SpeakText(String text)
        {
            reader.Dispose();
            if (text != "")
            {

                reader = new SpeechSynthesizer();
                reader.SpeakAsync(text);
                status = "SPEAKING";
                reader.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(reader_SpeakCompleted);
            }
            else
            {
                status = "NOTEXT";
            }
        }

        void reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            status= "IDLE";
        }

        public String Status
        {
            set
            {
                status = value;
            }
            get
            {
                return status;
            }
        }

    }
}
