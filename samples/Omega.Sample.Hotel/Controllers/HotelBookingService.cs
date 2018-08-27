using System;
using System.Collections.Concurrent;
using Servicecomb.Saga.Omega.Abstractions.Transaction;
using Servicecomb.Saga.Omega.Core.Transaction;

namespace Omega.Sample.Hotel.Controllers
{
    public class HotelBookingService
    {
        private readonly ConcurrentDictionary<int, HotelBooking> _bookings = new ConcurrentDictionary<int, HotelBooking>();

        [Compensable("Cancel")]
        public void Order(HotelBooking booking)
        {
            if (booking.Amount > 2)
            {
                throw new ArgumentException("can not order the rooms large than two");
            }
            booking.Confirm();
            _bookings.TryAdd(booking.Id, booking);
        }

        void Cancel(HotelBooking booking)
        {
            _bookings.TryGetValue(booking.Id, out var hotelBooking);
            hotelBooking?.Cancel();
        }

    }
}
