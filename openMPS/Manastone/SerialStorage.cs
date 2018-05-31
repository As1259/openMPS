#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Data.HashFunction;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Management;
using de.fearvel.sql.connectors;

namespace de.fearvel.manastone.serialManagement
{
    public static class SerialStorage
    {
        private static readonly string ManaLocation =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Manastone License System";

        private static readonly string Manafile = "\\MANASTONE.L";
        private static SQLITEConnector _sqlCon;

        private static string CreateHashedKey()
        {
            var os = new ManagementObject("Win32_OperatingSystem=@");
            var sh = new SpookyHashV2();
            var b = sh.ComputeHash((string) os["SerialNumber"] + (string) os["InstallDate"]);
            return b.Aggregate("", (current, t) => current + t);
        }

        internal static void OpenSerialStorage()
        {
            if (!Directory.Exists(ManaLocation))
                Directory.CreateDirectory(ManaLocation);

            if (File.Exists(ManaLocation + Manafile))
                _sqlCon = new SQLITEConnector(ManaLocation + Manafile, CreateHashedKey());
            else CreateSerialStorage();
            //    _sqlCon.SetPassword("");
        }

        private static void CreateSerialStorage()
        {
            _sqlCon = new SQLITEConnector(ManaLocation + Manafile);
            _sqlCon.SetPassword(CreateHashedKey());
            CreateStructure();
        }

        public static void AddSerialNumber(string serial, int programId, string activationKey)
        {
            if (CheckForStoredProgramId(programId).Length != 0 || CheckForStoredSerialNumber(serial)) return;
            const string s = "Insert into MANASERIAL " +
                             "(MSE_Serial, MSE_ProgramID,MSE_ActivationKey) " +
                             "Values " +
                             "(@MSESerial, @MSEProgramID, @MSEActivationKey)";

            var com = new SQLiteCommand(s);
            com.Parameters.AddWithValue("@MSESerial", serial);
            com.Parameters.AddWithValue("@MSEProgramID", programId);
            com.Parameters.AddWithValue("@MSEActivationKey", activationKey);
            com.Prepare();
            _sqlCon.ShellCommand(com);
        }

        public static string CheckForStoredProgramId(int programId)
        {
            if (_sqlCon == null) OpenSerialStorage();
            const string s = "Select MSE_Serial from MANASERIAL where MSE_ProgramID = @MSEProgramID";
            var com = new SQLiteCommand(s);
            com.Parameters.AddWithValue("@MSEProgramID", programId);
            com.Prepare();
            var dt = _sqlCon.ShellDTCommand(com);
            return dt.Rows.Count == 1 ? dt.Rows[0].Field<string>("MSE_Serial") : "";
        }

        public static bool CheckForStoredSerialNumber(string serial)
        {
            if (_sqlCon == null) OpenSerialStorage();
            const string s = "Select MSE_Serial from MANASERIAL where MSE_Serial = @MSESerial";
            var com = new SQLiteCommand(s);
            com.Parameters.AddWithValue("@MSESerial", serial);
            com.Prepare();
            var dt = _sqlCon.ShellDTCommand(com);
            return dt.Rows.Count == 1 ? true : false;
        }

        public static void DeleteSerial(string serial)
        {
            if (_sqlCon == null) OpenSerialStorage();
            const string s = "Delete from MANASERIAL where MSE_Serial = @MSESerial";
            var com = new SQLiteCommand(s);
            com.Parameters.AddWithValue("@MSESerial", serial);
            com.Prepare();
            _sqlCon.ShellCommand(com);
        }

        private static void CreateStructure()
        {
            try
            {
                var sqlCreate = "CREATE TABLE INFO " +
                                "( " +
                                "  MSI_Version INTEGER NOT NULL " +
                                "); ";
                _sqlCon.Shell(sqlCreate);
                sqlCreate = "CREATE TABLE MANASERIAL " +
                            "( " +
                            "  MSE_ID Integer PRIMARY KEY AUTOINCREMENT, " +
                            "  MSE_Serial varchar(500) NOT NULL, " +
                            "  MSE_DateAdded datetime NOT NULL Default CURRENT_TIMESTAMP, " +
                            "  MSE_ProgramID INTEGER NOT NULL, " +
                            "  MSE_ActivationKey varchar(500) NOT NULL " +
                            ");";
                _sqlCon.Shell(sqlCreate);
                _sqlCon.Shell("Insert into INFO (MSI_Version) values (1);");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void Close()
        {
            _sqlCon.Close();
        }
    }
}