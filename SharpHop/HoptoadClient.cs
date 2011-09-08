using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System;
using System.Xml;

namespace SharpHop
{
	
	public class StringWriterWithEncoding : StringWriter
    {
        private Encoding _encoding;
        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
            : base(sb)
        {
            _encoding = encoding;
        }
        public override Encoding Encoding
        {
            get
            {
                return _encoding;
            }
        }
    }
	
    public class HoptoadClient
    {
        private readonly string _apiKey;
		private readonly string _aggregatorUrl;

        public HoptoadClient(string apiKey, string aggregatorUrl)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException("apiKey");
            }
			
			if (string.IsNullOrEmpty(aggregatorUrl))
            {
                throw new ArgumentNullException("apiKey");
            }

            _apiKey = apiKey;
			_aggregatorUrl = aggregatorUrl;
			
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
			
            using (StringWriter sw = new StringWriterWithEncoding(sb, Encoding.UTF8))
            {

				Console.WriteLine(sw.Encoding.EncodingName.ToString());
				
                XmlSerializer serializer = new XmlSerializer(typeof(notice));
					
                serializer.Serialize(sw, notice);
            }
			
			Console.WriteLine("This is the document: " + sb.ToString());

   //         HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://hoptoadapp.com/notifier_api/v2/notices");
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_aggregatorUrl);
            request.Method = "POST";
			request.ContentType = "text/xml; encoding='utf-8'";
            byte[] encodedData = new UTF8Encoding().GetBytes(sb.ToString());
            
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
