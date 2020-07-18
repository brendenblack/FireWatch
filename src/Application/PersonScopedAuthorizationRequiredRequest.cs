using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Application
{
    public abstract class PersonScopedAuthorizationRequiredRequest
    {
        /// <summary>
        /// The id of the owner of the requested resource.
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// The id of the user who is requesting this resource.
        /// </summary>
        public string RequestorId { get; set; }
    }
}
