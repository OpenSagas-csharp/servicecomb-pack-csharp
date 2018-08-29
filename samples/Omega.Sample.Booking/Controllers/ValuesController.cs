using System;
using System.Data.Common;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicecomb.Saga.Omega.Abstractions.Transaction;
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
            // mark a reservation of car (no exception)
            await httpClient.GetAsync("http://localhost:5002/api/values");
            // book a hotel (no exception)
            await httpClient.GetAsync("http://localhost:5003/api/values");
            throw new Exception("just test unknown exception");
            return Ok("ok");
        }


        [HttpGet, SagaStart]
        [Route("book1")]
        public ActionResult Book1()
        {
            // throw new a exception for test
            throw new DbUpdateException("I'm a dbUpdateException", new Exception());
            // init basic httpclient
            var httpClient = new HttpClient();
            // mark a reservation of car
            httpClient.GetAsync("http://localhost:5002/api/values");
            // book a hotel
            httpClient.GetAsync("http://localhost:5003/api/values");
            return Ok("ok");
        }

        [HttpGet, SagaStart]
        [Route("book2")]
        public ActionResult Book2()
        {
            // init basic httpclient
            var httpClient = new HttpClient();
            // mark a reservation of car , this will be throw a exception from car-service
            httpClient.GetAsync("http://localhost:5002/api/values").Wait();
            // book a hotel
            httpClient.GetAsync("http://localhost:5003/api/values").Wait();
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
