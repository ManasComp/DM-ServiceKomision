using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace DM_Service.Models
{
    public class PolozkaGroup : ObservableCollection<Polozka>, INotifyPropertyChanged
    {
        public int PikuZaDen
        {
            get
            {
                int piky = 0;
                foreach (Polozka polozka in this)
                {
                    if (polozka.Objekt is Pick)
                    {
                        piky += (polozka.Objekt as Pick).CountPicksInList;
                    }
                }
                return piky;
            }
        }

        public string Nazev
        {
            get
            {
                return DateTime.Today.ToString("dddd dd. MM. yyyy");
            }
        }

        public void Užpdate()
        {
            Changed(nameof(PikuZaDen));
            Changed(nameof(Nazev));
        }

        public event PropertyChangedEventHandler PropertyChanged1;

        protected void Changed(string vlastnost)
        {
            if (PropertyChanged1 != null)
                PropertyChanged1(this, new PropertyChangedEventArgs(vlastnost));
        }
    }
}
