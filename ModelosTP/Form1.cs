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
        private int TiempoSimulacion;
        private int colaTerminales = 0;
        private int maximaColaTerminales = 0;
        private int TiempoMaxColaTerminales = 0;
        private int TiempoMaxAtencCaja = 0;
        private int terminalesLibres = 1;
        private List<Evento> eventos;

        public Form1()
        {
            InitializeComponent();
        }

        private void bComenzar_Click(object sender, EventArgs e)
        {
            cantidadCajas.Enabled = false;
            cantidadTerminales.Enabled = false;
            horasSimulacion.Enabled = false;

            Evento ev = new Evento();
            ev.Cliente = planificarCliente();
            eventos.Add(ev);
            while (TiempoSimulacion < Int32.Parse(horasSimulacion.Value.ToString()))
            {
                if (eventos.Count > 0)
                {
                    Evento evento = eventos[0];
                    if(evento.Cliente != null)
                    {
                        TiempoSimulacion = TiempoSimulacion + evento.Cliente.HoraLlegada;
                    }
                    else
                    {

                    }
                }
            }
        }

        private Cliente planificarCliente()
        {
            int hora = TiempoSimulacion + generarXNormal(7.08, 2.78, 0.14);
            Cliente cliente = new Cliente();
            cliente.HoraLlegada = hora;
            return new Cliente();
        }

        private void bDetener_Click(object sender, EventArgs e)
        {
            cantidadCajas.Enabled = true;
            cantidadTerminales.Enabled = true;
            horasSimulacion.Enabled = true;
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

        private void insertarEvento()
        {
            bool ok = false;
            do
            {
                int indice = 0;
                if (eventos.Count > 0)
                {
                    Evento e = eventos[indice];
                }
            } while (ok == false);
        }
    }
}
