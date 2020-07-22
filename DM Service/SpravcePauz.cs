using DM_Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DM_Service
{
    public class SpravcePauz:INotifyPropertyChanged
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

        public SpravcePauz()
        {
            Paus = new List<Paus>();
            Duration = TimeSpan.FromTicks(0);
        }

        private List<Paus> Paus;

        public int PauzyCount
        {
            get
            {
                return Paus.Count;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddBreak(Paus pauza)
        {
            Paus.Add(pauza);
            Service.MainList.Add(new Polozka(pauza));
            Duration += pauza.PauzaDuration;
        }

        public void RemovePaus(Paus pauza)
        {
            if (Paus.Contains(pauza))
            {
                Paus.Remove(pauza);
                Service.MainList.Remove(new Polozka(pauza));
            }

            else
            {
                throw new ArgumentNullException("pauza neexistuje");
            }
        }

        public void EditPic(Paus OldPause, Paus NewPaus)
        {
            if (Paus.Contains(OldPause))
            {
                Paus[Paus.IndexOf(OldPause)] = NewPaus;
                Service.MainList[Service.MainList.IndexOf(new Polozka(OldPause))] = new Polozka(NewPaus);
            }

            else
            {
                throw new ArgumentNullException("pauza neexistuje");
            }
        }

        protected void Changed(string vlastnost)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
        }
    }
}
