using System;
using System.Collections.Generic;
using System.Text;

namespace DM_Service.Models
{
    public class Paus
    {
        public Paus(DateTime zacatekPauźy, DateTime konecPauzy)
        {
            if (zacatekPauźy < konecPauzy)
            {
                ZacatekPauźy = zacatekPauźy;
                KonecPauzy = konecPauzy;
            }
            else
            {
                throw new ArgumentNullException("špatný zadání pauzy");
            }
        }

        public DateTime ZacatekPauźy { get; private set; }
        public DateTime KonecPauzy { get; private set; }
        public TimeSpan PauzaDuration 
        {
            get
            {
                    return KonecPauzy - ZacatekPauźy;
            }
        }
    }
}
