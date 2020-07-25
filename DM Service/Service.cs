﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace DM_Service.Models
{

    public class Service : INotifyPropertyChanged
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

        public static void AddItem(Item item)
        {
            if ((MainList.Count > 0) && (MainList[MainList.Count - 1].Count > 0) && (MainList[MainList.Count - 1][MainList[MainList.Count - 1].Count - 1].Added.Date == DateTime.Today.Date))
            {
                MainList[MainList.Count - 1].Insert(0, item);
                MainList[MainList.Count - 1].UpDate();                   
            }

            else
            {
                ItemGroup itemGroup = new ItemGroup();
                itemGroup.Insert(0, item);
                itemGroup.UpDate();
                MainList.Add(itemGroup);
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

        public int ShouldHavePicks
        {
            get
            {
                return (int)Math.Round(((Norm / (ShiftDuration - PauseManager.Duration).TotalSeconds) * -(PauseManager.Duration + TimeRemaining - ShiftDuration).TotalSeconds), 0);
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

        public event PropertyChangedEventHandler PropertyChanged;
        public static event PropertyChangedEventHandler PropertyChangedStatic;

        public Service()
        {
            morningStart = TimeSpan.FromHours(5.75);
            morningEnd = TimeSpan.FromHours(13.5);
            afternoonStart = TimeSpan.FromHours(14);
            afternoonName = TimeSpan.FromHours(21.75);
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

        public static void Remove(Item item)
        {
            if (MainList[MainList.Count - 1].Contains(item))
            {
                MainList[MainList.Count - 1].Remove(item);
                if (MainList[MainList.Count - 1].Count == 0)
                {
                    MainList.Remove(MainList[MainList.Count - 1]);
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
            }
            ShiftDuration = ShiftEnd - ShiftStart;

        }

        protected void Changed(string vlastnost)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
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
