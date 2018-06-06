using System;

namespace de.fearvel.openMPS.Database.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    ///     Exception class for SQLiteErfassung
    /// </summary>
    public class SqLiteCollectedInformationPackageException : ArgumentException
    {
        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:de.fearvel.openMPS.SQLiteConnectionTools.SQLiteErfassungException" /> class.
        /// </summary>
        public SqLiteCollectedInformationPackageException()
        {
        }

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:de.fearvel.openMPS.SQLiteConnectionTools.SQLiteErfassungException" /> class.
        /// </summary>
        /// <param name="s">The s.</param>
        public SqLiteCollectedInformationPackageException(string s) : base(s)
        {
        }
    }
}