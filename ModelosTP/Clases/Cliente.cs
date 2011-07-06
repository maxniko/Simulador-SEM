using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelosTP.Clases
{
    class Cliente
    {
        int horaLlegada;
        int tiempoEsperaCaja = 0;
        int idCliente;

        public Cliente()
        {
        }
        public Cliente(int id)
        {
        }

        /// <summary>
        /// Momento en que el cliente llega a esperar en la cola para la caja
        /// </summary>
        public int TiempoEsperaCaja
        {
            get { return tiempoEsperaCaja; }
            set { tiempoEsperaCaja = value; }
        }

        public int HoraLlegada
        {
            get { return horaLlegada; }
            set { horaLlegada = value; }
        }

        public int IdCliente
        {
            get { return idCliente; }
            set { idCliente = value; }
        }
    }
}
