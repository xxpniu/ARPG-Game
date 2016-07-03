using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tester.DataSource
{
    public class SqlServer : DapperQ.Mapping.DataSource
    {
        public SqlServer()
            : base()
        {
            var conf = ConfigurationManager.ConnectionStrings["SqlServer"];
            base.ConnectionString = conf.ConnectionString;
            base.ProviderName = conf.ProviderName;
            base.Name = conf.Name;
        }
    }
}
