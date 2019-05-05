// Copyright (c) 2018 / 2019, Andreas Schreiner

using System;

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
