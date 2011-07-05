using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelosTP.Clases
{
    class Cliente
    {
        int horaLlegadaAbsoluta;//, idCliente;
        int horaLlegada;
        int tiempoEsperaCaja;
        
        public int HoraLlegadaAbsoluta
        {
            get { return horaLlegadaAbsoluta; }
            set { horaLlegadaAbsoluta = value; }
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

        //public int IdCliente
        //{
        //    get { return idCliente; }
        //    set { idCliente = value; }
        //}
    }
}
