#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using de.fearvel.net.SQL.Connector;

namespace de.fearvel.openMPS.Database
{
    /// <summary>
    ///     contains the static sqlite connections.
    /// </summary>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }

    /// <summary>
    ///     Contains the connection to the Collector SQLITE
    /// </summary>
    public static class Collector
    {
        private const string Enckey = "aWHXzuLJxUWZ9UMSCpx4Y49Ubzh2h3QhQq7eHJP5vCVupybQMzJCtJnv9vmgp3r4";

        /// <summary>
        ///     The connection
        /// </summary>
        private static SqliteConnector _connection;

        /// <summary>
        ///     boolean that contains the information of the status of the Sqlite connection
        /// </summary>
        private static bool opened;

        /// <summary>
        ///     Opens the connection.
        /// </summary>
        /// <param name="name">The name.</param>
        public static void open(string name)
        {
            try
            {
                _connection = new SqliteConnector(name);
                checkFile();
                opened = true;
            }
            catch (Exception)
            {
                throw new SQLiteErfassungException();
            }
        }

        /// <summary>
        ///     Opens the connection.
        /// </summary>
        /// <param name="name">The name.</param>
        public static void openENC(string name)
        {
            try
            {
                _connection = new SqliteConnector(name, Enckey);
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
            _connection.SetPassword(Enckey);
        }

        public static void disableENC()
        {
            _connection.SetPassword("");
        }

        /// <summary>
        ///     Closes this connection.
        /// </summary>
        public static void close()
        {
            if (opened)
                _connection.Close();
            else
                throw new SQLiteErfassungException();
        }

        /// <summary>
        ///     executes sql quarry
        /// </summary>
        /// <param name="cmd">The command.</param>
        public static void shell(string cmd)
        {
            if (opened)
                _connection.NonQuery(cmd);
            else
                throw new SQLiteErfassungException();
        }

        /// <summary>
        ///     executes sql quarry
        ///     returns DataTable
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        public static DataTable shellDT(string cmd)
        {
            if (opened)
                return _connection.Query(cmd);
            throw new SQLiteErfassungException();
        }

        /// <summary>
        ///     Checks the file.
        /// </summary>
        private static void checkFile()
        {
            try
            {
                _connection.Query("select * from INFO;");
            }
            catch (Exception e)
            {
                Debug.Write(e.ToString());
                throw new SQLiteErfassungException();
            }
        }
    }

    /// <summary>
    ///     Exception class for SQLiteErfassung
    /// </summary>
    public class SQLiteErfassungException : ArgumentException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SQLiteErfassungException" /> class.
        /// </summary>
        public SQLiteErfassungException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SQLiteErfassungException" /> class.
        /// </summary>
        /// <param name="s">The s.</param>
        public SQLiteErfassungException(string s) : base(s)
        {
        }
    }
}