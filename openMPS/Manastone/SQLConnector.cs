#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Data.Common;

namespace de.fearvel.sql.connectors
{
    public abstract class SQLConnector
    {
        private const string _VERSION = "V 2.0";
        protected DbConnection Connect;
        protected DbConnectionStringBuilder ConStr;

        public bool IsOpen
        {
            get
            {
                if (Connect == null) return false;
                return true;
            }
        }

        public string ConnectionName { set; get; } = "";

        public string Address { get; set; }

        public int Port { get; set; }

        public string Database { get; set; }

        public string User { get; set; }

        public string Version => _VERSION;

        public bool SSLMode { get; set; }

        public string Type { get; set; }

        public DataTable SqlShellDt(DbCommand com)
        {
            var command = com;
            var dt = new DataTable();
            dt.Load(command.ExecuteReader());
            return dt;
        }

        public void SqlShell(DbCommand com)
        {
            var command = com;
            command.ExecuteNonQuery();
        }

        public void Close()
        {
            if (IsOpen)
            {
                Connect.Close();
                Connect = null;
                GC.Collect();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public abstract DataTable ShellDt(string sqlCMD);

        public abstract void Shell(string sqlCMD);

        public abstract DataTable GetTableNames();
    }
}