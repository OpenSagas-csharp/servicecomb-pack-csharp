using System;

namespace Omega.Sample.Car.Controllers
{
    [Serializable]
    public class CarBooking
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public bool Confirmed { get; set; }
        public bool Cancelled { get; set; }

        public void Confirm()
        {
            Confirmed = true;
            Cancelled = false;
        }

        public void Cancel()
        {
            Confirmed = false;
            Cancelled = true;
        }
    }
}
