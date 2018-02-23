#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using de.as1259.manastone.serialManagement.InstallBased;

namespace de.as1259.manastone.serialManagement
{
    public static class SerialManager
    {
        public static bool ApplySerial(string s)
        {
            try
            {
                Serial t = GetSerialType(s);
                if (!t.CheckAndStore(s)) throw new ArgumentException();
                SerialStorage.OpenSerialStorage();
                if (SerialStorage.CheckForStoredSerialNumber(t.ContainedSerial.SerialNumber))
                    throw new ArgumentException();
                SerialStorage.AddSerialNumber(t.ContainedSerial.SerialNumber,
                    t.ContainedSerial.ProgramIdentificationNumber, "0");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static SerialContainer GetSerialContainer(int programid)
        {
            var serial = SerialStorage.CheckForStoredProgramId(programid);
            if (serial.Length == 0) return null;
            var t = new I01();
            return t.StoreSerial(serial);
        }

        public static bool ActivateSerialNumber()
        {
            return true;
        }

        public static bool DeactivateSerialNumber()
        {
            return true;
        }

        public static bool CheckLicence(int programid)
        {
            var serial = SerialStorage.CheckForStoredProgramId(programid);
            if (serial.Length == 0) return false;
            var t = new I01();
            return t.SerialValidity(serial);
        }

        public static void DeleteSerialFromStorage(string serial)
        {
            SerialStorage.DeleteSerial(serial);
        }

        private static bool checkDOE(string serial)
        {
            var t = new I01();
            var sc = t.StoreSerial(serial);
            return sc.DateOfExpiry < DateTime.Now;
        }

        public static bool CheckActivation()
        {
            return true;
        }

        private static I01 GetSerialType(string s)
        {
            return new I01();
        }
    }
}