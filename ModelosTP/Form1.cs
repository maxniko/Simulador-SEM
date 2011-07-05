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
        private int TiempoPromedioAtencCaja = 0;
        private int TiempoPermanenciaCliente = 0;
        private int terminalesLibres = 1;
        private int totalClientesAtendidos = 0;
        private int tiempoOcioso = 0;
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
                terminales.Add(new Terminal());
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
                        atenderCaja(ev);
                        break;
                    case 2:
                        terminarAtencCaja(ev);
                        break;
                    default:
                        break;
                }
            }
            rTiempoEsperaPromedioCajas.Text = "Tiempo de espera promedio en la cola de las cajas: " + (TiempoPromedioAtencCaja / totalClientesAtendidos);
            rTiempoEsperaMaximoCajas.Text = "Tiempo de espera máximo en la cola de las cajas: " + TiempoMaxAtencCaja;
        }

        /// <summary>
        /// Agrega un evento a la lista de eventos con la hora absoluta a la que llegará el próximo cliente
        /// </summary>
        private void planificarLlegadaCliente()
        {
            Evento e = new Evento();
            e.HoraEjecucionAbsoluta = TiempoSimulacion + generarXNormal(7.08, 2.78, 0.14, 0, 14);
            e.TipoEvento = 0;
            insertarEvento(e);
        }

        private void llegaCliente(int tiempo)
        {
            TiempoSimulacion = tiempo;
            planificarLlegadaCliente();
            Cliente c = new Cliente();
            c.HoraLlegada = TiempoSimulacion;
            int decision = r.Next(0, 100);
            if (decision <= 30) //se va a la caja directamente
            {
                //Caja caj = cajaConMenorFila();
                //caj.ColaClientes.Add(c);
                insertarClienteEnColaMasChica(c);
            }
            else //pasa primero por las terminales
            {
                colaTerminales++;
                terminalLibre();
            }
        }

        private void atenderCaja(Evento e)
        {
            TiempoSimulacion = e.HoraEjecucionAbsoluta;
            tiempoOcioso = tiempoOcioso + (TiempoSimulacion - cajas[e.IdCaja].TiempoInactivo);
            int aux = TiempoSimulacion - cajas[e.IdCaja].ColaClientes[0].TiempoEsperaCaja;
            if (aux > TiempoMaxAtencCaja)
            {
                TiempoMaxAtencCaja = aux;
            }
            planificarTiempoCaja(e.IdCaja);
        }

        private void terminarAtencCaja(Evento e)
        {
            cajas[e.IdCaja].TiempoInactivo = TiempoSimulacion;
            TiempoPermanenciaCliente = TiempoPermanenciaCliente + (TiempoSimulacion - cajas[e.IdCaja].ColaClientes[0].HoraLlegada);
            totalClientesAtendidos++;
            cajas[e.IdCaja].ColaClientes.RemoveAt(0);
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
                    planificarUsoTerminal(t);
                }
            }
        }

        private Caja cajaConMenorFila()
        {
            Caja caja = new Caja();
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

        private void insertarClienteEnColaMasChica(Cliente cliente)
        {
            int fila = 10000000;
            int cajaMenor = 0;
            for (int x = 0; x < cajas.Count; x++)
            {
                Caja c = cajas[x];
                if (c.ColaClientes.Count < fila)
                {
                    fila = c.ColaClientes.Count;
                    cajaMenor = x;
                }
            }
            cliente.TiempoEsperaCaja = TiempoSimulacion;
            cajas[cajaMenor].ColaClientes.Add(cliente);
            if (fila == 0)
            {
                tiempoOcioso = TiempoSimulacion - cajas[cajaMenor].TiempoInactivo;
                planificarTiempoCaja(cajaMenor);
            }
        }

        /// <summary>
        /// Cuándo termina de usarse una terminal
        /// </summary>
        private void planificarTiempoCaja(int nroCaja)
        {
            Evento e = new Evento();
            e.HoraEjecucionAbsoluta = TiempoSimulacion + generarXNormal(9.9, 3.56, 0.11, 1, 18);
            e.TipoEvento = 2; //salida de la caja
            e.IdCaja = nroCaja;
            insertarEvento(e);
        }


        /// <summary>
        /// Cuándo termina de usarse una terminal
        /// </summary>
        private void planificarUsoTerminal(Terminal t)
        {
            Evento e = new Evento();
            e.HoraEjecucionAbsoluta = TiempoSimulacion + generarXUniforme(0.09);
            e.TipoEvento = 3;
            insertarEvento(e);
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

        /// <summary>
        /// Detiene la simulación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDetener_Click(object sender, EventArgs e)
        {
            cantidadCajas.Enabled = true;
            cantidadTerminales.Enabled = true;
            horasSimulacion.Enabled = true;
        }

        /// <summary>
        /// Genera una variable aleatoria X con distribución normal tomando como parámetro la media, desviación,
        /// moda, intervalo inferior e intervalo superior
        /// </summary>
        /// <param name="media">Media de los datos observados</param>
        /// <param name="desviacion">Desviación estándar de los datos observados</param>
        /// <param name="moda">Moda de los datos observados</param>
        /// <param name="limiteInferiorIntervalo">Valor mínimo observado/obtenible</param>
        /// <param name="limiteSuperiorIntervalo">Valor máximo observado/obtenible</param>
        /// <returns>Variable X generada aleatoriamente</returns>
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

        /// <summary>
        /// Genera una variable aleatoria X con distribución uniforme tomando como parámetro la moda
        /// </summary>
        /// <param name="moda">Moda de los datos observados</param>
        /// <returns>Variable X generada aleatoriamente</returns>
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

        private void insertarEvento(Evento eNuevo)
        {
            bool ok = false;
            do
            {
                int indice = 0;
                if (eventos.Count > 0)
                {
                    Evento e = eventos[indice];
                    if (eNuevo.HoraEjecucionAbsoluta > e.HoraEjecucionAbsoluta)
                    {
                        eventos.Insert(indice + 1, eNuevo);
                        ok = true;
                    }
                }
                else
                {
                    eventos.Add(eNuevo);
                    ok = true;
                }
            } while (ok == false);
        }
    }
}
