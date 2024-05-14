using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace ProjectBase.Database.Connection
{
    public static class ContextConnectionService
    {
        public static IConfiguration _configuration;
        public static string DefaultConnectionStringName { get; private set; }

        static ContextConnectionService()
        {
            ConnectionStrings = new List<KeyValuePair<string, string>>();
        }

        public static void SetDefaultConnectionStringName(string _defaultConnectionStringName)
        {
            if (string.IsNullOrEmpty(DefaultConnectionStringName))
                DefaultConnectionStringName = _defaultConnectionStringName;
            else
                throw new InvalidOperationException(
                    "DefaultConnectionStringName string name can only be set once"
                );
        }

        public static List<KeyValuePair<string, string>> ConnectionStrings { get; set; }

        public static object locker = new object();

        public static void AddConnectionString(
            string connectionStringName,
            string connectionStingValue
        )
        {
            lock (locker)
            {
                var connectionExist = ConnectionStrings
                    .Where(r => r.Key.Equals(connectionStringName))
                    .Any();

                if (connectionExist == false)
                    ConnectionStrings.Add(
                        new KeyValuePair<string, string>(connectionStringName, connectionStingValue)
                    );
            }
        }

        public static string GetConnectionString(string connectionStringName)
        {
            lock (locker)
            {
                var connectionExist = ConnectionStrings
                    .Where(r => r.Key.Equals(connectionStringName))
                    .Any();

                if (connectionExist == true)
                    return ConnectionStrings
                        .FirstOrDefault(r => r.Key.Equals(connectionStringName))
                        .Value;
                else
                    throw new KeyNotFoundException(
                        $"Connection string with key {connectionStringName} is not registered"
                    );
            }
        }


    }
}
