using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace App.Db.Remote
{
    public static class RemoteDbUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        [Conditional("DEBUG")]
        public static void DebugWriteSqlParameters(List<Microsoft.Data.SqlClient.SqlParameter> parameters,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Debug.WriteLine($"Caller member name: {memberName}\n");
            Debug.WriteLine($"Caller file path: {sourceFilePath}\n");
            Debug.WriteLine($"Caller line number: {sourceLineNumber}\n");
            Debug.WriteLine($"SQL parameters:");
            for (int i = 0; i < parameters.Count; i++)
            {
                Microsoft.Data.SqlClient.SqlParameter param = parameters[i];
                string val = param.Value == null ? "null" : param.Value.ToString();
                string str = $"{i+1}. {param.ParameterName}: '{val}' ({param.Value.GetType()})";
                Debug.WriteLine(str);
            }
            Debug.WriteLine("");
        }

    }
}
