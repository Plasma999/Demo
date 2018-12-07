using System.Diagnostics;

namespace APIDemo.App
{
    public class EventLogger : ILogger
    {
        private readonly EventLogEntryType eventLogEntryType;

        public EventLogger()
        {
            eventLogEntryType = EventLogEntryType.Error;
        }

        public EventLogger(EventLogEntryType customType)
        {
            eventLogEntryType = customType;
        }

        public void Log(string msg)
        {
            EventLog.WriteEntry(Const.AP_ID, msg, eventLogEntryType);
        }
    }
}