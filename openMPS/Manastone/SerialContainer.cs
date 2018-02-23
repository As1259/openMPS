#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;

namespace de.as1259.manastone.serialManagement
{
    public class SerialContainer
    {
        internal SerialContainer(string serialNumber, string praefix, int custId, int authId,
            int progId, int licType, int actType, DateTime doe, string hash)
        {
            SerialNumber = serialNumber.Length > 0 ? serialNumber : throw new ArgumentException();
            CustomerIdentificationNumber = custId > 0 || custId <= 99999 ? custId : throw new ArgumentException();
            AuthorityIdentificationNumber = authId > 0 || authId <= 99999 ? authId : throw new ArgumentException();
            ProgramIdentificationNumber = progId > 0 || progId <= 99999 ? progId : throw new ArgumentException();
            LicenseType = licType > 0 || licType <= 9 ? licType : throw new ArgumentException();
            ActivationType = actType > 0 || actType <= 9 ? actType : throw new ArgumentException();
            DateOfExpiry = doe > new DateTime(0) ? doe : throw new ArgumentException();

            Praefix = praefix.Length == 3 ? praefix : throw new ArgumentException();
            Hash = hash.Length == 6 ? hash : throw new ArgumentException();
        }

        internal string SerialNumber { get; }

        internal int CustomerIdentificationNumber { get; }

        internal int AuthorityIdentificationNumber { get; }

        internal int ProgramIdentificationNumber { get; }

        internal int LicenseType { get; }

        internal int ActivationType { get; }

        internal DateTime DateOfExpiry { get; }

        internal string Praefix { get; }

        internal string Hash { get; }
    }
}