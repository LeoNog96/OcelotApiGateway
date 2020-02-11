using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CatalogoWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CatalogoController
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Item 1", "Item 2" };
        }
    }
}