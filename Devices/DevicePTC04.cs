using System;
using System.IO.Ports;
using Modbus.Device;
using System.Threading;
using System.Timers;
using log4net;


namespace AUA.AiS_FruiT
{
    public class DevicePTC04
    {
        private static readonly ILog pLogger = LogHelper.GetLoggerRollingFileAppender("Root");
        private static readonly ILog eLogger = LogHelper.GetLoggerRollingFileAppender("Exception");


        public bool IsConnected { get; set; }

        public DevicePTC04()
        {
           
        }

        public void Connect(string portNumber)
        {
           
            try
            {
                //IsConnected = false;
            }
            catch (Exception ex)
            {
                IsConnected = false;
                eLogger.Error(ex.ToString());
            }

        }
    }

}