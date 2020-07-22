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
            zacatekPauzy = DateTime.FromBinary(0);
            List_listView.ItemsSource = Service.MainList;
            refresh();
        }

        private void Add_Butoon_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Input_Entry.Text))
            {
                service.SpravcePiku.AddPick(new Pick (int.Parse(Input_Entry.Text)));
                Input_Entry.Text = "";
                Progress_ProgressBar.Progress = ((double)service.SpravcePiku.TotalCount / (double)service.Norma);
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
            Should_ProgressBar.Progress = ((double)service.ShouldHavePicks / (double)service.Norma);
        }

        public DateTime zacatekPauzy { get; private set; }
        private void Pause_Button_Clicked(object sender, EventArgs e)
        {
            if(zacatekPauzy == DateTime.FromBinary(0))
            {
                zacatekPauzy = DateTime.Now;
                Add_Butoon.IsEnabled = false;
                Input_Entry.IsVisible = false;
            }
            else
            {
                service.SpravcePauz.AddBreak(new Paus(zacatekPauzy, DateTime.Now));
                zacatekPauzy = DateTime.FromBinary(0);
                Add_Butoon.IsEnabled = true;
                Input_Entry.IsVisible = true;
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
