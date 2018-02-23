#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Diagnostics;
using de.as1259.openMPS.SQLiteConnectionTools.Connector;

namespace de.as1259.openMPS.SQLiteConnectionTools
{
    /// <summary>
    ///     Contains the connection to the config SQLITE
    /// </summary>
    public static class CounterConfig
    {
        private const string ENCKEY = "aWHXzuLJxUWZ9UMSCpx4Y49Ubzh2h3QhQq7eHJP5vCVupybQMzJCtJnv9vmgp3r4";

        /// <summary>
        ///     The connection
        /// </summary>
        private static SQLiteConnector connection;

        /// <summary>
        ///     boolean that contains the information of the status of the Sqlite connection
        /// </summary>
        private static bool opened;

        /// <summary>
        ///     Opens the connection
        /// </summary>
        /// <param name="name">The name.</param>
        public static void open(string name)
        {
            try
            {
                connection = new SQLiteConnector(name);
                checkFile();
                opened = true;
            }
            catch (Exception)
            {
                throw new SQLiteZaehlerConfigException();
            }
        }

        public static void openENC(string name)
        {
            try
            {
                connection = new SQLiteConnector(name, ENCKEY);
                checkFile();
                opened = true;
            }
            catch (Exception)
            {
                open(name);
                enableENC();
            }
        }

        public static void enableENC()
        {
            connection.setPassword(ENCKEY);
        }

        public static void disableENC()
        {
            connection.setPassword("");
        }

        /// <summary>
        ///     Closes this connection.
        /// </summary>
        public static void close()
        {
            if (opened)
                connection.close();
            else
                throw new SQLiteZaehlerConfigException();
        }

        /// <summary>
        ///     Execute sql quarry
        /// </summary>
        /// <param name="cmd">The command.</param>
        public static void shell(string cmd)
        {
            if (opened)
                connection.sqlShell(cmd);
            else
                throw new SQLiteZaehlerConfigException();
        }

        /// <summary>
        ///     Execute sql quarry
        ///     returns DataTable
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        public static DataTable shellDT(string cmd)
        {
            if (opened)
                return connection.sqlShellDT(cmd);
            throw new SQLiteZaehlerConfigException();
        }

        /// <summary>
        ///     Checks the file.
        /// </summary>
        private static void checkFile()
        {
            try
            {
                connection.sqlShellDT("select * from INFO;");
            }
            catch (Exception e)
            {
                Debug.Write("\n\n!!! " + e);
                throw new SQLiteZaehlerConfigException();
            }
        }
    }

    /// <summary>
    ///     Exception class for SQLiteZaehlerConfig
    /// </summary>
    public class SQLiteZaehlerConfigException : ArgumentException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SQLiteZaehlerConfigException" /> class.
        /// </summary>
        public SQLiteZaehlerConfigException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SQLiteZaehlerConfigException" /> class.
        /// </summary>
        /// <param name="e">The e.</param>
        public SQLiteZaehlerConfigException(string e) : base(e)
        {
        }
    }
}