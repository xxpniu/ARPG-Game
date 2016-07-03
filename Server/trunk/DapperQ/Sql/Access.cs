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
  

using DapperQ.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DapperQ.Sql
{
    internal class Access : SqlServer
    {
        const string NOSUPPORTINDEX = "Can not support index!";
        protected override string GenUpdateSelectStatement()
        {
            return string.Format("UPDATE {0} SET {1} {2}", Target,
                string.Join(", ", Settor.Select(o => string.Format("[{0}]={1}", o.Key, o.Value)).ToArray()),
                (Where.Count > 0 ?
                string.Format("WHERE {0}", string.Join(" AND ", Where.Where(o => o.Length > 0).Select(o => o.ToString()).ToArray())) :
                string.Empty));
        }

        protected override string GenSelectStatement(Type type)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("SELECT {0}", Top > 0 ? string.Format(" TOP {0} ", Top) : string.Empty);
            sb.Append(string.Join(" ", string.Format("{0}", GetSelectColumns(type)), GenFromStatement()));

            if (GroupBy.Count > 0)
                sb.AppendFormat(" GROUP BY {0}", string.Join(", ", GroupBy.Where(o => o.Length > 0).Select(o => o.ToString()).ToArray()));

            if (OrderBy.Count > 0)
                sb.AppendFormat(" ORDER BY {0}", string.Join(", ", OrderBy.Select(o => o.ToString()).ToArray()));

            return sb.ToString();
        }

        protected override string GenFromStatement()
        {
            var sb = new StringBuilder();

            sb.AppendFormat(" FROM {0} ", (From.Count < 1 && Join != null) ? GetJoinText(Join.GetStack().OfType<On>()) : string.Join(", ", From.Select(o => o.Value).ToArray()));

            if (Where.Count > 0)
                sb.AppendFormat("WHERE {0}", string.Join(" AND ", Where.Where(o => o.Length > 0).Select(o => o.ToString()).ToArray()));

            return sb.ToString();
        }

        private string GetJoinText(IEnumerable<On> enumerable)
        {
            if (enumerable == null || enumerable.Count() < 1)
                return string.Empty;

            var sb = new StringBuilder();
            var stacked = new List<string>();
            foreach (var item in enumerable)
            {
                if (sb.Length == 0)
                {
                    sb.AppendFormat(" {0} {3} {1} ON {2} ",
                        string.Format("[{0}] AS [{1}]", item.table1, item.a),
                        string.Format("[{0}] AS [{1}]", item.table2, item.b),
                        item.exp, Join.Mode);
                    stacked.Add(item.a);
                    stacked.Add(item.b);
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(item.a) && !stacked.Contains(item.a))
                {
                    sb.Insert(0, string.Format("[{0}] AS [{1}] {2} (", item.table1, item.a, Join.Mode));
                    sb.AppendFormat(") ON {0}", item.exp);
                    stacked.Add(item.a);
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(item.b) && !stacked.Contains(item.b))
                {
                    sb.Insert(0, string.Format("[{0}] AS [{1}] {2} (", item.table2, item.b, Join.Mode));
                    sb.AppendFormat(") ON {0}", item.exp);
                    stacked.Add(item.b);
                    continue;
                }
            }

            return sb.ToString();
        }

        protected override string GenIndexExistsStatement()
        {
            throw new Exception(NOSUPPORTINDEX);
        }

        protected override string GenGetIndexStatement()
        {
            throw new Exception(NOSUPPORTINDEX);
        }
        protected override string GenDropIndexStatement()
        {
            throw new Exception(NOSUPPORTINDEX);
        }
        protected override string GenCreateIndexStatement()
        {
            throw new Exception(NOSUPPORTINDEX);
        }
    }
}
