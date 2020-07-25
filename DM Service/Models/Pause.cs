using System;
using System.Collections.Generic;
using System.Text;

namespace DM_Service.Models
{
    public class Pause
    {
        public Pause(DateTime startPause, DateTime endPause)
        {
            if (startPause < endPause)
            {
                StartPause = startPause;
                EndPause = endPause;
            }
            else
            {
                throw new ArgumentNullException("wrong pause or WorkShutDown initialize");
            }
        }

        public DateTime StartPause { get; private set; }
        public DateTime EndPause { get; private set; }
        public TimeSpan PauseDuration
        {
            get
            {
                return EndPause - StartPause;
            }
        }
    }
}
