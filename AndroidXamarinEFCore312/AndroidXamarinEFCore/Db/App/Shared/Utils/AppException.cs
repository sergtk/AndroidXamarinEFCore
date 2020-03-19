using System;
using System.Collections.Generic;
using System.Text;

namespace App.Shared.Utils
{
    /// <summary>
    /// This exception should be used by default when thrown exception in the app
    /// </summary>
    public class AppException : ApplicationException
    {
        public string MemberName { get; private set; }

        public string SourceFilePath { get; private set; }

        public int SourceLineNumber { get; private set; }

        public AppException(string message = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0) : base(message)
        {
            this.MemberName = memberName;
            this.SourceFilePath = sourceFilePath;
            this.SourceLineNumber = sourceLineNumber;
        }

        public AppException(
            System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0) : base(info, context)
        {
            this.MemberName = memberName;
            this.SourceFilePath = sourceFilePath;
            this.SourceLineNumber = sourceLineNumber;
        }

        public AppException(string message, Exception innerException,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0) : base(message, innerException)
        {
            this.MemberName = memberName;
            this.SourceFilePath = sourceFilePath;
            this.SourceLineNumber = sourceLineNumber;
        }
    }
}
