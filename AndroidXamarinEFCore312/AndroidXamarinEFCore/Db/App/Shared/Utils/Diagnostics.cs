using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

using System.Buffers;


namespace App.Shared.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class Diagnostics
    {
        private static void DoExceptionToStringExtended(StringBuilder sb, Exception ex)
        {
            // Check support of .Net Standard 2.1 and/or .Net Core 3.0 by System.Buffers
            // System.Buffers.SequenceReader<byte> sr = new SequenceReader<byte>(new ReadOnlySequence<byte>(new byte[] { }));

            if (ex == null)
            {
                return;
            }
            sb.AppendLine("------");
            sb.AppendLine($"{ex.GetType().FullName}");

            sb.Append($"Error message: ${ex.Message}");
            sb.AppendLine();

            sb.AppendLine($"Stack trace:\n{ex.StackTrace}");

            AggregateException aggEx = ex as AggregateException;
            if (aggEx != null)
            {
                for (int i = 0; i < aggEx.InnerExceptions.Count; i++)
                {
                    sb.AppendLine($"------ Inner of aggregate exception {i + 1} of {aggEx.InnerExceptions.Count} start");
                    DoExceptionToStringExtended(sb, aggEx.InnerExceptions[i]);
                    sb.AppendLine($"------ Inner of aggregate exception {i + 1} of {aggEx.InnerExceptions.Count} end");
                }
            }
            else
            {
                DoExceptionToStringExtended(sb, ex.InnerException);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string ExceptionToStringExtended(Exception ex,
            bool? isHandled = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("======");

            // Add caller info
            // Ref: https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute?view=netframework-4.7.2
            sb.AppendLine($"Caller member name: {memberName}");
            sb.AppendLine($"Caller file path: {sourceFilePath}");
            sb.AppendLine($"Caller line number: {sourceLineNumber}");
            sb.AppendLine($"Report generation time: {DateTimeOffset.Now}");
            if (isHandled != null)
            {
                sb.AppendLine($"Exception handled: {isHandled.Value}");
            }

            DoExceptionToStringExtended(sb, ex);

            sb.AppendLine("======");
            string ret = sb.ToString();
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        //[Conditional("DEBUG")]
        public static void DebugWriteLineExtended(Exception ex, bool? isHandled = null,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string msg = $"Error:\n{ExceptionToStringExtended(ex, isHandled, memberName, sourceFilePath, sourceLineNumber)}";
            Debug.WriteLine(msg);
            Console.WriteLine(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string ExceptionToStringMessages(Exception ex)
        {
            string ret = "";
            for (Exception ex1 = ex; ex1 != null; ret += $"{ex1.Message}\n", ex1 = ex1.InnerException) ;
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        //[Conditional("DEBUG")]
        public static void DebugWriteLine(params object[] args)
        {
            string str = string.Join(string.Empty, args.Select(arg => arg.ToString()).ToArray());
            Debug.WriteLine(str);
            Console.WriteLine(str);
        }

    }
}
