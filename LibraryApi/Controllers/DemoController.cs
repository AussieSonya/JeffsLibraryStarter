
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class DemoController : ControllerBase
    {
        IConfiguration _config;
        ILogger _logger;

        public DemoController(IConfiguration config, ILoggerFactory factory)
        {
            _config = config;
            _logger = factory.CreateLogger("DemoController");
        }

        [HttpGet("/message")]
        public ActionResult GetMessage()
        {
            var msg = _config.GetValue<string>("message");
            _logger.LogInformation("They just got the message!");
            return Ok($"The message is {msg}");
        }

        // Route Params
        // GET /employees/19
        [HttpGet("/employees/{employeeId:int}", Name ="employees#get")]
        public ActionResult LookupEmployee([FromRoute] int employeeId)
        {
            // go to the database whatever..
            var response = new GetEmployeeDetailsResponse
            {
                Id = employeeId,
                Name = "Joe Schmidt",
                Department = "DEV",
                StartingSalary = 50000
            };
            return Ok(response);
        }

        // GET /blogs/2020/09/18
        [HttpGet("/blogs/{year:int}/{month:int:range(1,12)}/{day:int:range(1,31)}")]
        public ActionResult GetBlogPosts([FromRoute] int year, [FromRoute] int month, [FromRoute] int day)
        {
            return Ok($"Getting blogs for {year}/{month}/{day}");
        }
        // Query Strings
        // GET /employees?dept=DEV
        [HttpGet("/employees")]
        public ActionResult GetEmployees([FromQuery] string department = "All", [FromQuery] decimal minSalary = 0)
        {
            return Ok($"Returning all employees from {department} with a minumum salary of {minSalary:c}");
        }

        // Headers
        [HttpGet("/whoami")]
        public ActionResult ShowUserAgent([FromHeader(Name ="User-Agent")] string userAgent)
        {
            return Ok($"I see you are running {userAgent}");
        }

        // Entities

        [HttpPost("/employees")]
        public ActionResult Hire([FromBody] PostEmployeeCreate employeeToHire)
        {
            // Your template for doing a post to a collection.
            // Validate the data. If it is bad, send a 400 with or without details.
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // POST is not idempotent and it is not safe.
            // - You post to make things happen.
            //  - do your programmery stuff. Whatever that.
            // Reponse
            // 201 Created Status code. 
            // Add a Location to the response with the URI of the brand new resource.
            // To send them a copy of that resource - exactly as they'd get from following the location header.
            var response = new GetEmployeeDetailsResponse
            {
                Id = new Random().Next(50, 10000),
                Name = employeeToHire.Name,
                Department = employeeToHire.Department,
                StartingSalary = employeeToHire.StartingSalary
            };
            return CreatedAtRoute("employees#get", new { employeeId = response.Id }, response);

        }

        
    }

    public class PostEmployeeCreate
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Department { get; set; }
        [Range(10000,200000)]
        public decimal StartingSalary { get; set; }
    }

    public class GetEmployeeDetailsResponse
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Department { get; set; }
        [Range(10000, 200000)]
        public decimal StartingSalary { get; set; }
    }
}
