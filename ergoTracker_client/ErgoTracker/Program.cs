using Microsoft.Kinect;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErgoTracker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MyKinect myKinect = new MyKinect();
            if (!myKinect.InitializeKinectSensor(true, true, true)) Application.Exit();
            
            string appName = Process.GetCurrentProcess().ProcessName + ".exe";
            SetIEVersionKeyForWebBrowserControl(appName);
            var Form_1 = new Form1();
//            var KinectView = new KinectForm(myKinect.getSensor());

            Form_1.Show();

//            KinectView.Show();

            using (TaskBarControl tbc = new TaskBarControl(myKinect))
            {
                tbc.Display();

                Application.Run();
            }
            //Application.Run(new Form1());
            //Application.Run(new KinectForm());
        }

        private static void SetIEVersionKeyForWebBrowserControl(string appName)
        {
            RegistryKey key = null;
            try
            {
                key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);

                if (key == null)
                {
                    EventLog.WriteEntry("Couldn't find registry key", "Registry key could not be found.");
                    return;
                }

                string findAppKey = Convert.ToString(key.GetValue(appName));

                if (findAppKey == "" + 11001)
                {
                    key.Close();
                    return;
                }
                
                key.SetValue(appName, unchecked(11001), RegistryValueKind.DWord);

                findAppKey = Convert.ToString(key.GetValue(appName));

                if (findAppKey == "" + 11001)
                    EventLog.WriteEntry("Successfully changed IE version key for web browser.", "Current version is 11001");
                else
                    EventLog.WriteEntry("Unsuccessful in changing IE version key for web browser.", "Current version is " + findAppKey);
                    
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("An exception has occurred in setting ie version for web browser control", e.ToString());
            }
            finally
            {
                if (key != null)
                    key.Close();
            }
        }
    }
}
