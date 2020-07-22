using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace DM_Service.Models
{
    public class ItemGroup : ObservableCollection<Item>, INotifyPropertyChanged
    {
        public int PiksInDay
        {
            get
            {
                int piks = 0;
                foreach (Item item in this)
                {
                    if (item.Original is Pick)
                    {
                        piks += (item.Original as Pick).CountPicksInList;
                    }
                }
                return piks;
            }
        }

        public string Name
        {
            get
            {
                return DateTime.Today.ToString("dddd dd. MM. yyyy");
            }
        }

        public void UpDate()
        {
            Changed(nameof(PiksInDay));
            Changed(nameof(Name));
        }

        public event PropertyChangedEventHandler PropertyChanged1;

        protected void Changed(string property)
        {
            if (PropertyChanged1 != null)
                PropertyChanged1(this, new PropertyChangedEventArgs(property));
        }
    }
}
