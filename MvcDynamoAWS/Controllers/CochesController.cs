using Microsoft.AspNetCore.Mvc;
using MvcDynamoAWS.Models;
using MvcDynamoAWS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcDynamoAWS.Controllers
{
    public class CochesController : Controller
    {
        private ServiceDynamoDB service;

        public CochesController(ServiceDynamoDB service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Coche> coches = await this.service.GetCochesAsync();
            return View(coches);
        }

        public async Task<IActionResult> Details(int id)
        {
            Coche car = await this.service.FindCocheAsync(id);
            return View(car);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.service.DeleteCocheAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Coche car, string incluirmotor, string tipo, int cilindrada, int caballos)
        {
            if(incluirmotor != null)
            {
                car.Motor = new Motor();
                car.Motor.Tipo = tipo;
                car.Motor.Cilindrada = cilindrada;
                car.Motor.Caballos = caballos;
            }
            await this.service.CreateCocheAsync(car);
            return RedirectToAction("Index");
        }


        public IActionResult SearchCoches()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchCoches(string marca)
        {
            List<Coche> coches = await this.service.SearchCochesAsync(marca);

            return View(coches);
        }
    }

}
