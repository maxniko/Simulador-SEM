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
    public partial class ParametrosLlegadaCliente : Form
    {
        public double media;
        public double desviacion;
        public double moda;
        public double limiteInferior;
        public double limiteSuperior;

        public ParametrosLlegadaCliente(double media, double desviacion, double moda, double limInf, double limSup)
        {
            InitializeComponent();
            numericUpDown1.Value = Decimal.Parse(media.ToString());
            numericUpDown2.Value = Decimal.Parse(desviacion.ToString());
            numericUpDown3.Value = Decimal.Parse(moda.ToString());
            numericUpDown4.Value = Decimal.Parse(limInf.ToString());
            numericUpDown5.Value = Decimal.Parse(limSup.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            media = Double.Parse(numericUpDown1.Value.ToString());
            desviacion = Double.Parse(numericUpDown2.Value.ToString());
            moda = Double.Parse(numericUpDown3.Value.ToString());
            limiteInferior = Double.Parse(numericUpDown4.Value.ToString());
            limiteSuperior = Double.Parse(numericUpDown5.Value.ToString());

            this.Close();
        }
    }
}
