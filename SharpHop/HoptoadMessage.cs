using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpHop
{
    public class HoptoadMessage
    {
        private string _appName;
        private string _appVersion;
        private string _appUrl;

        private string _errorClass;
        private string _errorMessage;
        private IEnumerable<HoptoadBackTrack> _backTracks;
        private string _errorDomain;

        private string _environmentName;

        public HoptoadMessage(
            string appName, 
            string appVersion, 
            string appUrl, 
            string errorClass, 
            string errorMessage, 
            IEnumerable<HoptoadBackTrack> backTracks, 
            string errorDomain, 
            string environmentName)
        {
            _appName = appName;
            _appVersion = appVersion;
            _appUrl = appUrl;
            _errorClass = errorClass;
            _errorMessage = errorMessage;
            _backTracks = backTracks;
            _errorDomain = errorDomain;
            _environmentName = environmentName;
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

        public string ErrorClass
        {
            get { return _errorClass; }
            set { _errorClass = value; }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        public IEnumerable<HoptoadBackTrack> BackTracks
        {
            get { return _backTracks; }
            set { _backTracks = value; }
        }

        public string ErrorDomain
        {
            get { return _errorDomain; }
            set { _errorDomain = value; }
        }

        public string EnvironmentName
        {
            get { return _environmentName; }
            set { _environmentName = value; }
        }
    }

    public class HoptoadBackTrack
    {
        private string _lineNumber;
        private string _fileName;
        private string _methodName;

        public HoptoadBackTrack(string lineNumber, string fileName, string methodName)
        {
            _lineNumber = lineNumber;
            _fileName = fileName;
            _methodName = methodName;
        }

        public string LineNumber
        {
            get { return _lineNumber; }
            set { _lineNumber = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string MethodName
        {
            get { return _methodName; }
            set { _methodName = value; }
        }
    }
}