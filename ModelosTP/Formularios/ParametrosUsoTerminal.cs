using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModelosTP.Formularios
{
    public partial class ParametrosUsoTerminal : Form
    {
        public double moda;

        public ParametrosUsoTerminal(double moda)
        {
            InitializeComponent();
            numericUpDown3.Value = Decimal.Parse(moda.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            moda = Double.Parse(numericUpDown3.Value.ToString());
            this.Close();
        }
    }
}
