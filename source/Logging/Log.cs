using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Logging
{
    public class Log
    {
        public static TraceSwitch Switch { get; set; }

        static Log()
        {
            Switch = null;
        }

        public static void WriteLine(string message, TraceLevel level, string subsystem = null)
        {
            if (Switch == null)
            {
                return;
            }

            message = String.Format("[{0:dd-MM-yyyy hh:mm:ss:ffff}] {1}: {2}", DateTime.Now, level, message);
            
            if (String.IsNullOrEmpty(subsystem)) {
                Trace.WriteLineIf(level >= Switch.Level, message);
            }
            else {
                Trace.WriteLineIf(level >= Switch.Level, message, subsystem);
            }
        }

        [Conditional("TRACE")]
        public static void Verbose(string message)
        {
            WriteLine(message, TraceLevel.Verbose);
        }

        [Conditional("TRACE")]
        public static void Verbose(string message, string subsystem)
        {
            WriteLine(message, TraceLevel.Verbose, subsystem);
        }

        [Conditional("TRACE")]
        public static void Info(string message)
        {
            WriteLine(message, TraceLevel.Info);
        }

        [Conditional("TRACE")]
        public static void Info(string message, string subsystem)
        {
            WriteLine(message, TraceLevel.Info, subsystem);
        }

        [Conditional("TRACE")]
        public static void Warning(string message)
        {
            WriteLine(message, TraceLevel.Warning);
        }

        [Conditional("TRACE")]
        public static void Warning(string message, string subsystem)
        {
            WriteLine(message, TraceLevel.Warning, subsystem);
        }

        [Conditional("TRACE")]
        public static void Error(string message)
        {
            WriteLine(message, TraceLevel.Error);
        }

        [Conditional("TRACE")]
        public static void Error(string message, string subsystem)
        {
            WriteLine(message, TraceLevel.Error, subsystem);
        }

        [Conditional("TRACE")]
        public static void Error(Exception ex)
        {
            WriteLine(ex.ToString(), TraceLevel.Error);
        }

        [Conditional("TRACE")]
        public static void Error(Exception ex, string subsystem)
        {
            WriteLine(ex.ToString(), TraceLevel.Error, subsystem);
        }
    }
}   
