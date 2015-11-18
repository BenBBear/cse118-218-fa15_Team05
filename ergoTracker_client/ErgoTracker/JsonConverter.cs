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
        public static string createJSONString(string email, Skeleton skel)
        {
            string jsonString = "{";
            writeBasicInformation(email, jsonString);
            writeFrameData(skel, jsonString);
            closeJsonStringObject(jsonString);

            return jsonString;
        }

        private static void writeBasicInformation(string email, string jsonString)
        {
            double current_time = ConvertToUnixTimestamp(DateTime.Today);
            string new_string = "\"email\":\"" + email + "\", \"date\":\"" + current_time + "\"";
            jsonString += new_string;
        }

        private static void writeFrameData(Skeleton skel, string jsonString)
        {
            if (skel == null) return;

            jsonString += ", \"SkeletonData\":[ ";
            Joint[] joints = { skel.Joints[JointType.Head], skel.Joints[JointType.ShoulderCenter],
                                skel.Joints[JointType.ShoulderLeft], skel.Joints[JointType.ShoulderRight],
                                skel.Joints[JointType.ElbowLeft], skel.Joints[JointType.ElbowRight],
                                skel.Joints[JointType.Spine], skel.Joints[JointType.HipCenter],
                                skel.Joints[JointType.HipLeft], skel.Joints[JointType.HipRight] };

            string joint_str = "{";
            foreach (Joint j in joints)
            {
                string joint_name = j.ToString();
                joint_str += "\"jointname\":\"" + joint_name + "\"";
                float x_coord = j.Position.X;
                joint_str += ", \"x\":" + x_coord;
                float y_coord = j.Position.Y;
                joint_str += ", \"y\":" + y_coord;
                float z_coord = j.Position.Z;
                joint_str += ", \"z\":" + z_coord;
                joint_str += "},";
                jsonString += joint_str;
            }

            jsonString.Remove(jsonString.Count() - 1);
            jsonString += "]";
        }

        private static void closeJsonStringObject(string jsonString)
        {
            jsonString += "}";
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
