using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tester.DataSource;
using DapperQ.Mapping;

namespace Tester
{
    [Tablename("usr")]
    public class User : EntityBase
    {
        public int Id { get; set; }
        public string Name {get;set;}
        [Fieldname("pwd")]
        public string Password { get; set; }
        public int Type { get; set; }
        public int Level { get; set; }
        public bool IsOpen { get; set; }
        public DateTime CreateOn { get; set; }
    }

    [Tablename("image")]
    public class Image : EntityBase
    {
        public int Id { get; set; }

        [Fieldname("usrid")]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
        public int Level { get; set; }
        public int IsOpen { get; set; }
        public string Path { get; set; }
        public DateTime CreateOn { get; set; }

        [Exclude]
        public string DownloadCount { get; set; }
    }

    public class Photo : EntityBase
    {
        public int Id { get; set; }
        [Fieldname("usrid")]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
        public int Level { get; set; }
        public int IsOpen { get; set; }
        public string Path { get; set; }
        public DateTime CreateOn { get; set; }
    }

    public class Message : EntityBase
    {
        public int Id { get; set; }
        [Fieldname("usrid")]
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
