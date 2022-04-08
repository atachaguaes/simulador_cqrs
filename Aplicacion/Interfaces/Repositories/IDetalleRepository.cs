using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces.Repositories
{
    public interface IDetalleRepository
    {
        List<Detalle> ObtenerDetallesPorIdCredito(int idCredito);
        bool AgregarDetalle(Detalle detalle);
        bool EliminarDetalles(int idCredito);

    }
}
