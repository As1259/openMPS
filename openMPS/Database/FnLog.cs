using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using de.fearvel.fnLogger;

namespace de.fearvel.openMPS.Database
{
    class FnLog : FnLogController
    {
        private static FnLog _instance = null;

        public FnLog(string path, string fileName, string encKey) : base(path, fileName, encKey)
        {
        }

        public static void SetInstance(string path, string fileName, string encKey)
        {
            _instance = new FnLog( path,  fileName,  encKey);
        }

        public static FnLog GetInstance() => _instance;

        protected override string LogServer() => "https://127.0.0.1:9024/SendLog";

        protected override string ProgramName() => "openMPS V." + GetFileVersion();

        protected override TelemetryType Telemetry() => TelemetryType.LogLocalSendAll;

        private string GetFileVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}
