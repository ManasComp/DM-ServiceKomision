﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DM_Service.Models
{
    public class WorkShutDown : Pause
    {
        Service Service;
        public WorkShutDown(DateTime startPause, DateTime endPause, Service service) : base(startPause, endPause)
        {
            Service = service;
        }

        public int ShouldHavePicks
        {
            get
            {
                return (int)Math.Round(((Service.Norm / (Service.ShiftDuration - Service.MaxPause).TotalSeconds) * (PauseDuration).TotalSeconds), 0);
            }
        }
    }
}
