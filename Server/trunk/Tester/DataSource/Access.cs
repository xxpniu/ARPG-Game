using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tester.DataSource
{
    public class Access : DapperQ.Mapping.DataSource
    {
        public Access()
            : base()
        {
            var conf = ConfigurationManager.ConnectionStrings["Access"];
            base.ConnectionString = conf.ConnectionString;
            base.ProviderName = conf.ProviderName;
            base.Name = conf.Name;
        }
    }
}
