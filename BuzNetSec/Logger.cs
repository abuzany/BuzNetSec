using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BuzNetSec
{
    /// <summary>
    /// This class provide a method to write in EventLogger.
    /// </summary>
    /// <remarks>
    /// Ange Buzany.
    /// </remarks>
    public class Logger
    {
        private static LoggerMode _mode = LoggerMode.WithoutLog;

        public static LoggerMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        /// <summary>
        /// Write log in Windows event viewer or Console.
        /// </summary>
        /// <param name="type">
        /// Type of message Information, Debug or Error.
        /// </param>
        /// <param name="msg">
        /// Message for write.
        /// </param>
        /// <returns>
        /// The method returns an integer.
        /// </returns>
        public static void WriteLog(LoggerType type, string msg)
        {
            try
            {
                EventLog myLog = new EventLog();

                if (!EventLog.SourceExists("BuzNetSec"))
                {
                    EventLog.CreateEventSource("BuzNetSec", "AVRS");
                }

                switch(type)
                {
                    case LoggerType.ERROR:

                        if (_mode == LoggerMode.EventViewer)
                        {
                            myLog.Source = "BuzNetSec";
                            myLog.WriteEntry(msg, EventLogEntryType.Error);
                        }
                        else if(_mode == LoggerMode.Console)
                        {
                            Console.WriteLine(msg);
                        }

                        break;

                    case LoggerType.DEBUG:

                        if (_mode == LoggerMode.EventViewer)
                        {
                            myLog.Source = "BuzNetSec";
                            myLog.WriteEntry(msg, EventLogEntryType.Information);
                        }                        
                        else if(_mode == LoggerMode.Console)
                        {
                            Console.WriteLine(msg);
                        }
                        
                        break;

                    case LoggerType.INFO:

                        if (_mode == LoggerMode.EventViewer)
                        {
                            myLog.Source = "BuzNetSec";
                            myLog.WriteEntry(msg, EventLogEntryType.Information);
                        }
                        else if (_mode == LoggerMode.Console)
                        {
                            Console.WriteLine(msg);
                        }
                            
                        break;

                }
            }
            catch(Exception e)
            {

            }
        }//End method WriteLog

    }//End class Logger

    public enum LoggerType
    {
        ERROR = 0,
        DEBUG = 1,
        INFO = 2,
    }//End enum LogType

    public enum LoggerMode
    {
        EventViewer = 0,
        Console = 1,
        WithoutLog = -1
    }//End enum LogMode

}
