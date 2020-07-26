using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace DM_Service.Models
{
    public class ItemGroup : ObservableCollection<Item>, INotifyPropertyChanged
    {
        private string shiftName;
        public string ShiftName
        {
            get
            {
                return shiftName;
            }
            private set
            {
                shiftName = value;
                Changed(nameof(ShiftName));
            }
        }
        public ItemGroup(string shiftName)
        {
            ShiftName = shiftName;
            this.CollectionChanged += Check;
            piksInDay = 0;
        }

        public void Check(object sender, NotifyCollectionChangedEventArgs e)
        {
            int piks = 0;
            foreach (Item item in this)
            {
                if (item.Original is Pick)
                {
                    piks += (item.Original as Pick).CountPicksInList;
                }
                //Service service = new Service();
                if (piks >= 630)
                {
                    PicksColor = Color.Green;
                }
                else
                {
                    PicksColor = Color.Red;
                }
            }
            Trace.WriteLine(PiksInDay);
            PiksInDay = piks;
            Trace.WriteLine(PiksInDay);
        }

        private Color picksColor;
        public Color PicksColor 
        {
            get
            {
                return picksColor;
            }
            private set
            {
                picksColor = value;
                Changed(nameof(PicksColor));
            }
        }

        private int piksInDay;
        public int PiksInDay
        {
            get
            {
                return piksInDay;
            }
            private set
            {
                Trace.WriteLine("zmena");
                piksInDay = value;
                Changed(nameof(PiksInDay));
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
            Check(null, null);
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
