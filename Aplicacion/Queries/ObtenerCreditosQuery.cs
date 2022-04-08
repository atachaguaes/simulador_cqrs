using Dominio.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Queries
{
    public class ObtenerCreditosQuery : IRequest<List<Credito>>
    {

    }
}
