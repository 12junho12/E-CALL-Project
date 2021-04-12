using System.Text;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace AUA.AiS_FruiT
{

    /// <summary>
    /// https://stackoverflow.com/questions/38616642/log4net-not-working-when-running-wpf-applications-executable
    /// https://stackoverflow.com/questions/308436/log4net-programmatically-specify-multiple-loggers-with-multiple-file-appenders
    /// </summary>
    public static class LogHelper
    {
        static LogHelper()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }

        public static ILog GetLoggerRollingFileAppender(string logName)
        {
            var log = LogManager.Exists(logName);

            if (log != null) return log;

            var appenderName = $"{logName}Appender";
            log = LogManager.GetLogger(logName);

            if (logName.Contains("Exception"))
            {
                SetLevel(logName, "All");
                ((Logger)log.Logger).AddAppender(ExceptionRollingFileAppender(appenderName));
            }
            else if (logName.Contains("Result"))
            {
                SetLevel(logName, "All");
                ((Logger)log.Logger).AddAppender(ResultRollingFileAppender(appenderName));
            }
            else
            {
                SetLevel(logName, "All");
                ((Logger)log.Logger).AddAppender(RootRollingFileAppender(appenderName));
            }


            return log;
        }
        private static void SetLevel(string logName, string levelName)
        {
            ILog log = LogManager.GetLogger(logName);
            Logger l = (Logger)log.Logger;

            l.Level = l.Hierarchy.LevelMap[levelName];
        }
        private static RollingFileAppender RootRollingFileAppender(string appenderName)
        {
            var layout = new PatternLayout { ConversionPattern = "%date [%level] - %message [%type.%method]%newline" };
            layout.ActivateOptions();

            var appender = new RollingFileAppender
            {
                Name = appenderName,
                DatePattern = "yyyy-MM-dd'_Normal.log'",
                File = @"Log\",
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Date,
                MaxSizeRollBackups = 2,
                MaximumFileSize = "1000KB",
                Layout = layout,
                ImmediateFlush = true,
                StaticLogFileName = false,
                LockingModel = new FileAppender.MinimalLock(),
                Encoding = Encoding.UTF8,
            };

            appender.ActivateOptions();

            return appender;
        }
        private static RollingFileAppender ExceptionRollingFileAppender(string appenderName)
        {
            var layout = new PatternLayout { ConversionPattern = "%date [%level] - %message [%type.%method]%newline" };
            layout.ActivateOptions();

            var appender = new RollingFileAppender
            {
                Name = appenderName,
                DatePattern = "yyyy-MM-dd'_Exception.log'",
                File = @"Log\",
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Date,
                MaxSizeRollBackups = 2,
                MaximumFileSize = "1000KB",
                Layout = layout,
                ImmediateFlush = true,
                StaticLogFileName = false,
                LockingModel = new FileAppender.MinimalLock(),
                Encoding = Encoding.UTF8,
            };

            appender.ActivateOptions();

            return appender;
        }
        private static RollingFileAppender ResultRollingFileAppender(string appenderName)
        {
            var layout = new PatternLayout { ConversionPattern = "%date - %message%newline" };
            layout.ActivateOptions();

            var appender = new RollingFileAppender
            {
                Name = appenderName,
                DatePattern = "yyyy-MM-dd'_Result.log'",
                File = @"Log\",
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Date,
                MaxSizeRollBackups = 2,
                MaximumFileSize = "1000KB",
                Layout = layout,
                ImmediateFlush = true,
                StaticLogFileName = false,
                LockingModel = new FileAppender.MinimalLock(),
                Encoding = Encoding.UTF8,
            };

            appender.ActivateOptions();

            return appender;
        }
    }
}
