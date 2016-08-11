using System;
using System.Data;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.MySql;
using MySql.Data.MySqlClient;

namespace LoginServer
{

    public class DBContext : DataConnection
    {
        public DBContext() : base(GetDataProvider(), GetConnection()) { }

        private static IDataProvider GetDataProvider()
        {
            return new MySqlDataProvider();
        }

        private static IDbConnection GetConnection()
        {
            LinqToDB.Common.Configuration.AvoidSpecificDataProviderAPI = true;

            var dbConnection = new MySqlConnection(Appliaction.Current.ConnectionString);
            return dbConnection;
        }

        public ITable<TB_Account> TB_Account
        {
            get
            {
                return GetTable<TB_Account>();
            }
        }

        public string QueryMd5(string value)
        {
            var command = this.CreateCommand();
            command.CommandText = "select md5('" + value + "') as `MD5`";
            var reader = command.ExecuteReader();
            var md5 = string.Empty;
            if (reader.Read())
            {
                md5 = reader.GetString(0);
            }
            reader.Close();
            return md5;
        }
    }
}

