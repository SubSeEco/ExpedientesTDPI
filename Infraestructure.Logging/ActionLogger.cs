using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using log4net.Config;

namespace Infrastructure.Logging
{
    public class ActionLogger
    {
        private ILog _log = null;

        public void LoadConfig(string nameLogger)
        {
            log4net.Config.XmlConfigurator.Configure();
            _log = log4net.LogManager.GetLogger(nameLogger);
        }


        public void Error(Exception ex)
        {
            _log.Error(ex.Message, ex);

        }


        public void Info(string message)
        {
            _log.Info(message);
        }
    }
}
