using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Management;
using de.fearvel.net.SQL.Connector;
namespace de.fearvel.openMPS.SQLiteConnectionTools
{
    public abstract class SqLiteConnect
    {
        /// <summary>
        ///     Contains the connection to the config SQLITE
        /// </summary>

        private Dictionary<string, string> _version;

        protected abstract string FileName { get; }
        protected string FilePath = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "oMPS");
        public Dictionary<string, string> Version
        {
            get
            {
                if (_version.Count == 0)
                {
                    GetVersion();
                }

                return _version;

            }
            private set => _version = value;
        }

        private void GetVersion()
        {
            try
            {
                foreach (DataRow ds in _connection.Query("select * from Version;").Rows)
                {
                    _version.Add(ds.Field<string>("Identifier"), ds.Field<string>("val"));
                }
            }
            catch (Exception)
            {
                throw new SQLiteConnectionTools.MPSSQLiteException();
            }
        }


        protected SqLiteConnect()
        {
            Version = new Dictionary<string, string>();
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
                GetVersion();
                _opened = true;
            }
            catch (Exception)
            {
                throw new SQLiteConnectionTools.MPSSQLiteException();
            }
        }
       
        public virtual void OpenEncrypted(string name)
        {
            try
            {
                _connection = new SqliteConnector(name, GetCPUUID());
                GetVersion();
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
                GetVersion();
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
            if (Directory.Exists(FilePath)) return;
            Directory.CreateDirectory(FilePath);
        }
        /// <summary>
        ///     Closes this connection.
        /// </summary>
        public void Close()
        {
            if (_opened)
                _connection.Close();
            else
                throw new SQLiteConnectionTools.MPSSQLiteException();
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
                throw new SQLiteConnectionTools.MPSSQLiteException();
        }

        /// <summary>
        ///     Execute sql quarry
        ///     returns DataTable
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        public DataTable Query(string cmd)
        {
            if (_opened)
                return _connection.Query(cmd);
            throw new SQLiteConnectionTools.MPSSQLiteException();
        }
    }
}

