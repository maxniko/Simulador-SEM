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
    }
}
