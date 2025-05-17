using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(Guid ownerId) : base($"Owner with id: {ownerId} does not exist in the db")
        { }
    }
}
