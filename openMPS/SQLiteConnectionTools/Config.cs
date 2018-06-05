#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.CompilerServices;

namespace de.fearvel.openMPS.SQLiteConnectionTools
{
    /// <summary>
    ///     Contains the connection to the config SQLITE
    /// </summary>
    public class Config : SqLiteConnect
    {
        private static Config _instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Config GetInstance()
        {
            return _instance ?? (_instance = new Config());
        }

        protected override string FileName => "config.db";

        public override void GenerateTables()
        {
            GenerateInformationTable();
            GenerateCredentialsTable();
            GenerateDevicesTable();
        }

        public void GenerateInformationTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS Version" +
                     " (Identifier varchar(200),val varchar(30)," +
                     " CONSTRAINT uq_Version_Identifier UNIQUE (Identifier));");
            if (Query("SELECT * FROM Version").Rows.Count == 0)
            {
                NonQuery("INSERT INTO Version (Identifier,val) VALUES ('MPS'," +
                         "'" + FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion + "');");
            }
        }
        public void GenerateCredentialsTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS CREDENTIALS" +
                     " (id INTEGER NOT NULL" +
                     " CONSTRAINT pk_CREDENTIALS_id PRIMARY KEY AUTOINCREMENT," +
                     " Name varchar(200) NOT NULL DEFAULT ''," +
                     " Value varchar(200) NOT NULL DEFAULT '');");
        }
        public void GenerateDevicesTable()
        {
            NonQuery("CREATE TABLE IF NOT EXISTS DEVICES" +
                     " (Aktiv BOOL NOT NULL DEFAULT 'true'," +
                     " IP varchar(39) NOT NULL DEFAULT ''," +
                     " Modell varchar(250)," +
                     " Seriennummer varchar(250)," +
                     " AssetNumber varchar(250) NOT NULL DEFAULT ''," +
                     " id INTEGER NOT NULL CONSTRAINT pk_DEVICES_id PRIMARY KEY AUTOINCREMENT);");
        }
       
    }

}