using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.DTO.Vehicle
{
    public class VehicleDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string VehicleType { get; set; }

        public string EngineAndFuelType { get; set; }

        public double PriceFrom { get; set; }

        public double PriceTo { get; set; }

        public int Ratings { get; set; }

    }
}
