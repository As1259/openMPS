#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using de.fearvel.net.SQL.Connector;
using de.fearvel.openMPS.Database.Exceptions;

namespace de.fearvel.openMPS.Database
{
    /// <summary>
    ///     Contains the connection to the config SQLITE
    /// </summary>
    public class InitialisationFile
    {
        private SqliteConnector _connection;
        public InitialisationFile(string fileName)
        {
            _connection = new SqliteConnector(fileName);
        }

        public Dictionary<string, string> LoadInitialSettings()
        {
            try
            {
                var directory = new Dictionary<string, string>();
                foreach (DataRow ds in _connection.Query("select * from Directory;").Rows)
                {
                    directory.Add(ds.Field<string>("Identifier"), ds.Field<string>("val"));
                }
                return directory;
            }
            catch (Exception)
            {
                throw new MPSSQLiteException();
            }
        }



    }

}