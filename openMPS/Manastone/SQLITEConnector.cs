#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace de.fearvel.sql.connectors
{
    public class SQLITEConnector : SQLConnector
    {
        public SQLITEConnector(string fileName)
        {
            connectToDatabase(fileName);
        }

        public SQLITEConnector(string fileName, string enckey)
        {
            try
            {
                connectToDatabase(fileName, enckey);
            }
            catch (Exception)
            {
                connectToDatabase(fileName);
                SetPassword(enckey);
            }
        }

        public void connectToDatabase(string fileName, string enckey = "")
        {
            try
            {
                ConStr = new SQLiteConnectionStringBuilder();
                ((SQLiteConnectionStringBuilder) ConStr).DataSource = fileName;
                ((SQLiteConnectionStringBuilder) ConStr).Version = 3;
                if (enckey.Length != 0) ((SQLiteConnectionStringBuilder) ConStr).Password = enckey;
                var s = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                if (!File.Exists(fileName)) SQLiteConnection.CreateFile(fileName);
                Connect = new SQLiteConnection(ConStr.ConnectionString + ";");
                Connect.Open();
                Address = fileName;
                Type = "SQLite";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public void SetPassword(string pass)
        {
            ((SQLiteConnection) Connect).ChangePassword(pass);
        }

        public override DataTable ShellDt(string sqlCMD)
        {
            return SqlShellDt(new SQLiteCommand(sqlCMD, (SQLiteConnection) Connect));
        }

        public override void Shell(string sqlCMD)
        {
            SqlShell(new SQLiteCommand(sqlCMD, (SQLiteConnection) Connect));
        }

        public void ShellCommand(SQLiteCommand sqlCom)
        {
            sqlCom.Connection = (SQLiteConnection) Connect;
            SqlShell(sqlCom);
        }

        public DataTable ShellDTCommand(SQLiteCommand sqlCom)
        {
            sqlCom.Connection = (SQLiteConnection) Connect;
            return SqlShellDt(sqlCom);
        }

        public override DataTable GetTableNames()
        {
            throw new NotImplementedException();
        }
    }
}