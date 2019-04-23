using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.fearvel.openMPS.DataTypes.Exceptions
{
    public class SnmpIdentNotFoundException : Exception
    {
        public SnmpIdentNotFoundException()
        {
        }

        public SnmpIdentNotFoundException(string message) : base(message)
        {
        }
    }
}
