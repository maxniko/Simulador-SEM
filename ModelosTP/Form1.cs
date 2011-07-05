using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ModelosTP.Clases;
using System.IO;

namespace ModelosTP
{
    public partial class Form1 : Form
    {
        #region variables
        Random r = new Random();
        private int TiempoSimulacion;
        private int maximaColaTerminales = 0;
        private int TiempoMaxColaTerminales = 0;
        private int TiempoMaximoEsperaEnColaCaja = 0;
        private int TiempoTotalEsperaEnColaCaja = 0;
        private int TiempoPermanenciaCliente = 0;
        private int terminalesLibres = 1;
        private int totalClientesAtendidos = 0;
        private int tiempoOcioso = 0;
        private List<Evento> eventos = new List<Evento>();
        private List<Caja> cajas = new List<Caja>();
        private List<Terminal> terminales = new List<Terminal>();
        private int colaTerminales = 0;
        private int clientesEnBanco = 0;
        private int numeroEvento = 0;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void bComenzar_Click(object sender, EventArgs e)
        {
            bDetener_Click(sender, e);
            generarLog();
            numeroEvento = 0;

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
                loguear(ev);
                numeroEvento++;
                switch (ev.TipoEvento)
                {
                    case 0:
                        llegaCliente(ev);
                        break;
                    case 1:
                        terminarUsarTerminal(ev);
                        break;
                    case 2:
                        terminarAtencCaja(ev);
                        break;
                    default:
                        break;
                }
                if (TiempoSimulacion > (horasSimulacion.Value * 60) - 20)
                {
                    bool ok = true;
                }
            }
            rColaMaximaTerminales.Text = "Tamaño máximo de la cola en terminales: " + maximaColaTerminales;
            rTiempoEsperaPromedioCajas.Text = "Tiempo de espera promedio en la cola de las cajas: " + (TiempoTotalEsperaEnColaCaja / totalClientesAtendidos);
            rTiempoEsperaMaximoCajas.Text = "Tiempo de espera máximo en la cola de las cajas: " + TiempoMaximoEsperaEnColaCaja;
            rTiempoAcumuladoOcioso.Text = "Tiempo acumulado de cajeros ociosos: " + tiempoOcioso;
            rTiempoTramiteCliente.Text = "Tiempo promedio para trámites de un cliente: " + (TiempoPermanenciaCliente/totalClientesAtendidos).ToString();
            rTotalClientes.Text = "Total de clientes atendidos: " + totalClientesAtendidos;
        }

        /// <summary>
        /// Agrega un evento a la lista de eventos con la hora absoluta a la que llegará el próximo cliente
        /// </summary>
        private void planificarLlegadaCliente()
        {
            Evento e = new Evento();
            e.HoraEjecucionAbsoluta = TiempoSimulacion + generarXNormal(7.08, 2.78, 0.14, 0, 14);
            e.TipoEvento = 0;
            Cliente c = new Cliente();
            c.IdCliente = clientesEnBanco;
            clientesEnBanco++;
            e.Cliente = c;
            insertarEvento(e);
        }

        /// <summary>
        /// El tiempo de ejecución es igual al evento que contiene la llegada del cliente.
        /// Se dispara la llegada de un cliente (se crea un cliente) y se lo agrega a una de las filas de
        /// las cajas o a la fila de las terminales (con una probabilidad 1/30 1/70 respectivamente.
        /// </summary>
        /// <param name="tiempo"></param>
        private void llegaCliente(Evento e)
        {
            TiempoSimulacion = e.HoraEjecucionAbsoluta;
            planificarLlegadaCliente();
            e.Cliente.HoraLlegada = TiempoSimulacion;
            int decision = r.Next(0, 100);
            if (decision <= 30) //se va a la caja directamente
            {
                insertarClienteEnColaMasChica(e);
            }
            else //pasa primero por las terminales
            {
                colaTerminales++;
                ocuparTerminal(e);
            }
        }

