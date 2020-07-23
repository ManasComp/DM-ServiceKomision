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
            Pauses = new List<Pause>();
            Duration = TimeSpan.Zero;
        }

        private List<Pause> Pauses;

        public int PausesCount
        {
            get
            {
                return Pauses.Count;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddPause(Pause pause)
        {
            Pauses.Add(pause);
            Service.AddItem(new Item(pause));
            Duration += pause.PauseDuration;
        }

        public void RemovePause(Pause pause)
        {
            if (Pauses.Contains(pause))
            {
                Pauses.Remove(pause);
                Service.Remove(new Item(pause));
            }
            else
            {
                throw new ArgumentNullException("pause does not exist");
            }
        }

        public void EditPause(Pause OldPause, Pause NewPaus)
        {
            if (Pauses.Contains(OldPause))
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
