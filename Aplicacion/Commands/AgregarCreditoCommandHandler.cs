﻿using Aplicacion.Interfaces.Repositories;
using Dominio.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Commands
{
    public class AgregarCreditoCommandHandler : IRequestHandler<AgregarCreditoCommand, int>
    {
        private readonly ICreditoRepository _creditoRepository;

        public AgregarCreditoCommandHandler(ICreditoRepository creditoRepository)
        {
            _creditoRepository = creditoRepository;
        }

        public Task<int> Handle(AgregarCreditoCommand request, CancellationToken cancellationToken)
        {
            //[IdCredito], [DNI], [Monto], [TasaAnual], [DiaPago], [FechaDesembolso], [FechaPrimeraCuota], [NumeroCuotas], [TasaDiaria], [TasaMensual], [Activo]
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

            var result = _creditoRepository.AgregarCredito(credito);

            return Task.FromResult(result);
        }
    }
}