        /// <summary>
        /// Un cliente comenzará a ser atendido.
        /// </summary>
        /// <param name="e"></param>
        private Evento ocuparCaja(Evento e)
        {
            if (cajas[e.IdCaja].Estado == 0)
            {
                cajas[e.IdCaja].Estado = 1;
                tiempoOcioso += (TiempoSimulacion - cajas[e.IdCaja].TiempoInactivo);
                planificarTiempoCaja(e.IdCaja, e.Cliente);

                TiempoSimulacion = e.HoraEjecucionAbsoluta;
                int aux = TiempoSimulacion - cajas[e.IdCaja].ColaClientes[0].TiempoEsperaCaja;
                if (aux > TiempoMaximoEsperaEnColaCaja)
                {
                    TiempoMaximoEsperaEnColaCaja = aux;
                }
                e.TipoEvento = 4;
                loguear(e);
            }
            else
            {
                e.Cliente.TiempoEsperaCaja = TiempoSimulacion;
                e.TipoEvento = 5;
                loguear(e);
            }
            return e;
        }

        private void terminarAtencCaja(Evento e)
        {
            if (cajas[e.IdCaja].ColaClientes.Count != 0)
            {
                if (cajas[e.IdCaja].ColaClientes.Count > 1)
                {
                    planificarTiempoCaja(e.IdCaja, cajas[e.IdCaja].ColaClientes[1]);
                    Evento ev = e;
                    ev.Cliente = cajas[e.IdCaja].ColaClientes[1];
                    ev.TipoEvento = 4;
                    loguear(ev);
                }
                else
                {
                    cajas[e.IdCaja].Estado = 0;
                    cajas[e.IdCaja].TiempoInactivo = TiempoSimulacion;
                }
                TiempoPermanenciaCliente = TiempoPermanenciaCliente + (TiempoSimulacion - cajas[e.IdCaja].ColaClientes[0].HoraLlegada);
                totalClientesAtendidos++;
                cajas[e.IdCaja].ColaClientes.RemoveAt(0);
                if (cajas[e.IdCaja].ColaClientes.Count == 0)
                {
                    cajas[e.IdCaja].TiempoInactivo = TiempoSimulacion;
                }
            }
        }

        /// <summary>
        /// Ocupa una terminal y resta un cliente de la cola de terminales
        /// </summary>
        private void ocuparTerminal(Evento e)
        {
            for(int i = 0; i < terminales.Count; i++)
            {
                Terminal t = terminales[i];
                if (t.Estado == 0)
                {
                    t.Estado = 1;
                    t.Cliente = e.Cliente;
                    terminales[i] = t;
                    colaTerminales--;
                    planificarUsoTerminal(t);
                    break;
                }
                else if (maximaColaTerminales < colaTerminales)
                {
                    maximaColaTerminales = colaTerminales;
                }
            }
            e.TipoEvento = 3;
            loguear(e);
        }

