using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ExplProducts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            loadData();
        }

        #region variables initialisation
        private string[] metal_names, oxy_names, chn_names, chnprod_names;
        private int[] metal_val, oxy_val;
        private double[] metal_atoms, oxy_atoms, chn_atoms, metal_atoms_new, oxy_atoms_new, chn_atoms_new, chnprod_moles;
        private double[] metal_mass, chn_mass, oxy_mass, chnprod_mass,chnprod_qi;
        private double[,] prod_mass,prod_qi;
        private string[,] prod_names;
        private double[,] prod_moles;

        Dictionary<double, double>[] chnprod_cp;
        Dictionary<double, double>[,] prod_cp;

        private double Qvv;
        private double Rovv;
        private double Dkr;
        private double Dvv;
        private double Mvv;

        private bool alldata = true;
        private string missing = "";
            
        #endregion

        private void loadData()
        {
            #region variables first init
            metal_names = new string[] { "K", "Ca", "Na", "Al", "P", "Si" };
            metal_val = new int[] { 1, 2, 1, 3, 5, 4 };
            metal_atoms = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            metal_mass = new double[] { 39.0983, 40.08, 22.98977, 26.98154, 30.97376, 28.086 };

            chnprod_cp = new Dictionary<double, double>[7];
            for (int i = 0; i < 7; i++ )
                chnprod_cp[i] = new Dictionary<double, double>();
            prod_cp = new Dictionary<double, double>[3,6];
            for (int i = 0; i < 3; i++ )
                for (int j = 0; j < 6; j++)
                    prod_cp[i,j] = new Dictionary<double, double>();
            
            chn_names = new string[] { "C", "H", "N" };
            chn_atoms = new double[] { 0.0, 0.0, 0.0 };
            chn_mass = new double[] { 12.0108, 1.00795, 14.0067 };

            oxy_names = new string[] { "F", "Cl", "O" };
            oxy_val = new int[] { 1, 1, 2 };
            oxy_atoms = new double[] { 0.0, 0.0, 0.0 };
            oxy_mass = new double[] { 18.9984, 35.453, 15.9994 };

            prod_names = new string[,] { { "KF", "CaF2", "NaF", "AlF3", "PF5", "SiF4" }, 
                                         { "KCl", "CaCl2", "NaCl", "AlCl3", "PCl5", "SiCl4" }, 
                                         { "K2O", "CaO", "Na2O", "Al2O3", "P2O5", "SiO2" } };
            prod_moles = new double[,] { { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }, 
                                         { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }, 
                                         { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 } };
            prod_mass =  new double[,] { { 58.0967, 78.0768, 41.98817, 83.97674, 125.96576, 104.0796 },
                                         { 74.5513, 110.986, 58.44277, 133.34054, 208.23876, 169.898 },
                                         { 94.196, 56.0794, 61.97894, 101.96128, 141.94452, 60.0848} };
            prod_qi =    new double[,] { { 569.9, 1228.0, 576.6, 1510.4, 1593, 1614.9 }, 
                                         { 437.02, 796.18, 410.4, 584.1, 366.9, 687.8 }, 
                                         { 363.6, 635.85, 418.4, 1677.5, 0.0, 910.7 } };

            chnprod_names = new string[] { "CO2", "CO", "C", "H2O", "H2", "O2", "N2" };
            chnprod_moles = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            chnprod_mass =  new double[] { 44.0096, 28.0102, 12.0108, 18.0153, 2.0159, 31.9988, 28.0134 };
            chnprod_qi =    new double[] { 393.7, 113.86, 0.0, 241.91, 0.0, 0.0, 0.0};

            System.IO.StreamReader file;
            string line;
            string[] split = { };
            char[] filter = { ';' };
            DirectoryInfo dataDir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\" + "data" + "\\");
            FileInfo[] dataFiles = dataDir.GetFiles();
            #endregion

            #region check file exists
            for (int i = 0; i < oxy_names.Length; i++)
                for (int j = 0; j < metal_names.Length; j++ )
                {
                    alldata = false;
                    foreach (FileInfo _file in dataFiles)
                    {
                        if (_file.Name.Equals(prod_names[i, j] + ".csv"))
                        {
                            alldata = true;
                            break;
                        }
                    }
                    if (!alldata)
                    {
                        missing += prod_names[i, j] + "; ";
                    }
                }
            for (int i = 0; i < chnprod_names.Length; i++ )
            {
                alldata = false;
                foreach (FileInfo _file in dataFiles)
                {
                    if (_file.Name.Equals(chnprod_names[i] + ".csv"))
                    {
                        alldata = true;
                        break;
                    }
                }
                if (!alldata)
                {
                    missing += chnprod_names[i] + "; ";
                }
            }
            #endregion

            #region load data from files
            if (missing.Length == 0)
            {
                for (int i = 0; i < chnprod_names.Length; i++)
                {
                    file = new System.IO.StreamReader(dataDir + chnprod_names[i] + ".csv");
                    while ((line = file.ReadLine()) != null)
                    {
                        split = line.Split(filter);
                        double temp = 0;
                        double cp = 0;
                        double.TryParse(split[0], out temp);
                        double.TryParse(split[1], out cp);
                        chnprod_cp[i].Add(temp, cp);
                    }
                }
                for (int i = 0; i < oxy_names.Length; i++)
                    for (int j = 0; j < metal_names.Length; j++)
                    {
                        file = new System.IO.StreamReader(dataDir + prod_names[i, j] + ".csv");
                        while ((line = file.ReadLine()) != null)
                        {
                            split = line.Split(filter);
                            double temp, cp;
                            double.TryParse(split[0], out temp);
                            double.TryParse(split[1], out cp);
                            prod_cp[i, j].Add(temp, cp);
                        }
                    }
            } else
            {
                string s = "Нет справочных данных по следующим продуктам: ";
                for (int i = 0; i < missing.Length - 2; i++)
                    s += missing[i];
                s += "\nРасчет невозможен. Обратитесь за справкой к разработчику.";
                outBox.ForeColor = Color.Red;
                outBox.Text = s;
                buttonCalculation.Enabled = false;
                buttonLoadData.Enabled = false;
                buttonSaveData.Enabled = false;
            }
            #endregion
        }

        private void readDataFromFields()
        {
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
        }

        private void firstOxydation()
        {
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
                    }
                    else
                    {
                        prod_moles[i, j] = oxy_atoms_new[i];
                        metal_atoms_new[j] -= oxy_atoms_new[i] * oxy_val[i] / metal_val[j];
                        oxy_atoms_new[i] = 0;
                    }
                }
            }
            #endregion

            #region CHN-oxydation
            // N -> N2
            chnprod_moles[6] = chn_atoms_new[2] / 2;
            chn_atoms_new[2] = 0;

            // H2O
            if (oxy_atoms_new[2] >= chn_atoms_new[1] / 2) // O >= H / 2
            {
                chnprod_moles[3] = chn_atoms_new[1] / 2; // _H2O = H / 2
                oxy_atoms_new[2] -= chn_atoms_new[1] / 2; // O = O - H / 2
                chn_atoms_new[1] = 0; // H = 0
            }
            else
            {
                chnprod_moles[3] = oxy_atoms_new[2]; // _H2O = O
                chn_atoms_new[1] -= oxy_atoms_new[2] * 2; // H = H - 2 * O
                oxy_atoms_new[2] = 0; // O = 0
                chnprod_moles[4] = chn_atoms_new[1] / 2; // _H2 = H / 2;
                chn_atoms_new[1] = 0; // H = 0
            }

            // CO
            if (chn_atoms_new[0] >= oxy_atoms_new[2]) // C >= O
            {
                chnprod_moles[1] = oxy_atoms_new[2]; // _CO = O
                chn_atoms_new[0] -= oxy_atoms_new[2]; // C = C - O
                oxy_atoms_new[2] = 0; // O = 0
                chnprod_moles[2] = chn_atoms_new[0]; // _C = C
            }
            else
            {
                chnprod_moles[1] = chn_atoms_new[0]; // _CO = C
                oxy_atoms_new[2] -= chn_atoms_new[0]; // O = O - C
                chn_atoms_new[0] = 0; // C = 0
            }

            // CO2
            if (oxy_atoms_new[2] > chnprod_moles[1])
            {
                chnprod_moles[0] = chnprod_moles[1]; // _CO2 = _CO
                oxy_atoms_new[2] -= chnprod_moles[1]; // O = O - _CO
                chnprod_moles[1] = 0; // _CO = 0
            }
            else
            {
                chnprod_moles[0] = oxy_atoms_new[2]; // _CO2 = O
                chnprod_moles[1] -= oxy_atoms_new[2]; // _CO = _CO - O
                oxy_atoms_new[2] = 0; // O = 0
            }

            // O2
            chnprod_moles[5] = oxy_atoms_new[2] / 2; // _O2 = o / 2
            oxy_atoms_new[2] = 0; // O = 0
            #endregion
        }
        
        private double calcMass()
        {
            double result = 0.0;

            for (int i = 0; i < metal_atoms.Length; i++)
                result += metal_atoms[i] * metal_mass[i];
            for (int i = 0; i < oxy_atoms.Length; i++)
                result += oxy_atoms[i] * oxy_mass[i];
            for (int i = 0; i < chn_atoms.Length; i++)
                result += chn_atoms[i] * chn_mass[i];
            
            return result;
        }

        private double get_cp(string prodName, double temp)
        {
            int i = -1, j = -1;
            for (int q = 0; q < chnprod_names.Length; q++ )
            { 
                if (prodName.Equals(chnprod_names[q]))
                {
                    i = q;
                    break;
                }
            }
            for (int q = 0; q < oxy_names.Length; q++)
                for (int m = 0; m < metal_names.Length; m++)
                {
                    if (prodName.Equals(prod_names[q, m]))
                    {
                        i = q;
                        j = m;
                        break;
                    }
                }

            double[] keys = new double[70];
            double[] values = new double[70];
            if (j > -1) // in prod
            {
                //prod_cp[i,j]
                prod_cp[i, j].Keys.CopyTo(keys, 0);
                prod_cp[i, j].Values.CopyTo(values, 0);
            }
            else // in chnprod
            {
                //chnprod_cp[i]
                chnprod_cp[i].Keys.CopyTo(keys, 0);
                chnprod_cp[i].Values.CopyTo(values, 0);
            }
            double t1 = keys[0], t2 = keys[0];
            for (i = 1; i < 70; i++ )
            {
                if (keys[i] > keys[i - 1])
                {
                    t2 = keys[i];
                    t1 = keys[i - 1];
                    if (t2 > temp)
                        return values[i - 1] + (temp - t1) * (values[i] - values[i - 1]) / (t2 - t1);
                } else
                {
                    return values[i];
                }
            }

            return 0.0;
        }

        private double solve_Qv(double[,] prod, double[] chnprod)
        {
            double q = 0.0;
            for (int i = 0; i < oxy_names.Length; i++)
                for (int j = 0; j < metal_names.Length; j++)
                    q += prod[i, j] * prod_qi[i, j];
            for (int i = 0; i < chnprod_names.Length; i++)
                q += chnprod[i] * chnprod_qi[i];
            return q - Qvv;
        }

        private void reactionPrint()
        {
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
                        s += "+" + prod_moles[i, j].ToString("##.####") + prod_names[i, j];
            for (int i = 0; i < chnprod_moles.Length; i++)
                if (chnprod_moles[i] > 0)
                    s += "+" + chnprod_moles[i].ToString("##.####") + chnprod_names[i];
            // out string
            s = s.Replace("==>+", "==>");
            s += "\nMолярная масса ВВ: " + Mvv.ToString("##.####") + " кг/кМоль\n";
            outBox.Text += s;
        }

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

        private void buttonLoadData_Click(object sender, EventArgs e)
        {
            System.IO.StreamReader file;
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\" + "save";
            DialogResult loadresult = openFileDialog.ShowDialog(); // Show the dialog.
            if (loadresult == DialogResult.OK) // Test result.
            {
                try
                {
                    file = new System.IO.StreamReader(openFileDialog.FileName);
                    List<string> lines = new List<string>();
                    string line = "";
                    while ((line = file.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                    if (lines.Count < 17)
                    {
                        outBox.Text = "Incorrect file format.";
                    }
                    else
                    {
                        textBoxC.Text = lines[0];
                        textBoxH.Text = lines[1];
                        textBoxN.Text = lines[2];
                        textBoxO.Text = lines[3];
                        textBoxCl.Text = lines[4];
                        textBoxF.Text = lines[5];
                        textBoxK.Text = lines[6];
                        textBoxCa.Text = lines[7];
                        textBoxNa.Text = lines[8];
                        textBoxAl.Text = lines[9];
                        textBoxP.Text = lines[10];
                        textBoxSi.Text = lines[11];
                        textBoxQvv.Text = lines[12];
                        textBoxMaxRo.Text = lines[13];
                        textBoxRovv.Text = lines[14];
                        textBoxDkr.Text = lines[15];
                        textBoxDvv.Text = lines[16];
                    }
                    file.Close();
                }
                catch (IOException)
                {
                }
            }
        }

        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + "\\" + "save";
            DialogResult writeresult = saveFileDialog.ShowDialog();
            if (writeresult == DialogResult.OK) // Test result.
            {
                try
                {
                    string name = saveFileDialog.FileName;
                    string[] lines = new string[17];
                    lines[0] = textBoxC.Text;
                    lines[1] = textBoxH.Text;
                    lines[2] = textBoxN.Text;
                    lines[3] = textBoxO.Text;
                    lines[4] = textBoxCl.Text;
                    lines[5] = textBoxF.Text;
                    lines[6] = textBoxK.Text;
                    lines[7] = textBoxCa.Text;
                    lines[8] = textBoxNa.Text;
                    lines[9] = textBoxAl.Text;
                    lines[10] = textBoxP.Text;
                    lines[11] = textBoxSi.Text;
                    lines[12] = textBoxQvv.Text;
                    lines[13] = textBoxMaxRo.Text;
                    lines[14] = textBoxRovv.Text;
                    lines[15] = textBoxDkr.Text;
                    lines[16] = textBoxDvv.Text;
                    File.WriteAllLines(name, lines);
                }
                catch (IOException)
                {
                }
            }
        }
        
        private void buttonCalculation_Click(object sender, EventArgs e)
        {
            if (missing.Length == 0)
            {

                #region variables reset
                metal_atoms = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                chn_atoms = new double[] { 0.0, 0.0, 0.0 };
                oxy_atoms = new double[] { 0.0, 0.0, 0.0 };
                prod_moles = new double[,] { { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }, 
                                         { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }, 
                                         { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 } };
                chnprod_moles = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                #endregion

                readDataFromFields();

                Mvv = calcMass();

                firstOxydation();

                reactionPrint();

                double Q = solve_Qv(prod_moles, chnprod_moles);

                outBox.Text += "Теплота взрыва: " + Q.ToString("##.##") + " кДж/кг\n";
            }
        }
    }
}
