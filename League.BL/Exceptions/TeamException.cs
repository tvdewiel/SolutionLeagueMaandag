using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.BL.Exceptions
{
    public class TeamException : Exception
    {
        public TeamException(string? message) : base(message)
        {
        }

        public TeamException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
