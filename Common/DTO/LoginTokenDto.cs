﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class LoginTokenDto
    {
        public string TokenKey { get; set; }
        public string Token { get; set; }
        public string MainUrl { get; set; }
    }
}
