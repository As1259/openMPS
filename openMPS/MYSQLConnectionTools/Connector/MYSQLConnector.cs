#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;

namespace de.fearvel.openMPS.MYSQLConnectionTools.Connector
{
    /// <summary>
    ///     Main MYSQL Connector Namespace
    /// </summary>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }

    /// <summary>
    ///     MYSQL CONNECTION CLASS  V1.0
    /// </summary>
    public class MYSQLConnector
    {
        /// <summary>
        ///     The SQL connection Variable
        /// </summary>
        private MySqlConnection sqlConn;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MSSQLConnector" /> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="database">The databaseName.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public MYSQLConnector(string serverName, string database, string username, string password)
        {
            connectToDatabase(serverName, database, username, password, 3306);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MYSQLConnector" /> class.
        ///     EXPERIMENTAL
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="database">The database.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="certificateFile">The certificate file.</param>
        /// <param name="certificatePassword">The certificate password.</param>
        public MYSQLConnector(string serverName, string database, string username, string password,
            string certificateFile, string certificatePassword)
        {
            connectToDatabase(serverName, database, username, password, 3306, certificateFile, certificatePassword);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MSSQLConnector" /> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="database">The database.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="port">The port.</param>
        public MYSQLConnector(string serverName, string database, string username, string password, int port)
        {
            connectToDatabase(serverName, database, username, password, port);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MYSQLConnector" /> class.
        ///     EXPERIMENTAL
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="database">The database.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="port">The port.</param>
        /// <param name="certificateFile">The certificate file.</param>
        /// <param name="certificatePassword">The certificate password.</param>
        public MYSQLConnector(string serverName, string database, string username, string password, int port,
            string certificateFile, string certificatePassword)
        {
            connectToDatabase(serverName, database, username, password, port, certificateFile, certificatePassword);
        }

        /// <summary>
        ///     Connects to the database.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="database">The database.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="port">The port.</param>
        public void connectToDatabase(string serverName, string database, string username, string password, int port)
        {
            try
            {
                sqlConn = new MySqlConnection("server='" + serverName + "';user='" + username + "';database='" +
                                              database + "';port=" + port + ";password='" + password +
                                              "';Ssl Mode=Required;");

                sqlConn.Open();
                Console.Out.WriteLine("");
                //MessageBox.Show(sqlConn.Credential.UserId, "wda", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                //  MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Connects to database.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="database">The database.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="port">The port.</param>
        /// <param name="certificateFile">The certificate file.</param>
        /// <param name="certificatePassword">The certificate password.</param>
        public void connectToDatabase(string serverName, string database, string username, string password, int port,
            string certificateFile, string certificatePassword)
        {
            try
            {
                sqlConn = new MySqlConnection("server='" + serverName + "';user='" + username + "';database='" +
                                              database + "';port=" + port + ";password='" + password +
                                              "';SSL Mode=Required;" + "CertificateFile=" + certificateFile +
                                              ";CertificatePassword=" + certificatePassword + ";");

                sqlConn.Open();
                Console.Out.WriteLine("");
                //MessageBox.Show(sqlConn.Credential.UserId, "wda", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                //  MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Executes a sql command
        /// </summary>
        /// <param name="sqlcmd">The SQLCMD.</param>
        /// <returns>DataTable</returns>
        public DataTable sqlShellDT(string sqlcmd)
        {
            var command = new MySqlCommand(sqlcmd, sqlConn);
            var dt = new DataTable();
            dt.Load(command.ExecuteReader());
            return dt;
        }

        /// <summary>
        ///     Executes a sql command
        /// </summary>
        /// <param name="sqlcmd">The SQLCMD.</param>
        public void sqlShell(string sqlcmd)
        {
            var command = new MySqlCommand(sqlcmd, sqlConn);
            command.ExecuteNonQuery();
        }

        /// <summary>
        ///     Executes a sql command
        /// </summary>
        /// <param name="sqlcmd">The SQLCMD.</param>
        /// <returns>SqlCommand</returns>
        public MySqlCommand sqlShellCMD(string sqlcmd)
        {
            return new MySqlCommand(sqlcmd, sqlConn);
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