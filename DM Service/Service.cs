using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Security;
using System.Text;
using System.Xml.Schema;

namespace DM_Service.Models
{

    public class Service : INotifyPropertyChanged
    {
        private string nazevSmeny;
        public string NazevSmeny
        {
            get
            {
                return nazevSmeny;
            }
            private set
            {
                nazevSmeny = value;
                Changed(nameof(NazevSmeny));
            }
        }

        private TimeSpan zacatekSmeny;
        public TimeSpan ZacatekSmeny
        {
            get
            {
                return zacatekSmeny;
            }
            private set
            {
                zacatekSmeny = value;
                Changed(nameof(zacatekSmeny));
            }
        }
        private TimeSpan konecSmeny;
        public TimeSpan KonecSmeny
        {
            get
            {
                return konecSmeny;
            }
            private set
            {
                konecSmeny = value;
                Changed(nameof(konecSmeny));
            }
        }
        private TimeSpan smena;
        public TimeSpan Smena
        {
            get
            {
                return smena;
            }
            private set
            {
                smena = value;
                Changed(nameof(value));
            }
        }

        public static ObservableCollection<PolozkaGroup> Kolekce;
        private static ObservableCollection<Polozka> mainList;
        public static ObservableCollection<Polozka> MainList
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

        private int norma;
        public int Norma
        {
            get
            {
                return norma;
            }
            private set
            {
                norma = value;
                Changed(nameof(Norma));
            }
        }

        private SpravcePauz spravcePauz;
        public SpravcePauz SpravcePauz
        {
            get
            {
                return spravcePauz;
            }
            private set
            {
                spravcePauz = value;
                Changed(nameof(SpravcePauz));
            }
        }
        private SpravcePiku spravcePiku;
        public SpravcePiku SpravcePiku
        {
            get
            {
                return spravcePiku;
            }
            private set
            {
                spravcePiku = value;
                Changed(nameof(SpravcePiku));
            }
        }

        public int ShouldHavePicks
        {
            get
            {
                return (int)Math.Round(((Norma / (Smena - SpravcePauz.Duration).TotalSeconds) * -(SpravcePauz.Duration + TimeRemaining - Smena).TotalSeconds), 0);
            }
        }

        public TimeSpan TimeRemaining
        {
            get
            {
                return (Smena - (DateTime.Now.TimeOfDay - ZacatekSmeny));
            }
        }

        private TimeSpan RanniStart;
        private TimeSpan RanniKonec;
        private TimeSpan OdpoledniStart;
        private TimeSpan OdpoledniKonec;

        public event PropertyChangedEventHandler PropertyChanged;
        public static event PropertyChangedEventHandler PropertyChangedStatic;

        public Service()
        {
            RanniStart = TimeSpan.FromHours(5.75);
            RanniKonec = TimeSpan.FromHours(13.5);
            OdpoledniStart = TimeSpan.FromHours(14);
            OdpoledniKonec = TimeSpan.FromHours(21.75);
            Norma = 630;
            Cas();

            SpravcePauz = new SpravcePauz();
            SpravcePiku = new SpravcePiku();
            MainList = new ObservableCollection<Polozka>();
            Changed(nameof(MainList));
        }

        private void Cas()
        {
            if ((DateTime.Now.TimeOfDay < RanniKonec) && (DateTime.Now.TimeOfDay > RanniStart))
            {
                ZacatekSmeny = RanniStart;
                KonecSmeny = RanniKonec;
                NazevSmeny = "Ranní";
            }
            else if ((DateTime.Now.TimeOfDay < OdpoledniKonec) && (DateTime.Now.TimeOfDay > OdpoledniStart))
            {
                ZacatekSmeny = OdpoledniStart;
                KonecSmeny = OdpoledniKonec;
                NazevSmeny = "Odpolední";
            }
            else
            {
                Smena = TimeSpan.FromTicks(0);
                NazevSmeny = "je volno";
            }
            Smena = KonecSmeny - ZacatekSmeny;

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
            Color color;
            if (spravcePiku.TotalCount >= ShouldHavePicks)
            {
                color = Color.Green;
            }
            else
            {
                color = Color.Red;
            }
            Changed(nameof(spravcePauz.PauzyCount));
            Changed(nameof(spravcePiku.PaletCount));
            System.Diagnostics.Trace.WriteLine(spravcePiku.PaletCount);
            Changed(nameof(TimeRemaining));
            Changed(nameof(ShouldHavePicks));
            return color;
        }
    }
}
