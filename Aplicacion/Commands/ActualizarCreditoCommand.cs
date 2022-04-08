using Dominio.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Commands
{
    public class ActualizarCreditoCommand : IRequest<int>
    {
        public int IdCredito { get; set; }
        public string? DNI { get; set; }
        public double Monto { get; set; }
        public double TasaAnual { get; set; }
        public int DiaPago { get; set; }
        public DateTime FechaDesembolso { get; set; }
        public DateTime FechaPrimeraCuota { get; set; }
        public int Cuotas { get; set; }
        public double TasaDiaria { get; set; }
        public double TasaMensual { get; set; }
        public int Activo { get; set; }
        public List<Detalle> Detalles { get; set; }

        public ActualizarCreditoCommand()
        {
            Detalles = new List<Detalle>();
        }


    }
}
