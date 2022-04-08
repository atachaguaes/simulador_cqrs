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
    public class ObtenerCreditosQueryHandler : IRequestHandler<ObtenerCreditosQuery, List<Credito>>
    {
        private readonly ICreditoRepository _creditoRepository;

        public ObtenerCreditosQueryHandler(ICreditoRepository creditoRepository)
        {
            _creditoRepository = creditoRepository;
        }

        public Task<List<Credito>> Handle(ObtenerCreditosQuery request, CancellationToken cancellationToken)
        {
            var creditos = _creditoRepository.ObtenerCreditos();
            return Task.FromResult(creditos);
        }
    }
}
