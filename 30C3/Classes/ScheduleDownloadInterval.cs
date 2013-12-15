using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _30C3
{
    /// <summary>
    /// Class which defines all possible schedule download intervals
    /// </summary>
    public class ScheduleDownloadInterval
    {
        public static Dictionary<string, TimeSpan> Options
        {
            get
            {
                return new Dictionary<string, TimeSpan>()
                {
                    {"On Startup", new TimeSpan(0)},
                    {"Every hour", new TimeSpan(1,0,0)},
                    {"Every 6 hours", new TimeSpan(6,0,0)},
                    {"Every 12 hours", new TimeSpan(12,0,0)},
                    {"Every day", new TimeSpan(24,0,0)}
                };
            }
        }
    }
}
