﻿using DotNetCore.DataLayer.Dapper;
using System;

namespace DotNetCore.DataLayer.DapperTests
{
    public class EnvironmentSettings : IDatabaseConnectionSettings
    {
        public string Host => GetValue("DB_HOST");

        public uint Port
        {
            get
            {
                uint.TryParse(GetValue("DB_PORT"), out uint databasePort);

                return databasePort;
            }
        }

        public string DatabaseName => GetValue("DB_NAME");

        public string User => GetValue("DB_USER");

        public string Password => GetValue("DB_PASSWORD");

        public bool Pooling => GetValue("DB_POOLING")?.ToLower() != "false";

        private static string GetValue(string environmentKey)
        {
            try
            {
                return Environment.GetEnvironmentVariable(environmentKey);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
