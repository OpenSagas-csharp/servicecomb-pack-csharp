using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servicecomb.Saga.Omega.Core.Transaction;

namespace Omega.Sample.Car.Controllers
{
    public class CarBookingService
    {
        private readonly ConcurrentDictionary<int, CarBooking> _bookings = new ConcurrentDictionary<int, CarBooking>();


        [Compensable("Cancel")]
        public void Order(CarBooking carBooking)
        {
            carBooking.Confirm();
            _bookings.TryAdd(carBooking.Id, carBooking);
        }

        void Cancel(CarBooking booking)
        {
            _bookings.TryGetValue(booking.Id, out var carBooking);
            carBooking?.Cancel();
        }
    }
}
