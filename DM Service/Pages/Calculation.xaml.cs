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

        Operation operation;

        List<string> previousText = new List<string>();

        bool validity = false;

        private void Button_Clicked(object sender, EventArgs e)
        {
            {
                string objStr = ((Button)sender).Text.ToString();

                if (objStr == (string)RovnaSe_Button.Text)
                {
                    operation = new Operation(Displej_TextBox.Text);
                    Displej_TextBox.Text = operation.Result.ToString();
                    BindingContext = operation;
                    validity = true;
                }

                else if ((objStr == (string)CE_Button.Text))
                {
                    if (previousText.Count() > 0)
                    {
                        Displej_TextBox.Text = previousText[previousText.Count() - 1];
                        previousText.RemoveAt(previousText.Count() - 1);
                        if (validity == true)
                        {
                            operation.Save = "";
                            validity = false;
                        }
                    }
                }

                else if ((objStr == (string)C_Button.Text))
                {
                    Displej_TextBox.Text = "0";
                    previousText = new List<string>();
                    if (validity == true)
                    {
                        operation.Save = "";
                        validity = false;
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