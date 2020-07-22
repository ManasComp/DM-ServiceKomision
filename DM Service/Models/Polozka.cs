using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml.Internals;

namespace DM_Service.Models
{
    public class Polozka:INotifyPropertyChanged
    {
        private string nazev;
        public string Nazev
        {
            get
            {
                return nazev;
            }
            private set
            {
                nazev = value;
                Changed(nameof(Nazev));
            }
        }
        private string cas;
        public string Cas
        {
            get
            {
                return cas;
            }
            private set
            {
                cas = value;
                Changed(nameof(cas));
            }
        }
        private string specifikace;
        public string Specifikace
        {
            get
            {
                return specifikace;
            }
            private set
            {
                specifikace = value;
                Changed(nameof(Specifikace));
            }
        }

        public Polozka(Pick pick)
        {
            Nazev = "Pick";
            Cas = pick.PickListSave.ToString("HH:mm");
            Specifikace = pick.CountPicksInList.ToString();
            Objekt = pick as object;
        }

        public Polozka(Paus pauza)
        {
            Nazev = "Pauza";
            Cas = string.Format("{0} - {1}", pauza.ZacatekPauźy.ToString("HH:mm:ss"), pauza.KonecPauzy.ToString("HH:mm:ss"));
            Specifikace = string.Format("{0}:{1}",pauza.PauzaDuration.Minutes, pauza.PauzaDuration.Seconds);
            Objekt = pauza as object;
        }

        private object objekt;
        public object Objekt
        {
            get
            {
                return objekt;
            }
            private set
            {
                objekt = value;
                Changed(nameof(Objekt));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Changed(string vlastnost)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
        }
    }
}
