using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Api;

namespace WebStore.Controllers
{
    public class WebAPITestController : Controller
    {
        private readonly IValuesService _ValuesService;

        public WebAPITestController(IValuesService ValuesService) => _ValuesService = ValuesService;

        public IActionResult Index()
        {
            var values = _ValuesService.Get();
            return View(values);
        }
    }
}