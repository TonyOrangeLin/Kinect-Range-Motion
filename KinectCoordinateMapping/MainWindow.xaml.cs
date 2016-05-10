using KinectCoordinateMapping.ButtonCommand;
using KinectCoordinateMapping.FrameStore;
using KinectCoordinateMapping.Recorder;
using Microsoft.Kinect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;
using System.Media;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using KinectCoordinateMapping.ProgressSquencer;
using KinectCoordinateMapping.ProfileDocument;
using System.Runtime.Serialization;
using Microsoft.Win32;

namespace KinectCoordinateMapping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public enum mode {None, AngleMode, LenghtMode, PlaneMode, LineBisection, ZoomIn, ZoomOut, Drag};
    public enum MotionMode {None, HipFlexion, KickStraight, HeelRaise, ToeRaise, HipAbduction, ElbowFlexion, ShoulderFlexion, ShoulderAbduction, ShoulderExtension, ExternalRotation, ExternalRotation90, InternalRotation };
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        int cntemplimit = 4;
        private KinectSensor _sensor;
        private MultiSourceFrameReader _reader;
        private List<Target> TargetList = new List<Target>();
        public CameraSpacePoint[] ColorInSkeleton;
        //private const double NumbersOfTarget = 4;
        //private const double AnticipatedUU = 60;
        //private const double AnticipatedVV = 140;
        //private int[] boolPixels;
        private WriteableBitmap colorBitmap;
        private AngleCalulator AngleCal = new AngleCalulator();
        private int frameskipcount = 0;
        private mode measureMode = mode.AngleMode;
        private Body[] bodies;
        private ushort[] depthPixels;
        private byte[] colorPixels;
        private byte[] bodyIndexs;
        /// <summary>
        /// Coordinate mapper to map one type of point to another
        /// </summary>
        private CoordinateMapper coordinateMapper = null;
        private Vector3D floorVector;
        double angle;
        double measuredLength;
        Vector displayVector;
        //private DrawingGroup drawingGroup;
        // Create the drawing group we'll use for drawing
        /// <summary>
        /// Drawing group for body rendering output
        /// </summary>

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        //private DrawingImage imageSource;
        bool isZoomIn = false;
        double ballSize = 5;
        double lineSize = 2;
        int zoomOffsetX = 0;
        int zoomOffestY = 0;
        bool isDotDisplay = true;
        CommandInterface modeCommnad;
        protected int startX;
        protected int startY;
        protected int middleX;
        protected int middleY;
        protected int endX;
        protected int endY;
        public ZoomStruct zoomStruct;
        public DisplayStruct displayStruct;
        public List<Group> groupList = new List<Group>();
        //int transformX;
        //int transformY;
        //int transformX2;
        //int transformY2;
        //Point tempPoint;
        /// <summary>
        /// Current record/playback status text to display
        /// </summary>
        private string recordPlayStatusText = string.Empty;
        private List<FrameStorage> frameList = new List<FrameStorage>();
        private List<Target> manualMarkList = new List<Target>();//追蹤的點
        private List<Target> manualPingList = new List<Target>();//追蹤後固定的點

        /// <summary>
        /// Store floor vector every frame in order to save in FrameStorge
        /// </summary>
        Vector4 floorVector4D;

       
        BitmapSource originBitampSource;
        //private bool[] bodyIndexPixels = new bool[1920 * 1080];
        /// <summary>
        /// 存snapshot的地方
        /// </summary>
        FrameStorage replayFrame;
        /// <summary>
        /// 判斷是否在重播，用來決定frameready要播kinect給的影像檔或是選擇的snapshot
        /// </summary>
        bool isSnapShot;
        private string kinectStatusText = string.Empty;
        /// <summary>
        /// Description of the data contained in the body index frame
        /// </summary>
        private FrameDescription bodyIndexFrameDescription = null;
        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 計時器
        /// </summary>
        DispatcherTimer timer = new DispatcherTimer();
        /// <summary>
        /// 配合Utilities/ColorExtensions.cs如果是true就會new新的記憶體，避免shallow copy
        /// </summary>
        bool isReadToSnapShot = false;
        /// <summary>
        /// 提示的類別，目前有tts、人體模型與顏色提示
        /// </summary>
        //private HintProcess hintProcess = new HintProcess(true, true);
        /// <summary>
        /// 避免色球追蹤Ping後 會太快再Ping下一次，所以要冷卻時間，配合Timer
        /// </summary>
        private int snapShotCoolDown = 0;
        /// <summary>
        /// 判斷重播模式按鈕是否按下去，按下去就開啟重播模式(snapshot)
        /// </summary>
        bool isReplayButtonClick = false;
        /// <summary>
        /// 淺綠色色球的預定YUV值得U
        /// </summary>
        int default1UU = 120;//LIGHT GREEN
        /// <summary>
        /// 淺綠色色球的預定YUV值得V
        /// </summary>
        int default1VV = 92;//LIGHT GREEN
        /// <summary>
        /// 黃色色球的預定YUV值得U
        /// </summary>
        int default2UU = 78;//YELLOW
        /// <summary>
        /// 黃色色球的預定YUV值得V
        /// </summary>
        int default2VV = 125;//YELLOW
        /// <summary>
        /// 掌控流程的class
        /// </summary>
        ProgressSeq progressSeq;
        /// <summary>
        /// 一號色球是否在設定
        /// </summary>
        bool isOneColorSetting = false;
        /// <summary>
        /// 二號色球是否在設定
        /// </summary>
        bool isTwoColorSetting = false;
        /// <summary>
        /// 多張色球融合
        /// </summary>
        FrameStorageFusion frameStorageFusion;
        /// <summary>
        /// 紀錄算出的指標數據參考學長
        /// </summary>
        private List<Double> Data = new List<Double>();
        /// <summary>
        /// 多張色球融合的參考點(存脖子位置)三圍
        /// </summary>
        private CameraSpacePoint neckPoint;
        /// <summary>
        /// 多張色球融合的參考點(存脖子位置)二維
        /// </summary>
        private ColorSpacePoint neckPointColor;
        int startFrameIndex = 0;
        FrameStorage frontFrameStorage;
        FrameStorage backFrameStorage;
        FrameStorage sideFrameStorage;
        private List<HintElement> hintElementList;
        int drawCoolDown = 0;
        bool isTrackBody;
        bool AnteriorChecked = true;
        bool LeftLateralChecked = true;
        bool PosteriorChecked = true;
        float humanZdepth;
        Body patientBody;
        int hintCoolDown = 0;
        int readyCoolDown = 0;
        private const double AnticipatedUU = 60;
        private const double AnticipatedVV = 140;
        int cntemp;
        int CatchSuccess = 0;
        Body patientFrontBody;
        bool takePatienBodyDataFlag = false;
        MotionMode motionMode = MotionMode.None;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (snapShotCoolDown > 0)
            {
                snapShotCoolDown--;
                //if (snapShotCoolDown <= 0)
                //{
                //    snapShotCoolDown = 0;
                //}
            }
            if (hintCoolDown > 0)
            {
                hintCoolDown--;
                if (hintCoolDown <= 0)
                {
                    hintCoolDown = 0;
                    //ProcessHandler();
                    hintCoolDown = 10;
                    //10秒提醒一次
                }
            }
            if (readyCoolDown > 0)
            {
                if (patientBody != null)
                {
                    readyCoolDown--;
                    ReadyCoolDownSnapshot(readyCoolDown);
                }
            }
        }

        private void ReadyCoolDownSnapshot(int count)
        {
            //if (progressSeq.NowStep == Step.FrontReady  || progressSeq.NowStep == Step.BackReady || progressSeq.NowStep == Step.SideReady)
            //{
            //    startProcessButton.Content = count.ToString();
            //    if (count <= 3)
            //    {
            //        hintProcess.TTSHint(count.ToString());
            //    }
            //    if (count <= 0)
            //    {
            //        TakeSnapShot();
            //        count = 0;
            //        takePatienBodyDataFlag = true;
            //        progressSeq.Start();
            //        ProcessHandler();


            //    }
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();
            _sensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;
            groupList = new List<Group>();
            zoomStruct = new ZoomStruct();
            displayStruct = new DisplayStruct();
            modeCommnad = new ZoomInCommand(this);

            //record = new Record();
            //playback = new Playback();
            Target target;
            target = new Target(0);
            //target.Setting(0, 0, 100, 168);//orange
            target.Setting(0, 0, default1UU, default1VV);//light green
            manualMarkList.Add(target);
            target = new Target(1);
            target.Setting(0, 0, default2UU, default2VV);//yellow
            manualMarkList.Add(target);

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            if (_sensor != null)
            {
                _sensor.Open();
                this.coordinateMapper = this._sensor.CoordinateMapper;
                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body | FrameSourceTypes.BodyIndex);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
                this.colorBitmap = new WriteableBitmap(this._sensor.ColorFrameSource.FrameDescription.Width, this._sensor.ColorFrameSource.FrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);
                //this.boolPixels = new int[this._sensor.DepthFrameSource.FrameDescription.LengthInPixels];
                measureMode = mode.None;
                isDotDisplay = false;
                displayStruct.isDotDisplay = isDotDisplay;
                lineSizeslider.Value = 2;
                ballSizeSlider.Value = 10;
                displayStruct.linesize = lineSizeslider.Value;
                displayStruct.ballsize = ballSizeSlider.Value;
                dotDisplayUpdate();
                UpdateModeButton();

               
            }
            this.boolPixels = new int[1920 * 1080];
            this.KinectStatusText = this._sensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            progressSeq = new ProgressSeq();
            frameStorageFusion = new FrameStorageFusion();

            for (int d = 0; d <= 93; d++)   //指標特徵運算A1~A66
            {
                //List<double> array = new List<double>() { };
                //DataList.Add(array);
                Data.Add(0);
            }

            hintElementList = new List<HintElement>();
            hintElementList.Add(new HintNone());
            hintElementList.Add(new HintFrontReady());
            hintElementList.Add(new HintFrontHead());
            hintElementList.Add(new HintFrontShoulder());
            hintElementList.Add(new HintIliac());
            hintElementList.Add(new HintTrochanter());
            hintElementList.Add(new HintTuberosity());
            hintElementList.Add(new HintPatella());
            hintElementList.Add(new HintKneeJointLine());
            hintElementList.Add(new HintLateralmMlleoli());
            hintElementList.Add(new HintMedialMalleoli());

            hintElementList.Add(new HintBackReady());
            hintElementList.Add(new HintScapulaUp());
            hintElementList.Add(new HintScapula());
            hintElementList.Add(new HintMidpointLeg());
            hintElementList.Add(new HintIntermalleolarLine());
            hintElementList.Add(new HintTendon());

            hintElementList.Add(new HintSideReady());
            hintElementList.Add(new HintTragusAndSeventhCervicalVertebra());
            hintElementList.Add(new HintAcromionAndElbow());
            hintElementList.Add(new HintAnteriorSuperiorIliacSpineAndPosteriorSuperiorIliacSpine());
            hintElementList.Add(new HintTrochanterAndKneeJoint());
            hintElementList.Add(new HintMalleolusAndMetatarsus());

            hintElementList.Add(new HintDone());
            int NumbersOfTarget = 10;
            for (int t = 0; t <= NumbersOfTarget; t++)
            {
                Target target2 = new Target(t);
                TargetList.Add(target2);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // set the status text
            this.KinectStatusText = this._sensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.SensorNotAvailableStatusText;
        }

        public string KinectStatusText
        {
            get
            {
                return this.kinectStatusText;
            }

            set
            {
                if (this.kinectStatusText != value)
                {
                    this.kinectStatusText = value;
                    kinectStatusBarItem.Content = value;
                    // notify any bound elements that the text has changed
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("KinectStatusText"));
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the current status text to display for the record/playback features
        /// </summary>
        public string RecordPlaybackStatusText
        {
            get
            {
                return this.recordPlayStatusText;
            }

            set
            {
                if (this.recordPlayStatusText != value)
                {
                    this.recordPlayStatusText = value;

                    // notify any bound elements that the text has changed
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("RecordPlaybackStatusText"));
                    }
                }
            }
        }

        private CameraSpacePoint[] ColorToSkeleton(ushort[] input)
        {
            CameraSpacePoint[] ColorInSkel = new CameraSpacePoint[1920 * 1080];
            coordinateMapper.MapColorFrameToCameraSpace(input, ColorInSkel);

            return ColorInSkel;
        }

        private bool CompareTwoPoint(Point a, Point b)
        {
            if (a == null || b == null)
            {
                return false;
            }

            if (a.X == b.X && a.Y == b.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //private void HumanBodyHintWarning()
        //{
        //    SolidColorBrush greenBrush = new SolidColorBrush(Color.FromRgb(90, 255, 105));
        //    SolidColorBrush redBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        //    SolidColorBrush yellowBrush = new SolidColorBrush(Color.FromRgb(255, 255, 90));

        //    if (progressSeq.NowStep > Step.FrontReady && progressSeq.NowStep < Step.BackReady)
        //    {
        //        if (progressSeq.NowStep == Step.FrontHead)
        //        {
        //            hintProcess.humanBodyWindow.button2_front.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button3_front.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.FrontShoulder)
        //        {
        //            hintProcess.humanBodyWindow.button5_front.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button6_front.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.Iliac)
        //        {
        //            hintProcess.humanBodyWindow.button12_front.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button13_front.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.Trochanter)
        //        {
        //            hintProcess.humanBodyWindow.button14_front.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button15_front.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.Tuberosity)
        //        {
        //            hintProcess.humanBodyWindow.button18_front.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button21_front.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.Patella)
        //        {
        //            hintProcess.humanBodyWindow.button17_front.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button20_front.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.KneeJointLine)
        //        {
        //            hintProcess.humanBodyWindow.button16_front.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button19_front.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.LateralmMlleoli)
        //        {
        //            hintProcess.humanBodyWindow.button22_front.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button25_front.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.MedialMalleoli)
        //        {
        //            hintProcess.humanBodyWindow.button23_front.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button26_front.Background = yellowBrush;
        //        }
        //    }
        //    else if (progressSeq.NowStep > Step.BackReady && progressSeq.NowStep < Step.SideReady)
        //    {
        //        if (progressSeq.NowStep == Step.ScapulaUp)
        //        {
        //            hintProcess.humanBodyWindow.button17_back.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.Scapula)
        //        {
        //            hintProcess.humanBodyWindow.button8_back.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button7_back.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.MidpointLeg)
        //        {
        //            hintProcess.humanBodyWindow.button32_back.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button33_back.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.IntermalleolarLine)
        //        {
        //            hintProcess.humanBodyWindow.button35_back.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button39_back.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.Tendon)
        //        {
        //            hintProcess.humanBodyWindow.button41_back.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button37_back.Background = yellowBrush;
        //        }
        //    }
        //    else if (progressSeq.NowStep > Step.SideReady && progressSeq.NowStep < Step.Done)
        //    {
        //        if (progressSeq.NowStep == Step.TragusAndSeventhCervicalVertebra)
        //        {
        //            hintProcess.humanBodyWindow.button2_side.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button8_side.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.AcromionAndElbow)
        //        {
        //            hintProcess.humanBodyWindow.button5_side.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.AnteriorSuperiorIliacSpineAndPosteriorSuperiorIliacSpine)
        //        {
        //            hintProcess.humanBodyWindow.button21_side.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button22_side.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.TrochanterAndKneeJoint)
        //        {
        //            hintProcess.humanBodyWindow.button23_side.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button24_side.Background = yellowBrush;
        //        }
        //        if (progressSeq.NowStep == Step.MalleolusAndMetatarsus)
        //        {
        //            hintProcess.humanBodyWindow.button30_side.Background = yellowBrush;
        //            hintProcess.humanBodyWindow.button31_side.Background = yellowBrush;
        //        }
        //    }

        //}
        //private void HumanBodyHint(List<Target> list)
        //{
        //    SolidColorBrush greenBrush = new SolidColorBrush(Color.FromRgb(90, 255, 105));
        //    SolidColorBrush redBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        //    SolidColorBrush yellowBrush = new SolidColorBrush(Color.FromRgb(255, 255, 90));
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        if (progressSeq.NowStep > Step.FrontReady && progressSeq.NowStep < Step.BackReady)
        //        {
        //            if (list[i].TargetID == 2)
        //            {
        //                hintProcess.humanBodyWindow.button2_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 3)
        //            {
        //                hintProcess.humanBodyWindow.button3_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 5)
        //            {
        //                hintProcess.humanBodyWindow.button5_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 6)
        //            {
        //                hintProcess.humanBodyWindow.button6_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 12)
        //            {
        //                hintProcess.humanBodyWindow.button12_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 13)
        //            {
        //                hintProcess.humanBodyWindow.button13_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 14)
        //            {
        //                hintProcess.humanBodyWindow.button14_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 15)
        //            {
        //                hintProcess.humanBodyWindow.button15_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 17)
        //            {
        //                hintProcess.humanBodyWindow.button17_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 20)
        //            {
        //                hintProcess.humanBodyWindow.button20_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 16)
        //            {
        //                hintProcess.humanBodyWindow.button16_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 19)
        //            {
        //                hintProcess.humanBodyWindow.button19_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 18)
        //            {
        //                hintProcess.humanBodyWindow.button18_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 21)
        //            {
        //                hintProcess.humanBodyWindow.button21_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 23)
        //            {
        //                hintProcess.humanBodyWindow.button23_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 26)
        //            {
        //                hintProcess.humanBodyWindow.button26_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 22)
        //            {
        //                hintProcess.humanBodyWindow.button22_front.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 25)
        //            {
        //                hintProcess.humanBodyWindow.button25_front.Background = greenBrush;
        //            }
        //        }
        //        else if (progressSeq.NowStep > Step.BackReady && progressSeq.NowStep < Step.SideReady)
        //        {
        //            if (list[i].TargetID == 8)
        //            {
        //                hintProcess.humanBodyWindow.button8_back.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 7)
        //            {
        //                hintProcess.humanBodyWindow.button7_back.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 17)
        //            {
        //                hintProcess.humanBodyWindow.button17_back.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 32)
        //            {
        //                hintProcess.humanBodyWindow.button32_back.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 33)
        //            {
        //                hintProcess.humanBodyWindow.button33_back.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 35)
        //            {
        //                hintProcess.humanBodyWindow.button35_back.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 39)
        //            {
        //                hintProcess.humanBodyWindow.button39_back.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 41)
        //            {
        //                hintProcess.humanBodyWindow.button41_back.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 37)
        //            {
        //                hintProcess.humanBodyWindow.button37_back.Background = greenBrush;
        //            }
        //        }
        //        else if (progressSeq.NowStep > Step.SideReady && progressSeq.NowStep < Step.Done)
        //        {
        //            if (list[i].TargetID == 2)
        //            {
        //                hintProcess.humanBodyWindow.button2_side.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 5)
        //            {
        //                hintProcess.humanBodyWindow.button5_side.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 8)
        //            {
        //                hintProcess.humanBodyWindow.button8_side.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 21)
        //            {
        //                hintProcess.humanBodyWindow.button21_side.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 22)
        //            {
        //                hintProcess.humanBodyWindow.button22_side.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 23)
        //            {
        //                hintProcess.humanBodyWindow.button23_side.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 24)
        //            {
        //                hintProcess.humanBodyWindow.button24_side.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 30)
        //            {
        //                hintProcess.humanBodyWindow.button30_side.Background = greenBrush;
        //            }
        //            if (list[i].TargetID == 31)
        //            {
        //                hintProcess.humanBodyWindow.button31_side.Background = greenBrush;
        //            }
        //        }
        //    }
        //}

        //private void TrackColorBall()
        //{
        //    if (snapShotCoolDown <= 0)
        //    {
        //        bool isDuplicate = false;
        //        if (progressSeq.NowStep == Step.FrontReady || progressSeq.NowStep == Step.BackReady || progressSeq.NowStep == Step.None || progressSeq.NowStep == Step.Done || progressSeq.NowStep == Step.SideReady)
        //        {

        //        }
        //        else
        //        {
        //            for (int i = 0; i < manualMarkList.Count; i++)
        //            {
        //                if (isTrackBody || true)
        //                {
        //                    manualMarkList[i].Cal(colorPixels, ColorInSkeleton, humanZdepth);
        //                    if (manualMarkList[i].isPing)
        //                    {
        //                        manualMarkList[i].isPing = false;
        //                        if (manualPingList.Count == 0)
        //                        {
        //                            Target target = new Target(manualMarkList[i].TargetID);
        //                            target.Setting((int)manualMarkList[i].point2D().X, (int)manualMarkList[i].point2D().Y);
        //                            target.setPoint3D(manualMarkList[i].point3D());
        //                            manualPingList.Add(target);
        //                        }
        //                        else
        //                        {
        //                            for (int j = 0; j < manualPingList.Count; j++)
        //                            {
        //                                if (CompareTwoPoint(manualPingList[j].point2D(), manualMarkList[i].point2D()))
        //                                {
        //                                    //同一點不記錄
        //                                    isDuplicate = true;
        //                                    break;
        //                                }
        //                            }
        //                            if (!isDuplicate)
        //                            {
        //                                if (manualPingList[0].TargetID == manualMarkList[i].TargetID)
        //                                {
        //                                    //避免兩個點在同一個地方
        //                                }
        //                                else
        //                                {
        //                                    Target target = new Target(manualMarkList[i].TargetID);
        //                                    target.Setting((int)manualMarkList[i].point2D().X, (int)manualMarkList[i].point2D().Y);
        //                                    target.setPoint3D(manualMarkList[i].point3D());
        //                                    manualPingList.Add(target);
        //                                    int count1 = 0;
        //                                    int count2 = 0;
        //                                    for (int k = 0; k < manualPingList.Count; k++)
        //                                    {
        //                                        if (manualPingList[k].TargetID == 0)
        //                                        {
        //                                            count1++;
        //                                        }
        //                                        if (manualPingList[k].TargetID == 1)
        //                                        {
        //                                            count2++;
        //                                        }
        //                                    }
        //                                    if (count1 == count2)
        //                                    {

        //                                        if (true || hintElementList[(int)progressSeq.NowStep].CheckPointIfRoughlyCorrect(manualPingList[0], manualPingList[1], bodies, coordinateMapper)) //先不做驗證
        //                                        {
        //                                            hintElementList[(int)progressSeq.NowStep].TagTargetID(manualPingList);
        //                                            HumanBodyHint(manualPingList);
        //                                            TakeSnapShot();
        //                                            snapShotCoolDown = 3;
        //                                            hintProcess.HintGreen();
        //                                            manualPingList = new List<Target>();
          
        //                                            progressSeq.Start();
        //                                            manualMarkList[0].CleanExpectColorPoint();
        //                                            manualMarkList[1].CleanExpectColorPoint();
        //                                            for (int z = 0; z < hintElementList[(int)progressSeq.NowStep].JointTypeList.Length; z++)
        //                                            {
        //                                                Joint tempJoint = patientBody.Joints[hintElementList[(int)progressSeq.NowStep].JointTypeList[z]];
        //                                                ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
        //                                                colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
        //                                                colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;

        //                                                manualMarkList[0].SetExpectColorPoint(colorPoint.X, colorPoint.Y);
        //                                                manualMarkList[1].SetExpectColorPoint(colorPoint.X, colorPoint.Y);
        //                                            }
                                                    


        //                                            ProcessHandler();
        //                                        }
        //                                        else
        //                                        {
        //                                            snapShotCoolDown = 3;
        //                                            hintProcess.HintRed();
        //                                            hintProcess.TTSHint("失敗");
        //                                            manualPingList = new List<Target>();
        //                                            ProcessHandler();
        //                                        }


        //                                    }

        //                                }
        //                            }
        //                        }

        //                    }
        //                }

        //            }
        //        }
        //    }
        //}

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            MultiSourceFrame reference = e.FrameReference.AcquireFrame();

            if (!isSnapShot)
            {
                using (var frame = reference.ColorFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        FrameDescription colorDesc = frame.FrameDescription;
                        if (colorPixels == null)
                        {
                            uint colorSize = colorDesc.LengthInPixels;
                            colorPixels = new byte[colorSize * 4];
                        }

                        frame.CopyConvertedFrameDataToArray(colorPixels, ColorImageFormat.Bgra);

                        //TrackColorBall();

                        
                        if (zoomStruct.IsZoom)
                        {
                            originBitampSource = frame.ToBitmap(isReadToSnapShot);

                            int cropX = (int)((double)1920 / zoomStruct.ZoomRatio);
                            int cropY = (int)((double)1080 / zoomStruct.ZoomRatio);
                            CroppedBitmap leftImageCropped = new CroppedBitmap(originBitampSource, new Int32Rect(zoomStruct.ZoomOffsetX, zoomStruct.ZoomOffsetY, cropX, cropY));
                            camera.Source = leftImageCropped;
                            camera.Stretch = Stretch.Uniform;
                        }
                        else
                        {
                            originBitampSource = frame.ToBitmap(isReadToSnapShot);
                            camera.Source = frame.ToBitmap();
                            camera.Stretch = Stretch.Uniform;
                        }
                        isReadToSnapShot = false;
                    }
                }


                // Depth
                using (var frame = reference.DepthFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {

                        var depthDesc = frame.FrameDescription;
                        if (depthPixels == null)
                        {
                            uint depthSize = depthDesc.LengthInPixels;
                            depthPixels = new ushort[depthSize];
                        }

                        frame.CopyFrameDataToArray(depthPixels);
                        if (frameskipcount > 4)
                        {
                            ColorInSkeleton = ColorToSkeleton(depthPixels);  //ColorPoint to SkeletonPoint  必須每個frame都作!!

                            for (int i = 0; i < TargetList.Count; i++)
                            {
                                TargetList[i].RefreshTarget(ColorInSkeleton, isZoomIn, zoomOffsetX, zoomOffestY, zoomStruct);
                            }
                            //frameStorage.ColorInSkeleton = ColorInSkeleton;
                            //for (int i = 0; i < modeCommnad.TargetList.Count; i++)
                            //{
                            //    modeCommnad.TargetList[i].RefreshTarget(ColorInSkeleton, isZoomIn, zoomOffsetX, zoomOffestY, zoomStruct);
                            //}
                            //for (int i = 0; i < manualMarkList.Count; i++)
                            //{
                            //    manualMarkList[i].RefreshTarget(ColorInSkeleton, isZoomIn, zoomOffsetX, zoomOffestY, zoomStruct);
                            //}
                            //for (int i = 0; i < groupList.Count; i++)
                            //{
                            //    for (int j = 0; j < groupList[i].groupList.Count; j++)
                            //    {
                            //        groupList[i].groupList[j].RefreshTarget(ColorInSkeleton, isZoomIn, zoomOffsetX, zoomOffestY, zoomStruct);
                            //    }
                            //    groupList[i].Calculate();
                            //}
                            frameskipcount = 0;
                        }
                        frameskipcount++;
                    }

                }


                // Body
                using (var frame = reference.BodyFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        if (this.bodies == null)
                        {
                            this.bodies = new Body[frame.BodyCount];
                        }
                        bodies = new Body[frame.BodyFrameSource.BodyCount];
          
                        Vector4 vector4d = frame.FloorClipPlane;
                    
                        floorVector4D = vector4d;
                        floorVector.X = vector4d.X;
                        floorVector.Y = vector4d.Y;
                        floorVector.Z = vector4d.Z;

                      
                        displayVector = new Vector(vector4d.X, vector4d.Y);


                        frame.GetAndRefreshBodyData(bodies);

                        if (bodies[0].Joints[JointType.Neck].TrackingState == TrackingState.Tracked)
                        {
                            isTrackBody = true;
                        }
                        foreach (Body body in this.bodies)
                        {
                            if (body.IsTracked)
                            {

                                Joint tempJoint = body.Joints[JointType.Neck];
                                if (tempJoint.TrackingState == TrackingState.Tracked)
                                {
                                    neckPoint = tempJoint.Position;
                                    humanZdepth = neckPoint.Z;
                                    ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                                    colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                                    colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                                    neckPointColor = colorPoint;
                                }
                                else
                                {
                                    neckPoint = new CameraSpacePoint();
                                    neckPointColor = new ColorSpacePoint();
                                }

                                tempJoint = body.Joints[JointType.AnkleLeft];
                                Joint tempJoint2 = body.Joints[JointType.AnkleRight];
                                if (tempJoint.TrackingState == TrackingState.Tracked && tempJoint2.TrackingState == TrackingState.Tracked)
                                {
                                    //紀錄兩點鐘垂腺的地方
                                }
                                //if (takePatienBodyDataFlag)
                                //{
                                //    patientFrontBody = patientBody;
                                //    takePatienBodyDataFlag = false;
                                //}
                                //if (patientBody == null)
                                //{
                                    patientBody = body;
                                //}
                                //if (patientBody != null)
                                //{
                                //    //hintProcess.TTSHint("偵測到人體");
                                //}
                                break;
                            }
                        }

                    }
                }
                using (BodyIndexFrame frame = reference.BodyIndexFrameReference.AcquireFrame())
                {
                    if (frame != null)
                    {
                        var bodyIndexDesc = frame.FrameDescription;
                        if (bodyIndexs == null)
                        {
                            uint bodyIndexSize = bodyIndexDesc.LengthInPixels;
                            bodyIndexs = new byte[bodyIndexSize];
                        }
                        frame.CopyFrameDataToArray(bodyIndexs);
                    }
                }

                for (int i = 0; i < TargetList.Count; i++)
                {
                    TargetList[i].Cal(colorPixels, ColorInSkeleton, boolPixels);
                }
            }
            else
            {
                if (zoomStruct.IsZoom)
                {
                    originBitampSource = replayFrame.bitmapSource;

                    int cropX = (int)((double)1920 / zoomStruct.ZoomRatio);
                    int cropY = (int)((double)1080 / zoomStruct.ZoomRatio);
                    CroppedBitmap leftImageCropped = new CroppedBitmap(originBitampSource, new Int32Rect(zoomStruct.ZoomOffsetX, zoomStruct.ZoomOffsetY, cropX, cropY));
                    camera.Source = leftImageCropped;
                    camera.Stretch = Stretch.Uniform;
                }
                else
                {
                    originBitampSource = replayFrame.bitmapSource;
                    camera.Source = replayFrame.bitmapSource;
                    camera.Stretch = Stretch.Uniform;
                }
            }
            Calculate();

            

            
            //if (drawCoolDown >= 3)
            //{
                Draw();

            
            //    drawCoolDown = 0;
            //}
            //else
            //{
            //    drawCoolDown++;
            //}
        }

        private bool isInfinity(CameraSpacePoint input)
        {
            if (float.IsInfinity(input.X) || float.IsInfinity(input.Y) || float.IsInfinity(input.Z))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Calculate()
        {
            //if (measureMode == mode.AngleMode && modeCommnad.TargetList.Count == 3)
            //{
            //    angle = AngleCal.AngleBetween(modeCommnad.TargetList[0], modeCommnad.TargetList[1], modeCommnad.TargetList[2]);
            //}
            //else if ((measureMode == mode.LenghtMode || measureMode == mode.LineBisection) && modeCommnad.TargetList.Count == 2)
            //{
            //    measuredLength = AngleCal.Length(modeCommnad.TargetList[0], modeCommnad.TargetList[1]);
            //}
        }

        public void RightMouseUp(int x, int y, int index)
        {
            //groupList.RemoveAt(index);
        }
        private int[] boolPixels;
        private void DrawHistoryGroup()
        {
            if (!isSnapShot)
            {
                for (int i = 0; i < groupList.Count; i++)
                {
                    groupList[i].Draw(canvas, displayStruct, zoomStruct, checkBoxShowXYZ.IsChecked == true, floorVector);
                }
                for (int i = 0; i < TargetList.Count; i++)
                {
                    TargetList[i].Draw(canvas, displayStruct, zoomStruct, false);
                }
            }            
            else
            {
                List<Group> replayGroupList = replayFrame.groupList;
                for (int k = 0; k < replayGroupList.Count; k++)
                {
                    replayGroupList[k].Draw(canvas, displayStruct, zoomStruct, checkBoxShowXYZ.IsChecked == true, floorVector);
                }
            }
        }

        private void DrawCurrent()
        {
            //modeCommnad.Draw(canvas, displayStruct);
        }

        private void DrawManualMark()
        {
            if (!isSnapShot)
            {
                for (int j = 0; j < manualMarkList.Count; j++)
                {
                    manualMarkList[j].Draw(canvas, displayStruct, zoomStruct, false);
                }
                for (int k = 0; k < manualPingList.Count; k++)
                {
                    manualPingList[k].Draw(canvas, displayStruct, zoomStruct, false);
                }
            }
        }

        private void Draw()
        {
            canvas.Children.Clear();
            DrawHistoryGroup();
            DrawCurrent();
            DrawManualMark();
            DrawRangeMotion();
            //DrawOther();
        }



        public void ClearTargetList()
        {
            //modeCommnad.TargetList = new List<Target>();
            Update();
        }

        public void ClearHistoryGroup()
        {
            ClearTargetList();
            groupList.Clear();
            Update();
        }

        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            ClearHistoryGroup();
            Update();
        }

        private void clearNowButton_Click(object sender, RoutedEventArgs e)
        {
            ClearTargetList();
            Update();
        }

        private void ballSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ballSize = ballSizeSlider.Value;
            displayStruct.ballsize = ballSize;
            Update();
        }

        private void lineSizeslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lineSize = lineSizeslider.Value;
            displayStruct.linesize = lineSize;

            Update();
        }

        #region ButtonRegion

        private void angleButton_Click(object sender, RoutedEventArgs e)
        {
            //modeCommnad = new AngleCommand(this);
            //measureMode = mode.AngleMode;
            //UpdateModeButton();
        }

        private void lengthButton_Click(object sender, RoutedEventArgs e)
        {
            //modeCommnad = new LineCommand(this);
            //measureMode = mode.LenghtMode;
            //UpdateModeButton();
        }

        private void bisectionButton_Click(object sender, RoutedEventArgs e)
        {
            //modeCommnad = new BisectCommand(this);
            //measureMode = mode.LineBisection;
            //UpdateModeButton();
        }

        public void UpdateModeButton()
        {
            ClearTargetList();
            angleButton.Background = Brushes.White;
            angleButton.Foreground = Brushes.Black;
            lengthButton.Background = Brushes.White;
            lengthButton.Foreground = Brushes.Black;
            bisectionButton.Background = Brushes.White;
            bisectionButton.Foreground = Brushes.Black;
            zoomInButton.Background = Brushes.White;
            zoomInButton.Foreground = Brushes.Black;
            zoomOutButton.Background = Brushes.White;
            zoomOutButton.Foreground = Brushes.Black;
            dragButton.Background = Brushes.White;
            dragButton.Foreground = Brushes.Black;
            if (measureMode == mode.AngleMode)
            {
                angleButton.Background = Brushes.Black;
                angleButton.Foreground = Brushes.White;
            }
            else if (measureMode == mode.LenghtMode)
            {
                lengthButton.Background = Brushes.Black;
                lengthButton.Foreground = Brushes.White;
            }
            else if (measureMode == mode.LineBisection)
            {
                bisectionButton.Background = Brushes.Black;
                bisectionButton.Foreground = Brushes.White;
            }
            else if (measureMode == mode.ZoomIn)
            {
                zoomInButton.Background = Brushes.Black;
                zoomInButton.Foreground = Brushes.White;
            }
            else if (measureMode == mode.ZoomOut)
            {
                zoomOutButton.Background = Brushes.Black;
                zoomOutButton.Foreground = Brushes.White;
            }
            else if (measureMode == mode.Drag)
            {
                dragButton.Background = Brushes.Black;
                dragButton.Foreground = Brushes.White;
            }
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            if (groupList.Count != 0)
            {
                groupList.RemoveAt(groupList.Count - 1);
            }
        }

        private void zoomInButton_Click(object sender, RoutedEventArgs e)
        {
            measureMode = mode.ZoomIn;
            modeCommnad = new ZoomInCommand(this);
            UpdateModeButton();
        }

        private void zoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            measureMode = mode.ZoomOut;
            modeCommnad = new ZoomOutCommand(this);
            UpdateModeButton();
        }

        private void camera_MouseEnter(object sender, MouseEventArgs e)
        {
            if (measureMode == mode.AngleMode)
            {
                Cursor = Cursors.Cross;
            }
            else if (measureMode == mode.LenghtMode)
            {
                Cursor = Cursors.Cross;
            }
            else if (measureMode == mode.LineBisection)
            {
                Cursor = Cursors.Cross;
            }
            else if (measureMode == mode.ZoomIn)
            {
                Cursor = Cursors.ScrollAll;
            }
            else if (measureMode == mode.ZoomOut)
            {
                Cursor = Cursors.ScrollAll;
            }

        }

        private void camera_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void ballDisplayButton_Click(object sender, RoutedEventArgs e)
        {
            isDotDisplay = false;
            displayStruct.isDotDisplay = false;
            dotDisplayUpdate();
        }

        private void dotDisplayButton_Click(object sender, RoutedEventArgs e)
        {
            isDotDisplay = true;
            displayStruct.isDotDisplay = true;
            dotDisplayUpdate();
        }

        private void dotDisplayUpdate()
        {
            if (isDotDisplay)
            {
                ballSizeSlider.IsEnabled = false;
                dotDisplayButton.Background = Brushes.Black;
                dotDisplayButton.Foreground = Brushes.White;
                ballDisplayButton.Background = Brushes.White;
                ballDisplayButton.Foreground = Brushes.Black;
            }
            else
            {
                ballSizeSlider.IsEnabled = true;
                dotDisplayButton.Background = Brushes.White;
                dotDisplayButton.Foreground = Brushes.Black;
                ballDisplayButton.Background = Brushes.Black;
                ballDisplayButton.Foreground = Brushes.White;
            }

        }

        private void dragButton_Click(object sender, RoutedEventArgs e)
        {
            modeCommnad = new DragCommand(this);
            measureMode = mode.Drag;
            UpdateModeButton();
        }
        #endregion

        
        private void DrawOther()
        {

            if (patientBody != null )
            {
                if (patientBody.IsTracked)
                {
                    foreach (Joint joint in patientBody.Joints.Values)
                    {
                        if (joint.TrackingState == TrackingState.Tracked)
                        {
                            // 3D space point
                            CameraSpacePoint jointPosition = joint.Position;

                            // 2D space point
                            Point point = new Point();

                            ColorSpacePoint colorPoint = _sensor.CoordinateMapper.MapCameraPointToColorSpace(jointPosition);

                            point.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            point.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;

                            point = CoordinateTransform.ReverseFromFullScreenToScreen((int)point.X, (int)point.Y, zoomStruct);
                            // Draw
                            Ellipse ellipse = new Ellipse
                            {
                                Fill = Brushes.Green,
                                Width = 6,
                                Height = 6
                            };

                            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
                            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

                            canvas.Children.Add(ellipse);
                        }
                    }

                    //if (hintElementList[(int)progressSeq.NowStep].JointTypeList != null)
                    //{
                    //    for (int i = 0; i < hintElementList[(int)progressSeq.NowStep].JointTypeList.Length; i++)
                    //    {
                    //        CameraSpacePoint jointPosition = patientBody.Joints[hintElementList[(int)progressSeq.NowStep].JointTypeList[i]].Position;

                    //        // 2D space point
                    //        Point point = new Point();

                    //        ColorSpacePoint colorPoint = _sensor.CoordinateMapper.MapCameraPointToColorSpace(jointPosition);

                    //        point.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                    //        point.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;

                    //        point = CoordinateTransform.ReverseFromFullScreenToScreen((int)point.X, (int)point.Y, zoomStruct);
                    //        // Draw
                    //        Ellipse ellipse = new Ellipse
                    //        {
                    //            Fill = Brushes.Brown,
                    //            Width = 30,
                    //            Height = 30
                    //        };

                    //        Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
                    //        Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

                    //        canvas.Children.Add(ellipse);
                    //    }
                    //}
                }
            }
        }

        private void camera_MouseMove(object sender, MouseEventArgs e)
        {
            int mouseX = (int)e.GetPosition(canvas).X;
            int mouseY = (int)e.GetPosition(canvas).Y;
            modeCommnad.MouseMove(mouseX, mouseY);
            Update();
        }

        private void camera_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            int mouseX = (int)e.GetPosition(canvas).X;
            int mouseY = (int)e.GetPosition(canvas).Y;
            Point tempPoint = CoordinateTransform.TransformFromScreenToFullScreen(mouseX, mouseY, zoomStruct);
            int x = (int)tempPoint.X;
            int y = (int)tempPoint.Y;
            int di = (x + y * 1920);   //di = depthpixel index   
            int ci = di * 4;

            double UU = -0.169 * colorPixels[ci + 2] - 0.331 * colorPixels[ci + 1] + 0.5 * colorPixels[ci] + 128;
            double VV = 0.5 * colorPixels[ci + 2] - 0.419 * colorPixels[ci + 1] - 0.081 * colorPixels[ci] + 128;
            System.Diagnostics.Debug.WriteLine(UU.ToString() + ", " + VV.ToString());
            if (isOneColorSetting)
            {
                setOneColorButton.Background = new SolidColorBrush(Color.FromRgb(colorPixels[ci + 2], colorPixels[ci + 1], colorPixels[ci]));
                default1UU = (int)UU;
                default1VV = (int)VV;
                manualMarkList.Clear();
                manualMarkList = new List<Target>();
                Target target;
                target = new Target(0);
                target.Setting(0, 0, default1UU, default1VV);
                manualMarkList.Add(target);
                target = new Target(1);
                target.Setting(0, 0, default2UU, default2VV);
                manualMarkList.Add(target);
                isOneColorSetting = false;
            }
            if (isTwoColorSetting)
            {
                setTwoColorButton.Background = new SolidColorBrush(Color.FromRgb(colorPixels[ci + 2], colorPixels[ci + 1], colorPixels[ci]));
                default2UU = (int)UU;
                default2VV = (int)VV;
                manualMarkList.Clear();
                manualMarkList = new List<Target>();
                Target target;
                target = new Target(0);
                target.Setting(0, 0, default1UU, default1VV);
                manualMarkList.Add(target);
                target = new Target(1);
                target.Setting(0, 0, default2UU, default2VV);
                manualMarkList.Add(target);
                isTwoColorSetting = false;
            }

        }

        public void canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
        
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int mouseX = (int)e.GetPosition(canvas).X;
            int mouseY = (int)e.GetPosition(canvas).Y;
            Point tempPoint = CoordinateTransform.TransformFromScreenToFullScreen(mouseX, mouseY, zoomStruct);
            int x = (int)tempPoint.X;
            int y = (int)tempPoint.Y;
            int di = (x + y * 1920);   //di = depthpixel index   
            int ci = di * 4;

            double UU = -0.169 * colorPixels[ci + 2] - 0.331 * colorPixels[ci + 1] + 0.5 * colorPixels[ci] + 128;
            double VV = 0.5 * colorPixels[ci + 2] - 0.419 * colorPixels[ci + 1] - 0.081 * colorPixels[ci] + 128;
            //System.Diagnostics.Debug.WriteLine(UU.ToString() + ", " + VV.ToString());

            setOneColorButton.Background = new SolidColorBrush(Color.FromRgb(colorPixels[ci + 2], colorPixels[ci + 1], colorPixels[ci]));
            //default1UU = (int)UU;
            //default1VV = (int)VV;


            CatchSuccess = 0;
            
            TargetList[cntemp].Setting(x, y, UU, VV);
            cntemp++;
            if (cntemp >= cntemplimit)
            {
                cntemp = 0;
            }
        }
        
        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int mouseX = (int)e.GetPosition(camera).X;
            int mouseY = (int)e.GetPosition(camera).Y;
            modeCommnad.LeftButtonRelease(mouseX, mouseY);
            Update();
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            int mouseX = (int)e.GetPosition(canvas).X;
            int mouseY = (int)e.GetPosition(canvas).Y;
            modeCommnad.MouseMove(mouseX, mouseY);
            Update();
        }

        private void camera_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int mouseX = (int)e.GetPosition(camera).X;
            int mouseY = (int)e.GetPosition(camera).Y;
            modeCommnad.LeftButtonPress(mouseX, mouseY);
            Update();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int mouseX = (int)e.GetPosition(canvas).X;
            int mouseY = (int)e.GetPosition(canvas).Y;
            modeCommnad.LeftButtonPress(mouseX, mouseY);
            Update();
        }

        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int mouseX = (int)e.GetPosition(canvas).X;
            int mouseY = (int)e.GetPosition(canvas).Y;
            MouseScroll(mouseX, mouseY, e.Delta);
            Update();
        }

        private void camera_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int mouseX = (int)e.GetPosition(canvas).X;
            int mouseY = (int)e.GetPosition(canvas).Y;
            MouseScroll(mouseX, mouseY, e.Delta);
            Update();
        }

        private void MouseScroll(int x, int y, int delta)
        {
            int mouseX = x;
            int mouseY = y;
            int scrollValue = delta;
            if (measureMode == mode.ZoomIn && scrollValue > 0)
            {
                modeCommnad.LeftButtonRelease(mouseX, mouseY);
            }
            else if (measureMode == mode.ZoomOut && scrollValue < 0)
            {
                modeCommnad.LeftButtonRelease(mouseX, mouseY);
            }
        }

        private void visionDetailCollapseButton_Click(object sender, RoutedEventArgs e)
        {
            if (visionDetailGrid.Visibility == Visibility.Visible)
            {
                visionDetailGrid.Visibility = Visibility.Hidden;
            }
            else if (visionDetailGrid.Visibility == Visibility.Hidden)
            {
                visionDetailGrid.Visibility = Visibility.Visible;
            }
        }

        private void Update()
        {
            Calculate();
            Draw();
        }

        private void saveResultButton_Click(object sender, RoutedEventArgs e)
        {
            TakeSnapShot();
        }

        private void AddItemToList()
        {
            Image tempImage = new Image();
            tempImage.Source = frameList[frameList.Count - 1].bitmapSource;
            tempImage.Stretch = Stretch.Uniform;
            tempImage.Height = 50;
            tempImage.Width = 50;
            historyListView.Items.Add(tempImage);
        }

        private void TakeSnapShot()
        {
            isReadToSnapShot = true;
            frameList.Add(new FrameStorage(floorVector4D, originBitampSource, ColorInSkeleton, groupList, manualPingList, neckPoint, neckPointColor));
            AddItemToList();
        }

        private void recordDisplayClick(object sender, RoutedEventArgs e)
        {
            if (RecordGrid.Visibility == Visibility.Visible)
            {
                RecordGrid.Visibility = Visibility.Hidden;
                this.Height -= 70;
            }
            else if (RecordGrid.Visibility == Visibility.Hidden)
            {
                RecordGrid.Visibility = Visibility.Visible;
                this.Height += 70;
            }
        }
        
        private void detailDisplayClick(object sender, RoutedEventArgs e)
        {
            if (visionDetailGrid.Visibility == Visibility.Visible)
            {
                visionDetailGrid.Visibility = Visibility.Hidden;
            }
            else if (visionDetailGrid.Visibility == Visibility.Hidden)
            {
                visionDetailGrid.Visibility = Visibility.Visible;
            }
        }

        private void snapshotDisplayClick(object sender, RoutedEventArgs e)
        {

        }

        private void lineDisplayClick(object sender, RoutedEventArgs e)
        {
           
        }

        private void frontDocClick(object sender, RoutedEventArgs e)
        {
            AnteriorChecked = !AnteriorChecked;
        }
        private void backDocClick(object sender, RoutedEventArgs e)
        {
            PosteriorChecked = !PosteriorChecked;
        }
        private void sideDocClick(object sender, RoutedEventArgs e)
        {
            LeftLateralChecked = !LeftLateralChecked;
        }
        private void MakeDocClick(object sender, RoutedEventArgs e)
        {
            MakeWord makeWord = new MakeWord();
            makeWord.WriteWord(Data, AnteriorChecked, LeftLateralChecked, PosteriorChecked);
        }

        private void LoadDataClick(object sender, RoutedEventArgs e)
        {
            //LoadFile();
        }

        private void historyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = historyListView.SelectedIndex;
            if (isSnapShot)
            {
                replayFrame = frameList[index];
            }
        }

        private void replayButton_Click(object sender, RoutedEventArgs e)
        {
            isReplayButtonClick = !isReplayButtonClick;
            if (isReplayButtonClick)
            {
                replayButton.Content = "解除重播";
                isSnapShot = true;
                if (frameList.Count == 0)
                {
                    isSnapShot = false;
                    isReplayButtonClick = !isReplayButtonClick;
                }
                else
                {
                    replayFrame = frameList[0];
                    historyListView.SelectedIndex = 0;
                }

            }
            else
            {
                replayButton.Content = "開始重播";
                isSnapShot = false;

            }
        }

        private void savePNGButton_Click(object sender, RoutedEventArgs e)
        {
            //Screenshot();
        }
        private void SaveImageClick(object sender, RoutedEventArgs e)
        {
            //Screenshot();
        }

        private void SaveDataClick(object sender, RoutedEventArgs e)
        {
            //Screenshot();
        }
        private void saveDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (replayFrame != null)
            {
                string time = System.DateTime.UtcNow.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);

                string myPhotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                string path = System.IO.Path.Combine(myPhotos, "KinectScreenshot--Postures" + time + ".bin");

                //Stream TestFileStream = File.Create(path);
                //BinaryFormatter serializer = new BinaryFormatter();
                //serializer.Serialize(TestFileStream, replayFrame);
                //TestFileStream.Close();

                IFormatter binFmt = new BinaryFormatter();
                Stream s = File.Open(path, FileMode.Create);
                binFmt.Serialize(s, replayFrame);
                s.Close();
            }
        }

        private void setOneColorButton_Click(object sender, RoutedEventArgs e)
        {
            setColorHintLabel.Content = "請在畫面上用滑鼠右鍵點選顏色";
            isOneColorSetting = true;
        }

        private void setTwoColorButton_Click(object sender, RoutedEventArgs e)
        {
            setColorHintLabel.Content = "請在畫面上用滑鼠右鍵點選顏色";
            isTwoColorSetting = true;
        }

        private void resetSnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            historyListView.Items.Clear();
            frameList.Clear();
        }
        
        //private void ProcessHandler()
        //{
        //    CalculateFrontResult CFR = new CalculateFrontResult();

        //    startProcessLabel.Content = hintElementList[(int)progressSeq.NowStep].HintSentence;
        //    hintProcess.TTSHint(hintElementList[(int)progressSeq.NowStep].HintSentence);
        //    startProcessButton.Content = "完成";
        //    //HumanBodyHintWarning();
            
        //    if (progressSeq.NowStep == Step.FrontReady)
        //    {
        //        hintCoolDown = 0;
        //        startProcessButton.Content = "完成";
        
        //        readyCoolDown = 10;
        //    }
        //    else if (progressSeq.NowStep == Step.BackReady)
        //    {
        //        hintCoolDown = 0;
        //        startProcessButton.Content = "完成";
        //        frontFrameStorage = frameStorageFusion.FuseBodyPoint(frameList);

           
        //        readyCoolDown = 10;
        //        isReadToSnapShot = true;
        //        frameList.Add(frontFrameStorage);
        //        AddItemToList();

        //        Data = CFR.CalculateResult(Data, frontFrameStorage, canvas, zoomStruct, displayStruct);
        //        startFrameIndex = frameList.Count;
        //    }
        //    else if (progressSeq.NowStep == Step.SideReady)
        //    {
        //        hintCoolDown = 0;
        //        startProcessButton.Content = "完成";
        //        backFrameStorage = frameStorageFusion.FuseBodyPoint(frameList, startFrameIndex, frameList.Count);
        //        //hintProcess.TTSHint("準備照相");
        //        readyCoolDown = 10;

        //        isReadToSnapShot = true;
        //        frameList.Add(backFrameStorage);
        //        AddItemToList();

        //        Data = CFR.CalculateBackResult(Data, backFrameStorage);
        //        startFrameIndex = frameList.Count;
        //    }
        //    else if (progressSeq.NowStep == Step.Done)
        //    {
        //        hintCoolDown = 0;
        //        startProcessButton.Content = "完成";

        //        sideFrameStorage = frameStorageFusion.FuseBodyPoint(frameList, startFrameIndex, frameList.Count);

        //        isReadToSnapShot = true;
        //        frameList.Add(sideFrameStorage);
        //        AddItemToList();
        //        //全部完成
        //        Data = CFR.CalculateSideResult(Data, sideFrameStorage);
        //    }
        //    else
        //    {
        //        manualMarkList[0].CleanExpectColorPoint();
        //        manualMarkList[1].CleanExpectColorPoint();
        //        if (patientBody != null)
        //        {
        //            for (int z = 0; z < hintElementList[(int)progressSeq.NowStep].JointTypeList.Length; z++)
        //            {
        //                Joint tempJoint = patientBody.Joints[hintElementList[(int)progressSeq.NowStep].JointTypeList[z]];
        //                ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
        //                colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
        //                colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;

        //                manualMarkList[0].SetExpectColorPoint(colorPoint.X, colorPoint.Y);
        //                manualMarkList[1].SetExpectColorPoint(colorPoint.X, colorPoint.Y);
        //            }
        //        }
        //        startProcessButton.Content = "跳過這步";
        //        hintCoolDown = 10;
        //    }
        //}

        private void startProcessButton_Click(object sender, RoutedEventArgs e)
        {
            progressSeq.Start();
            //ProcessHandler();
        }

        private void manualFusionButton_Click(object sender, RoutedEventArgs e)
        {
            FrameStorage framestorage = frameStorageFusion.FuseBodyPoint(frameList);
            isReadToSnapShot = true;
            frameList.Add(framestorage);
            AddItemToList();
        }

        private void resetManualMarkButton_Click(object sender, RoutedEventArgs e)
        {
            manualMarkList.Clear();
            Target target;
            target = new Target(0);
            target.Setting(0, 0, default1UU, default1VV);//light green
            manualMarkList.Add(target);
            target = new Target(1);
            target.Setting(0, 0, default2UU, default2VV);//yellow
            manualMarkList.Add(target);
            int NumbersOfTarget = 10;
            TargetList = new List<Target>();
            for (int t = 0; t <= NumbersOfTarget; t++)
            {
                Target target2 = new Target(t);
                TargetList.Add(target2);
            }
            cntemp = 0;
        }

        private int angleLimit1 = 90;
        private int angleLimit2 = 90;
        private void RangeMotionModeButtonClicked(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            //腿部動作
            if (btn.Content.ToString().Contains("Hip Flexion"))
            {
                motionMode = MotionMode.HipFlexion;
                angleLimitLabel1.Content = "大腿與身體角度";
                angleLimitLabel2.Content = "膝蓋角度";
                cntemplimit = 4;
                angleLimit2TextBox.IsEnabled = true;
                angleLimitLabel2.IsEnabled = true;
            }
            if (btn.Content.ToString().Contains("Kick Straight"))
            {
                motionMode = MotionMode.KickStraight;
                angleLimitLabel1.Content = "膝蓋角度";
                angleLimitLabel2.Content = "無";
                cntemplimit = 3;
                angleLimit2TextBox.IsEnabled = false;
                angleLimitLabel2.IsEnabled = false;
            }
            if (btn.Content.ToString().Contains("Heel Raise"))
            {
                motionMode = MotionMode.HeelRaise;
                angleLimitLabel1.Content = "腳跟角度";
                angleLimitLabel2.Content = "無";
                cntemplimit = 3;
                angleLimit2TextBox.IsEnabled = false;
                angleLimitLabel2.IsEnabled = false;
            }
            if (btn.Content.ToString().Contains("Toe Raise"))
            {
                motionMode = MotionMode.ToeRaise;
                angleLimitLabel1.Content = "腳跟角度";
                angleLimitLabel2.Content = "無";
                cntemplimit = 3;
                angleLimit2TextBox.IsEnabled = false;
                angleLimitLabel2.IsEnabled = false;
            }
            if (btn.Content.ToString().Contains("Hip Abduction"))
            {
                motionMode = MotionMode.HipAbduction;
                angleLimitLabel1.Content = "膝蓋角度";
                angleLimitLabel2.Content = "無";
                cntemplimit = 4;
                angleLimit2TextBox.IsEnabled = false;
                angleLimitLabel2.IsEnabled = false;
            }
            /*******************************************************/
            //手部
            /******************************************************/
            if (btn.Content.ToString().Contains("Elbow Flexion"))
            {
                motionMode = MotionMode.ElbowFlexion;
                cntemplimit = 3;
                angleLimit2TextBox.IsEnabled = false;
                angleLimitLabel2.IsEnabled = false;
            }
            if (btn.Content.ToString().Contains("Shoulder Flexion"))
            {
                motionMode = MotionMode.ShoulderFlexion;
                cntemplimit = 4;
                angleLimit2TextBox.IsEnabled = false;
                angleLimitLabel2.IsEnabled = false;
            }
            if (btn.Content.ToString().Contains("Shoulder Abduction"))
            {
                motionMode = MotionMode.ShoulderAbduction;
                cntemplimit = 3;
                angleLimit2TextBox.IsEnabled = false;
                angleLimitLabel2.IsEnabled = false;
            }
            if (btn.Content.ToString().Contains("Shoulder Extension"))
            {
                motionMode = MotionMode.ShoulderExtension;
                cntemplimit = 3;
                angleLimit2TextBox.IsEnabled = false;
                angleLimitLabel2.IsEnabled = false;
            }
            if (btn.Content.ToString().Contains("External Rotation"))
            {
                motionMode = MotionMode.ExternalRotation;
            }
            if (btn.Content.ToString().Contains("External Rotation at 90"))
            {
                motionMode = MotionMode.ExternalRotation90;
            }
            if (btn.Content.ToString().Contains("Internal Rotation at 90"))
            {
                motionMode = MotionMode.InternalRotation;
            }
        }

        private void FindTargetBall(MotionMode motionmode, Body[] bodies)
        {
            foreach (Body body in this.bodies)
            {
                if (body.IsTracked)
                {
                    /*******************************************************/
                    //腿部
                    /******************************************************/
                    if (motionmode == MotionMode.HipFlexion)
                    {
                        if (!TargetList[0].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.AnkleRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[0].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);  
                        }
                        if (!TargetList[1].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.KneeRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[1].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                        if (!TargetList[2].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.HipRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[2].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                        if (!TargetList[3].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.ShoulderRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[3].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                    }
                    if (motionmode == MotionMode.KickStraight)
                    {
                        if (!TargetList[0].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.AnkleRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[0].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                        if (!TargetList[1].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.KneeRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[1].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                        if (!TargetList[2].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.HipRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[2].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                       
                    }
                    if (motionmode == MotionMode.HeelRaise)
                    {
                        if (!TargetList[0].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.FootRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[0].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                        if (!TargetList[1].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.AnkleRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[1].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                        if (!TargetList[2].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.KneeRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[2].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                    }
                    if (motionmode == MotionMode.ToeRaise)
                    {
                        if (!TargetList[0].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.FootRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[0].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                        if (!TargetList[1].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.AnkleRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[1].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                        if (!TargetList[2].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.KneeRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[2].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }

                    }
                    if (motionmode == MotionMode.HipAbduction)
                    {
                        if (!TargetList[0].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.FootRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[0].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                        if (!TargetList[1].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.AnkleRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[1].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }
                        if (!TargetList[2].IsTracked())
                        {
                            Joint tempJoint = body.Joints[JointType.KneeRight];
                            ColorSpacePoint colorPoint = coordinateMapper.MapCameraPointToColorSpace(tempJoint.Position);
                            colorPoint.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                            colorPoint.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;
                            TargetList[2].Setting((int)colorPoint.X, (int)colorPoint.Y, 78, 125);
                        }

                    }
                    /*******************************************************/
                    //手部
                    /******************************************************/
                }
            }
        }

        private void doctorSettingEnterbutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                angleLimit1 = Int32.Parse(angleLimit1TextBox.Text);
            }
            catch
            {

            }
            try
            {
                angleLimit2 = Int32.Parse(angleLimit2TextBox.Text);
            }
            catch
            {

            }
            FindTargetBall(motionMode, bodies);
        }

        private void DrawRangeMotion()
        {
            #region 腿部
            if (motionMode == MotionMode.HipFlexion)
            {
                double LegAngle = AngleCal.AngleBetween(TargetList[0], TargetList[1], TargetList[2]);
                Point point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                AddPixel.Text(point.X + 30, point.Y - 20, LegAngle.ToString("f3"), Color.FromRgb(255, 0, 0), canvas);

                Polygon myPolygon = new Polygon();
                myPolygon.Stroke = System.Windows.Media.Brushes.Black;
                if (LegAngle > angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.Red;
                }
                else if (LegAngle < angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
                }
                myPolygon.StrokeThickness = 2;
                myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
                myPolygon.VerticalAlignment = VerticalAlignment.Center;
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[0].point2D().X, (int)TargetList[0].point2D().Y, zoomStruct);
                System.Windows.Point Point1 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                System.Windows.Point Point2 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[2].point2D().X, (int)TargetList[2].point2D().Y, zoomStruct);
                System.Windows.Point Point3 = new System.Windows.Point(point.X, point.Y);
                PointCollection myPointCollection = new PointCollection();
                myPointCollection.Add(Point1);
                myPointCollection.Add(Point2);
                myPointCollection.Add(Point3);
                myPolygon.Points = myPointCollection;

                canvas.Children.Add(myPolygon);

                double HipAngle = AngleCal.AngleBetween(TargetList[1], TargetList[2], TargetList[3]);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                AddPixel.Text(point.X + 30, point.Y - 20, HipAngle.ToString("f3"), Color.FromRgb(255, 0, 0), canvas);

                myPolygon = new Polygon();
                myPolygon.Stroke = Brushes.Black;
                if (LegAngle > angleLimit2)
                {
                    myPolygon.Fill = Brushes.Red;
                }
                else if (LegAngle < angleLimit2)
                {
                    myPolygon.Fill = Brushes.LightSeaGreen;
                }
                myPolygon.StrokeThickness = 2;
                myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
                myPolygon.VerticalAlignment = VerticalAlignment.Center;
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[0].point2D().X, (int)TargetList[0].point2D().Y, zoomStruct);
                Point1 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                Point2 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[2].point2D().X, (int)TargetList[2].point2D().Y, zoomStruct);
                Point3 = new System.Windows.Point(point.X, point.Y);
                myPointCollection = new PointCollection();
                myPointCollection.Add(Point1);
                myPointCollection.Add(Point2);
                myPointCollection.Add(Point3);
                myPolygon.Points = myPointCollection;

                canvas.Children.Add(myPolygon);
            }
            if (motionMode == MotionMode.KickStraight)
            {
                double LegAngle = AngleCal.AngleBetween(TargetList[0], TargetList[1], TargetList[2]);
                Point point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                AddPixel.Text(point.X + 30, point.Y - 20, LegAngle.ToString("f3"), Color.FromRgb(255, 0, 0), canvas);

                Polygon myPolygon = new Polygon();
                myPolygon.Stroke = System.Windows.Media.Brushes.Black;
                if (LegAngle > angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.Red;
                }
                else if (LegAngle < angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
                }
                myPolygon.StrokeThickness = 2;
                myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
                myPolygon.VerticalAlignment = VerticalAlignment.Center;
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[0].point2D().X, (int)TargetList[0].point2D().Y, zoomStruct);
                System.Windows.Point Point1 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                System.Windows.Point Point2 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[2].point2D().X, (int)TargetList[2].point2D().Y, zoomStruct);
                System.Windows.Point Point3 = new System.Windows.Point(point.X, point.Y);
                PointCollection myPointCollection = new PointCollection();
                myPointCollection.Add(Point1);
                myPointCollection.Add(Point2);
                myPointCollection.Add(Point3);
                myPolygon.Points = myPointCollection;

                canvas.Children.Add(myPolygon);
            }
            if (motionMode == MotionMode.ToeRaise)
            {
                double LegAngle = AngleCal.AngleBetween(TargetList[0], TargetList[1], TargetList[2]);
                Point point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                AddPixel.Text(point.X + 30, point.Y - 20, LegAngle.ToString("f3"), Color.FromRgb(255, 0, 0), canvas);

                Polygon myPolygon = new Polygon();
                myPolygon.Stroke = System.Windows.Media.Brushes.Black;
                if (LegAngle > angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.Red;
                }
                else if (LegAngle < angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
                }
                myPolygon.StrokeThickness = 2;
                myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
                myPolygon.VerticalAlignment = VerticalAlignment.Center;
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[0].point2D().X, (int)TargetList[0].point2D().Y, zoomStruct);
                System.Windows.Point Point1 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                System.Windows.Point Point2 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[2].point2D().X, (int)TargetList[2].point2D().Y, zoomStruct);
                System.Windows.Point Point3 = new System.Windows.Point(point.X, point.Y);
                PointCollection myPointCollection = new PointCollection();
                myPointCollection.Add(Point1);
                myPointCollection.Add(Point2);
                myPointCollection.Add(Point3);
                myPolygon.Points = myPointCollection;

                canvas.Children.Add(myPolygon);
            }
            if (motionMode == MotionMode.HeelRaise)
            {
                double LegAngle = AngleCal.AngleBetween(TargetList[0], TargetList[1], TargetList[2]);
                Point point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                AddPixel.Text(point.X + 30, point.Y - 20, LegAngle.ToString("f3"), Color.FromRgb(255, 0, 0), canvas);

                Polygon myPolygon = new Polygon();
                myPolygon.Stroke = System.Windows.Media.Brushes.Black;
                if (LegAngle > angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.Red;
                }
                else if ( LegAngle < angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
                }
                myPolygon.StrokeThickness = 2;
                myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
                myPolygon.VerticalAlignment = VerticalAlignment.Center;
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[0].point2D().X, (int)TargetList[0].point2D().Y, zoomStruct);
                System.Windows.Point Point1 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                System.Windows.Point Point2 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[2].point2D().X, (int)TargetList[2].point2D().Y, zoomStruct);
                System.Windows.Point Point3 = new System.Windows.Point(point.X, point.Y);
                PointCollection myPointCollection = new PointCollection();
                myPointCollection.Add(Point1);
                myPointCollection.Add(Point2);
                myPointCollection.Add(Point3);
                myPolygon.Points = myPointCollection;

                canvas.Children.Add(myPolygon);
            }
            if (motionMode == MotionMode.HipAbduction)
            {

            }
            #endregion

            #region 手部
            if (motionMode == MotionMode.ElbowFlexion)
            {
                double elbowAngle = AngleCal.AngleBetween(TargetList[0], TargetList[1], TargetList[2]);
                Point point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                AddPixel.Text(point.X + 30, point.Y - 20, elbowAngle.ToString("f3"), Color.FromRgb(255, 0, 0), canvas);

                Polygon myPolygon = new Polygon();
                myPolygon.Stroke = System.Windows.Media.Brushes.Black;
                if (elbowAngle > angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.Red;
                }
                else if (elbowAngle < angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
                }
                myPolygon.StrokeThickness = 2;
                myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
                myPolygon.VerticalAlignment = VerticalAlignment.Center;
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[0].point2D().X, (int)TargetList[0].point2D().Y, zoomStruct);
                System.Windows.Point Point1 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                System.Windows.Point Point2 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[2].point2D().X, (int)TargetList[2].point2D().Y, zoomStruct);
                System.Windows.Point Point3 = new System.Windows.Point(point.X, point.Y);
                PointCollection myPointCollection = new PointCollection();
                myPointCollection.Add(Point1);
                myPointCollection.Add(Point2);
                myPointCollection.Add(Point3);
                myPolygon.Points = myPointCollection;

                canvas.Children.Add(myPolygon);
            }
            if (motionMode == MotionMode.ShoulderFlexion)
            {
                double elbowAngle = AngleCal.AngleBetween(TargetList[0], TargetList[1], TargetList[2]);
                Point point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                AddPixel.Text(point.X + 30, point.Y - 20, elbowAngle.ToString("f3"), Color.FromRgb(255, 0, 0), canvas);

                Polygon myPolygon = new Polygon();
                myPolygon.Stroke = System.Windows.Media.Brushes.Black;
                if (elbowAngle > angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.Red;
                }
                else if (elbowAngle < angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
                }
                myPolygon.StrokeThickness = 2;
                myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
                myPolygon.VerticalAlignment = VerticalAlignment.Center;
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[0].point2D().X, (int)TargetList[0].point2D().Y, zoomStruct);
                System.Windows.Point Point1 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                System.Windows.Point Point2 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[2].point2D().X, (int)TargetList[2].point2D().Y, zoomStruct);
                System.Windows.Point Point3 = new System.Windows.Point(point.X, point.Y);
                PointCollection myPointCollection = new PointCollection();
                myPointCollection.Add(Point1);
                myPointCollection.Add(Point2);
                myPointCollection.Add(Point3);
                myPolygon.Points = myPointCollection;

                canvas.Children.Add(myPolygon);
            }
            if (motionMode == MotionMode.ShoulderAbduction)
            {
                double elbowAngle = AngleCal.AngleBetween(TargetList[0], TargetList[1], TargetList[2]);
                Point point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                AddPixel.Text(point.X + 30, point.Y - 20, elbowAngle.ToString("f3"), Color.FromRgb(255, 0, 0), canvas);

                Polygon myPolygon = new Polygon();
                myPolygon.Stroke = System.Windows.Media.Brushes.Black;
                if (elbowAngle > angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.Red;
                }
                else if (elbowAngle < angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
                }
                myPolygon.StrokeThickness = 2;
                myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
                myPolygon.VerticalAlignment = VerticalAlignment.Center;
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[0].point2D().X, (int)TargetList[0].point2D().Y, zoomStruct);
                System.Windows.Point Point1 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                System.Windows.Point Point2 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[2].point2D().X, (int)TargetList[2].point2D().Y, zoomStruct);
                System.Windows.Point Point3 = new System.Windows.Point(point.X, point.Y);
                PointCollection myPointCollection = new PointCollection();
                myPointCollection.Add(Point1);
                myPointCollection.Add(Point2);
                myPointCollection.Add(Point3);
                myPolygon.Points = myPointCollection;

                canvas.Children.Add(myPolygon);
            }
            if (motionMode == MotionMode.ShoulderExtension)
            {
                double elbowAngle = AngleCal.AngleBetween(TargetList[0], TargetList[1], TargetList[2]);
                Point point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                AddPixel.Text(point.X + 30, point.Y - 20, elbowAngle.ToString("f3"), Color.FromRgb(255, 0, 0), canvas);

                Polygon myPolygon = new Polygon();
                myPolygon.Stroke = System.Windows.Media.Brushes.Black;
                if (elbowAngle > angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.Red;
                }
                else if (elbowAngle < angleLimit1)
                {
                    myPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
                }
                myPolygon.StrokeThickness = 2;
                myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
                myPolygon.VerticalAlignment = VerticalAlignment.Center;
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[0].point2D().X, (int)TargetList[0].point2D().Y, zoomStruct);
                System.Windows.Point Point1 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[1].point2D().X, (int)TargetList[1].point2D().Y, zoomStruct);
                System.Windows.Point Point2 = new System.Windows.Point(point.X, point.Y);
                point = CoordinateTransform.ReverseFromFullScreenToScreen((int)TargetList[2].point2D().X, (int)TargetList[2].point2D().Y, zoomStruct);
                System.Windows.Point Point3 = new System.Windows.Point(point.X, point.Y);
                PointCollection myPointCollection = new PointCollection();
                myPointCollection.Add(Point1);
                myPointCollection.Add(Point2);
                myPointCollection.Add(Point3);
                myPolygon.Points = myPointCollection;

                canvas.Children.Add(myPolygon);
            }

            #endregion
        }
    }
}
