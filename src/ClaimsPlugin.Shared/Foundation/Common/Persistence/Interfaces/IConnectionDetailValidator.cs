using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimsPlugin.Shared.Foundation.Common.Persistence.Interfaces
{
    public interface IConnectionDetailValidator
    {
        bool TryValidate(string connectionDetail);
    }
}
