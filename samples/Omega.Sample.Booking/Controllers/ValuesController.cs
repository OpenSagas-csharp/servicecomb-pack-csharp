using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Servicecomb.Saga.Omega.Core.Transaction;

namespace Omega.Sample.Booking.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet, SagaStart]
        public String Get()
        {
            //return new string[] { "value1", "value2" };
            var httpClient = new HttpClient();
            httpClient.GetAsync("http://localhost:5002/api/values");
            var content = httpClient.GetAsync("http://localhost:5003/api/values");
            Thread.Sleep(5000);
            throw new NullReferenceException();
            return "";
        }

        [HttpGet, SagaStart]
        [Route("book")]
        public async Task<ActionResult> Book()
        {
            // init basic httpclient
            var httpClient = new HttpClient();
            // mark a reservation of car
            await httpClient.GetAsync("http://localhost:5002/api/values");
            // book a hotel
            await httpClient.GetAsync("http://localhost:5003/api/values");
            return Ok("ok");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
