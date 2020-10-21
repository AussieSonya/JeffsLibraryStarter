using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class CachingController : ControllerBase
    {

        private readonly ICacheTheCatalog _catalog;

        public CachingController(ICacheTheCatalog catalog)
        {
            _catalog = catalog;
        }

        [HttpGet("/caching/catalog")]
        public async Task<ActionResult> GetCatalog()
        {
            var catalog = await _catalog.GetCatalogAsync();
            return Ok(catalog);
        }

        [HttpGet("/caching/info")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration =15)]
        public ActionResult GetSomeInfo()
        {
            return Ok(new
            {
                Message = "Hello from the server",
                CreatedAt = DateTime.Now.ToLongTimeString()
            });
        }
    }
}
