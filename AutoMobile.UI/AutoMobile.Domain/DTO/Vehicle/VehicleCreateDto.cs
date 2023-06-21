using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoMobile.Domain.DTO.Vehicle
{
    public class VehicleCreateDto
    {
        public string Name { get; set; }

        public string Brand { get; set; }

        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; }


        [Display(Name = "Engine/Fuel Type")]
        public string EngineAndFuelType { get; set; }

        public string Transmission { get; set; }

        public int Engine { get; set; }

        public int TopSpeed { get; set; }

        public int Mileage { get; set; }

        public int Range { get; set; }

        [Display(Name = "Seating Capacity")]
        public string SeatingCapacity { get; set; }

        [Display(Name = "Base Price")]
        public double PriceFrom { get; set; }

        [Display(Name = "Top-End Price")]
        public double PriceTo { get; set; }

        public int Ratings { get; set; }
    }
}
