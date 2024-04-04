using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ClaimsPlugin.Domain.SharedKernel
{
   public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken token = default);
    }
}