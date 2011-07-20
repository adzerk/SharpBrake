using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System;

namespace SharpHop
{
    public class HoptoadClient
    {
        private readonly string _apiKey;

        public HoptoadClient(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException("apiKey");
            }

            _apiKey = apiKey;
        }

        public void Send(HoptoadMessage message)
        {
            notice notice = new notice
                                {
                                    version = "2.0",
                                    apikey = _apiKey,
                                    notifier = new notifier
                                                   {
                                                       name = message.AppName,
                                                       version = message.AppVersion,
                                                       url = message.AppUrl
                                                   },
                                    error = new error
                                                {
                                                    @class = message.ErrorClass,
                                                    message = message.ErrorMessage
                                                }
                                };

            IList<backtraceLine> backtraceLines = new List<backtraceLine>();
            foreach (HoptoadBackTrack backTrack in message.BackTracks)
            {
                backtraceLines.Add(new backtraceLine()
                                       {
                                           file = backTrack.FileName,
                                           method = backTrack.MethodName,
                                           number = backTrack.LineNumber
                                       });
            }

            notice.error.backtrace = backtraceLines.ToArray();

            notice.serverenvironment = new serverEnvironment { environmentname = message.EnvironmentName };

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(notice));
                serializer.Serialize(sw, notice);
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://hoptoadapp.com/notifier_api/v2/notices");
            request.Method = "POST";

            byte[] encodedData = new ASCIIEncoding().GetBytes(sb.ToString());
            request.ContentType = "text/xml";
            request.ContentLength = encodedData.Length;

            using (Stream newStream = request.GetRequestStream())
            {
                newStream.Write(encodedData, 0, encodedData.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HoptoadException(response.StatusCode.ToString());
            }
        }
    }
}