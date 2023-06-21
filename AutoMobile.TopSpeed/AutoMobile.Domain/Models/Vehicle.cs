using AutoMobile.Domain.Common;


namespace AutoMobile.Domain.Models
{
    public class Vehicle : BaseModel
    {
        public string Name { get; set; }

        public string Brand { get; set; }

        public string VehicleType { get; set; }

        public string EngineAndFuelType { get; set; }

        public string Transmission { get; set; }

        public int Engine { get; set; }

        public int TopSpeed { get; set; }

        public int Mileage { get; set; }

        public int Range { get; set; }

        public string SeatingCapacity { get; set; }

        public double PriceFrom { get; set; }

        public double PriceTo { get; set; }

        public int Ratings { get; set; }
    }
}
