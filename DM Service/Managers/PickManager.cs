using DM_Service.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace DM_Service
{
    public class PickManager:PropertyChangedClass
    {
        public PickManager()
        {
            TotalCount = 0;
            PalletCount = 0;
        }

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

        private int palletCount;
        public int PalletCount
        {
            get
            {
                return palletCount;
            }
            set
            {
                palletCount = value;
                Changed(nameof(PalletCount));
            }
        }

        public void AddPick(Pick pick)
        {
            TotalCount += pick.CountPicksInList;
            PalletCount += 1;
        }

        public void RemovePick(Pick pick)
        {
            PalletCount -= 1;
            TotalCount -= pick.CountPicksInList;
        }

        public void EditPick(Pick OldPick, Pick NewPick)
        {
            if (Service.MainList[Service.MainList.Count - 1].Contains(new Item(OldPick)))
            {
                Service.Edit(new Item(OldPick), new Item(NewPick));
            }

            else
            {
                throw new ArgumentNullException("pick does not exist");
            }
        }
    }
}
