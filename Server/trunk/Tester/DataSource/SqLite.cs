using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tester.DataSource
{
    public class SqLite : DapperQ.Mapping.DataSource
    {
        public SqLite()
            : base()
        {
            var conf = ConfigurationManager.ConnectionStrings["SqLite"];
            base.ConnectionString = conf.ConnectionString;
            base.ProviderName = conf.ProviderName;
            base.Name = conf.Name;
        }
    }
}
