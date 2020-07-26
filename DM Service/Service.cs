using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace DM_Service.Models
{

    public class Service : PropertyChangedClass
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

        private TimeSpan shiftStart;
        public TimeSpan ShiftStart
        {
            get
            {
                return shiftStart;
            }
            private set
            {
                shiftStart = value;
                Changed(nameof(shiftStart));
            }
        }
        private TimeSpan shiftEnd;
        public TimeSpan ShiftEnd
        {
            get
            {
                return shiftEnd;
            }
            private set
            {
                shiftEnd = value;
                Changed(nameof(shiftEnd));
            }
        }
        private TimeSpan shiftDuration;
        public TimeSpan ShiftDuration
        {
            get
            {
                return shiftDuration;
            }
            private set
            {
                shiftDuration = value;
                Changed(nameof(value));
            }
        }

        private static ObservableCollection<ItemGroup> mainList;
        public static ObservableCollection<ItemGroup> MainList
        {
            get
            {
                return mainList;
            }
            private set
            {
                mainList = value;
                ChangedStatic(nameof(MainList));
            }
        }

        public void AddItem(Item item)
        {
            if ((MainList.Count > 0) && (MainList[MainList.Count - 1].Count > 0) && (MainList[MainList.Count - 1][MainList[MainList.Count - 1].Count - 1].Added.Date == DateTime.Today.Date))
            {
                if (Check(item, true))
                {
                    MainList[MainList.Count - 1].Insert(0, item);
                    MainList[MainList.Count - 1].UpDate();
                }
            }

            else
            {
                if  (Check(item, true))
                {
                    ItemGroup itemGroup = new ItemGroup();
                    itemGroup.Insert(0, item);
                    itemGroup.UpDate();
                    MainList.Add(itemGroup);
                }
            }
        }

        private bool Check(Item item, bool add)
        {
            if (item.Original is Pick)
            {
                Pick pick = item.Original as Pick;
                if (add)
                {
                    if (pick.CountPicksInList > 0)
                    {
                        pickManager.AddPick(pick);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    pickManager.RemovePick(pick);
                    return true;
                }
            }
            else if (item.Original is WorkShutDown)
            {
                WorkShutDown workShutDown = item.Original as WorkShutDown;
                if (add)
                {
                    pickManager.AddPick(new Pick(workShutDown.ShouldHavePicks), false);
                    return true;
                }
                else
                {
                    pickManager.RemovePick(new Pick(workShutDown.ShouldHavePicks), false);
                    return true;
                }
            }
            else if (item.Original is Pause)
            {
                Pause pause = (item.Original as Pause);
                if (add)
                {
                    pauseManager.AddPause(pause);
                    return true;
                }
                else
                {
                    pauseManager.RemovePause(pause);
                    return true;
                }
            }
            else
            {
                throw new Exception("bad item");
            }
        }

        private int norm;
        public int Norm
        {
            get
            {
                return norm;
            }
            private set
            {
                norm = value;
                Changed(nameof(Norm));
            }
        }

        private PauseManager pauseManager;
        public PauseManager PauseManager
        {
            get
            {
                return pauseManager;
            }
            private set
            {
                pauseManager = value;
                Changed(nameof(PauseManager));
            }
        }
        private PickManager pickManager;
        public PickManager PickManager
        {
            get
            {
                return pickManager;
            }
            private set
            {
                pickManager = value;
                Changed(nameof(PickManager));
            }
        }

        public TimeSpan MaxPause { get; private set; }
        public bool IsPause { get; set; } = false;
        private int shoudlHavePicks;
        public int ShouldHavePicks
        {
            get
            {
                if (!IsPause)
                {
                    TimeSpan freeTime;
                    if (PauseManager.PausesCount >= PauseManager.MaximumPauses)
                    {
                        freeTime = MaxPause;
                    }
                    else
                    {
                        freeTime = PauseManager.Duration;
                    }
                    shoudlHavePicks = (int)Math.Round(((Norm / (ShiftDuration - freeTime).TotalSeconds) * -(PauseManager.Duration + TimeRemaining - ShiftDuration).TotalSeconds), 0);
                }
                return shoudlHavePicks;
            }
        }

        public TimeSpan TimeRemaining
        {
            get
            {
                if (shiftStart == TimeSpan.Zero)
                {
                    return TimeSpan.Zero;
                }
                else
                {
                    return (ShiftDuration - (DateTime.Now.TimeOfDay - ShiftStart));
                }
            }
        }

        private TimeSpan morningStart;
        private TimeSpan morningEnd;
        private TimeSpan afternoonStart;
        private TimeSpan afternoonName;

        public static event PropertyChangedEventHandler PropertyChangedStatic;

        public Service()
        {
            morningStart = TimeSpan.FromHours(5.75);
            morningEnd = TimeSpan.FromHours(13.5);
            afternoonStart = TimeSpan.FromHours(14);
            afternoonName = TimeSpan.FromHours(21.75);
            MaxPause = TimeSpan.FromHours(0.5);
            Norm = 630;
            ShiftAllocation();

            PauseManager = new PauseManager();
            PickManager = new PickManager();
            MainList = new ObservableCollection<ItemGroup>();
            Changed(nameof(MainList));
        }

        public static void Edit(Item oldItem, Item newItem)
        {
            if (oldItem.Added.Date == DateTime.Now.Date && newItem.Added.Date == DateTime.Now.Date)
            {
                MainList[MainList.Count - 1][MainList[MainList.Count - 1].IndexOf(oldItem)] = newItem;
            }
        }

        public void Remove(Item item)
        {
            if (MainList[MainList.Count - 1].Contains(item))
            {
                if (Check(item, false))
                {
                    MainList[MainList.Count - 1].Remove(item);
                    if (MainList[MainList.Count - 1].Count == 0)
                    {
                        MainList.Remove(MainList[MainList.Count - 1]);
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("item does not exist");
            }
        }

        private void ShiftAllocation()
        {
            if ((DateTime.Now.TimeOfDay < morningEnd) && (DateTime.Now.TimeOfDay > morningStart))
            {
                ShiftStart = morningStart;
                ShiftEnd = morningEnd;
                ShiftName = "Morning";
            }
            else if ((DateTime.Now.TimeOfDay < afternoonName) && (DateTime.Now.TimeOfDay > afternoonStart))
            {
                ShiftStart = afternoonStart;
                ShiftEnd = afternoonName;
                ShiftName = "Afternoon";
            }
            else
            {
                ShiftDuration = TimeSpan.Zero;
                ShiftStart = TimeSpan.Zero;
                ShiftEnd = TimeSpan.Zero;
                norm = 0;
                ShiftName = "Free day";
            }//testov8n9
            //if (DateTime.Now.DayOfWeek = DayOfWeek.Saturday|| DateTime.Now.DayOfWeek = DayOfWeek.Sunday)
            //{
            //    ShiftDuration = TimeSpan.Zero;
            //    ShiftStart = TimeSpan.Zero;
            //    ShiftEnd = TimeSpan.Zero;
            //    norm = 0;
            //    ShiftName = "Weekend";
            //}
            ShiftDuration = ShiftEnd - ShiftStart;

        }

        protected static void ChangedStatic(string vlastnost)
        {
            if (PropertyChangedStatic != null)
                PropertyChangedStatic(MainList, new PropertyChangedEventArgs(vlastnost));
        }

        public Color Refresh()
        {
            ShiftAllocation();
            Color color;
            if (pickManager.TotalCount >= ShouldHavePicks)
            {
                color = Color.Green;
            }
            else
            {
                color = Color.Red;
            }
            Changed(nameof(pauseManager.PausesCount));
            Changed(nameof(pickManager.PalletCount));
            Changed(nameof(TimeRemaining));
            Changed(nameof(ShouldHavePicks));
            return color;
        }
    }
}
