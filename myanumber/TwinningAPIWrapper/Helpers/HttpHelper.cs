using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TwinningAPIWrapper.Helpers
{
    internal class HttpHelper
    {
        public delegate void OnHttpSuccessCallback(string response);
        public delegate void OnHttpErrorCallback(string message);

        public static void HttpGet(string url, OnHttpSuccessCallback onSuccessCallback, OnHttpErrorCallback onErrorCallback)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "&no_cache=" + Guid.NewGuid().ToString());

            request.Method = "GET";

            request.BeginGetResponse((IAsyncResult result) =>
            {
                HttpWebRequest r = (HttpWebRequest)result.AsyncState;

                HttpWebResponse response = null;
                Stream streamResponse = null;
                StreamReader reader = null;

                try
                {
                    response = (HttpWebResponse)r.EndGetResponse(result);

                    streamResponse = response.GetResponseStream();
                    reader = new StreamReader(streamResponse);

                    onSuccessCallback(reader.ReadToEnd());
                }
                catch (Exception e)
                {
                    onErrorCallback(e.Message);
                }
                finally
                {
                    if (streamResponse != null)
                    {
                        streamResponse.Close();
                    }

                    if (reader != null)
                    {
                        reader.Close();
                    }

                    if (response != null)
                    {
                        response.Close();
                    }
                }

            }, request);

        }

        public static void HttpPost(string url, Dictionary<string, string> parameters, OnHttpSuccessCallback onSuccessCallback, OnHttpErrorCallback onErrorCallback)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "&no_cache=" + Guid.NewGuid().ToString());

            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            request.BeginGetRequestStream((IAsyncResult asyncResult) =>
            {
                HttpWebRequest req = (HttpWebRequest)asyncResult.AsyncState;

                Stream postStream = req.EndGetRequestStream(asyncResult);

                StringBuilder postParameters = new StringBuilder();

                if (parameters != null)
                {
                    foreach (String key in parameters.Keys)
                    {
                        postParameters.Append(String.Format("{0}={1}&", key, parameters[key]));
                    }
                }

                byte[] byteArray = Encoding.UTF8.GetBytes(postParameters.ToString());

                postStream.Write(byteArray, 0, byteArray.Length);
                postStream.Close();

                req.BeginGetResponse((IAsyncResult result) =>
                {
                    HttpWebRequest r = (HttpWebRequest)result.AsyncState;

                    HttpWebResponse response = null;
                    Stream streamResponse = null;
                    StreamReader reader = null;

                    try
                    {
                        response = (HttpWebResponse)r.EndGetResponse(result);

                        streamResponse = response.GetResponseStream();
                        reader = new StreamReader(streamResponse);

                        onSuccessCallback(reader.ReadToEnd());
                    }
                    catch (Exception e)
                    {
                        onErrorCallback(e.Message);
                    }
                    finally
                    {
                        if (streamResponse != null)
                        {
                            streamResponse.Close();
                        }

                        if (reader != null)
                        {
                            reader.Close();
                        }

                        if (response != null)
                        {
                            response.Close();
                        }
                    }

                }, req);

            }, request);
        }
    }
}
