using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using de.fearvel.net.DataTypes.Exceptions;
using de.fearvel.net.Security.Crypto;

namespace de.fearvel.openMPS.Security
{


    /// <summary>
    /// TESTING
    /// </summary>
    public class InitInformationManager
    {



        /// <summary>
        /// the Instance of this Singleton
        /// </summary>
        private static InitInformationManager _instance;

        /// <summary>
        /// GetInstance for the Singleton
        /// </summary>
        /// <returns>instance</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static InitInformationManager GetInstance()
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
        public static void SetInstance(string file, string key, string iv)
        {
            _instance = new InitInformationManager(file,  key,  iv);
        }

        private InitInformationManager(string file, string key, string iv)
        {
            if (File.Exists(file))
            {
                SimpleDES.Decrypt(OpenFile(file),key,iv);

            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        private string OpenFile(string file)
        {
            var fs = File.OpenRead(file);
            StreamReader read = new StreamReader(fs);
            return read.ReadToEnd();
        }
    }
}