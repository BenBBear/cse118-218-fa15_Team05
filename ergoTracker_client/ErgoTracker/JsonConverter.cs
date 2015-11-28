using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErgoTracker
{
    class JsonConverter
    {
        public static string createKinectDataString(string email)
        {
            string jsonString = "{";
            jsonString = writeBasicInformation(email, jsonString);
//            jsonString = writeFrameData(skel, jsonString);
//            jsonString = closeJsonStringObject(jsonString);

            return jsonString;
        }

        private static string writeBasicInformation(string email, string jsonString)
        {
            double current_time = ConvertToUnixTimestamp(DateTime.Today);
            string new_string = "\"userId\":\"" + email + "\"," + " \"Points\":[";
            jsonString += new_string;
            return jsonString;
        }

        public static string writeFrameData(Skeleton skel, string jsonString)
        {
            if (skel == null) return jsonString;

            Joint[] joints = { skel.Joints[JointType.Head], skel.Joints[JointType.ShoulderCenter],
                                skel.Joints[JointType.ShoulderLeft], skel.Joints[JointType.ShoulderRight],
                                skel.Joints[JointType.ElbowLeft], skel.Joints[JointType.ElbowRight],
                                skel.Joints[JointType.Spine], skel.Joints[JointType.HipCenter],
                                skel.Joints[JointType.HipLeft], skel.Joints[JointType.HipRight] };

            string joint_str = "{ ";
            foreach (Joint j in joints)
            {
                string joint_name = j.JointType.ToString();
                joint_str += "\"" + joint_name + "\":{";
                float x_coord = j.Position.X;
                joint_str += "\"x\":" + x_coord;
                float y_coord = j.Position.Y;
                joint_str += ", \"y\":" + y_coord;
                float z_coord = j.Position.Z;
                joint_str += ", \"z\":" + z_coord;
                joint_str += "},";
                jsonString += joint_str;
                joint_str = "";
            }

            jsonString = jsonString.Remove(jsonString.Count() - 1);
            jsonString += "},";

            return jsonString;
        }

        public static string closeJsonStringObject(string jsonString)
        {
            jsonString = jsonString.Remove(jsonString.Count() - 1);
            jsonString += "]}";

            return jsonString;
        }

        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
