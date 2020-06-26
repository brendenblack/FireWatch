using Firewatch.Application.Common.Interfaces;
using System;

namespace Firewatch.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
