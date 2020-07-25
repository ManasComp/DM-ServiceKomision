using DM_Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DM_Service
{
    public class PauseManager:INotifyPropertyChanged
    {
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
            PausesCount =0;
        }

        private int pausesCount;
        public int PausesCount
        {
            get
            {
                return pausesCount;
            }
            set
            {
                pausesCount = value;
                Changed(nameof(PausesCount));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddPause(Pause pause)
        {
            Service.AddItem(new Item(pause));
            Duration += pause.PauseDuration;
            PausesCount += 1;
        }

        //public void RemovePause(Pause pause)
        //{
        //    if (Service.MainList[Service.MainList.Count - 1].Contains(new Item(pause)))
        //    {
        //        Service.Remove(new Item(pause));
        //        PausesCount -= 1;
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException("pause does not exist");
        //    }
        //}

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

        protected void Changed(string vlastnost)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
        }
    }
}
