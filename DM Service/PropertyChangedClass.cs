using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DM_Service
{
    public abstract class PropertyChangedClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Changed(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
