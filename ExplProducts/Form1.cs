﻿using System;
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
        private int[] metal_val, oxy_val, chnprod_gases;
        private double[] metal_atoms, oxy_atoms, chn_atoms, metal_atoms_new, oxy_atoms_new, chn_atoms_new, chnprod_moles;
        private double[] metal_mass, chn_mass, oxy_mass, chnprod_mass,chnprod_qi;
        private double[,] prod_mass,prod_qi,prod_coeff,prod_dens;
        private string[,] prod_names;
        private double[,] prod_moles;
        private double c_dens;
        private double[,] IngCoeff;

        Dictionary<double, double>[] chnprod_cp;
        Dictionary<double, double>[,] prod_cp;

        private double[] cho = { 0.0, 0.0, 0.0 };

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
            metal_names = new string[] { "K", "Na", "Li", "Ca", "Mg", "Al", "B", "Si", "P" };
            metal_val = new int[] { 1, 1, 1, 2, 2, 3, 3, 4, 5 };
            metal_atoms = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            metal_mass = new double[] { 39.0983, 22.98977, 6.9412, 40.08, 24.305, 26.98154, 10.812, 28.086, 30.97376 };

            chnprod_cp = new Dictionary<double, double>[9];
            for (int i = 0; i < 9; i++ )
                chnprod_cp[i] = new Dictionary<double, double>();
            prod_cp = new Dictionary<double, double>[3,9];
            for (int i = 0; i < 3; i++ )
                for (int j = 0; j < 9; j++)
                    prod_cp[i, j] = new Dictionary<double, double>();
            
            chn_names = new string[] { "C", "H", "N" };
            chn_atoms = new double[] { 0.0, 0.0, 0.0 };
            chn_mass = new double[] { 12.0108, 1.00795, 14.0067 };

            oxy_names = new string[] { "F", "Cl", "O" };
            oxy_val = new int[] { 1, 1, 2 };
            oxy_atoms = new double[] { 0.0, 0.0, 0.0 };
            oxy_mass = new double[] { 18.9984, 35.453, 15.9994 };

            prod_names = new string[,] { { "KF", "NaF", "LiF", "CaF2", "MgF2", "AlF3", "BF3", "SiF4", "PF5" }, 
                                         { "KCl", "NaCl", "LiCl", "CaCl2", "MgCl2", "AlCl3", "BCl3", "SiCl4", "PCl5" }, 
                                         { "K2O", "Na2O", "Li2O", "CaO", "MgO", "Al2O3", "B2O3", "SiO2", "P2O5" } };
            prod_moles = new double[,] { { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }, 
                                         { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }, 
                                         { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 } };
            prod_coeff = new double[,] { { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 }, 
                                         { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 }, 
                                         { 0.5, 0.5, 0.5, 1.0, 1.0, 0.5, 0.5, 1.0, 0.5 } };
            prod_mass =  new double[,] { { 58.0967, 41.98817, 25.9396, 78.0768, 62.3108, 83.97674,67.8072,  104.0796, 125.96576 },
                                         { 74.5513, 58.44277, 42.3942, 110.986, 95.211, 133.34054, 117.171, 169.898, 208.23876 },
                                         { 94.196, 61.97894, 29.8818, 56.0794, 40.3044, 101.96128, 69.6222, 60.0848, 141.94452} };
            prod_qi =    new double[,] { { 569.9, 576.6, 618.3, 1228.0, 1124.2, 1510.4, 0.0, 1614.9, 1593 }, 
                                         { 437.02, 410.4, 408.54, 796.18, 644.3, 584.1, 0.0, 687.8, 366.9 }, 
                                         { 363.6, 418.4, 597.88, 635.85, 601.5, 1677.5, 1273.5, 910.7, 1124.371 } };
            prod_dens  = new double[,] { { 2.505, 2.56, 2.63, 3.181, 3.177, 0.0, 0.0, 0.0, 0.0 }, 
                                         { 1.98, 2.165, 2.07, 2.15, 2.316, 0.0, 0.0, 0.0, 0.0 }, 
                                         { 2.32, 2.39, 2.013, 3.37, 3.58, 3.65, 2.18, 2.655, 2.39 } };
            
            c_dens = 2.267; // 3.515 алмаз

            chnprod_names = new string[] { "CO2", "CO", "C", "H2O", "H2", "O2", "N2", "HF", "HCl" };
            chnprod_moles = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            chnprod_mass =  new double[] { 44.0096, 28.0102, 12.0108, 18.0153, 2.0159, 31.9988, 28.0134, 20.00635, 36.46095 };
            chnprod_qi =    new double[] { 393.7, 113.86, 0.0, 241.91, 0.0, 0.0, 0.0, 273.3, 92.31 };
            chnprod_gases =    new int[] { 1, 1, 0, 1, 1, 1, 1, 1, 1 };

            IngCoeff = new double[,] { { 1.0, 1.0, 1.0, 1.0 }, { 1.511, 1.511, 1.511, 1.511 } };

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
                        //temp = double.Parse(split[0], System.Globalization.CultureInfo.InvariantCulture);
                        double.TryParse(split[1], out cp);
                        //cp = double.Parse(split[1], System.Globalization.CultureInfo.InvariantCulture);
                        chnprod_cp[i].Add(temp, cp - 8.3143 * chnprod_gases[i]);
                    }
                    file.Close();
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
                            //temp = double.Parse(split[0], System.Globalization.CultureInfo.InvariantCulture);
                            double.TryParse(split[1], out cp);
                            //cp = double.Parse(split[1], System.Globalization.CultureInfo.InvariantCulture);
                            prod_cp[i, j].Add(temp, cp);
                        }
                        file.Close();
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
            //CHN
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
            //Oxy
            if (double.TryParse(textBoxF.Text, out number))
            {
                oxy_atoms[0] = number;
            }
            if (double.TryParse(textBoxCl.Text, out number))
            {
                oxy_atoms[1] = number;
            }
            if (double.TryParse(textBoxO.Text, out number))
            {
                oxy_atoms[2] = number;
            }
            //Metals
            if (double.TryParse(textBoxK.Text, out number))
            {
                metal_atoms[0] = number;
            }
            if (double.TryParse(textBoxNa.Text, out number))
            {
                metal_atoms[1] = number;
            }
            if (double.TryParse(textBoxLi.Text, out number))
            {
                metal_atoms[2] = number;
            }
            if (double.TryParse(textBoxCa.Text, out number))
            {
                metal_atoms[3] = number;
            }
            if (double.TryParse(textBoxMg.Text, out number))
            {
                metal_atoms[4] = number;
            }
            if (double.TryParse(textBoxAl.Text, out number))
            {
                metal_atoms[5] = number;
            }
            if (double.TryParse(textBoxB.Text, out number))
            {
                metal_atoms[6] = number;
            }
            if (double.TryParse(textBoxSi.Text, out number))
            {
                metal_atoms[7] = number;
            }
            if (double.TryParse(textBoxP.Text, out number))
            {
                metal_atoms[8] = number;
            }
            // other params parsing
            if (double.TryParse(textBoxQvv.Text, out number))
            {
                Qvv = number;
            }
            // Negative Qvv
            if (checkBoxNegative.Checked == true)
            {
                Qvv = -Qvv;
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
            metal_atoms_new = new double[9] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
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
                        if ((oxy_val[i] < 2 && metal_val[j] < 3) || (oxy_val[i] > 1))
                        {
                            prod_moles[i, j] = prod_coeff[i, j] * metal_atoms_new[j];
                            oxy_atoms_new[i] -= metal_atoms_new[j] * metal_val[j] / oxy_val[i];
                            metal_atoms_new[j] = 0;
                        }
                    }
                    else
                    {
                        if ((oxy_val[i] < 2 && metal_val[j] < 3) || (oxy_val[i] > 1))
                        {
                            prod_moles[i, j] = prod_coeff[i, j] * oxy_atoms_new[i] * oxy_val[i] / metal_val[j];
                            metal_atoms_new[j] -= prod_moles[i, j] / prod_coeff[i, j];
                            oxy_atoms_new[i] = 0;
                        }
                    }
                }
            }
            #endregion

            #region F Cl to HF HCl
            chnprod_moles[7] = oxy_atoms_new[0]; // HF
            chn_atoms_new[2] -= oxy_atoms_new[0];
            chnprod_moles[8] = oxy_atoms_new[1]; // HCl
            chn_atoms_new[2] -= oxy_atoms_new[1];
            #endregion

            cho[0] = chn_atoms_new[0];
            cho[1] = chn_atoms_new[1];
            cho[2] = oxy_atoms_new[2];

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

        private double solve_Qv()
        {
            double q = 0.0;
            for (int i = 0; i < oxy_names.Length; i++)
                for (int j = 0; j < metal_names.Length; j++)
                    q += prod_moles[i, j] * prod_qi[i, j];
            for (int i = 0; i < chnprod_names.Length; i++)
                q += chnprod_moles[i] * chnprod_qi[i];

            return q - Qvv;
        }

        private double getKvg(double t)
        {
            return 1.2283211732e-13 * Math.Pow(t, 4) - 1.3708704872e-9 * Math.Pow(t, 3) 
                   + 4.8180203745e-6 * Math.Pow(t, 2) - 0.0030257287278 * t + 0.321160182936;
        }

        private double getB(string prod, double T)
        {
            //chnprod { "CO2", "CO", "C", "H2O", "H2", "O2", "N2", "HF", "HCl" };
            double d = 0.0;
            if (prod == "CO2")
                d = (3.1271186 * Math.Pow(10, -5) + 4.9667861 * Math.Pow(10, -10) * T - 16.510759 / Math.Pow(T, 2)) * Math.Pow(10, 6);
            else if (prod == "CO")
                d = (3.6071846 * Math.Pow(10, -5) - 3.1907692 * Math.Pow(10, -10) * T - 6.3027692 / Math.Pow(T, 2)) * Math.Pow(10, 6);
            else if (prod == "C")
                d = 0.0;
            else if (prod == "H2O")
                d = (1.5988709 * Math.Pow(10, -5) + 6.5681201 * Math.Pow(10, -10) * T - 39.43786 / Math.Pow(T, 2)) * Math.Pow(10, 6);
            else if (prod == "H2")
                d = (1.809596 * Math.Pow(10, -5) + 2.8121968 * Math.Pow(10, -11) * T - 0.40375573 / Math.Pow(T, 2)) * Math.Pow(10, 6);
            else if (prod == "O2")
                d = Math.Exp(-6.1804783 - 1083.5894 / T - 0.50422788 * Math.Log(T)) * Math.Pow(10, 6);
            else if (prod == "N2")
                d = 4.6203951 * Math.Pow(10, -7) * Math.Pow(0.99971877, T) * Math.Pow(T, 0.63546147) * Math.Pow(10, 6);
            else if (prod == "HF")
                d = 0.069 * (-0.465753462 + 20.6854316 * 461 / T - 165.619309 * Math.Pow(461, 2) / Math.Pow(T, 2) + 631.85106 * Math.Pow(461, 3) / Math.Pow(T, 3)
                             - 1318.63692 * Math.Pow(461, 4) / Math.Pow(T, 4) + 1493.58 * Math.Pow(461, 5) / Math.Pow(T, 5)) * 1000;
            else if (prod == "HCl")
                d = 0.0858 * (0.26919324 + 1.69613468 * 324.69 / T - 19.9226234 * 324.69 * 324.69 / T / T + 70.3694151 * Math.Pow(324.69, 3) / Math.Pow(T, 3)
                    - 144.65505 * Math.Pow(324.69, 4) / Math.Pow(T, 4) + 15.8137 * Math.Pow(324.69, 5) / Math.Pow(T, 5) + 192.38009 * Math.Pow(324.69, 6) / Math.Pow(T, 6)) * 1000;
            
            return (double)d;
        }

        private double getG(double T)
        {
            double Vkg = 1000 / Rovv; // Объем 1 кг ВВ

            double alpha = 0.0; // Объем конденсированной фазы, см3
            for (int i = 0; i < oxy_names.Length; i++)
                for (int j = 0; j < metal_names.Length; j++)
                {
                    if ((oxy_val[i] < 2 && metal_val[j] < 3) || (oxy_val[i] > 1))
                    {
                        alpha += prod_moles[i, j] * prod_mass[i, j] / prod_dens[i, j];
                    }
                }
            alpha += chnprod_moles[2] * c_dens; // Плюс углерод

            double Vg = Vkg - alpha; // Объем занимаемый газами взрыва, см3
            double GasDolya = 1 - alpha * Rovv / 1000; // Доля объема, занимаемая газами

            double b = 0; // Второй вириальный коэффициент
            for (int i = 0; i < chnprod_names.Length; i++ )
            {
                b += chnprod_moles[i] * getB(chnprod_names[i], T);
            }

            double x1 = b / Vg;
            double Npr = 0.0; // Продукты взрыва, моль
                for (int i = 0; i < chnprod_moles.Length; i++ )
                {
                    Npr += chnprod_moles[i] * chnprod_gases[i];
                }
            double logG = 4.38 * Npr * GasDolya * (x1 + 0.625 * Math.Pow(x1, 2) 
                          + 0.287 * Math.Pow(x1, 3) + 0.193 * Math.Pow(x1, 4)) / b;
            return Math.Pow(10, logG);
        }

        private double getIngCoeff()
        {
            double result = 0.0;
            
            return result;
        }

        private double foundT(double q)
        {
            //double delta = 1000000000;
            // { "CO2", "CO", "C", "H2O", "H2", "O2", "N2" };
            for (int t = 300; t < 6000; t++ )
            {
                double Qcp = 0.0;
                for (int i = 0; i < oxy_names.Length; i++)
                    for (int j = 0; j < metal_names.Length; j++)
                        Qcp += prod_moles[i, j] * get_cp(prod_names[i, j], t) * t / 1000;
                for (int i = 0; i < chnprod_names.Length; i++)
                    Qcp += chnprod_moles[i] * get_cp(chnprod_names[i], t) * t / 1000;
                /*
                if (Math.Abs(q - Qcp) > delta)
                    return (double)t;
                delta = Math.Abs(q - Qcp);
                */
                if (Qcp > q)
                    return (double)t;
            }
            return 0.0;
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
            s = "Реакция взрывчатого превращения:\n" + s;
            s += "\n";
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

        private void textBoxLi_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxLi.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxMg_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxMg.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxB_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxB.Text.IndexOf('.') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void textBoxBa_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 46 && textBoxBa.Text.IndexOf('.') != -1)
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
                        textBoxLi.Text = lines[12];
                        textBoxMg.Text = lines[13];
                        textBoxB.Text = lines[14];
                        textBoxBa.Text = lines[15];
                        if (lines[16] == "1")
                        {
                            checkBoxNegative.Checked = true;
                        }
                        else
                        {
                            checkBoxNegative.Checked = false;
                        }
                        textBoxQvv.Text = lines[17];
                        textBoxMaxRo.Text = lines[18];
                        textBoxRovv.Text = lines[19];
                        textBoxDkr.Text = lines[20];
                        textBoxDvv.Text = lines[21];
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
                    string[] lines = new string[25];
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
                    lines[12] = textBoxLi.Text;
                    lines[13] = textBoxMg.Text;
                    lines[14] = textBoxB.Text;
                    lines[15] = textBoxBa.Text;
                    if (checkBoxNegative.Checked == true)
                    {
                        lines[16] = "1";
                    }
                    else
                    {
                        lines[16] = "0";
                    }
                    lines[20] = textBoxQvv.Text;
                    lines[21] = textBoxMaxRo.Text;
                    lines[22] = textBoxRovv.Text;
                    lines[23] = textBoxDkr.Text;
                    lines[24] = textBoxDvv.Text;
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
                outBox.Text = "";

                #region variables reset
                for (int i = 0; i < metal_atoms.Length; i++ )
                    metal_atoms[i] = 0.0;
                for (int i = 0; i < chn_atoms.Length; i++)
                    chn_atoms[i] = 0.0;
                for (int i = 0; i < oxy_atoms.Length; i++) 
                    oxy_atoms[i] = 0.0;
                for (int i = 0; i < oxy_atoms.Length; i++)
                    for (int j = 0; j < metal_atoms.Length; j++) 
                        prod_moles[i, j] = 0.0;
                for (int i = 0; i < chnprod_moles.Length; i++)
                    chnprod_moles[i] = 0.0;
                #endregion

                readDataFromFields();

                Mvv = calcMass();

                firstOxydation();

                //reactionPrint();

                double Q = 0.0;
                double Qcorr = 0.0;
                double T = 0.0;
                bool correction = true;
                do
                {
                    Q = solve_Qv();
                    T = foundT(Q);
                    if (chnprod_moles[1] > 0 || chnprod_moles[4] > 0)
                    {
                        double kvg = getKvg(T);
                        double g = getG(T);
                        kvg *= g;
                        double M = cho[1] / 2 - cho[2] + cho[0];
                        double R = cho[2] - cho[0];
                        double A = (kvg * M + cho[2]) / (2 * (kvg - 1));
                        //chnprod { "CO2", "CO", "C", "H2O", "H2", "O2", "N2" };
                        chnprod_moles[0] = -A + Math.Sqrt(A * A + cho[0] * R / (kvg - 1)); // CO2
                        chnprod_moles[3] = cho[2] - cho[0] - chnprod_moles[0]; // H2O
                        chnprod_moles[1] = cho[0] - chnprod_moles[0]; // CO
                        chnprod_moles[4] = cho[1] / 2 - chnprod_moles[3]; // H2
                        Qcorr = solve_Qv();
                    } else
                    {
                        Qcorr = Q;
                        correction = false;
                        reactionPrint();
                        break;
                    }
                    
                } while ((Q - Qcorr) / Q > 0.001);

                if (correction)
                    reactionPrint();
                
                double Npr = 0.0; // Продукты взрыва, моль
                for (int i = 0; i < chnprod_moles.Length; i++ )
                {
                    Npr += chnprod_moles[i] * chnprod_gases[i];
                }
                double n = Npr / Mvv; // Продукты взрыва, моль/кг
                double V = Npr * 22.4; // Обхем гадов взрыва, л
                double eps = 0.0; // Твердые компоненты, моль/кг
                for (int i = 0; i < oxy_names.Length; i++ )
                    for (int j = 0; j < metal_names.Length; j++ )
                    {
                        eps += prod_moles[i, j] * prod_mass[i, j];
                    }
                for (int i = 0; i < chnprod_names.Length; i++)
                    eps -= chnprod_moles[i] * chnprod_mass[i] * (chnprod_gases[i] - 1);
                eps /= Mvv;
                double D = Math.Sqrt(1 - eps) * (0.314 * Math.Sqrt(n * T) * Rovv + 1.95) * 1000; // Скорость детонации
                D *= (1 - Dkr / Dvv); // Скорость детонации в диаметре

                
                outBox.Text += "\n";
                outBox.Text += "Теплота взрыва: " + Qcorr.ToString("##.##") + " кДж/кг\n";
                outBox.Text += "Температура взрыва: " + T.ToString("##.##") + " К\n";
                outBox.Text += "Масса брутто-формулы ВВ: " + Mvv.ToString("##.####") + " г\n";
                outBox.Text += "Продукты взрыва ВВ: " + Npr.ToString("##.####") + " моль\n";
                outBox.Text += "Продукты взрыва ВВ: " + n.ToString("##.####") + " моль/кг\n";
                outBox.Text += "Твердые компоненты ВВ: " + eps.ToString("##.####") + " кг/кг\n";
                outBox.Text += "Объем газов взрыва: " + V.ToString("##.##") + " л/кг\n";
                outBox.Text += "Скорость детонации ВВ: " + D.ToString("##.##") + " м/с\n";
            }
        }
    }
}
