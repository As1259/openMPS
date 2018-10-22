using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Management;
using de.fearvel.net.SQL.Connector;
using de.fearvel.openMPS.Database.Exceptions;

namespace de.fearvel.openMPS.Database
{
    public abstract class SqLiteConnect
    {
        /// <summary>
        ///     Contains the connection to the config SQLITE
        /// </summary>

        private Dictionary<string, string> _directory;

        protected abstract string FileName { get; }
        protected string FilePath = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "oMPS");
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

        private void ReadFromDirectory()
        {
            try
            {
                _connection.Query("select * from Directory;", out DataTable dt);
                foreach (DataRow ds in dt.Rows)
                {
                    _directory.Add(ds.Field<string>("Identifier"), ds.Field<string>("val"));
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
        protected SqLiteConnect()
        {
            Directory = new Dictionary<string, string>();
        }


        /// <summary>
        ///     The connection
        /// </summary>
        private SqliteConnector _connection;

        /// <summary>
        ///     boolean that contains the information of the status of the Sqlite connection
        /// </summary>
        private bool _opened;

        /// <summary>
        ///     Opens the connection
        /// </summary>
        /// <param name="name">The name.</param>
        public virtual void Open(string name)
        {
            try
            {
                _connection = new SqliteConnector(name);
                ReadFromDirectory();
                _opened = true;
            }
            catch (Exception)
            {
                throw new MPSSQLiteException();
            }
        }

        public virtual void OpenEncrypted(string name)
        {
            try
            {
                _connection = new SqliteConnector(name, GetCPUUID());
                ReadFromDirectory();
                _opened = true;
            }
            catch (Exception)
            {
                Open(name);
                EnableEncryption();
            }
        }
        protected virtual void OpenWithCustomEncrypted(string name, string key)
        {
            try
            {
                _connection = new SqliteConnector(name, key);
                ReadFromDirectory();
                _opened = true;
            }
            catch (Exception)
            {
                Open(name);
                EnableEncryption();
            }
        }

        public abstract void GenerateTables();

        public void OpenEncrypted()
        {
            CheckPath();
            OpenEncrypted(Path.Combine(FilePath, FileName));
            GenerateTables();
        }

        public void Open()
        {
            CheckPath();
            SqliteConnector c = new SqliteConnector(Path.Combine(FilePath, FileName));
            _connection = c;
            _opened = true;
            //  Open(Path.Combine(FilePath, FileName));
            GenerateTables();
        }


        private string GetCPUUID()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (var o in moc)
            {
                var mo = (ManagementObject)o;
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }

            return cpuInfo;
        }



        public void EnableEncryption()
        {
            _connection.SetPassword(GetCPUUID());
        }

        public void DisableEncryption()
        {
            _connection.SetPassword("");
        }

        private void CheckPath()
        {
            if (System.IO.Directory.Exists(FilePath)) return;
            System.IO.Directory.CreateDirectory(FilePath);
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
                + " (Identifier,val)"
                + " Values (@Identifier,@val);"))
            {
                command.Parameters.AddWithValue("@Identifier", key);
                command.Parameters.AddWithValue("@val", value);
                command.Prepare();
                NonQuery(command);
            }
        }
    }
}

