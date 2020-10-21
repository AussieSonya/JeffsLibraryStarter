using LibraryApi.Models.ServerStatus;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class ServerStatusController : Controller
    {
        private ISystemTime _systemTime;

        public ServerStatusController(ISystemTime systemTime)
        {
            _systemTime = systemTime;
        }



        // GET /serverstatus
        [HttpGet("/serverstatus")]
        public ActionResult GetTheServerStatus()
        {
            var response = new GetServerStatusResponse()
            {
                Message = "Looks Good",
                CheckedBy = "Joe Schmidt",
                LastChecked = _systemTime.GetCurrent()
            };
            return Ok(response);
        }

    }
}
