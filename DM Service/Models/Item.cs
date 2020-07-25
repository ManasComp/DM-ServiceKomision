using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml.Internals;

namespace DM_Service.Models
{
    public class Item:PropertyChangedClass
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                name = value;
                Changed(nameof(Name));
            }
        }

        private DateTime added;
        public DateTime Added
        {
            get
            {
                return added;
            }
            private set
            {
                added = value;
                Changed(nameof(Added));
            }
        }
        private string duration;
        public string Duration
        {
            get
            {
                return duration;
            }
            private set
            {
                duration = value;
                Changed(nameof(duration));
            }
        }
        private string specification;
        public string Specification
        {
            get
            {
                return specification;
            }
            private set
            {
                specification = value;
                Changed(nameof(Specification));
            }
        }

        public Item(Pick pick)
        {
            Name = "Pick";
            Duration = pick.PickListSave.ToShortTimeString();
            Specification = pick.CountPicksInList.ToString();
            Original = pick as object;
            Added = DateTime.Now;
        }

        public Item(Pause pause)
        {
            Name = "Pause";
            Duration = string.Format("{0} - {1}", pause.StartPause.ToLongTimeString(), pause.EndPause.ToLongTimeString());
            Specification = string.Format("{0}:{1}", pause.PauseDuration.Minutes, pause.PauseDuration.Seconds);
            Original = pause as object;
            Added = DateTime.Now;
        }

        private object original;
        public object Original
        {
            get
            {
                return original;
            }
            private set
            {
                original = value;
                Changed(nameof(Original));
            }
        }
    }
}
