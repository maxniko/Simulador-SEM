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
        Random r = new Random();
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

            for (int i = 0; i < 50; i++)
            {
                richTextBox1.Text = richTextBox1.Text + generarXNormal(7.08, 2.78, 0.14, 0, 14).ToString() + "\n";
            }

            /*Evento ev = new Evento();
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
            }*/
        }

        private Cliente planificarCliente()
        {
            int hora = TiempoSimulacion + generarXNormal(7.08, 2.78, 0.14, 0, 14);
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

        private int generarXNormal(double media, double desviacion, double moda, double limiteInferiorIntervalo, double limiteSuperiorIntervalo)
        {
            bool listo = false;
            int x = 0;
            do
            {
                
                double numerador, denominador, numeradorExponente, 
                    denominadorExponente, exponente, probabilidadNormalDeX, 
                    probDivModa, r1, r2, ex;
                r1 = r.Next(0, 100);
                r1 /= 100;
                ex = limiteInferiorIntervalo + ((limiteSuperiorIntervalo - limiteInferiorIntervalo) * r1);
                ex = Math.Round(ex, 0);
                x = Int32.Parse(ex.ToString());
                r2 = r.Next(0, 100);
                r2 /= 100;
                
                numeradorExponente = (-1) * Math.Pow((ex - media), 2);
                denominadorExponente = 2 * Math.Pow(desviacion, 2);
                exponente = numeradorExponente / denominadorExponente;
                numerador = Math.Pow(Math.E, exponente);
                denominador = desviacion * Math.Sqrt(2 * Math.PI);
                probabilidadNormalDeX = numerador / denominador;
                
                probDivModa = probabilidadNormalDeX / moda;
                if (probDivModa > r2)
                {
                    listo = true;
                }
            } while (listo == false);
            return x;
        }

        private int generarXUniforme(double moda)
        {
            bool listo = false;
            int x = 0;
            do
            {
                double r1, r2, probabilidadX, probDivModa;
                r1 = r.Next(0, 100);
                r1 /= 100;
                x = Int32.Parse((11 * r1).ToString());
                r2 = r.Next(0, 100);
                r2 /= 100;
                probabilidadX = x / 11;
                probDivModa = probabilidadX / moda;
                if (probDivModa > r2)
                {
                    listo = true;
                }
            } while (listo == false);
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
