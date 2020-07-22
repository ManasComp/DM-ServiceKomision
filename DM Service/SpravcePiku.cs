using DM_Service.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace DM_Service
{
    public class SpravcePiku:INotifyPropertyChanged
    {
        public SpravcePiku()
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

        public int PaletCount
        {
            get
            {
                return Picks.Count;
            }
        }

        public void AddPick(Pick pick)
        {
            Picks.Add(pick);
            Service.MainList.Add(new Polozka(pick));
            TotalCount+=pick.CountPicksInList;
        }

        public void RemovePick(Pick pick)
        {
            if (Picks.Contains(pick))
            {
                Picks.Remove(pick);
                Service.MainList.Remove(new Polozka(pick));
            }

            else
            {
                throw new ArgumentNullException("pick neexistuje");
            }
        }

        public void EditPick(Pick OldPick, Pick NewPick)
        {
            if (Picks.Contains(OldPick))
            {
                Picks[Picks.IndexOf(OldPick)] = NewPick;
                Service.MainList[Service.MainList.IndexOf(new Polozka(OldPick))] = new Polozka(NewPick);
            }

            else
            {
                throw new ArgumentNullException("pick neexistuje");
            }
        }

        protected void Changed(string vlastnost)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
        }

    }
}
