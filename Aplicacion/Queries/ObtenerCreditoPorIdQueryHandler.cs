using Aplicacion.Interfaces.Repositories;
using Dominio.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Queries
{
    public class ObtenerCreditoPorIdQueryHandler : IRequestHandler<ObtenerCreditoPorIdQuery, Credito>
    {
        private readonly ICreditoRepository _creditoRepository;

        public ObtenerCreditoPorIdQueryHandler(ICreditoRepository creditoRepository)
        {
            _creditoRepository = creditoRepository;
        }

        public Task<Credito> Handle(ObtenerCreditoPorIdQuery request, CancellationToken cancellationToken)
        {
            var credito = _creditoRepository.ObtenerCreditoPorIdCredito(request.Id);

            return Task.FromResult(credito);
        }
    }
}
