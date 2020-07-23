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
            pauseStart = DateTime.FromBinary(0);
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
            //if (Service.MainList.Count > 0)
            //{
            //    foreach (Item item in Service.MainList[0])
            //    {
            //        System.Diagnostics.Trace.WriteLine(item.Name);
            //    }
            //    System.Diagnostics.Trace.WriteLine("***************");
            //}

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
            PauseLasts_Label.Text = string.Format("{0}:{1}", (DateTime.Now - pauseStart).Minutes, (DateTime.Now - pauseStart).Seconds);
        }

        private DateTime pauseStart;
        private void Pause_Button_Clicked(object sender, EventArgs e)
        {
            if (pauseStart == DateTime.FromBinary(0))
            {
                pauseStart = DateTime.Now;
                Picks_StackLayout.IsVisible = false;
                PauseStart_Grid.IsVisible = true;
                AddPause_Butoon.Text = "Stop Pause";
                PauseStart_Label.Text = pauseStart.ToShortTimeString();
                PauseLasts_Label.Text = string.Format("{0}:{1}", (DateTime.Now - pauseStart).Minutes, (DateTime.Now - pauseStart).Seconds);
            }
            else
            {
                service.PauseManager.AddPause(new Pause(pauseStart, DateTime.Now));
                pauseStart = DateTime.FromBinary(0);
                PauseStart_Grid.IsVisible = false;
                Picks_StackLayout.IsVisible = true;
                AddPause_Butoon.Text = "Add Pause";
            }
            refresh();
        }

        private void Delete_MenuItem_Clicked(object sender, EventArgs e)
        {

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
    }
}
