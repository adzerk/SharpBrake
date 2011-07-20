using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SharpHop
{
    [Serializable]
    public class HoptoadException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public HoptoadException()
        {
        }

        public HoptoadException(string message) : base(message)
        {
        }

        public HoptoadException(string message, Exception inner) : base(message, inner)
        {
        }

        protected HoptoadException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}