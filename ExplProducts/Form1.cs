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
        private string[] metal_names, oxy_names, chn_names, chnprod_names;
        private int[] metal_val, oxy_val;
        private double[] metal_atoms, oxy_atoms, chn_atoms, metal_atoms_new, oxy_atoms_new, chn_atoms_new, chnprod_moles;
        private string[,] prod_names;
        private double[,] prod_moles;
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
            metal_names = new string[] { "K", "Ca", "Na", "Al", "P", "Si" };
            metal_val = new int[] { 1, 2, 1, 3, 5, 4 };
            metal_atoms = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };

            chn_names = new string[] { "C", "H", "N" };
            chn_atoms = new double[] { 0.0, 0.0, 0.0};

            oxy_names = new string[] { "F", "Cl", "O" };
            oxy_val = new int[] { 1, 1, 2 };
            oxy_atoms = new double[] { 0.0, 0.0, 0.0 };

            prod_names = new string[,] { { "KF", "CaF2", "NaF", "AlF3", "PF5", "SiF4" }, 
                                         { "KCl", "CaCl2", "NaCl", "AlCl3", "PCl5", "SiCl4" }, 
                                         { "K2O", "CaO", "Na2O", "Al2O3", "P2O5", "SiO2" } };
            prod_moles = new double[,] { { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }, 
                                         { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }, 
                                         { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 } };

            chnprod_names = new string[] { "CO2", "CO", "C", "H2O", "H2", "O2", "N2" };
            chnprod_moles = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };

            #region read data from form fields
            double number = 0.0;
            // atoms parsing
            if (double.TryParse(textBoxC.Text, out number))
            {
                chn_atoms[0] = number;
            }
            if (double.TryParse(textBoxH.Text, out number))
            {
                chn_atoms[1] = number;
            }
            if (double.TryParse(textBoxN.Text, out number))
            {
                chn_atoms[2] = number;
            }
            if (double.TryParse(textBoxO.Text, out number))
            {
                oxy_atoms[2] = number;
            }
            if (double.TryParse(textBoxCl.Text, out number))
            {
                oxy_atoms[1] = number;
            }
            if (double.TryParse(textBoxF.Text, out number))
            {
                oxy_atoms[0] = number;
            }
            if (double.TryParse(textBoxK.Text, out number))
            {
                metal_atoms[0] = number;
            }
            if (double.TryParse(textBoxCa.Text, out number))
            {
                metal_atoms[1] = number;
            }
            if (double.TryParse(textBoxNa.Text, out number))
            {
                metal_atoms[2] = number;
            }
            if (double.TryParse(textBoxAl.Text, out number))
            {
                metal_atoms[3] = number;
            }
            if (double.TryParse(textBoxP.Text, out number))
            {
                metal_atoms[4] = number;
            }
            if (double.TryParse(textBoxSi.Text, out number))
            {
                metal_atoms[5] = number;
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
            
            #region first oxydation
                #region atoms copy
                metal_atoms_new = new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                for (int i = 0; i < metal_atoms_new.Length; i++)
                {
                    metal_atoms_new[i] = metal_atoms[i];
                }
                oxy_atoms_new = new double[3] { 0.0, 0.0, 0.0 };
                for (int i = 0; i < oxy_atoms_new.Length; i++)
                {
                    oxy_atoms_new[i] = oxy_atoms[i];
                }
                chn_atoms_new = new double[3] { 0.0, 0.0, 0.0 };
                for (int i = 0; i < chn_atoms_new.Length; i++)
                {
                    chn_atoms_new[i] = chn_atoms[i];
                }
                #endregion

                #region metals oxydation
                for (int i = 0; i < oxy_atoms.Length; i++)
                {
                    for (int j = 0; j < metal_atoms.Length; j++)
                    {
                        if (oxy_atoms_new[i] * oxy_val[i] >= metal_atoms_new[j] * metal_val[j])
                        {
                            prod_moles[i, j] = metal_atoms_new[j] * metal_val[j] / oxy_val[i];
                            oxy_atoms_new[i] -= metal_atoms_new[j] * metal_val[j] / oxy_val[i];
                            metal_atoms_new[j] = 0;
                        } else
                        {
                            prod_moles[i, j] = oxy_atoms_new[i];
                            oxy_atoms_new[i] = 0;
                            metal_atoms_new[j] -= oxy_atoms_new[i] * oxy_val[i] / metal_val[j];
                        }
                    }
                }
                #endregion

                #region CHN-oxydation
                // N -> N2
                chnprod_moles[6] = chn_atoms_new[2] / 2;
                chn_atoms_new[2] = 0;
                
                // CO
                if (chn_atoms_new[0] >= oxy_atoms_new[2]) // C >= O
                {
                    chnprod_moles[1] = oxy_atoms_new[2]; // _CO = O
                    chn_atoms_new[0] -= oxy_atoms_new[2]; // C = C - O
                    oxy_atoms_new[2] = 0; // O = 0
                    chnprod_moles[2] = chn_atoms_new[0]; // _C = C
                } else
                {
                    chnprod_moles[1] = chn_atoms_new[0]; // _CO = C
                    oxy_atoms_new[2] -= chn_atoms_new[0]; // O = O - C
                    chn_atoms_new[0] = 0; // C = 0
                }
                
                // H2O
                if (oxy_atoms_new[2] >= chn_atoms_new[1] / 2) // O >= H / 2
                {
                    chnprod_moles[3] = chn_atoms_new[1] / 2; // _H2O = H / 2
                    oxy_atoms_new[2] -= chn_atoms_new[1] / 2; // O = O - H / 2
                    chn_atoms_new[1] = 0; // H = 0
                } else
                {
                    chnprod_moles[3] = oxy_atoms_new[2]; // _H2O = O
                    chn_atoms_new[1] -= oxy_atoms_new[2] * 2; // H = H - 2 * O
                    oxy_atoms_new[2] = 0; // O = 0
                    chnprod_moles[4] = chn_atoms_new[1] / 2; // _H2 = H / 2;
                    chn_atoms_new[1] = 0; // H = 0
                }
                
                // CO2
                if (oxy_atoms_new[2] > chnprod_moles[1])
                {
                    chnprod_moles[0] = chnprod_moles[1]; // _CO2 = _CO
                    oxy_atoms_new[2] -= chnprod_moles[1]; // O = O - _CO
                    chnprod_moles[1] = 0; // _CO = 0
                } else
                {
                    chnprod_moles[0] = oxy_atoms_new[2]; // _CO2 = O
                    chnprod_moles[1] -= oxy_atoms_new[2]; // _CO = _CO - O
                    oxy_atoms_new[2] = 0; // O = 0
                }
                
                // O2
                chnprod_moles[5] = oxy_atoms_new[2] / 2; // _O2 = o / 2
                oxy_atoms_new[2] = 0; // O = 0
                #endregion

                #region first oxydation print
                string s = "";
                // atoms
                for (int i = 0; i < chn_names.Length; i++)
                    if (chn_atoms[i] > 0)
                        s += chn_names[i] + chn_atoms[i].ToString();
                for (int i = 0; i < oxy_names.Length; i++)
                    if (oxy_atoms[i] > 0)
                        s += oxy_names[i] + oxy_atoms[i].ToString();
                for (int i = 0; i < metal_names.Length; i++)
                    if (metal_atoms[i] > 0)
                        s += metal_names[i] + metal_atoms[i].ToString();
                if (s.Length > 0)
                    s += "==>";
                // products
                for (int i = 0; i < oxy_names.Length; i++)
                    for (int j = 0; j < metal_names.Length; j++)
                        if (prod_moles[i, j] > 0)
                            s += "+" + prod_moles[i, j].ToString() + prod_names[i, j];
                for (int i = 0; i < chnprod_moles.Length; i++)
                    if (chnprod_moles[i] > 0)
                        s += "+" + chnprod_moles[i].ToString() + chnprod_names[i];
                // out string
                s = s.Replace("==>+", "==>");
                outBox.Text = s;
                #endregion

            #endregion
        }
    }
}
