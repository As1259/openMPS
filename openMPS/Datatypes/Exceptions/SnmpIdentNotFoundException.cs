
using System;

namespace de.fearvel.openMPS.DataTypes.Exceptions
{
    /// <summary>
    /// Exception for a missing identifier
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    public class SnmpIdentNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpIdentNotFoundException" /> class.
        /// </summary>
        public SnmpIdentNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpIdentNotFoundException" /> class.
        /// </summary>
        /// <param name="e">The e.</param>
        public SnmpIdentNotFoundException(string message) : base(message)
        {
        }
    }
}
