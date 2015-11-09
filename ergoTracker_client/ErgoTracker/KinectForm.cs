using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErgoTracker
{
    public partial class KinectForm : Form
    {
        public KinectForm()
        {
            InitializeComponent();
        }

        Microsoft.Kinect.KinectSensor myKinect;

        private void KinectForm_Load(object sender, EventArgs e)
        {
            if (Microsoft.Kinect.KinectSensor.KinectSensors.Count == 0)
            {
                MessageBox.Show("No Kinect devices detected!", "Camera View");
                return;
            }

            try
            {
                myKinect = Microsoft.Kinect.KinectSensor.KinectSensors[0];
                myKinect.DepthStream.Enable();
                myKinect.ColorStream.Enable();

                myKinect.Start();

                myKinect.AllFramesReady += new EventHandler<Microsoft.Kinect.AllFramesReadyEventArgs>(myKinect_AllFramesReady);
            }
            catch
            {
                MessageBox.Show("Kinect Initialization Failed");
                return;
            }
        }

        Bitmap _bitmap;

        void myKinect_AllFramesReady(object sender, Microsoft.Kinect.AllFramesReadyEventArgs e)
        {
            myKinect_SensorDepthFrameReady(e);
            kinectVideoBox.Image = _bitmap;
        }

        void myKinect_SensorDepthFrameReady(Microsoft.Kinect.AllFramesReadyEventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                using (var frame = e.OpenDepthImageFrame())
                {
                    _bitmap = CreateBitmapFromDepthFrame(frame);
                }
            }
        }

        private Bitmap CreateBitmapFromDepthFrame(Microsoft.Kinect.DepthImageFrame frame)
        {
            if (frame != null)
            {
                var bitmapImage = new Bitmap(frame.Width, frame.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
                var g = Graphics.FromImage(bitmapImage);
                g.Clear(Color.FromArgb(0, 34, 68));

                var _pixelData = new short[frame.PixelDataLength];
                frame.CopyPixelDataTo(_pixelData);
                System.Drawing.Imaging.BitmapData bmapdata = bitmapImage.LockBits(new Rectangle(0, 0, frame.Width, frame.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmapImage.PixelFormat);
                IntPtr ptr = bmapdata.Scan0;
                Marshal.Copy(_pixelData, 0, ptr, frame.Width * frame.Height);
                bitmapImage.UnlockBits(bmapdata);

                return bitmapImage;
            }

            return null;
        }
    }
}
