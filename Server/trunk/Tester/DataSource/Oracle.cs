using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tester.DataSource
{
    public class Oracle : DapperQ.Mapping.DataSource
    {
        public Oracle()
            : base()
        {
            var conf = ConfigurationManager.ConnectionStrings["Oracle"];
            base.ConnectionString = conf.ConnectionString;
            base.ProviderName = conf.ProviderName;
            base.Name = conf.Name;
        }
    }
}
