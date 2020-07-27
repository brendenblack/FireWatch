using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Domain.Exceptions
{
    public class OptionContractInvalidException : Exception
    {
        public OptionContractInvalidException(string input, Exception ex)
            : base($"An exception occurred while parsing option contract \"{input}\".", ex)
        {
        }

        public OptionContractInvalidException(string input)
            : base($"Unable to understand option contract \"{input}\".")
        { 
        }

    }
}
