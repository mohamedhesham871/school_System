using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
   public class NullRefrenceException(string message): Exception($"{message}  : Can't perform this operation on null object Or value")
    {
    }
}
