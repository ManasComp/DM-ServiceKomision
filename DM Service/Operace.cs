using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DM_Service
{
    class Operace : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

    private Ukony ukony = new Ukony();

    public double Vysledek
    {
        get
        {
            return Vypocet();
        }
    }

    private string input;
    public Operace(string text)
    {
        input = text.Trim().ToLower();
    }

    private readonly string[] separators = { "+", "-", "*", "/" };
    private readonly string[] separatorsPrednost = { "*", "/" };

    private string ulozeni = "";
    public string Ulozeni
    {
        get
        {
            return ulozeni;
        }
        set
        {
            ulozeni = value;
            VyvolejZmenu(nameof(Ulozeni));
        }
    }

    private List<double> cisla;
    private List<string> operatory;
    private List<string> cisla_s;

    private List<string> operatory_def;
    private List<string> cisla_s_def;

    private void Vypis()
    {
        Trace.Write(cisla[0]);
        for (int i = 0; i < operatory.Count(); i++)
        {
            Trace.Write(operatory[i] + cisla[i + 1]);
        }
        Trace.WriteLine("");
    }

    public string UlozeniString()
    {
        Ulozeni += (cisla[0]).ToString();
        Trace.Write(cisla[0]);
        for (int i = 0; i < operatory.Count(); i++)
        {
            Trace.Write(operatory[i] + cisla[i + 1]);
            Ulozeni += String.Format(operatory[i] + cisla[i + 1]);
        }
        Trace.WriteLine("");
        return Ulozeni;
    }

    private double Vypocet()
    {
        Trace.WriteLine("Výpočet začal");
        double vysledek = 0;
        ParsovaniCisla();
        HledaniOperatoru();
        Trace.WriteLine("Zadání: " + input);
        OperantyPoSobe();
        if (cisla.Count() != operatory.Count() + 1)
        {
            throw new ArgumentException("Zadali jste blbost");
        }
        UlozeniString();
        Prednost();
        if (operatory.Count() > 0)
        {
            for (int i = 0; i <= operatory.Count(); i++)
            {
                i = 0;
                Kalkulace(i);
            }
        }
        vysledek = cisla[0];
        Trace.WriteLine("Výpočet skončil");
        Trace.WriteLine("**************");
        return vysledek;
    }

    private void VypisOperatoru()
    {
        for (int a = 0; a < operatory.Count(); a++)
        {
            Trace.Write(operatory[a] + " ");
        }
        Trace.WriteLine("");
        if (!(cisla_s == cisla_s_def && operatory == operatory_def))
        {
            Ulozeni = "ÚPRAVA: ";
        }
    }

    private void OperantyPoSobe()
    {
        Trace.WriteLine("OperantyPoSobe začal");
        while (input.StartsWith("+") || input.StartsWith("-") || input.StartsWith("*") || input.StartsWith("/"))
        {
            if (input.StartsWith("-") && cisla.Count() > 0)
            {
                cisla[0] = -1 * cisla[0];
            }
            operatory.RemoveAt(0);
            input = input.Remove(0, 1);
            cisla_s.RemoveAt(0);
            VypisOperatoru();
        }

        while (input.EndsWith("+") || input.EndsWith("-") || input.EndsWith("*") || input.EndsWith("/"))
        {
            operatory.RemoveAt(operatory.Count() - 1);
            input = input.Remove(input.Count() - 1, 1);
            cisla_s.RemoveAt(cisla_s.Count() - 1);
            VypisOperatoru();
        }

        for (int i = 0; i < operatory.Count(); i++)
        {
            if (cisla_s[i] == "")
            {
                if (operatory[i] == "+" || operatory[i] == "-")
                {
                    if ((operatory[i] == "-") && (cisla.Count() > 0))
                    {
                        cisla[i] = cisla[i] * -1;
                    }
                }
                operatory.RemoveAt(i);
                cisla_s.RemoveAt(i);
                VypisOperatoru();

                if (!(i >= operatory.Count()))
                {
                    i = 0;
                }
            }
        }
        Trace.WriteLine("OperantyPoSobe skončil");
    }

    private void ParsovaniCisla()
    {
        Trace.WriteLine("ParsovaniCisla začalo");
        cisla_s = new List<string>(input.Split(separators, StringSplitOptions.None));
        cisla_s_def = new List<string>(cisla_s);
        cisla = new List<double>();
        foreach (string cislo_s in cisla_s)
        {
            double cislo;
            if (!double.TryParse(cislo_s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("cs"), out cislo))
            {
                if (cislo_s != "")
                {
                    throw new ArgumentException("Nezadali jste číslo");
                }
            }
            else
            {
                cisla.Add(cislo);
            }
        }
        Trace.WriteLine("ParsovaniCisla skončilo");
    }

    private void HledaniOperatoru()
    {
        Trace.WriteLine("HledaniOperatoru začalo");
        operatory = new List<string>();

        for (int i = 0; i < input.Count(); i++)
        {
            if (separators.Contains((input[i]).ToString()))
            {
                operatory.Add(input[i].ToString());
            }
        }
        operatory_def = new List<string>(operatory);
        Trace.WriteLine("HledaniOperatoru skončilo");
    }

    private void Prednost()
    {
        Trace.WriteLine("Přednost začala");
        for (int i = 0; i < operatory.Count(); i++)
        {
            if (separatorsPrednost.Contains(operatory[i]))
            {
                Kalkulace(i);
                i = 0;
            }
        }
        Trace.WriteLine("Přednost skončila");
    }

    private void Kalkulace(int i)
    {
        double mezivysledek = ukony.Rozhodovani((double)cisla[i], (double)cisla[i + 1], operatory[i]);
        cisla[i] = mezivysledek;
        if (operatory.Count > 1)
        {
            cisla.RemoveAt(i + 1);
            operatory.RemoveAt(i);
            Vypis();
        }
        else
        {
            operatory.RemoveAt(i);
            Trace.WriteLine(cisla[i]);
        }
    }

    private void VyvolejZmenu(string vlastnost)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
        }
    }
}
}
