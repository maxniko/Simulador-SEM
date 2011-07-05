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
        private int maximaColaTerminales = 0;
        private int TiempoMaxColaTerminales = 0;
        private int TiempoMaxAtencCaja = 0;
        private int terminalesLibres = 1;
        private int totalClientesAtendidos = 0;
        private List<Evento> eventos = new List<Evento>();
        private List<Caja> cajas = new List<Caja>();
        private List<Terminal> terminales = new List<Terminal>();
        private int colaTerminales = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void bComenzar_Click(object sender, EventArgs e)
        {
            cantidadCajas.Enabled = false;
            cantidadTerminales.Enabled = false;
            horasSimulacion.Enabled = false;
            int numeroEvento = 0;

            for (int i = 0; i < cantidadCajas.Value; i++)
            {
                cajas.Add(new Caja());
            }

            for (int i = 0; i < cantidadTerminales.Value; i++)
            {
                terminales.Add(new Caja());
            }

            planificarLlegadaCliente();
            while (TiempoSimulacion <= (horasSimulacion.Value * 60))
            {
                Evento ev = eventos[numeroEvento];
                numeroEvento++;
                switch (ev.TipoEvento)
                {
                    case 0:
                        llegaCliente(ev.HoraEjecucionAbsoluta);
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }
            }

            /*
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

        private void planificarLlegadaCliente()
        {
            Evento e = new Evento();
            e.HoraEjecucionAbsoluta = TiempoSimulacion + generarXNormal(7.08, 2.78, 0.14, 0, 14);
            e.TipoEvento = 0;
            eventos.Add(e);
        }

        private void llegaCliente(int tiempo)
        {
            TiempoSimulacion = tiempo;
            planificarLlegadaCliente();
            Cliente c = new Cliente();
            c.HoraLlegadaAbsoluta = TiempoSimulacion;
            int decision = r.Next(0, 100);
            if (decision <= 30) //se va a la caja directamente
            {
                Caja caj = cajaConMenorFila();
                caj.ColaClientes.Add(c);
            }
            else //pasa primero por las terminales
            {
                colaTerminales++;
                terminalLibre();
            }
        }

        private void terminalLibre()
        {
            for(int i = 0; i < terminales.Count; i++)
            {
                Terminal t = terminales[i];
                if (t.Estado == 0)
                {
                    t.Estado = 1;
                    terminales[i] = t;
                    colaTerminales--;
                    planificarUsoTerminal();
                }
            }
        }

        private Caja cajaConMenorFila()
        {
            Caja caja;
            int fila = 10000000;
            foreach (Caja c in cajas)
            {
                if (c.ColaClientes.Count < fila)
                {
                    fila = c.ColaClientes.Count;
                    caja = c;
                }
            }
            return caja;
        }

        /// <summary>
        /// Cuándo termina de usarse una terminal
        /// </summary>
        private void planificarUsoTerminal()
        {
            Evento e = new Evento();
            e.HoraEjecucionAbsoluta = TiempoSimulacion + generarXUniforme(0.09);
            e.TipoEvento = 1;
            eventos.Add(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tiempo"></param>
        private void terminarUsarTerminal(int tiempo)
        {
            TiempoSimulacion = tiempo;
            if (colaTerminales != 0)
            {
                //Representa el cambio de que sale un cliente de la cola para usar una terminal
                colaTerminales--;
            }
            else
            {
                for (int i = 0; i < terminales.Count; i++)
                {
                    Terminal t = terminales[i];
                    if (t.Estado == 1)
                    {
                        t.Estado = 0;
                        terminales[i] = t;
                        break;
                    }
                }

            }
            Caja c = cajaConMenorFila();
            Cliente cl = new Cliente();
            cl.HoraLlegadaAbsoluta = TiempoSimulacion; //Hora a la que llega a esperar a la fila
            c.ColaClientes.Add(cl);
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
                double r1, r2, probabilidadX, probDivModa, ex;
                r1 = r.Next(0, 100);
                r1 /= 100;
                ex = 11 * r1;
                ex = Math.Round(ex, 0);
                x = Int32.Parse(ex.ToString());
                r2 = r.Next(0, 100);
                r2 /= 100;
                probabilidadX = ex / 11;
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
