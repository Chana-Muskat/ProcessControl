using Microsoft.AspNetCore.Mvc;
using System.Collections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProcessControl.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class netzerController : ControllerBase
    {
        // GET: api/<netzerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
           return new string[] { "value1-AA", "value2-BB" };
          
        }
        //[HttpGet("getAllIv")]
        //public IEnumerable<int> GetInvoice Get(int n)
        //{
        //    return new string[] { "value1", "value2" };

        //}

        // GET api/<netzerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value-ans from controler";
        }

        // POST api/<netzerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<netzerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<netzerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
