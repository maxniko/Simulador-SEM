using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelosTP.Clases
{
    class Terminal
    {
        private int estado = 0;

        /// <summary>
        /// 0 = libre.
        /// 1 = ocupated.
        /// </summary>
        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }
    }
}
