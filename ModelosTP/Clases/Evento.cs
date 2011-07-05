using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelosTP.Clases
{
    class Evento
    {
        private int horaEjecucionAbsoluta = 0;
        //private Cliente cliente = null;
        //private Caja caja = null;
        private int tipoEvento = 0;

        public int HoraEjecucionAbsoluta
        {
            get { return horaEjecucionAbsoluta; }
            set { horaEjecucionAbsoluta = value; }
        }

        //public Cliente Cliente
        //{
        //    get { return cliente; }
        //    set { cliente = value; }
        //}

        //public Caja Caja
        //{
        //    get { return caja; }
        //    set { caja = value; }
        //}

        public int TipoEvento
        {
            get { return tipoEvento; }
            set { tipoEvento = value; }
        }
    }
}
