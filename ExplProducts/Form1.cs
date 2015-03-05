using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExplProducts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region variables initialisation
        private string[] a_names;
        private double[] a_atoms;
        private double Qvv;
        private double Rovv;
        private double Dkr;
        private double Dvv;
        #endregion

        #region forms decimal input filtration
        private void textBoxC_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxC.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxH_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxH.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxN_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxN.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxO_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxO.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxCl_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxCl.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxF_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxF.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxK_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxK.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxCa_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxCa.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxNa_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxNa.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxAl_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxAl.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxP_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxP.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxSi_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxSi.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxQvv_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxQvv.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxMaxRo_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxMaxRo.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxRovv_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxRovv.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxDkr_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxDkr.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxDvv_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxDvv.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }
        #endregion
        
        private void buttonCalculation_Click(object sender, EventArgs e)
        {
            #region read data from form fields
            a_names =  new string[12] {"C", "H", "N", "O", "Cl", "F", 
                                       "K", "Ca", "Na", "Al", "P", "Si"};
            a_atoms = new double[12] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 
                                      0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
            double number = 0.0;
            // atoms parsing
            if (double.TryParse(textBoxC.Text, out number))
            {
                a_atoms[0] = number;
            }
            if (double.TryParse(textBoxH.Text, out number))
            {
                a_atoms[1] = number;
            }
            if (double.TryParse(textBoxN.Text, out number))
            {
                a_atoms[2] = number;
            }
            if (double.TryParse(textBoxO.Text, out number))
            {
                a_atoms[3] = number;
            }
            if (double.TryParse(textBoxCl.Text, out number))
            {
                a_atoms[4] = number;
            }
            if (double.TryParse(textBoxF.Text, out number))
            {
                a_atoms[5] = number;
            }
            if (double.TryParse(textBoxK.Text, out number))
            {
                a_atoms[6] = number;
            }
            if (double.TryParse(textBoxCa.Text, out number))
            {
                a_atoms[7] = number;
            }
            if (double.TryParse(textBoxNa.Text, out number))
            {
                a_atoms[8] = number;
            }
            if (double.TryParse(textBoxAl.Text, out number))
            {
                a_atoms[9] = number;
            }
            if (double.TryParse(textBoxP.Text, out number))
            {
                a_atoms[10] = number;
            }
            if (double.TryParse(textBoxSi.Text, out number))
            {
                a_atoms[11] = number;
            }
            // other params parsing
            if (double.TryParse(textBoxQvv.Text, out number))
            {
                Qvv = number;
            }
            if (double.TryParse(textBoxRovv.Text, out number))
            {
                Rovv = number;
            }
            if (double.TryParse(textBoxDkr.Text, out number))
            {
                Dkr = number;
            }
            if (double.TryParse(textBoxDvv.Text, out number))
            {
                Dvv = number;
            }
            #endregion


        }
    }
}
