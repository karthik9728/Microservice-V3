﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.DTO.User
{
    public class LoginResponseDto
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
