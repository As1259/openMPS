#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;

namespace de.fearvel.manastone.serialManagement
{
    public abstract class Serial
    {
        protected string Generation;
        protected string praefix;
        protected SerialContainer SerialC;

        internal bool IsStored => SerialC != null;
        internal SerialContainer ContainedSerial => SerialC;

        public abstract string GenerateSerial(int customerIdentificationNumber, int authorityIdentificationNumber,
            int programIdentificationNumber, DateTime dateOfExpiry, int licenceType, int activationtype = 1);

        public abstract bool SerialValidity(string serialNumber);
        internal abstract SerialContainer StoreSerial(string serialNumber);
        protected abstract bool ActivationValidity();
        internal abstract void Activate();

        internal bool CheckAndStore(string serialNumber)
        {
            if (!SerialValidity(serialNumber)) return false;
            SerialC = StoreSerial(serialNumber);
            return true;
        }

        internal bool CheckDateOfExpiry()
        {
            if (!IsStored) return false;
            return SerialC.LicenseType == 0 || DateTime.Now <= SerialC.DateOfExpiry;
        }

        internal bool CheckActivation()
        {
            return SerialC.ActivationType == 0 || ActivationValidity();
        }
    }
}