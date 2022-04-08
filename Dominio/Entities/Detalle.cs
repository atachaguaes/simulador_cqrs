using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    public class Detalle
    {
        public int IdDetalle { get; set; }
        public int IdCredito { get; set; }
        public int Item { get; set; }
        public DateTime FechaCuota { get; set; }
        public int DiasCouta { get; set; }
        public double SaldoCapital { get; set; }
        public double Capital { get; set; }
        public double Interes { get; set; }
        public double Cuota { get; set; }
        public int Activo { get; set; }

        public Detalle()
        {
            this.Activo = 1;
        }
    }
    
}
