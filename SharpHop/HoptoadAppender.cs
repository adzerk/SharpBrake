using System;
using System.Collections.Generic;
using log4net.Appender;
using log4net.Core;

namespace SharpHop
{
    public class HoptoadAppender : BufferingAppenderSkeleton
    {
        private HoptoadClient _client;
        private string _apiKey;
        private string _environmentName;
        private string _appName;
        private string _appVersion;
        private string _appUrl;

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

        public HoptoadAppender()
        {
        }

        protected override bool IsAsSevereAsThreshold(Level level)
        {
            return ((level == Level.Fatal) ||
                    (level == Level.Error) ||
                    (level == Level.Alert) ||
                    (level == Level.Critical) ||
                    (level == Level.Emergency) ||
                    (level == Level.Severe));
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
            {
                return;
            }

            try
            {
                _client = new HoptoadClient(ApiKey);

                IList<HoptoadBackTrack> backTracks = new List<HoptoadBackTrack>
                                                         {
                                                             new HoptoadBackTrack(
                                                                 loggingEvent.LocationInformation.LineNumber,
                                                                 loggingEvent.LocationInformation.FileName,
                                                                 loggingEvent.LocationInformation.MethodName)
                                                         };

                HoptoadMessage message = new HoptoadMessage(
                    _appName,
                    _appVersion,
                    _appUrl,
                    loggingEvent.LocationInformation.ClassName,
                    RenderLoggingEvent(loggingEvent),
                    backTracks,
                    loggingEvent.Domain,
                    _environmentName);

                _client.Send(message);
            }
            catch (Exception exception)
            {
                ErrorHandler.Error("Error during sending hoptoad notification", exception);
            }
        }

        protected override void SendBuffer(LoggingEvent[] events)
        {
            if (events == null)
            {
                return;
            }

            foreach (LoggingEvent loggingEvent in events)
            {
                Append(loggingEvent);
            }
        }
    }
}