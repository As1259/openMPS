using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Xml;
using de.fearvel.net.DataTypes;
using de.fearvel.net.SocketIo;
using de.fearvel.openMPS.Database;
using de.fearvel.net.DataTypes.Exceptions;
using de.fearvel.openMPS.DataTypes;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace de.fearvel.openMPS.SocketIo
{
    // ReSharper disable once InconsistentNaming
    public sealed class OpenMPSClient
    {
        /// <summary>
        /// The Server Url
        /// </summary>
        private string _url;


        /// <summary>
        /// the Instance of this Singleton
        /// </summary>
        private static OpenMPSClient _instance;

        /// <summary>
        /// GetInstance for the Singleton
        /// </summary>
        /// <returns>instance</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static OpenMPSClient GetInstance()
        {
            return _instance ?? throw new InstanceNotSetException();
        }

        /// <summary>
        /// Sets the Instance of OpenMPSClient
        /// Used to preset values like the Server URL
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void SetInstance(string serverUrl)
        {
            _instance = new OpenMPSClient(serverUrl);
        }

        /// <summary>
        /// Private Constructor
        /// </summary>
        /// <param name="serverUrl"></param>
        private OpenMPSClient(string serverUrl)
        {
            _url = serverUrl;
        }

        /// <summary>
        /// Checks Oif Version
        /// Returns true if Version greater/equal than Server Version
        /// Returns false if Version smaller than Server Version
        /// </summary>
        /// <returns></returns>
        public bool CheckOidVersion(out string oidServerVersion)
        {
            if (Config.GetInstance().Directory.TryGetValue("OidVersion", out string instVer))
            {
                var ver = SocketIoClient.RetrieveSingleValue<VersionWrapper>("https://localhost:9051",
                    "OidVersion", "OidVersionRequest", null);

                if (System.Version.TryParse(instVer, out Version instVersion) &&
                    System.Version.TryParse(ver.Version, out Version version))
                {
                    oidServerVersion = ver.Version;
                    return (instVersion.CompareTo(version) >= 0);
                }
            }

            throw new QueryFailedException();
        }

        private void DownloadAndUpdateOidTable(string oidVersion)
        {
            var oid = SocketIoClient.RetrieveSingleValue<List<Oid>>(_url,
                "OidOffer", "OidRequest", "");
            Config.GetInstance().UpdateOids(oidVersion, oid);
        }

        public void UpdateOidTable()
        {
            var thread = new Thread(UpdateOidTableThreaded);
            thread.Start();
         
        }

        public void UpdateOidTableThreaded()
        {
            try
            {
                if (!CheckOidVersion(out string oidServerVersion))
                {
                    DownloadAndUpdateOidTable(oidServerVersion);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void SendOidData(List<OidData> data)
        {
            var dataStr = JsonConvert.SerializeObject(data, Formatting.Indented).Trim()
                .Replace(System.Environment.NewLine, "");
            var ver = SocketIoClient.RetrieveSingleValue<VersionWrapper>(_url,
                "closer", "SendData", dataStr);
        }
    }
}