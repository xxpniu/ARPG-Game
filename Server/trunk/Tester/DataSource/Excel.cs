using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tester.DataSource
{
    public class Excel : DapperQ.Mapping.DataSource
    {
        public Excel()
            : base()
        {
            var conf = ConfigurationManager.ConnectionStrings["Excel"];
            base.ConnectionString = conf.ConnectionString;
            base.ProviderName = conf.ProviderName;
            base.Name = conf.Name;
        }
    }
}
