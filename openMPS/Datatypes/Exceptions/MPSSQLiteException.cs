﻿// Copyright (c) 2018 / 2019, Andreas Schreiner

using System;

namespace de.fearvel.openMPS.DataTypes.Exceptions
{
    /// <summary>
    ///     Exception class for SQLiteZaehlerConfig
    /// </summary>
    public class MPSSQLiteException : ArgumentException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MPSSQLiteException" /> class.
        /// </summary>
        public MPSSQLiteException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MPSSQLiteException" /> class.
        /// </summary>
        /// <param name="e">The e.</param>
        public MPSSQLiteException(string e) : base(e)
        {
        }
    }
}