using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErgoTracker
{
    public class MyKinect
    {
        private KinectSensor myKinect;
        int counter = 0;
        int totalDataCounter = 0;
        string data = "";

        public MyKinect()
        { }

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

            if (frame == null) return;

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
                //string jsonStr = JsonConverter.createKinectDataString("", skeletonToUse);
                //Console.WriteLine(jsonStr);
                if (totalDataCounter == 0)
                    data = JsonConverter.createKinectDataString(ApplicationInformation.Instance.getUsername());
                if (counter == 6)
                {
                    counter = 0;
                    totalDataCounter++;
                    data = JsonConverter.writeFrameData(skeletonToUse, data);

                    // should be flushing to the server about once a minute
                    if (totalDataCounter == 300)
                    {
                        data = JsonConverter.closeJsonStringObject(data);
                        // flush data to the server here!

                        totalDataCounter = 0;
                    }
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
