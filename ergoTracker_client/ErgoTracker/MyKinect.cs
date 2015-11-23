using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErgoTracker
{
    class MyKinect
    {
        private KinectSensor myKinect;
        int counter = 0;

        public MyKinect()
        { }

        ~MyKinect()
        {
            myKinect.AllFramesReady -= myKinect_AllFramesReady;
        }

        public Boolean InitializeKinectSensor(bool depthSensorActive, bool colorSensorActive, bool skeletonSensorActive)
        {
            if (KinectSensor.KinectSensors.Count == 0)
            {
                MessageBox.Show("No Kinect devices detected!", "Camera View");
                return false;
            }

            try
            {
                myKinect = KinectSensor.KinectSensors[0];
                if (depthSensorActive) myKinect.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                if (colorSensorActive) myKinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                if (skeletonSensorActive) myKinect.SkeletonStream.Enable();

                myKinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(myKinect_AllFramesReady);
                myKinect.Start();
            }
            catch
            {
                MessageBox.Show("Kinect Initialization Failed");
                return false;
            }

            return true;
        }

        private void myKinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            // do event handling of when all frames are ready here
            SkeletonFrame frame = e.OpenSkeletonFrame();

            Skeleton[] skeletons = new Skeleton[frame.SkeletonArrayLength];
            frame.CopySkeletonDataTo(skeletons);
            Skeleton skeletonToUse = null;

            float depth = float.MaxValue;
            foreach (Skeleton s in skeletons)
            {
                if (s.TrackingState == SkeletonTrackingState.Tracked)
                {
                    if (s.Position.Z < depth && s.Position.Z != 0)
                    {
                        depth = s.Position.Z;
                        skeletonToUse = s;
                    }
                }
            }

            if (skeletonToUse != null)
            {
                ++counter;
                string jsonStr = JsonConverter.createJSONString("", skeletonToUse);
                //Console.WriteLine(jsonStr);
                if (counter == 30)
                {
                    counter = 0;
                    // do web socket stuff here!

                }
            }

            frame.Dispose();
        }
        
        public KinectSensor getSensor()
        {
            return this.myKinect;
        }
    }
}
