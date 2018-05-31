#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Data.HashFunction;
using System.Globalization;
using System.Linq;

namespace de.fearvel.manastone.serialManagement.InstallBased
{
    public class I01 : Serial
    {
        private readonly CRC hash = new CRC(CRC.Standards[CRC.Standard.ARC]);

        public I01()
        {
            Generation = "I01";
            praefix = "I01";
        }


        public override string GenerateSerial(int customerIdentificationNumber, int authorityIdentificationNumber,
            int programIdentificationNumber, DateTime dateOfExpiry, int licenceType, int activationtype = 1)
        {
            return "";
        }

        private string FillIdNr(string s)
        {
            while (s.Length < 5) s = "0" + s;
            return s;
        }

        private string reArrange(string s)
        {
            var c = s.ToCharArray(); //28 Zeichen
            var outVal = "";
            outVal += c[5]; //0
            outVal += c[12]; //1
            outVal += c[16]; //2
            outVal += c[6]; //3
            outVal += c[8]; //4
            outVal += c[0]; //5
            outVal += c[3]; //6
            outVal += c[19]; //7
            outVal += c[4]; //8
            outVal += c[15]; //9
            outVal += c[14]; //10
            outVal += c[13]; //11
            outVal += c[1]; //12
            outVal += c[11]; //13
            outVal += c[10]; //14
            outVal += c[9]; //15
            outVal += c[2]; //16
            outVal += c[18]; //17
            outVal += c[17]; //18
            outVal += c[7]; //19
            return outVal;
        }

        private string intstringToHexstring(string s)
        {
            return int.Parse(s).ToString("X");
        }

        private string hexstringToIntstring(string s)
        {
            return int.Parse(s, NumberStyles.HexNumber).ToString();
        }

        public override bool SerialValidity(string s)
        {
            var ser = StoreSerial(s);
            var h = hash.ComputeHash(ser.Praefix + ser.LicenseType +
                                     FillIdNr(ser.CustomerIdentificationNumber.ToString())
                                     + FillIdNr(ser.AuthorityIdentificationNumber.ToString())
                                     + FillIdNr(ser.ProgramIdentificationNumber.ToString()) +
                                     ser.DateOfExpiry.ToString("ddMMyy"));
            var j = ser.Praefix + ser.LicenseType + FillIdNr(ser.CustomerIdentificationNumber.ToString())
                    + FillIdNr(ser.AuthorityIdentificationNumber.ToString())
                    + FillIdNr(ser.ProgramIdentificationNumber.ToString()) + ser.DateOfExpiry.ToString("ddMMyy");
            var o = h.Aggregate("", (current, t) => current + t);
            return ByteArrayHashToString(h).CompareTo(ser.Hash) == 0;
        }

        private string ByteArrayHashToString(byte[] b)
        {
            var o = b.Aggregate("", (current, t) => current + t);
            return o;
        }

        public DateTime StringToDateTime(string s)
        {
            while (s.Length < 6) s = "0" + s;
            return new DateTime(int.Parse(20 + s.Substring(4, 2)), int.Parse(s.Substring(2, 2)),
                int.Parse(s.Substring(0, 2)));
        }


        internal override SerialContainer StoreSerial(string serial)
        {
            var serialR = serial.Substring(3, 1) + reArrange(serial.Substring(4, serial.Length - 9));
            var hashkey = serial.Substring(serial.Length - 6, 6);
            var praefix = serial.Substring(0, 3);
            var licenceType = 10 - int.Parse(hexstringToIntstring(serialR.Substring(0, 1)));
            var customerIdentificationNumber = 99999 - int.Parse(hexstringToIntstring(serialR.Substring(1, 5)));
            var authorityIdentificationNumber = 99999 - int.Parse(hexstringToIntstring(serialR.Substring(6, 5)));
            var programIdentificationNumber = 99999 - int.Parse(hexstringToIntstring(serialR.Substring(11, 5)));
            var dateOfExpiry = StringToDateTime(hexstringToIntstring(serialR.Substring(16, 5)));
            return new SerialContainer(serial, praefix, customerIdentificationNumber, authorityIdentificationNumber,
                programIdentificationNumber, licenceType, 0, dateOfExpiry, hashkey);
        }

        protected override bool ActivationValidity()
        {
            return true;
        }

        internal override void Activate()
        {
        }
    }
}