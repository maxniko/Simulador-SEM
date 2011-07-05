using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelosTP.Clases
{
    class Evento
    {
        private int horaEjecucionAbsoluta = 0;
        private int tipoEvento = 0;
        //private int idCliente = 0;
        private int tipoCaja = 0;
        //private Cliente cliente = null;
        //private Caja caja = null;

        public int HoraEjecucionAbsoluta
        {
            get { return horaEjecucionAbsoluta; }
            set { horaEjecucionAbsoluta = value; }
        }

        //public int IdCliente
        //{
        //    get { return idCliente; }
        //    set { idCliente = value; }
        //}

        /// <summary>
        /// 0 = terminal.
        /// 1 = caja.
        /// </summary>
        public int TipoCaja
        {
            get { return tipoCaja; }
            set { tipoCaja = value; }
        }

        /// <summary>
        /// 0 = llega cliente.
        /// 1 = cliente termina de usar terminal.
        /// 2 = cliente termina de ser atendido.
        /// </summary>
        public int TipoEvento
        {
            get { return tipoEvento; }
            set { tipoEvento = value; }
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
    }
}
