using System;
using System.Collections.Generic;
using NLog;
using NLog.Targets;


namespace SharpHop
{
	
	[Target("HoptoadAppender")] 
    public sealed class HoptoadAppender: TargetWithLayout
    {
        private HoptoadClient _client;
        private string _apiKey;
        private string _environmentName;
        private string _appName;
        private string _appVersion;
        private string _appUrl;
		private string _aggregatorUrl;

        public string ApiKey
        {
            get { return _apiKey; }
            set { _apiKey = value; }
        }

        public string EnvironmentName
        {
            get { return _environmentName; }
            set { _environmentName = value; }
        }

        public string AppName
        {
            get { return _appName; }
            set { _appName = value; }
        }

        public string AppVersion
        {
            get { return _appVersion; }
            set { _appVersion = value; }
        }

        public string AppUrl
        {
            get { return _appUrl; }
            set { _appUrl = value; }
        }
		
		public string AggregatorUrl
		{
			get { return _aggregatorUrl; }
			set { _aggregatorUrl = value; }
		}

        public string Host { get; set; }
		
        public HoptoadAppender()
        {
			this.Host = System.Net.Dns.GetHostName();
        }
		
		protected override void Write(LogEventInfo logEvent) 
        { 
			
			if (logEvent.Exception == null)
			{
				return;
			}

			_client = new HoptoadClient(ApiKey, AggregatorUrl);
			
            string logMessage = logEvent.Exception.ToString();
			
			string stackTrace = logEvent.Exception.StackTrace.ToString();
			string[] lines = stackTrace.Split('\n');

            IList<HoptoadBackTrack> backTracks = new List<HoptoadBackTrack>();
			foreach (var line in lines)
			{
				char[] delims = { ' ', ',', ':', '\t', '<','>' };
				Console.WriteLine("line: " + line);
				string[] words = line.Split(delims);
				string fileName = words[10];
				string lineNumber = words[12];
				string methodName = words[3];
				Console.WriteLine("file #: " + fileName);
				Console.WriteLine("line #: " + lineNumber);
				Console.WriteLine("method: " + methodName);
				//line
				//file
				//method
				backTracks.Add(new HoptoadBackTrack(lineNumber, fileName, methodName));
			}


            HoptoadMessage message = new HoptoadMessage(
                _appName,
                _appVersion,
                _appUrl,
                logEvent.Exception.GetType().ToString(),
                logMessage,
                backTracks,
                "",
                _environmentName);
			
			Console.WriteLine("Sending exception data to hoptoad url");
            _client.Send(message);
           

        }

    }
}