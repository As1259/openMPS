﻿// Copyright (c) 2018 / 2019, Andreas Schreiner

using de.fearvel.net.DataTypes.AbstractDataTypes;

namespace de.fearvel.openMPS.DataTypes
{
    /// <summary>
    /// Request Class for An OidRequest
    /// </summary>
    public sealed class OidRequest : JsonSerializable<OidRequest>
    {
        /// <summary>
        /// Contains the Token
        /// </summary>
        private string _token;

        /// <summary>
        /// Flag which determines if an _token has been written
        /// </summary>
        private bool _tokenKeySet;

        /// <summary>
        /// Property which gets or sets the _activationKey
        /// Workaround to make _activationKey one time writable
        /// This workaround is needed because the JSON Deserializer
        /// can only handle a public setter 
        /// </summary>
        public string Token
        {
            get => _token;
            set
            {
                if (!_tokenKeySet)
                {
                    _token = value;
                    _tokenKeySet = true;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public OidRequest(string token)
        {
            Token = token;
        }
    }
}