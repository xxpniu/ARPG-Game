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
    internal class SqlServer : SqlStatement
    {
        protected override string GenInsertStatement()
        {
            return string.Format("INSERT INTO [{0}] ([{1}]) VALUES ({2})",
                Target,
                string.Join("], [", Settor.Select(o => o.Key).ToArray()),
                string.Join(", ", Settor.Select(o => o.Value).ToArray()));
        }

        protected override string GenUpdateSelectStatement()
        {
            return string.Format("UPDATE {0} SET {1} {2}", Target.Split(new string[] { "AS" }, StringSplitOptions.RemoveEmptyEntries)[0],
                string.Join(", ", Settor.Select(o => string.Format("[{0}]={1}", o.Key, o.Value)).ToArray()),
                (Where.Count > 0 ?
                string.Format("FROM {0} WHERE {1}", Target, string.Join(" AND ", Where.Where(o => o.Length > 0).Select(o => o.ToString()).ToArray())) :
                string.Empty));
        }

        protected override string GenInsertSelectStatement()
        {
            return string.Format("INSERT INTO [{0}] ({1}) {2}",
                Target, Columns.Count > 0 ? string.Join(", ", Columns.Select(o => o.Value.Split(new char[] { '.' })[1]).ToArray()) : "*",
                GenSelectStatement(null));
        }

        protected override string GenIndexExistsStatement()
        {
            return string.Format("SELECT 1 FROM sys.indexes idx JOIN sys.index_columns idxCol ON (idx.object_id = idxCol.object_id AND idx.index_id = idxCol.index_id)JOIN sys.tables tab ON (idx.object_id = tab.object_id) WHERE " +
                "tab.name='{0}' AND idx.name='{1}'", From.First().Value, IndexNames[0]);
        }

        protected override string GenGetIndexStatement()
        {
            return string.Format("SELECT idx.name AS idx_name FROM sys.indexes idx JOIN sys.index_columns idxCol ON (idx.object_id = idxCol.object_id AND idx.index_id = idxCol.index_id)JOIN sys.tables tab ON (idx.object_id = tab.object_id) WHERE " +
                "tab.name='{0}'", From.First().Value);
        }

        protected override string GenExistsStatement()
        {
            return string.Format("SELECT TOP 1 COUNT(1) AS [Count] {0}", GenFromStatement());
        }

        protected override string GenDropTableStatement()
        {
            return string.Format("DROP TABLE [{0}]", From.First().Value);
        }

        protected override string GenDropIndexStatement()
        {
            var flag = 0;
            var sb = new StringBuilder();
            foreach (var indexName in IndexNames)
            {
                sb.AppendFormat("DROP INDEX [IDX_{0}_{1}] on [{2}]", From.First().Value, indexName, From.First().Value);
                if (flag < (IndexNames.Count - 1))
                    sb.Append(";\r\n");

                flag++;
            }
            return sb.ToString();
        }

        protected override string GenDeleteStatement()
        {
            return string.Format("DELETE {0} {1}",
                From.Count > 0 ? string.Join(", ", From.Select(o => string.Format("[{0}]", SplitFrom(o.Value))).ToArray()) : SplitFrom(From.First().Value),
                GenFromStatement());
        }

        protected override string GenCreateIndexStatement()
        {
            var flag = 0;
            var sb = new StringBuilder();
            foreach (var indexName in IndexNames)
            {
                sb.AppendFormat("CREATE INDEX IDX_{0}_{1} ON {0}({1})", From.First().Value, indexName);
                if (flag < (IndexNames.Count - 1))
                    sb.Append(";\r\n");

                flag++;
            }
            return sb.ToString();
        }

        protected override string GenSelectStatement(Type type)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("SELECT {0}", Top > 0 ? string.Format(" TOP {0}", Top) : string.Empty);
            sb.Append(string.Join(" ", string.Format("{0}",
                ((Skip > 0 && OrderBy.Count > 0) ? string.Join(", ", GetSelectColumns(type), 
                string.Format("ROW_NUMBER() over(ORDER BY {0}) as ROWNUMBER", 
                string.Join(", ", OrderBy.Select(o => o.ToString()).ToArray()))) : 
                GetSelectColumns(type))), 
                GenFromStatement()));

            if (GroupBy.Count > 0)
                sb.AppendFormat(" GROUP BY {0}", string.Join(", ", GroupBy.Where(o => o.Length > 0).Select(o => o.ToString()).ToArray()));

            if (OrderBy.Count > 0)
                sb.AppendFormat(" ORDER BY {0}", string.Join(", ", OrderBy.Select(o => o.ToString()).ToArray()));

            return sb.ToString();
        }

        protected override string GenFromStatement()
        {
            var sb = new StringBuilder();

            sb.AppendFormat(" FROM {0} ", (From.Count < 1 && Join != null) ? Join.ToString() : string.Join(", ", From.Select(o => o.Value).ToArray()));

            if (Where.Count > 0)
            {
                sb.AppendFormat("WHERE {0}", string.Join(" AND ", Where.Where(o => o.Length > 0).Select(o => o.ToString()).ToArray()));
                if(Skip > 0 && OrderBy.Count > 0)
                    sb.AppendFormat(" AND ROWNUMBER>{0}", Skip);
            }
            else if (Skip > 0)
                sb.AppendFormat("WHERE ROWNUMBER>{0}", Skip);

            return sb.ToString();
        }

        internal override void Union(Dictionary<int, SqlStatement> sqlList)
        {
            From.Clear();
            From.Add(typeof(SqlStatement), string.Format("(\r\n{0}\r\n) AS [TMP{1}]", string.Join("\r\nUNION\r\n", sqlList.Select(o => o.Value.ToString()).ToArray()), SqlStatement.GetTimespan()));
            var where = string.Join(" AND ", Where.Select(o => o.ToString()).ToArray());
            if (!string.IsNullOrWhiteSpace(where))
            {
                Regex regex = new Regex(@"\[[_\d\w]+\]\.", RegexOptions.Compiled);
                Where.Clear();
                Where.Add(regex.Replace(where, string.Empty));
            }
        }
        public override string ToString(Type type)
        {
            var sql = base.ToString(type);
            return DatetimeFormat(sql);
        }
        protected override string DatetimeFormat(string sql)
        {
            Regex regex = new Regex(@"#(\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2})#", RegexOptions.Compiled | RegexOptions.Multiline);
            sql = regex.Replace(sql, "'$1'");

            return sql;
        }
    }
}
