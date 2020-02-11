using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PedidosWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidosController
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Produto 1", "Produto 2" };
        }
    }
}