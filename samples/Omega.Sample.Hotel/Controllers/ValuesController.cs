using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Omega.Sample.Hotel.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            HotelBookingService bookingService =new HotelBookingService();
            HotelBooking hotelBooking = new HotelBooking()
            {
                Id = 1,
                Amount = 3,
                Name = "test"
            };
            bookingService.Order(hotelBooking);
            return new string[] { "value1", "value2" };
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
