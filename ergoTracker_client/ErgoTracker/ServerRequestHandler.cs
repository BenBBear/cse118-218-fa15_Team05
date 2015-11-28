using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ErgoTracker
{
    class ServerRequestHandler
    {
        string url = "http://server.com"; // TODO: need to edit.
        string result = null;
            
        public void postKinectData(string jsonData)
        {
            result = null;
            url += "/post/kinect_data";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamwriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamwriter.Write(jsonData);
                streamwriter.Flush();
                streamwriter.Close();
            }

            httpWebRequest.BeginGetResponse(new AsyncCallback(GetKinectDataResponseCallback), httpWebRequest);
        }

        private void GetKinectDataResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                using (HttpWebResponse response = (HttpWebResponse)myRequest.EndGetResponse(asynchronousResult))
                {
                    Stream responseStream = response.GetResponseStream();
                    using (var reader = new StreamReader(responseStream))
                    {
                        result = reader.ReadToEnd();
                    }
                    responseStream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void getDiagnosticData(string jsonData)
        {
            result = null;
            url += "/get/diagnostic_data";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            using (var streamwriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamwriter.Write(jsonData);
                streamwriter.Flush();
                streamwriter.Close();
            }

            httpWebRequest.BeginGetResponse(new AsyncCallback(GetDiagnosticDataCallback), httpWebRequest);
        }

        private void GetDiagnosticDataCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                using (HttpWebResponse response = (HttpWebResponse)myRequest.EndGetResponse(asynchronousResult))
                {
                    Stream responseStream = response.GetResponseStream();
                    using (var reader = new StreamReader(responseStream))
                    {
                        result = reader.ReadToEnd();
                    }
                    responseStream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void getTodaysScore(string jsonData)
        {

        }
    }
}