        /// <summary>
        /// Inserta el cliente pasado por parámetro en la caja que tenga la cola más corta
        /// </summary>
        /// <param name="cliente"></param>
        private void insertarClienteEnColaMasChica(Evento e)
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
            e.Cliente.TiempoEsperaCaja = TiempoSimulacion;
            e.IdCaja = cajaMenor;
            cajas[cajaMenor].ColaClientes.Add(e.Cliente);
            ocuparCaja(e);
        }

        /// <summary>
        /// Planifica cuándo termina atenderse un cliente en una caja
        /// </summary>
        private void planificarTiempoCaja(int nroCaja, Cliente c)
        {
            Evento e = new Evento();
            e.HoraEjecucionAbsoluta = TiempoSimulacion + generarXNormal(9.9, 3.56, 0.11, 1, 18);
            e.TipoEvento = 2; //salida de un cliente (terminó de usar) de la caja
            e.IdCaja = nroCaja;
            e.Cliente = c;
            insertarEvento(e);
        }

        /// <summary>
        /// Cuándo termina de usarse una terminal
        /// </summary>
        private void planificarUsoTerminal(Terminal t)
        {
            Evento e = new Evento();
            e.HoraEjecucionAbsoluta = TiempoSimulacion + generarXUniforme(0.09);
            e.TipoEvento = 1;
            e.Cliente = t.Cliente;
            insertarEvento(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tiempo"></param>
        private void terminarUsarTerminal(Evento e)
        {
            TiempoSimulacion = e.HoraEjecucionAbsoluta;
            Cliente cl = e.Cliente;
            
            for (int i = 0; i < terminales.Count; i++)
            {
                Terminal t = terminales[i];
                if (t.Estado == 1)
                {
                    t.Estado = 0;
                    cl = t.Cliente;
                    terminales[i] = t;
                    break;
                }
            }

            cl.HoraLlegada = TiempoSimulacion; //Hora a la que llega a esperar a la fila
            e.Cliente = cl;
            insertarClienteEnColaMasChica(e);
        }

        /// <summary>
        /// Limpia todas las variables. Vuelve todo a cero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDetener_Click(object sender, EventArgs e)
        {
            TiempoSimulacion = 0;
            maximaColaTerminales = 0;
            TiempoMaxColaTerminales = 0;
            TiempoMaximoEsperaEnColaCaja = 0;
            TiempoTotalEsperaEnColaCaja = 0;
            TiempoPermanenciaCliente = 0;
            terminalesLibres = 1;
            totalClientesAtendidos = 0;
            tiempoOcioso = 0;
            eventos.Clear();
            cajas.Clear();
            terminales.Clear();
            colaTerminales = 0;
            clientesEnBanco = 0;
            numeroEvento = 0;
            
            rColaMaximaTerminales.Text = "Tamaño máximo de la cola en terminales: 0";
            rTiempoEsperaPromedioCajas.Text = "Tiempo de espera promedio en la cola de las cajas: 0";
            rTiempoEsperaMaximoCajas.Text = "Tiempo de espera máximo en la cola de las cajas: 0";
            rTiempoAcumuladoOcioso.Text = "Tiempo acumulado de cajeros ociosos: 0";
            rTiempoTramiteCliente.Text = "Tiempo promedio para trámites de un cliente: 0";
            rTotalClientes.Text = "Total de clientes atendidos: 0";
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

        /// <summary>
        /// Inserta un evento en una lista ordenada por la hora de ejecución absoluta del evento
        /// </summary>
        /// <param name="eNuevo">El evento que se desea insertar en la lista</param>
        private void insertarEvento(Evento eNuevo)
        {
            bool ok = false;
            int indice = eventos.Count - 1;
            do
            {
                if (eventos.Count > 0)
                {
                    Evento e = eventos[indice];
                    if (eNuevo.HoraEjecucionAbsoluta > e.HoraEjecucionAbsoluta)
                    {
                        eventos.Insert(indice + 1, eNuevo);
                        ok = true;
                    }
                    indice--;
                }
                else
                {
                    eventos.Add(eNuevo);
                    ok = true;
                }
            } while (ok == false);
        }

        /// <summary>
        /// Agrega una entrada en el log.txt con el evento
        /// </summary>
        /// <param name="e">El evento que se desea loguear</param>
        private void loguear(Evento e)
        {
            string fileName = "log.txt"; //archivo log.txt
            //nombre del archivo, establece el cursor al final de la linea, tipo de acceso
            FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream); //crea la accion
            string hora = Math.Ceiling(Double.Parse((e.HoraEjecucionAbsoluta / 60).ToString())).ToString() + ":" + (e.HoraEjecucionAbsoluta % 60).ToString();
            string entrada = hora + " => ";
            switch (e.TipoEvento)
            {
                case 0:
                    writer.WriteLine(entrada += "Llega un cliente al sistema");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine();
                    break;
                case 1:
                    writer.WriteLine(entrada += "Un cliente termina de usar una terminal");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine();
                    break;
                case 2:
                    writer.WriteLine(entrada += "Un cliente termina de ser atendido por la caja (y se va)");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine();
                    break;
                case 3:
                    writer.WriteLine(entrada += "Un cliente comienza a usar una terminal");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine();
                    break;
                case 4:
                    writer.WriteLine(entrada += "Un cliente comienza a ser atendido");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine();
                    break;
                case 5:
                    writer.WriteLine(entrada += "Un cliente comienza a esperar en la cola para una caja");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine();
                    break;
                default:
                    writer.WriteLine(entrada += "Evento irreconocible");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine();
                    break;
            }
            writer.Close();//cierra la escritura del 
        }

        /// <summary>
        /// Genera un archivo log.txt
        /// </summary>
        private void generarLog()
        {
            string fileName = "log.txt";
            FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("=======================   LOG DEL SIMULADOR SEM   =======================\n\n");
            writer.Close();
        }
    }
}
