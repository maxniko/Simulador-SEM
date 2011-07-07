using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ModelosTP.Clases;
using ModelosTP.Formularios;
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
        private List<Cliente> colaTerminales = new List<Cliente>();
        private int clientesEnBanco = 0;
        private int numeroEvento = -1;
        private int ultimoEventoEjecutado = 0;
        private int clientesQueEsperaron = 0;
        private List<int> salidaClienteAnterior = new List<int>();

        #region parámetros estadísticos
        public double mediaLlegadaCliente = 7.08;
        private double desviacionLlegadaCliente = 2.78;
        private double modaLlegadaCliente = 0.14;
        private double limiteInferiorLlegadaCliente = 0;
        private double limiteSuperiorLlegadaCliente = 14;

        private double modaUsoTerminal = 0.09;
        
        private double mediaAtencionCaja = 9.9;
        private double desviacionAtencionCaja = 3.56;
        private double modaAtencionCaja = 0.11;
        private double limiteInferiorAtencionCaja = 1;
        private double limiteSuperiorAtencionCaja = 18;

        #endregion

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void bComenzar_Click(object sender, EventArgs e)
        {
            
            bDetener_Click(sender, e);
            bloquear();
            generarLog();
            //numeroEvento = -1;
            ultimoEventoEjecutado = 0;

            for (int i = 0; i < cantidadCajas.Value; i++)
            {
                cajas.Add(new Caja());
                salidaClienteAnterior.Add(0);
            }

            for (int i = 0; i < cantidadTerminales.Value; i++)
            {
                terminales.Add(new Terminal());
            }
            bool ok = true;
            planificarLlegadaCliente();
            int minutos = Int32.Parse((horasSimulacion.Value * 60).ToString());
            while (TiempoSimulacion < minutos  && ok)
            {
                //Esto es para la barra de progreso
                int progreso = 0;
                progreso = 100 * TiempoSimulacion;
                int hs = Int32.Parse(horasSimulacion.Value.ToString());
                int minutosSimulacion = hs * 60;
                progreso /= minutosSimulacion;
                progressBar1.Value = progreso;
                //Hasta acá es para la barra de progreso

                try
                {
                    //La línea del mal...
                    Evento ev = eventos[ultimoEventoEjecutado];
                    ev.Mensaje = "Número evento :" + ultimoEventoEjecutado + ":";
                    ultimoEventoEjecutado++;
                    loguear(ev);
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
                    calcularTiempoOcioso();

                   
                }
                catch (Exception exc)
                {
                    /*
                    Evento kjh = new Evento();
                    kjh.TipoEvento = 6;
                    kjh.Mensaje = exc.StackTrace + " " + exc.Message;
                    loguear(kjh);
                    MessageBox.Show("Ud. se ha comunicado con el centro de atención al suicida.\n" +
                                    "Por favor, aguarde un momento y será atendido por uno de nuestros operadores.\n" +
                                    "No cuelgue, y no SE cuelgue.\n\n" +
                                    "Error:\n" + exc.Message);
                    ok = false;*/
                    bDetener_Click(sender, e);
                    bComenzar_Click(sender, e);
                }
            }
            try
            {
                int restarATotalCola = 0;
                foreach(Caja c in cajas)
                {
                    restarATotalCola += c.ColaClientes.Count;
                }
                rColaMaximaTerminales.Text = "Tamaño máximo de la cola en terminales (minutos): " + maximaColaTerminales;
                rTiempoEsperaPromedioCajas.Text = "Tiempo de espera promedio en la cola de las cajas (minutos): " + (TiempoTotalEsperaEnColaCaja / (clientesQueEsperaron - restarATotalCola));
                rTiempoEsperaMaximoCajas.Text = "Tiempo de espera máximo en la cola de las cajas (minutos): " + TiempoMaximoEsperaEnColaCaja;
                rTiempoAcumuladoOcioso.Text = "Tiempo acumulado de cajeros ociosos: " + tiempoOcioso + " minutos (~" + (tiempoOcioso / 60) + " horas)";
                rTiempoTramiteCliente.Text = "Tiempo promedio para trámites de un cliente (minutos): " + (TiempoPermanenciaCliente / totalClientesAtendidos).ToString();
                rTotalClientes.Text = "Total de clientes atendidos: " + totalClientesAtendidos;
            }
            catch (Exception exc)
            {
                Evento kjh = new Evento();
                kjh.TipoEvento = 6;
                kjh.Mensaje = exc.StackTrace;
                loguear(kjh);
                MessageBox.Show("Ud. se ha comunicado con el centro de atención al suicida.\n" +
                                "Por favor, aguarde un momento y será atendido por uno de nuestros operadores.\n" +
                                "No cuelgue, y no SE cuelgue.\n\n" +
                                "Error:\n" + exc.Message );
                ok = false;
            }
            progressBar1.Value = 100;
            desbloquear();
        }

        /// <summary>
        /// Agrega un evento a la lista de eventos con la hora absoluta a la que llegará el próximo cliente
        /// </summary>
        private void planificarLlegadaCliente()
        {
            Evento e = new Evento();
            e.HoraEjecucionAbsoluta = TiempoSimulacion + generarXNormal(mediaLlegadaCliente, desviacionLlegadaCliente, modaLlegadaCliente, limiteInferiorLlegadaCliente, limiteSuperiorLlegadaCliente);
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
                comenzarAUsarTerminal(e);
            }
        }

        /// <summary>
        /// Un cliente comenzará a ser atendido.
        /// </summary>
        /// <param name="e"></param>
        private void comenzarAUsarCaja(Evento e)
        {
            calcularTiempoOcioso();
            if (cajas[e.IdCaja].Estado == 0)
            {
                
                //Ocupa la caja
                cajas[e.IdCaja].Estado = 1;
                //Se asigna el cliente a la caja
                cajas[e.IdCaja].ClienteQueSeAtiende = e.Cliente;

                //Se planifica el tiempo de uso de la caja
                planificarTiempoCaja(e.IdCaja, e.Cliente);
                
                e.TipoEvento = 4;
                e.Mensaje = "Evento dependiente";
                loguear(e);
            }
            else
            {
                e.Cliente.HoraLlegadaColaCaja = TiempoSimulacion;
                cajas[e.IdCaja].ColaClientes.Add(e.Cliente);
                clientesQueEsperaron++;
                e.TipoEvento = 5;
                e.Mensaje = "Evento dependiente";
                loguear(e);
            }
            //return e;
        }
        /// <summary>
        /// se ejecuta en el tipo evento 2, 
        /// </summary>
        /// <param name="e"></param>
        private void terminarAtencCaja(Evento e)
        {
            TiempoSimulacion = e.HoraEjecucionAbsoluta;
            
            // variable para ahorrar calculos si hay o no cola
            bool hayCola = false;
            //Si hay cola en la caja
            if (cajas[e.IdCaja].ColaClientes.Count > 0)
            {
               //se planifica el siguiente cliente en cola 
                planificarTiempoCaja(e.IdCaja, cajas[e.IdCaja].ColaClientes[0]);

                //TiempoTotalEsperaEnColaCaja += (TiempoSimulacion - e.Cliente.HoraLlegadaColaCaja);
                e.Cliente = cajas[e.IdCaja].ColaClientes[0];
                e.TipoEvento = 4;
                e.Mensaje = "Evento dependiente de atender Cliente";
                loguear(e);
                hayCola = true;
                cajas[e.IdCaja].Estado = 1;
            }
            else
            {
                cajas[e.IdCaja].TiempoInactivo = TiempoSimulacion;
                cajas[e.IdCaja].Estado = 0;
            }
            //Se contabiliza un cliente atendido (para el total)
            totalClientesAtendidos++;
            //Se suma el total del tiempo que estuvo el cliente en el banco a un totalizador de dicho tiempo :)
            TiempoPermanenciaCliente += (TiempoSimulacion - cajas[e.IdCaja].ClienteQueSeAtiende.HoraLlegada);
           
            //Se contabiliza un cliente atendido (para esta caja)
            cajas[e.IdCaja].VecesUtilizada += 1;

            int aux = 0;
            if (cajas[e.IdCaja].ClienteQueSeAtiende.HoraLlegadaColaCaja != -1)
            {
                aux = salidaClienteAnterior[e.IdCaja] - cajas[e.IdCaja].ClienteQueSeAtiende.HoraLlegadaColaCaja;
                //Se suma el tiempo que esperó en la cola al total de tiempo de espera en cola
                TiempoTotalEsperaEnColaCaja += aux;
            }

            //El cliente que se está antendiendo FINALMENTE se fue.
            cajas[e.IdCaja].ClienteQueSeAtiende = null;

            salidaClienteAnterior[e.IdCaja] = TiempoSimulacion;

            //clientesQueEsperaron++;

            if (aux > TiempoMaximoEsperaEnColaCaja)
            {
                TiempoMaximoEsperaEnColaCaja = aux;
            }

            //si hay cola se supone que atiende al siguiente
            if (hayCola == true)
            {
                cajas[e.IdCaja].ClienteQueSeAtiende = cajas[e.IdCaja].ColaClientes[0];
                cajas[e.IdCaja].ColaClientes.RemoveAt(0);
            }
        }

        /// <summary>
        /// Ocupa una terminal y resta un cliente de la cola de terminales
        /// </summary>
        private void comenzarAUsarTerminal(Evento e)
        {
            bool libre = false;
            for(int i = 0; i < terminales.Count; i++)
            {
                Terminal t = terminales[i];
                if (t.Estado == 0)
                {
                    libre = true;
                    t.Estado = 1;
                    t.Cliente = e.Cliente;
                    terminales[i] = t;
                    planificarUsoTerminal(t);
                    e.TipoEvento = 3;
                    e.Mensaje = "Evento dependiente";
                    loguear(e);
                    break;
                }
            }
            if (!libre)
            {
                colaTerminales.Add(e.Cliente);
                e.TipoEvento = 7;
                e.Mensaje = "Evento dependiente";
                loguear(e);
                if (maximaColaTerminales < colaTerminales.Count)
                {
                    maximaColaTerminales = colaTerminales.Count;
                }
            }
        }

        /// <summary>
        /// Inserta el cliente pasado por parámetro en la caja que tenga la cola más corta
        /// </summary>
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
            if (cajas[cajaMenor].ColaClientes.Count > 0)
            {
                e.IdCaja = cajaMenor;
                e.Cliente.HoraLlegadaColaCaja = TiempoSimulacion;
                cajas[cajaMenor].ColaClientes.Add(e.Cliente);
                clientesQueEsperaron++;
            }
            else
            {
                e.Cliente.HoraLlegadaColaCaja = -1;
                e.IdCaja = cajaMenor;
                comenzarAUsarCaja(e);
            }
        }

        /// <summary>
        /// Planifica cuándo termina atenderse un cliente en una caja
        /// </summary>
        private void planificarTiempoCaja(int nroCaja, Cliente c)
        {
            Evento e = new Evento();
            //e.HoraEjecucionAbsoluta = TiempoSimulacion + generarXNormal(9.9, 3.56, 0.11, 1, 18);
            e.HoraEjecucionAbsoluta = TiempoSimulacion + generarXNormal(mediaAtencionCaja, desviacionAtencionCaja, modaAtencionCaja, limiteInferiorAtencionCaja, limiteSuperiorAtencionCaja);
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
            e.HoraEjecucionAbsoluta = TiempoSimulacion + generarXUniforme(modaUsoTerminal);
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
            
            for (int i = 0; i < terminales.Count; i++)
            {
                Terminal t = terminales[i];
                if (t.Estado == 1 && t.Cliente == e.Cliente)
                {
                    t.Estado = 0;
                    terminales[i] = t;
                    if (colaTerminales.Count > 0)
                    {
                        Evento nextCliente = new Evento();
                        nextCliente.Cliente = colaTerminales[0];
                        colaTerminales.RemoveAt(0);
                        nextCliente.TipoEvento = 3;
                        nextCliente.Terminal = t;
                        nextCliente.HoraEjecucionAbsoluta = TiempoSimulacion;
                        comenzarAUsarTerminal(nextCliente);
                    }
                    break;
                }
            }
            insertarClienteEnColaMasChica(e);
        }

        #region Métodos que seguro funcionan
        /// <summary>
        /// Recorre el List de cajas en busca de cajas ociosas y agrega el tiempo ocioso al total
        /// </summary>
        private void calcularTiempoOcioso()
        {
            foreach (Caja caja in cajas)
            {
                if (caja.Estado == 0)
                {
                    tiempoOcioso += (TiempoSimulacion - caja.TiempoInactivo);
                    caja.TiempoInactivo = TiempoSimulacion;
                }
            }
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
            colaTerminales.Clear();
            clientesEnBanco = 0;
            numeroEvento = -1;
            clientesQueEsperaron = 0;
            salidaClienteAnterior.Clear();

            rColaMaximaTerminales.Text = "Tamaño máximo de la cola en terminales (minutos): 0";
            rTiempoEsperaPromedioCajas.Text = "Tiempo de espera promedio en la cola de las cajas (minutos): 0";
            rTiempoEsperaMaximoCajas.Text = "Tiempo de espera máximo en la cola de las cajas (minutos): 0";
            rTiempoAcumuladoOcioso.Text = "Tiempo acumulado de cajeros ociosos: 0";
            rTiempoTramiteCliente.Text = "Tiempo promedio para trámites de un cliente (minutos): 0";
            rTotalClientes.Text = "Total de clientes atendidos: 0";
            progressBar1.Value = 0;
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
                        numeroEvento++;
                        ok = true;
                    }
                    indice--;
                }
                else
                {
                    eventos.Add(eNuevo);
                    numeroEvento++;
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
                    writer.WriteLine("\t\t:" + e.Mensaje + ": => Mensaje");
                    writer.WriteLine();
                    break;
                case 1:
                    writer.WriteLine(entrada += "Un cliente termina de usar una terminal");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine("\t\t:" + e.Mensaje + ": => Mensaje");
                    writer.WriteLine();
                    break;
                case 2:
                    writer.WriteLine(entrada += "Un cliente termina de ser atendido por la caja (y se va)");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine("\t\t:" + e.Mensaje + ": => Mensaje");
                    writer.WriteLine();
                    break;
                case 3:
                    writer.WriteLine(entrada += "Un cliente comienza a usar una terminal");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine("\t\t:" + e.Mensaje + ": => Mensaje");
                    writer.WriteLine();
                    break;
                case 4:
                    writer.WriteLine(entrada += "Un cliente comienza a ser atendido");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine("\t\t:" + e.Mensaje + ": => Mensaje");
                    writer.WriteLine();
                    break;
                case 5:
                    writer.WriteLine(entrada += "Un cliente comienza a esperar en la cola para una caja");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine("\t\t:" + e.Mensaje + ": => Mensaje");
                    writer.WriteLine();
                    break;
                case 6:
                    writer.WriteLine("############################################################");
                    writer.WriteLine("Se ha producido un error. Contáctese con los desarrolladores");
                    writer.WriteLine("############################################################");
                    writer.WriteLine();
                    writer.WriteLine(e.Mensaje);
                    writer.WriteLine();
                    break;
                case 7:
                    writer.WriteLine(entrada += "Un cliente comienza a esperar en la cola para usar una terminal");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine("\t\t:" + e.Mensaje + ": => Mensaje");
                    writer.WriteLine();
                    break;
                default:
                    writer.WriteLine(entrada += "Evento irreconocible");
                    writer.WriteLine("\t\t:" + e.TipoEvento + ": => Tipo de evento");
                    writer.WriteLine("\t\t:" + e.Cliente.IdCliente + ": => Cliente");
                    writer.WriteLine("\t\t:" + e.IdCaja + ": => Caja");
                    writer.WriteLine("\t\t:" + e.Mensaje + ": => Mensaje");
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
        
        /// <summary>
        /// Bloquea los botones y parámetros
        /// </summary>
        private void bloquear()
        {
            bComenzar.Enabled = false;
            bDetener.Enabled = false;
            cantidadCajas.Enabled = false;
            cantidadTerminales.Enabled = false;
            horasSimulacion.Enabled = false;
        }

        /// <summary>
        /// Desbloquea los botones y parámetros
        /// </summary>
        private void desbloquear()
        {
            bComenzar.Enabled = true;
            bDetener.Enabled = true;
            cantidadCajas.Enabled = true;
            cantidadTerminales.Enabled = true;
            horasSimulacion.Enabled = true;
        }
        #endregion

        #region Setters de parámetros
        /// <summary>
        /// Para cambiar los parámetros estadísticos de tiempos de llegada de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tiempoLlegadaClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParametrosLlegadaCliente p = new ParametrosLlegadaCliente(mediaLlegadaCliente, desviacionLlegadaCliente, modaLlegadaCliente, limiteInferiorLlegadaCliente, limiteSuperiorLlegadaCliente);
            p.ShowDialog();
            mediaLlegadaCliente = p.media;
            desviacionLlegadaCliente = p.desviacion;
            modaLlegadaCliente = p.moda;
            limiteSuperiorLlegadaCliente = p.limiteInferior;
            limiteSuperiorLlegadaCliente = p.limiteSuperior;
        }

        /// <summary>
        /// Para cambiar los parámetros estadísticos de tiempos de atención de una caja
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tiempoAtenciónCajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParametrosAtencionCaja p = new ParametrosAtencionCaja(mediaAtencionCaja, desviacionAtencionCaja, modaAtencionCaja, limiteInferiorAtencionCaja, limiteSuperiorAtencionCaja);
            p.ShowDialog();
            mediaAtencionCaja = p.media;
            desviacionAtencionCaja = p.desviacion;
            modaAtencionCaja = p.moda;
            limiteInferiorAtencionCaja = p.limiteInferior;
            limiteSuperiorAtencionCaja = p.limiteSuperior;
        }

        /// <summary>
        /// Para cambiar los parámetros estadísticos de tiempos de uso de una terminal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tiempoUsoTerminalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParametrosUsoTerminal p = new ParametrosUsoTerminal(modaUsoTerminal);
            p.ShowDialog();
            modaUsoTerminal = p.moda;
        }
        #endregion
    }
}
