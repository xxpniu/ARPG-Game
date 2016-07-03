/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS HEADER.
 *
 * Copyright 2014 rocky, rockylin@qq.com
 *
 * 
 * DapperQ.NET is free .NET library: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * DapperQ.NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with DapperQ.NET.  If not, see <http://www.gnu.org/licenses/>.
 *
 * DapperQ.NET是一个自由.NET类库，您可以自由分发、修改其中的源代码或者重新发布它，
 * 新的任何修改后的重新发布版必须同样在遵守LGPL3或更后续的版本协议下发布.
 * 关于LGPL协议的细则请参考COPYING、COPYING.LESSER文件，
 * 您可以在DapperQ.NET的相关目录中获得LGPL协议的副本，
 * 如果没有找到，请连接到 http://www.gnu.org/licenses/ 查看。
 *
 * - Author: Rocky Lin
 * - Contact: rockylin@qq.com
 * - License: GNU Lesser General Public License (LGPL)
 * - Blog and source code availability: https://dapperq.codeplex.com/
 */
  

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DapperQ.Sql
{
    internal class SqLite : MySql
    {
        protected override string GenUpdateSelectStatement()
        {
            return string.Format("UPDATE {0} SET {1} {2}", Target.Split(new string[] { "AS" }, StringSplitOptions.RemoveEmptyEntries)[0],
                string.Join(", ", Settor.Select(o => string.Format("[{0}]={1}", o.Key, o.Value)).ToArray()),
                (Where.Count > 0 ?
                string.Format("WHERE {0}", string.Join(" AND ", Where.Where(o => o.Length > 0).Select(o => o.ToString().Split(new char[] { '.' })[1]).ToArray())) :
                string.Empty));
        }

        protected override string GenIndexExistsStatement()
        {
            throw new Exception("Can not support check index!");
        }

        protected override string GenGetIndexStatement()
        {
            throw new Exception("Can not support get index names!");
        }
        protected override string GenDropIndexStatement()
        {
            var flag = 0;
            var sb = new StringBuilder();
            foreach (var indexName in IndexNames)
            {
                sb.AppendFormat("DROP INDEX [IDX_{0}_{1}]", From.First().Value, indexName, From.First().Value);
                if (flag < (IndexNames.Count - 1))
                    sb.Append(";\r\n");

                flag++;
            }
            return sb.ToString();
        }
        protected override string EnsureKeyword(string sql)
        {
            return sql;
        }
        protected override string DatetimeFormat(string sql)
        {
            Regex regex = new Regex(@"#(\d{4})/(\d{2})/(\d{2}) (\d{2}:\d{2}:\d{2})#", RegexOptions.Compiled | RegexOptions.Multiline);
            sql = regex.Replace(sql, "'$1-$2-$3T$4'");

            return sql;
        }
    }
}
