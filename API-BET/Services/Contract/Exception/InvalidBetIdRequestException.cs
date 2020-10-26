using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_BET.Services.Contract
{
    public class InvalidBetIdRequestException : Exception
    {
        public InvalidBetIdRequestException(string name)
            : base(name)
        {

        }
    }
}
