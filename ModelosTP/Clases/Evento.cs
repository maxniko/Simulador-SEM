using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelosTP.Clases
{
    class Evento
    {
        private int horaEjecucion = 0;
        private Cliente cliente = null;
        private Caja caja = null;

        public int HoraEjecucion
        {
            get { return horaEjecucion; }
            set { horaEjecucion = value; }
        }

        public Cliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        public Caja Caja
        {
            get { return caja; }
            set { caja = value; }
        }
    }
}
