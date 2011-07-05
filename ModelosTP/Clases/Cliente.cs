using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelosTP.Clases
{
    class Cliente
    {
        int horaLlegada;
        int tiempoEsperaCaja;
        int idCliente;

        public Cliente()
        {
        }
        public Cliente(int id)
        {
        }

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
