using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tester.DataSource
{
    public class Mysql : DapperQ.Mapping.DataSource
    {
        public Mysql()
            : base()
        {
            var conf = ConfigurationManager.ConnectionStrings["MySql"];
            base.ConnectionString = conf.ConnectionString;
            base.ProviderName = conf.ProviderName;
            base.Name = conf.Name;
        }
    }
}
