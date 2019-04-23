using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Management;
using de.fearvel.net.Security;
using de.fearvel.net.SQL.Connector;
using de.fearvel.openMPS.DataTypes.Exceptions;

namespace de.fearvel.openMPS.Database
{
    public abstract class SqliteConnect 
    {
        /// <summary>
        ///     Contains the connection to the config SQLITE
        /// </summary>

        private Dictionary<string, string> _directory;

        protected abstract string FileName { get; }
   //     protected string FilePath = Path.Combine(Environment.GetFolderPath(
   //         Environment.SpecialFolder.ApplicationData), "oMPS");
        protected string FilePath = "";
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

        public void ReloadDirectory()
        {
            _directory.Clear();
            ReadFromDirectory();
        }

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

        public void UpdateDirectory()
        {
            _directory.Clear();
            ReadFromDirectory();
        }
        protected SqliteConnect()
        {
            Directory = new Dictionary<string, string>();
        }


        /// <summary>
        ///     The connection
        /// </summary>
        private SqliteConnector _connection;

        internal SqliteConnector GetConnector()
        {
            return _connection;
        }

        /// <summary>
        ///     boolean that contains the information of the status of the Sqlite connection
        /// </summary>
        private bool _opened;

     public abstract void GenerateTables();


      public void Open()
      {
          SqliteConnector c = new SqliteConnector(Path.Combine(FilePath, FileName), Ident.GetCPUId());
          _connection = c;
          _opened = true;
          GenerateTables();
      }


        /// <summary>
        ///     Closes this connection.
        /// </summary>
        public void Close()
        {
            if (_opened)
                _connection.Close();
            else
                throw new MPSSQLiteException();
        }

        /// <summary>
        ///     Execute sql quarry
        /// </summary>
        /// <param name="cmd">The command.</param>
        public void NonQuery(string cmd)
        {
            if (_opened)
                _connection.NonQuery(cmd);
            else
                throw new MPSSQLiteException();
        }

        /// <summary>
        ///     Execute sql quarry
        /// </summary>
        /// <param name="cmd">The command.</param>
        public void NonQuery(SQLiteCommand cmd)
        {
            if (_opened)
                _connection.NonQuery(cmd);
            else
                throw new MPSSQLiteException();
        }

        /// <summary>
        ///     Execute sql quarry
        ///     returns DataTable
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        public DataTable Query(string cmd)
        {
            if (!_opened)
                throw new MPSSQLiteException();

             _connection.Query(cmd, out DataTable dt);
            return dt;
        }
        /// <summary>
        ///     Execute sql quarry
        ///     returns DataTable
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        public DataTable Query(SQLiteCommand cmd)
        {
            if (!_opened)
                throw new MPSSQLiteException();

            _connection.Query(cmd, out DataTable dt);
            return dt;
        }
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

