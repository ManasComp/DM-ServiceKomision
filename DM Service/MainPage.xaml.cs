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
        Service service;
        public MainPage()
        {
            InitializeComponent();
            service = new Service();
            BindingContext = service;
            pauseStart = DateTime.FromBinary(0);
            List_listView.ItemsSource = Service.MainList;
            refresh();
        }

        private void Add_Butoon_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Input_Entry.Text))
            {
                service.PickManager.AddPick(new Pick (int.Parse(Input_Entry.Text)));
                Input_Entry.Text = "";
                Progress_ProgressBar.Progress = ((double)service.PickManager.TotalCount / (double)service.Norm);
            }
            refresh();
        }

        private void Refresh_Butoon_Clicked(object sender, EventArgs e)
        {
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

            if(service.ShiftName=="Free day")
            {
                Add_Butoon.IsEnabled = false;
                AddPause_Butoon.IsEnabled = false;
            }

            else
            {
                Add_Butoon.IsEnabled = true;
                AddPause_Butoon.IsEnabled = true;
            }

        }

        private DateTime pauseStart;
        public string PauseStart
        {
            get
            {
                return pauseStart.ToShortTimeString();
            }
        }
        private void Pause_Button_Clicked(object sender, EventArgs e)
        {
            if(pauseStart == DateTime.FromBinary(0))
            {
                pauseStart = DateTime.Now;
                Add_Butoon.IsEnabled = false;
                Input_Entry.IsVisible = false;
                PauseStart_Label.IsVisible = true;
                AddPause_Butoon.Text = "Stop Pause";
            }
            else
            {
                service.PauseManager.AddPause(new Pause(pauseStart, DateTime.Now));
                pauseStart = DateTime.FromBinary(0);
                Add_Butoon.IsEnabled = true;
                PauseStart_Label.IsVisible = false;
                Input_Entry.IsVisible = true;
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
    }
}
