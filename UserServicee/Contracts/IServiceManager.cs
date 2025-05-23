﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IServiceManager
    {
        IUserService UserService { get; }
        IAuthenticationService AuthenticationService { get; }
    }
}
