#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace de.fearvel.openMPS.SQLiteConnectionTools.Connector
{
    /// <summary>
    ///     Sqlite main connection
    /// </summary>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }

    /// <summary>
    ///     The SQLite Management Class
    /// </summary>
    public class SQLiteConnector
    {
        /// <summary>
        ///     The SQL connection Var
        /// </summary>
        private SQLiteConnection sqlConn;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlControl" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public SQLiteConnector(string fileName)
        {
            //  createNewDatabase(fileName);
            connectToDatabase(fileName);
        }

        public SQLiteConnector(string fileName, string key)
        {
            //  createNewDatabase(fileName);
            connectToENCDatabase(fileName, key);
        }

        /// <summary>
        ///     Creates the new database.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void createNewDatabase(string fileName)
        {
            SQLiteConnection.CreateFile(fileName);
        }

        public void setPassword(string key)
        {
            sqlConn.ChangePassword(key);
        }

        /// <summary>
        ///     Connects to database.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private void connectToDatabase(string fileName)
        {
            sqlConn = new SQLiteConnection("Data Source=" + fileName + ";Version=3;");
            sqlConn.Open();
        }

        /// <summary>
        ///     Connects to database.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private void connectToENCDatabase(string fileName, string key)
        {
            sqlConn = new SQLiteConnection("Data Source=" + fileName + ";Version=3;Password=" + key + ";");
            sqlConn.Open();
        }

        /// <summary>
        ///     SQLs Access to the Class Obj which gets a command by thie Method.
        /// </summary>
        /// <param name="sqlcmd">The SQLCMD.</param>
        public void sqlShell(string sqlcmd)
        {
            var command = new SQLiteCommand(sqlcmd, sqlConn);
            command.ExecuteNonQuery();
        }

        /// <summary>
        ///     SQLs Access to the Class Obj which gets a command by thie Method.
        ///     returns a DataTable
        /// </summary>
        /// <param name="sqlcmd">The SQLCMD.</param>
        /// <returns>DataTable4</returns>
        public DataTable sqlShellDT(string sqlcmd)
        {
            var dt = new DataTable();

            try
            {
                var command = new SQLiteCommand(sqlcmd, sqlConn);
                dt.Load(command.ExecuteReader());
                return dt;
            }
            catch (Exception)
            {
                for (var i = 0; i < dt.GetErrors().Length; i++) Debug.Write(dt.GetErrors()[i].RowError);
                throw;
            }
        }

        /// <summary>
        ///     Closes this connection.
        /// </summary>
        public void close()
        {
            sqlConn.Close();
        }
    }
}