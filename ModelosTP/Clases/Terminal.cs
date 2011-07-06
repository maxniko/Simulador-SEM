using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelosTP.Clases
{
    class Terminal
    {
        private int estado = 0;
        private Cliente c;
        private int vecesUtilizada = 0;

        /// <summary>
        /// 0 = libre.
        /// 1 = ocupated.
        /// </summary>
        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public Cliente Cliente
        {
            get { return c; }
            set { c = value; }
        }

        public int VecesUtilizada
        {
            get { return vecesUtilizada; }
            set { vecesUtilizada = value; }
        }
    }
}
