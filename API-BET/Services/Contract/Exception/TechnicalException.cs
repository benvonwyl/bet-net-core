using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_BET.Services.Contract
{
    public class TechnicalException : Exception
    {
        public TechnicalException(string name)
            : base(name)
        {

        }
    }
}
