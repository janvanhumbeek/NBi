﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Odbc;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Client
{
    class OdbcClientFactory : DbClientFactory
    {
        protected override IClient Instantiate(DbProviderFactory factory, string connectionString)
            => new DbClient(factory, typeof(OdbcConnection), connectionString);
        
        protected override DbProviderFactory ParseConnectionString(string connectionString)
        {
            var csb = GetConnectionStringBuilder(connectionString);
            if (csb == null)
                return null;

            var driverName = ExtractDriverToken(csb, connectionString);
            if (string.IsNullOrEmpty(driverName))
                return null;

            var factory = GetDbProviderFactory(driverName);
            return factory;
        }

        private string ExtractDriverToken(DbConnectionStringBuilder connectionStringBuilder, string connectionString)
        {
            if (connectionStringBuilder.ContainsKey("Driver"))
                return ("System.Data.Odbc");

            return string.Empty;
        }
    }
}
