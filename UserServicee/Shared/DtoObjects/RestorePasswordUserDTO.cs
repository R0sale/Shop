using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOObjects
{
    public class RestorePasswordUserDTO
    {
        public string? Email { get; set; }
        public string? NewPassword { get; set; }
    }
}
