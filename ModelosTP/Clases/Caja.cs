using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelosTP.Clases
{
    class Caja
    {
        private List<Cliente> colaClientes = new List<Cliente>();
        int tiempoInactivo = 0;
        int estado = 0;
        int vecesUtilizada = 0;

        /// <summary>
        /// 0 = Ocioso
        /// 1 = Ocupada
        /// </summary>
        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public List<Cliente> ColaClientes
        {
            get { return colaClientes; }
            set { colaClientes = value; }
        }

        public int TiempoInactivo
        {
            get { return tiempoInactivo; }
            set { tiempoInactivo = value; }
        }

        public int VecesUtilizada
        {
            get { return vecesUtilizada; }
            set { vecesUtilizada = value; }
        }
    }
}
