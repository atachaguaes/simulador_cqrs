using Aplicacion.Interfaces.Repositories;
using Dominio.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Commands
{
    public class GenerarCreditoCommandHandler : IRequestHandler<GenerarCreditoCommand, Credito>
    {
        //private readonly ICreditoRepository _creditoRepository;

        //public GenerarCreditoCommandHandler(ICreditoRepository creditoRepository)
        //{
        //    _creditoRepository = creditoRepository;
        //}

        public Task<Credito> Handle(GenerarCreditoCommand request, CancellationToken cancellationToken)
        {
            var credito = new Credito();

            credito.IdCredito = request.IdCredito;
            credito.DNI = request.DNI;
            credito.Monto = request.Monto;
            credito.TasaAnual = request.TasaAnual;
            credito.DiaPago = request.DiaPago;
            credito.FechaDesembolso = request.FechaDesembolso;
            credito.FechaPrimeraCuota = request.FechaPrimeraCuota;
            credito.Cuotas = request.Cuotas;
            credito.TasaDiaria = request.TasaDiaria;
            credito.TasaMensual = request.TasaMensual;
            credito.Activo = request.Activo;
            credito.Detalles = request.Detalles;

            var result = credito.GenerarCredito(credito);
            //var result = _creditoRepository.GenerarCredito(credito);

            return Task.FromResult(result);
        }
    }
}
