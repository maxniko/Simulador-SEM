using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelosTP.Clases
{
    class Caja
    {
        private List<Cliente> colaClientes = new List<Cliente>();

        public List<Cliente> ColaClientes
        {
            get { return colaClientes; }
            set { colaClientes = value; }
        }
    }
}
