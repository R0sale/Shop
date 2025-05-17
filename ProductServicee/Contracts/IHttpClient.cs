using Shared.DTOObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IHttpClient
    {
        Task<UserDTO> GetUser(Guid id);
    }
}
