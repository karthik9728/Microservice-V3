using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.DTO.Brand
{
    public class BrandUpdateDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? EstablishedYear { get; set; }
    }
}
