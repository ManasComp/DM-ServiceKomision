using System;
using System.Collections.Generic;
using System.Text;

namespace DM_Service.Models
{
    public class Pick
    {
        public DateTime PickListSave { get; private set; }
        public int CountPicksInList { get; private set; }


        public Pick(int countPicksInList)
        {
            if (countPicksInList != 0)
            {
                CountPicksInList = countPicksInList;
                PickListSave = DateTime.Now;
            }
            else
            {
                throw new ArgumentNullException("0 piků");
            }
        }
    }
}
