using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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

        KinectSensor myKinect;

        private void KinectForm_Load(object sender, EventArgs e)
        {
            if (Microsoft.Kinect.KinectSensor.KinectSensors.Count == 0)
            {
                MessageBox.Show("No Kinect devices detected!", "Camera View");
                return;
            }

            try
            {
                myKinect = KinectSensor.KinectSensors[0];
                myKinect.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                myKinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                myKinect.SkeletonStream.Enable();

                myKinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(myKinect_AllFramesReady);
                myKinect.Start();
            }
            catch
            {
                MessageBox.Show("Kinect Initialization Failed");
                return;
            }
        }

        Bitmap _bitmap;

        void myKinect_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            myKinect_SensorDepthFrameReady(e);
            kinectVideoBox.Image = _bitmap;
        }

        void myKinect_SensorDepthFrameReady(AllFramesReadyEventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                using (var frame = e.OpenColorImageFrame())
                {
                    //_bitmap = CreateBitmapFromDepthFrame(frame);
                    _bitmap = CreateBitmapFromColorImage(frame);
                    DrawHead(e.OpenSkeletonFrame(), _bitmap);
                }
            }
        }

        private void DrawHead(SkeletonFrame frame, Bitmap bitmap)
        {
            if (frame != null)
            {
                Skeleton[] skeletons = new Skeleton[frame.SkeletonArrayLength];
                frame.CopySkeletonDataTo(skeletons);

                foreach (Skeleton s in skeletons)
                {
                    if (s.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        SkeletonPoint sloc = s.Joints[JointType.Head].Position;
                        ColorImagePoint cloc = myKinect.CoordinateMapper.MapSkeletonPointToColorPoint(sloc, ColorImageFormat.RgbResolution640x480Fps30);
                        markAtPoint(cloc, bitmap);
                        DrawSkeleton(s, bitmap);
                    }
                }
            }
        }

        private void DrawSkeleton(Skeleton s, Bitmap bitmap)
        {
            if (bitmap == null) return;

            Graphics g = Graphics.FromImage(bitmap);
            // draw head to shoulder center
            DrawBone(JointType.Head, JointType.ShoulderCenter, s, g);
            // draw shoulder center to shoulder left
            DrawBone(JointType.ShoulderCenter, JointType.ShoulderLeft, s, g);
            // draw shoulder center to shoulder right
            DrawBone(JointType.ShoulderCenter, JointType.ShoulderRight, s, g);
            // draw shoulder left to elbow left
            DrawBone(JointType.ShoulderLeft, JointType.ElbowLeft, s, g);
            // draw shoulder right to elbow right
            DrawBone(JointType.ShoulderRight, JointType.ElbowRight, s, g);
            // draw shoulder center to spine
            DrawBone(JointType.ShoulderCenter, JointType.Spine, s, g);
            // draw spine to hip center
            DrawBone(JointType.Spine, JointType.HipCenter, s, g);
            // draw hip center to hip left
            DrawBone(JointType.HipCenter, JointType.HipLeft, s, g);
            // draw hip center to hip right
            DrawBone(JointType.HipCenter, JointType.HipRight, s, g);
        }


        private Bitmap CreateBitmapFromDepthFrame(DepthImageFrame frame)
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

        private Bitmap CreateBitmapFromColorImage(ColorImageFrame frame)
        {
            if (frame != null)
            {
                byte[] pixelData = new byte[frame.PixelDataLength];
                frame.CopyPixelDataTo(pixelData);
                Bitmap bmap = new Bitmap(
                    frame.Width,
                    frame.Height,
                    PixelFormat.Format32bppRgb);
                BitmapData bmapdata = bmap.LockBits(new Rectangle(0, 0, frame.Width, frame.Height), ImageLockMode.WriteOnly, bmap.PixelFormat);
                IntPtr ptr = bmapdata.Scan0;
                Marshal.Copy(pixelData, 0, ptr, frame.PixelDataLength);
                bmap.UnlockBits(bmapdata);
                return bmap;
            }
            return null;
        }

        private void markAtPoint(ColorImagePoint p, Bitmap b)
        {
            if (b == null) return;
            Graphics g = Graphics.FromImage(b);
            g.DrawEllipse(Pens.Red, new Rectangle(p.X - 20, p.Y - 20, 40, 40));
        }

        private Point GetJoint(JointType j, Skeleton s)
        {
            SkeletonPoint sloc = s.Joints[j].Position;
            ColorImagePoint cloc = myKinect.CoordinateMapper.MapSkeletonPointToColorPoint(sloc, ColorImageFormat.RgbResolution640x480Fps30);
            return new Point(cloc.X, cloc.Y);
        }

        private void DrawBone(JointType j1, JointType j2, Skeleton s, Graphics g)
        {
            Point p1 = GetJoint(j1, s);
            Point p2 = GetJoint(j2, s);

            Pen drawingPen = new Pen(Color.LimeGreen, 5);
            try
            {
                g.DrawLine(drawingPen, p1, p2);
            }
            catch (Exception e)
            {
                System.Diagnostics.EventLog.WriteEntry("Error Occurred in drawing skeleton.", e.ToString());
            }
        }
    }
}
