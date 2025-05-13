using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Guid productId) : base($"Product with id: {productId} does not exist in the db")
        { }
    }
}
