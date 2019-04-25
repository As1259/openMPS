using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using de.fearvel.net.DataTypes;
using de.fearvel.net.DataTypes.Exceptions;
using de.fearvel.net.DataTypes.SocketIo;
using de.fearvel.net.FnLog;
using de.fearvel.net.Manastone;
using de.fearvel.net.SocketIo;
using de.fearvel.openMPS.Database;
using de.fearvel.openMPS.DataTypes;
using Newtonsoft.Json;

using Formatting = Newtonsoft.Json.Formatting;

namespace de.fearvel.openMPS.Net
{
    // ReSharper disable once InconsistentNaming
    public sealed class OpenMPSClient
    {
        /// <summary>
        /// The Server Url
        /// </summary>
        private readonly string _url;


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
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "CheckOidVersion");
            if (Config.GetInstance().Directory.TryGetValue("OidVersion", out string instVer))
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", " CheckOidVersion TryGetValue done");

                var ver = SocketIoClient.RetrieveSingleValue<VersionWrapper>(_url,
                    "OidVersionOffer", "OidVersionRequest", null, timeout: 30000);
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "OidVersionOffer received");

                if (System.Version.TryParse(instVer, out Version instVersion) &&
                    System.Version.TryParse(ver.Version, out Version version))
                {
                    FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "before assigning out");

                    oidServerVersion = ver.Version;
                    FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "CheckOidVersion Complete");
                    return (instVersion.CompareTo(version) >= 0);
                }
            }
            FnLog.GetInstance().AddToLogList(FnLog.LogType.CriticalError, "OpenMPSClient", "Missing Directory OidVersion Value");
            throw new QueryFailedException();
        }

        private bool CheckMinClientVersion()
        {
            try
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "CheckMinClientVersion");
               var ver = SocketIoClient.RetrieveSingleValue<VersionWrapper>(_url,
                    "MPSMinClientVersionOffer", "MPSMinClientVersionRequest", null, timeout: 30000);
                var progVersion =  System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

                if (System.Version.TryParse(progVersion, out Version programVersion) &&
                    System.Version.TryParse(ver.Version, out Version minversion))
                {
                    FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "CheckMinClientVersion Complete");
                    return (programVersion.CompareTo(minversion) >= 0);
                }
                FnLog.GetInstance().AddToLogList(FnLog.LogType.Error, "OpenMPSClient", "CheckMinClientVersion ResultNullOrNotReceivedException");
                throw new ResultNullOrNotReceivedException();

            }
            catch (Exception e)
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.Error, "OpenMPSClient", "CheckMinClientVersion" + e.Message);
                MessageBox.Show("Could not reach the openMPS Server");
                throw new ResultNullOrNotReceivedException();
            }

        }

        public void CheckForCompatibleVersion()
        {
            try
            {
                if (!CheckMinClientVersion())
                {
                    FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "CheckForCompatibleVersion expired version found");
                    FnLog.GetInstance().ProcessLogList();
                    MessageBox.Show("openMPS Version veraltet!!\nBitte laden Sie die neuste Version herunter");
                    Environment.Exit(1);
                }
            }
            catch (Exception e)
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.Error, "OpenMPSClient", "CheckMinClientVersion" + e.Message);
                MessageBox.Show("Could not reach the openMPS Server");
                Environment.Exit(1);
            }
        }


        private void DownloadAndUpdateOidTable(string oidVersion)
        {
            ManastoneClient.GetInstance().CheckToken();

            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "DownloadAndUpdateOidTable - " +ManastoneClient.GetInstance().Token);
                var oid = SocketIoClient.RetrieveSingleValue<List<Oid>>(_url,
                    "OidOffer", "OidRequest", new OidRequest(ManastoneClient.GetInstance().Token).Serialize(), timeout: 30000);
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "DownloadAndUpdateOidTable received" + oid.Count);
            Config.GetInstance().UpdateOids(oidVersion, oid);
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "DownloadAndUpdateOidTable Complete");

            

        }

        public void UpdateOidTable()
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "UpdateOidTable");

           var thread = new Thread(UpdateOidTableThreaded);
            thread.Start();
         
        }

        public void UpdateOidTableThreaded()
        {
            try
            {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "UpdateOidTableThreaded");

                if (!CheckOidVersion(out string oidServerVersion))
                {
                    FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "UpdateOidTableThreaded newer version detected");
                    DownloadAndUpdateOidTable(oidServerVersion);
                }
            }
            catch (Exception e) {
                FnLog.GetInstance().AddToLogList(FnLog.LogType.Error, "OpenMPSClient", "UpdateOidTableThreaded " + e.Message);

            }
        }

        public void SendOidData(List<OidData> data)
        {
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "SendOidData");
            var dataStr = JsonConvert.SerializeObject(data, Formatting.Indented).Trim()
                .Replace(System.Environment.NewLine, "");
            var res = SocketIoClient.RetrieveSingleValue<SimpleResult>(_url,
                "closer", "SendData", dataStr, timeout: 30000);
            FnLog.GetInstance().AddToLogList(FnLog.LogType.MajorRuntimeInfo, "OpenMPSClient", "SendOidData sent");

        }
    }
}