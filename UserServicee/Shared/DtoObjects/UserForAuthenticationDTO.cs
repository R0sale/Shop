﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOObjects
{
    public class UserForAuthenticationDTO
    {
        [Required(ErrorMessage = "Username is a required field")]
        public string? UserName { get; init; }
        [Required(ErrorMessage = "Password is a required field")]
        public string? Password { get; init; }
    }
}
