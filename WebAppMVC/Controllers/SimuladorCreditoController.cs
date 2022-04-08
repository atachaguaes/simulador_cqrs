using Aplicacion.Commands;
using Aplicacion.Queries;
using Dominio.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAppMVC.Controllers
{
    public class SimuladorCreditoController : Controller
    {
        private readonly IMediator _mediator;

        public SimuladorCreditoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult NuevoCredito()
        {
            Credito credito = new Credito();
            return View(credito);
        }
        [HttpPost]
        public async Task<IActionResult> GenerarCredito(GenerarCreditoCommand model)
        {
            Credito credito = await _mediator.Send(model);

            HttpContext.Session.SetString("CreditoGenerado", JsonConvert.SerializeObject(credito));

            return View(credito);
        }

        // GET: CreditoController
        public async Task<IActionResult> Index()
        {
            List<Credito> creditos;
            var query = new ObtenerCreditosQuery();

            creditos = await _mediator.Send(query);

            return View(creditos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCreditoPorId(int id)
        {
            Credito credito;
            var query = new ObtenerCreditoPorIdQuery
            {
                Id = id
            };
            credito = await _mediator.Send(query);

            return View(credito);
        }


        [HttpPost]
        public async Task<IActionResult> Guardar(AgregarCreditoCommand model)
        {
            var modelPrev = JsonConvert.DeserializeObject<Credito>(HttpContext.Session.GetString("CreditoGenerado"));

            model.Detalles = modelPrev.Detalles;

            int id = await _mediator.Send(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Modificar(int idCredito)
        {
            Credito credito;
            var query = new ObtenerCreditoPorIdQuery
            {
                Id = idCredito
            };
            
            credito = await _mediator.Send(query);
            
            return View(credito);

        }
        [HttpPost]
        public async Task<IActionResult> Modificar(GenerarCreditoCommand model)
        {
            Credito credito = await _mediator.Send(model);

            HttpContext.Session.SetString("ModificarSession", JsonConvert.SerializeObject(credito));

            return RedirectToAction("DetallesActualizar");
        }

        [HttpGet]
        public ActionResult DetallesActualizar()
        {
            var modelPrev = JsonConvert.DeserializeObject<Credito>(HttpContext.Session.GetString("ModificarSession"));
            
            Credito credito = modelPrev;
            
            return View(credito);
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(ActualizarCreditoCommand model)
        {
            var modelPrev = JsonConvert.DeserializeObject<Credito>(HttpContext.Session.GetString("ModificarSession"));
            model.Detalles = modelPrev.Detalles;

            int id = await _mediator.Send(model);

            return RedirectToAction("Index");
        }

    }
}
