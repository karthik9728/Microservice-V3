﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.Models
{
    public class SubMenu
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int MenuId { get; set; }

        public Menu Menu { get; set; }

        public bool IsActive { get; set; }
    }
}
