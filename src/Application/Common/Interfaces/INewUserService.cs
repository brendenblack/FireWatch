using Firewatch.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Firewatch.Application.Common.Interfaces
{
    public interface INewUserService
    {
        public Task<Result> InitializeNewUser(string personId, CancellationToken cancellationToken = new CancellationToken());
    }
}
