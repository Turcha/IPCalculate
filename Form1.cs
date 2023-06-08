using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NetworkCalculate
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Running();
        }

        private void Running()
        {
            string pattern = @"(\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3})";
            

            
            if (Regex.IsMatch(textBox2.Text, pattern))
            {
                IpCalculate calc = new IpCalculate(textBox1.Text, textBox2.Text);
                label8.Text = calc.Network;
                label9.Text = calc.Broadcast;
                label10.Text = calc.MinAddress;
                label11.Text = calc.MaxAddress;
                label12.Text = calc.Wildcard;
            }
            else
            {
                try
                {
                    IpCalculate calc = new IpCalculate(textBox1.Text, int.Parse(textBox2.Text));
                    label8.Text = calc.Network;
                    label9.Text = calc.Broadcast;
                    label10.Text = calc.MinAddress;
                    label11.Text = calc.MaxAddress;
                    label12.Text = calc.Wildcard;
                }
                catch(Exception exp)
                {
                    MessageBox.Show("Неверный формат!!!");
                }
            }
        }
    }
}
