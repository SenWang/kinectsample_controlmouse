using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Kinect;
using System.Timers;
using System.Windows.Forms;
using Microsoft.Kinect.Samples.CursorControl;

namespace WpfApplication1
{
    public partial class MainApp : Window
    {
        KinectSensor kinect;
        public MainApp(KinectSensor sensor) : this()
        {
            kinect = sensor;
            kinect.SkeletonStream.Enable();
            kinect.SkeletonFrameReady += kinect_SkeletonFrameReady;
            kinect.Start();

        }

        void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skf = e.OpenSkeletonFrame())
            {
                #region basic skeleton process
                if (skf == null)
                    return;

                Skeleton[] FrameSkeletons = new Skeleton[skf.SkeletonArrayLength];
                skf.CopySkeletonDataTo(FrameSkeletons);
                Skeleton user = (from s in FrameSkeletons
                                         where s.TrackingState == SkeletonTrackingState.Tracked
                                         select s).FirstOrDefault();
                if (user == null)
                    return;
                #endregion

                #region update gui
                Joint righthand = user.Joints[JointType.HandRight];
                rhand_x.Text = String.Format("右手 X:{0:0.0}", righthand.Position.X);
                rhand_y.Text = String.Format("右手 Y:{0:0.0}", righthand.Position.Y);
                rhand_z.Text = String.Format("右手 Z:{0:0.0}", righthand.Position.Z);
                #endregion

                NativeMethods.SendMouseInput(
                    (int)(righthand.Position.X * 1024),
                   -(int)(righthand.Position.Y * 768),
                    (int)SystemParameters.PrimaryScreenWidth,
                    (int)SystemParameters.PrimaryScreenHeight,
                    false
                );
            }
        }
        


        public MainApp()
        {
            InitializeComponent();
        }
    }
}
