using DM_Service.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace DM_Service
{
    public class PickManager:INotifyPropertyChanged
    {
        public PickManager()
        {
            Picks = new List<Pick>();
            TotalCount = 0;
        }

        private List<Pick> Picks;

        public event PropertyChangedEventHandler PropertyChanged;

        private int totalCount;
        public int TotalCount
        {
            get
            {
                return totalCount;
            }
            private set
            {
                totalCount = value;
                Changed(nameof(TotalCount));
            }
        }

        public int PalletCount
        {
            get
            {
                return Picks.Count;
            }
        }

        public void AddPick(Pick pick)
        {
            Picks.Add(pick);
            Service.AddItem(new Item(pick));
            TotalCount += pick.CountPicksInList;
        }

        public void RemovePick(Pick pick)
        {
            if (Picks.Contains(pick))
            {
                Picks.Remove(pick);
                Service.Remove(new Item(pick));
            }
            else
            {
                throw new ArgumentNullException("pick does not exist");
            }
        }

        public void EditPick(Pick OldPick, Pick NewPick)
        {
            if (Picks.Contains(OldPick))
            {
                Service.Edit(new Item(OldPick), new Item(NewPick));
            }

            else
            {
                throw new ArgumentNullException("pick does not exist");
            }
        }

        protected void Changed(string vlastnost)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
        }
    }
}
