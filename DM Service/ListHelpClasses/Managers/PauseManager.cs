using DM_Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DM_Service
{
    public class PauseManager:PropertyChangedClass
    {
        public int MaximumPauses { get; private set; }
        private TimeSpan duration;
        public TimeSpan Duration
        {
            get
            {
                return duration;
            }
            private set
            {
                duration = value;
                Changed(nameof(Duration));
            }
        }

        public PauseManager()
        {
            Duration = TimeSpan.Zero;
            MaximumPauses = 2;
            PausesCount = 0;
        }

        private int pausesCount;
        public int PausesCount
        {
            get
            {
                return pausesCount;
            }
            private set
            {
                pausesCount = value;
                Changed(nameof(PausesCount));
            }
        }

        public void AddPause(Pause pause)
        {
            Duration += pause.PauseDuration;
            PausesCount += 1;
        }

        public void RemovePause(Pause pause)
        {
            PausesCount -= 1;
            Duration -= pause.PauseDuration;
        }

        public void EditPause(Pause OldPause, Pause NewPaus)
        {
            if (Service.MainList[Service.MainList.Count - 1].Contains(new Item(OldPause)))
            {
                Service.Edit(new Item(OldPause), new Item(NewPaus));
            }

            else
            {
                throw new ArgumentNullException("pause does not exist");
            }
        }
    }
}
