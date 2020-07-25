using DM_Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DM_Service
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
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
            refresh();
        }

        private void Add_Butoon_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Input_Entry.Text))
            {
                service.PickManager.AddPick(new Pick(int.Parse(Input_Entry.Text)));
                Input_Entry.Text = "";
                Progress_ProgressBar.Progress = ((double)service.PickManager.TotalCount / (double)service.Norm);
            }
            refresh();
        }

        private void refresh()
        {
            PicksCount_Label.TextColor = service.Refresh();
            Should_ProgressBar.Progress = ((double)service.ShouldHavePicks / (double)service.Norm);
            if (service.PauseManager.PausesCount < service.PauseManager.MaximumPauses)
            {
                AddPause_Butoon.IsEnabled = true;
            }

            if (service.ShiftName == "Free day")
            {
                Add_Butoon.IsEnabled = false;
                AddPause_Butoon.IsEnabled = false;
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
            System.Diagnostics.Trace.WriteLine(sender);
            Service.Remove((sender as MenuItem).CommandParameter as Item);
        }

        private void Edit_MenuItem_Clicked(object sender, EventArgs e)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged2;
        protected void Changed(string property)
        {
            if (PropertyChanged2 != null)
                PropertyChanged2(this, new PropertyChangedEventArgs(property));
        }

        private void List_listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            List_listView.SelectedItem = null;
        }

        private DateTime AddPausePressedStart;

        private void AddPause_Butoon_Pressed(object sender, EventArgs e)
        {
            if (pressed == true)
            {
                AddPausePressedStart = DateTime.Now;
                System.Diagnostics.Trace.WriteLine("pressed");
            }       
        }

        private bool pressed = true;
        private bool pause = true;
        private void AddPause_Butoon_Released(object sender, EventArgs e)
        {
            if (pressed == true)
            {
                Picks_StackLayout.IsVisible = false;
                PauseStart_Grid.IsVisible = true;
                if ((AddPausePressedStart + TimeSpan.FromMilliseconds(500)) > DateTime.Now)
                {
                    AddPause_Butoon.Text = "Stop Pause";
                    pause = true;
                }
                else
                {
                    AddPause_Butoon.Text = "Stop ShutDown";
                    pause = false;
                }
                PauseStart_Label.Text = AddPausePressedStart.ToShortTimeString();
                PauseLasts_Label.Text = string.Format("{0}:{1}", AddPausePressedStart.Minute, AddPausePressedStart.Second);
                pressed = false;
            }
            else
            {
                PauseStart_Grid.IsVisible = false;
                Picks_StackLayout.IsVisible = true;
                AddPause_Butoon.Text = "Add Pause";
                if (pause)
                {
                    if (service.PauseManager.PausesCount >= service.PauseManager.MaximumPauses)
                    {
                        DisplayAlert("Error", "Too much pauses", "ok");
                    }
                    else
                    {
                        service.PauseManager.AddPause(new Pause(AddPausePressedStart, DateTime.Now));
                    }
                }
                else
                {
                    DisplayAlert("pressed", "pressed", "ok");
                    Service.AddItem(new Item(new WorkShutDown(AddPausePressedStart, DateTime.Now, service)));                    
                }
                pressed = true;
            }
            refresh();
        }
    }
}
