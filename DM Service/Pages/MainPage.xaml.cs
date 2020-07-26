using DM_Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Diagnostics;

namespace DM_Service
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private Command _refreshViewCommand;
        private Command RefreshViewCommand
        {
            get
            {
                return _refreshViewCommand ?? (_refreshViewCommand = new Command(() =>
                {
                    refresh();
                    Refresh_RefreshView.IsRefreshing = false;
                }));
            }
        }
        Service service;
        public MainPage()
        {
            InitializeComponent();
            service = new Service();
            BindingContext = service;
            List_listView.ItemsSource = Service.MainList;
            Refresh_RefreshView.Command = RefreshViewCommand;
            seconds = 1;
            isPressed = true;
            pressed = true;
            pause = true;
            refresh();
        }

        private void Add_Butoon_Clicked(object sender, EventArgs e)
        {
            Trace.WriteLine("pressed");
            if (!string.IsNullOrEmpty(Input_Entry.Text))
            {
                service.AddItem(new Item(new Pick(int.Parse(Input_Entry.Text))));
                Input_Entry.Text = "";
                Progress_ProgressBar.Progress = ((double)service.PickManager.TotalCount / (double)service.Norm);
            }
            refresh();
            foreach (ItemGroup itemGroup in Service.MainList)
            {
                Trace.WriteLine(itemGroup);
                foreach (Item item in itemGroup)
                {
                    Trace.WriteLine(item);
                }
            }
            Trace.WriteLine("unpressed");
        }

        private void refresh()
        {
            PicksCount_Label.TextColor = service.Refresh();
            Should_ProgressBar.Progress = ((double)service.ShouldHavePicks / (double)service.Norm);
            if (service.PauseManager.PausesCount < service.PauseManager.MaximumPauses)
            {
                AddPause_Butoon.IsEnabled = true;
            }

            if (service.ShiftName == "Free")
            {
                //Add_Butoon.IsEnabled = false;
                //AddPause_Butoon.IsEnabled = false;
                Trace.WriteLine("odkomentuj mě");
            }

            else
            {
                Add_Butoon.IsEnabled = true;
                AddPause_Butoon.IsEnabled = true;
            }

            PalleteCount_Label.Text = service.PickManager.PalletCount.ToString();
            Pauza_Count.Text = service.PauseManager.PausesCount.ToString();
            PauseLasts_Label.Text = string.Format("{0}:{1}", (DateTime.Now - AddPausePressedStart).Minutes, (DateTime.Now - AddPausePressedStart).Seconds);
        }

        private void Delete_MenuItem_Clicked(object sender, EventArgs e)
        {
            service.Remove((sender as MenuItem).CommandParameter as Item);
        }

        private void Edit_MenuItem_Clicked(object sender, EventArgs e)
        {
            //add edit function
        }

        public event PropertyChangedEventHandler PropertyChanged1;
        protected void Changed(string property)
        {
            if (PropertyChanged1 != null)
                PropertyChanged1(this, new PropertyChangedEventArgs(property));
        }

        private void List_listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            List_listView.SelectedItem = null;
        }

        private DateTime AddPausePressedStart;

        private void AddPause_Butoon_Pressed(object sender, EventArgs e)
        {
            isPressed = true;
            if (pressed == true)
            {
                AddPausePressedStart = DateTime.Now;
                Task.Run(PressingAsync);
                pressed = false;
            }
        }

        private int seconds;
        private bool isPressed;

        private async void PressingAsync()
        {
            while (isPressed)
            {
                if ((AddPausePressedStart + TimeSpan.FromSeconds(seconds)) < DateTime.Now)
                {
                    Vibration.Vibrate();
                    isPressed = false;
                }
            }
        }

        private bool pressed;
        private bool pause;
        private void AddPause_Butoon_Released(object sender, EventArgs e)
        {
            isPressed = false;
            if (service.IsPause == false)
            {
                if ((AddPausePressedStart + TimeSpan.FromSeconds(seconds - 0.1)) > DateTime.Now)
                {
                    if (service.PauseManager.PausesCount >= service.PauseManager.MaximumPauses)
                    {
                        DisplayAlert("Error", "Too much pauses", "ok");
                        return;
                    }
                    else
                    {
                        AddPause_Butoon.Text = "Stop Pause";
                        pause = true;
                    }
                }
                else
                {
                    AddPause_Butoon.Text = "Stop ShutDown";
                    pause = false;
                }
                service.IsPause = true;
                Picks_StackLayout.IsVisible = false;
                PauseStart_Grid.IsVisible = true;
                PauseStart_Label.Text = AddPausePressedStart.ToShortTimeString();
                PauseLasts_Label.Text = string.Format("{0}:{1}", AddPausePressedStart.Minute, AddPausePressedStart.Second);
            }
            else
            {
                PauseStart_Grid.IsVisible = false;
                Picks_StackLayout.IsVisible = true;
                AddPause_Butoon.Text = "Add Pause";
                if (pause)
                {
                    service.AddItem((new Item(new Pause(AddPausePressedStart, DateTime.Now))));
                }
                else
                {
                    service.AddItem(new Item(new WorkShutDown(AddPausePressedStart, DateTime.Now, service)));
                }
                service.IsPause = false;
                pressed = true;
            }
            refresh();
        }
    }
}
