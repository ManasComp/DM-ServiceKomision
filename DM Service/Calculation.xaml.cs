using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DM_Service
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Calculation : ContentPage
    {
        public Calculation()
        {
            InitializeComponent();
        }

        Operace operace;

        List<string> previousText = new List<string>();

        bool overeni = false;

        private void Button_Clicked(object sender, EventArgs e)
        {
            {
                string objStr = ((Button)sender).Text.ToString();

                if (objStr == (string)RovnaSe_Button.Text)
                {
                    operace = new Operace(Displej_TextBox.Text);
                    Displej_TextBox.Text = operace.Vysledek.ToString();
                    BindingContext = operace;
                    overeni = true;
                }

                else if ((objStr == (string)CE_Button.Text))
                {
                    if (previousText.Count() > 0)
                    {
                        Displej_TextBox.Text = previousText[previousText.Count() - 1];
                        previousText.RemoveAt(previousText.Count() - 1);
                        if (overeni == true)
                        {
                            operace.Ulozeni = "";
                            overeni = false;
                        }
                    }
                }

                else if ((objStr == (string)C_Button.Text))
                {
                    Displej_TextBox.Text = "0";
                    previousText = new List<string>();
                    if (overeni == true)
                    {
                        operace.Ulozeni = "";
                        overeni = false;
                    }
                }

                else
                {
                    previousText.Add(Displej_TextBox.Text);
                    if (Displej_TextBox.Text == "0")
                    {
                        Displej_TextBox.Text = "";
                    }
                    Displej_TextBox.Text += objStr;
                }
            }
        }
    }
}