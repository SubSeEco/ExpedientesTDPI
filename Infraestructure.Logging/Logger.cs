using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using log4net.Config;

namespace Infrastructure.Logging
{
    public static class Logger
    {
        private static ActionLogger actionLogger = null;
        public static void Configure(string LoggerName)
        {
            actionLogger = new ActionLogger();
            actionLogger.LoadConfig(LoggerName);
        }


        public static ActionLogger Execute()
        {
            return actionLogger;
        }
    }
}
