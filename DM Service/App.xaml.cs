﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DM_Service
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Calculation();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
