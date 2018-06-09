using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.fearvel.openMPS.Database.Exceptions
{
    class OidUpdateFileException : Exception
    {
        public OidUpdateFileException()
        {
        }
        public OidUpdateFileException(string s) : base(s)
        {
        }
    }
}
