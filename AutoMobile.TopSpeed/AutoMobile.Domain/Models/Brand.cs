using AutoMobile.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoMobile.Domain.Models
{
    public class Brand : BaseModel
    {
        [Required]
        public string Name { get; set; } 

        public int? EstablishedYear { get; set; }
    }
}
