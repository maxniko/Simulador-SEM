using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ModelosTP.Clases;

namespace ModelosTP
{
    public partial class Form1 : Form
    {
        private int horaSimulacion = 0;
        private List<Cliente> 
        public Form1()
        {
            InitializeComponent();
        }

        private void bComenzar_Click(object sender, EventArgs e)
        {
            cantidadCajas.Enabled = false;
            cantidadTerminales.Enabled = false;
            horasSimulacion.Enabled = false;
            retardoProximoCliente();
        }

        private void bDetener_Click(object sender, EventArgs e)
        {
            cantidadCajas.Enabled = true;
            cantidadTerminales.Enabled = true;
            horasSimulacion.Enabled = true;
        }

        private void planificarCliente()
        {
            int hora = horaSimulacion + generarXNormal(7.08, 2.78, 0.14);
            Cliente cliente = new Cliente();
            cliente.HoraLlegada = hora;
            return new Cliente();
        }

        private int generarXNormal(double media, double desviacion, double moda)
        {
            Random r = new Random();
            bool listo = false;
            int x = 0;
            do
            {
                double r1 = (r.Next(0, 100)) / 100;
                x = Int32.Parse((14 * r1).ToString());
                double r2 = (r.Next(0, 100)) / 100;
                double probabilidadNormalDeX = (Math.Pow(Math.E, (((-1) * Math.Pow((x - media), 2)) / (2 * Math.Pow(desviacion, 2))))) / (desviacion * Math.Sqrt(2 * Math.PI));
                double probDivModa = probabilidadNormalDeX / moda;
                if (probDivModa < r2)
                {
                    listo = true;
                }
            } while (listo = false);
            return x;
        }

        private int generarXUniforme(double moda)
        {
            Random r = new Random();
            bool listo = false;
            int x = 0;
            do
            {
                double r1 = (r.Next(0, 100)) / 100;
                x = Int32.Parse((11 * r1).ToString());
                double r2 = (r.Next(0, 100)) / 100;
                double probabilidadX = x / 11;
                double probDivModa = probabilidadX / moda;
                if (probDivModa < r2)
                {
                    listo = true;
                }
            } while (listo = false);
            return x;
        }
    }
}
