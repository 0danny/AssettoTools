using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssettoTools.Core.Helper
{
    public class Logger
    {
        public static void log(object message, [CallerMemberName] string callerMemberName = null)
        {
            Trace.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss tt")}][{Thread.CurrentThread.Name}][{callerMemberName}]: {message}");
        }
    }
}
