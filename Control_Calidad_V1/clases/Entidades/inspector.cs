using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control_Calidad_V1.clases.Entidades
{
    class inspector
    {
        public int InspeccionesHora { get; set; }
        public decimal SueldoHora { get; set; }
        public int Capacidad { get; set; }
        public decimal MargenPorcentaje { get; set; }
        public decimal CostoErrorxProducto { get; set; }
    }
}
