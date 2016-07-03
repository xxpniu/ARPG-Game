using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tester.DataSource
{
    public class PostgreSQL : DapperQ.Mapping.DataSource
    {
        public PostgreSQL()
            : base()
        {
            var conf = ConfigurationManager.ConnectionStrings["PostgreSQL"];
            base.ConnectionString = conf.ConnectionString;
            base.ProviderName = conf.ProviderName;
            base.Name = conf.Name;
        }
    }
}
