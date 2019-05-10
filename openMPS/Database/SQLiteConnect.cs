using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Runtime.CompilerServices;
using de.fearvel.net.Security;
using de.fearvel.net.SQL.Connector;
using de.fearvel.openMPS.DataTypes.Exceptions;

namespace de.fearvel.openMPS.Database
{
    /// <summary>
    /// abstract class for possible reuse purpose
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    public abstract class SqliteConnect
    {
        /// <summary>
        ///     Contains the connection to the config SQLITE
        /// </summary>
        private Dictionary<string, string> _directory;

        /// <summary>
        /// file name
        /// </summary>
        protected abstract string FileName { get; }

        /// <summary>
        /// standard file path
        /// </summary>
        protected string FilePath = "";

        /// <summary>
        /// Dictionary containing the Directory table content
        /// </summary>
        public Dictionary<string, string> Directory
        {
            get
            {
                if (_directory.Count == 0)
                {
                    ReadFromDirectory();
                }

                return _directory;
            }
            private set => _directory = value;
        }

        /// <summary>
        /// reloads the directory Dictionary data
        /// </summary>
        public void ReloadDirectory()
        {
            _directory.Clear();
            ReadFromDirectory();
        }

        /// <summary>
        /// reads the values from the directory into the dictionary
        /// </summary>
        private void ReadFromDirectory()
        {
            try
            {
                _connection.Query("select * from Directory;", out DataTable dt);
                foreach (DataRow ds in dt.Rows)
                {
                    _directory.Add(ds.Field<string>("Dkey"), ds.Field<string>("DVal"));
                }
            }
            catch (Exception)
            {
                throw new MPSSQLiteException();
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        protected SqliteConnect()
        {
            Directory = new Dictionary<string, string>();
        }


        /// <summary>
        /// The connection
        /// </summary>
        private SqliteConnector _connection;

        internal SqliteConnector GetConnector()
        {
            return _connection;
        }

        /// <summary>
        /// boolean that contains the information of the status of the Sqlite connection
        /// </summary>
        private bool _opened;

        /// <summary>
        /// abstract generate table function
        /// will be called on open
        /// </summary>
        public abstract void GenerateTables();

        /// <summary>
        /// opens the SqlLite database
        /// </summary>
        public void Open()
        {
            SqliteConnector c = new SqliteConnector(Path.Combine(FilePath, FileName), Ident.GetCPUId());
            _connection = c;
            _opened = true;
            GenerateTables();
        }


        /// <summary>
        /// Closes this connection.
        /// </summary>
        public void Close()
        {
            if (_opened)
                _connection.Close();
            else
                throw new MPSSQLiteException();
        }

        /// <summary>
        /// Execute sql quarry
        /// </summary>
        /// <param name="cmd">The command.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void NonQuery(string cmd)
        {
            if (_opened)
                _connection.NonQuery(cmd);
            else
                throw new MPSSQLiteException();
        }

        /// <summary>
        /// Execute sql quarry
        /// </summary>
        /// <param name="cmd">The command.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void NonQuery(SQLiteCommand cmd)
        {
            if (_opened)
                _connection.NonQuery(cmd);
            else
                throw new MPSSQLiteException();
        }

        /// <summary>
        /// Execute sql quarry
        /// returns DataTable
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DataTable Query(string cmd)
        {
            if (!_opened)
                throw new MPSSQLiteException();

            _connection.Query(cmd, out DataTable dt);
            return dt;
        }

        /// <summary>
        /// Execute sql quarry
        /// returns DataTable
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DataTable Query(SQLiteCommand cmd)
        {
            if (!_opened)
                throw new MPSSQLiteException();

            _connection.Query(cmd, out DataTable dt);
            return dt;
        }

        /// <summary>
        /// Inserts a key value pair into the database
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void InsertIntoDirectory(string key, string value)
        {
            using (var command = new SQLiteCommand(
                "Insert Into Directory"
                + " (DKey,DVal)"
                + " Values (@DKey,@DVal);"))
            {
                command.Parameters.AddWithValue("@DKey", key);
                command.Parameters.AddWithValue("@DVal", value);
                command.Prepare();
                NonQuery(command);
            }
        }
    }
}