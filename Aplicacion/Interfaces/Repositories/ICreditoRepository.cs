using Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces.Repositories
{
    public interface ICreditoRepository
    {
        List<Credito> ObtenerCreditos();
        Credito ObtenerCreditoPorIdCredito(int idCredito);
        int AgregarCredito(Credito credito);
        int ActualizarCredito(Credito credito);
        bool EliminarCredito(int idCredito);
        List<Detalle> ObtenerDetallesPorIdCredito(int idCredito);
        //Credito GenerarCredito(Credito credito);

    }
}
