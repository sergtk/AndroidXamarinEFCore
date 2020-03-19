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
            string replaceMsg = GetExceptionMessageReplacement(ex.Message);
            if (replaceMsg != ex.Message)
            {
                sb.Append($" [{replaceMsg}]");
            }
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
            if (ex is AppException)
            {
                AppException aimEx = ex as AppException;
                memberName = aimEx.MemberName;
                sourceFilePath = aimEx.SourceFilePath;
                sourceLineNumber = aimEx.SourceLineNumber;
            }
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

        /// <summary>
        /// It returns error message, mainly skipping useless `AggregateException`.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="extractInner"></param>
        /// <returns></returns>
        private static List<string> ExtractMeaningfulErrorMessages(Exception ex, bool extractInner = false)
        {
            List<string> ret = new List<string>();
            if (ex == null)
            {
                return ret;
            }

            if (ex is AggregateException)
            {
                AggregateException ex1 = ex as AggregateException;
                foreach (Exception exInner in ex1.InnerExceptions)
                {
                    List<string> ret1 = ExtractMeaningfulErrorMessages(exInner, extractInner);
                    ret.AddRange(ret1);
                }
            } else
            {
                ret.Add(ex.Message);
                if (extractInner)
                {
                    List<string> ret1 = ExtractMeaningfulErrorMessages(ex.InnerException, extractInner);
                    ret.AddRange(ret1);
                }
            }
            return ret;
        }


        private static readonly Lazy<IDictionary<string, Tuple<int, string>>> exceptionMessageReplacements_ = new Lazy<IDictionary<string, Tuple<int, string>>>(() =>
            new Dictionary<string, Tuple<int, string>>
            {
            }
        );


        /// <summary>
        /// </summary>
        /// <param name="srcMsg"></param>
        /// <returns></returns>
        private static string GetExceptionMessageReplacement(string srcMsg)
        {
            string cur = srcMsg;

            // Strip duplicated message
            // Ref: https://stackoverflow.com/questions/3707951/sqlexception-message-duplicated-when-calling-sqlserver-stored-proc
            string msgStripped = cur.Substring(0, (cur.Length - 1) / 2);
            if ($"{msgStripped}\n{msgStripped}" == cur)
            {
                cur = msgStripped;
            }
            msgStripped = cur.Substring(0, (cur.Length - 2) / 3);
            if ($"{msgStripped}\n{msgStripped}\n{msgStripped}" == cur)
            {
                cur = msgStripped;
            }

            Tuple<int, string> dst;
            bool replace = exceptionMessageReplacements_.Value.TryGetValue(cur, out dst);
            if (replace)
            {
                cur = $"{dst.Item2} (UI code: {dst.Item1})";
            }
            return cur;
        }


        /// <summary>
        /// Compose useful error message from exception passed
        /// </summary>
        /// <param name="ex"></param>
        public static string ComposeErrorMessage(Exception ex)
        {
            const int MaxResultLength = 600;

            List<string> messages = ExtractMeaningfulErrorMessages(ex, true);
            if (messages.Count == 0)
            {
                return string.Empty;
            }

            for (int mi = 0; mi < messages.Count; mi++)
            {
                messages[mi] = GetExceptionMessageReplacement(messages[mi]);
            }

            StringBuilder sb = new StringBuilder();

            if (messages.Count > 1)
            {
                sb.AppendLine("Several errors occur:");
                foreach (string msg in messages)
                {
                    sb.AppendLine($"* {msg}");
                    if (sb.Length >= MaxResultLength)
                    {
                        break;
                    }
                }
            } else
            {
                Debug.Assert(messages.Count == 1);
                sb.Append(messages[0]);
            }

            if (sb.Length >= MaxResultLength)
            {
                sb.Length = MaxResultLength - 3;
                sb.Append("...");
            }
            string ret = sb.ToString();
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstName"></param>
        /// <param name="lst"></param>
        /// <param name="indexShift"></param>
        public static void WriteList<T>(string lstName, IEnumerable<T> lst, int indexShift = 0)
        {
            Console.WriteLine(lstName);
            int i = 0;
            foreach (T item in lst)
            {
                Console.WriteLine($"{i + indexShift}. {item}");
                i++;
            }
            if (i == 0)
            {
                Console.WriteLine("None.");
            }
            //Console.WriteLine();
        }
    }
}
