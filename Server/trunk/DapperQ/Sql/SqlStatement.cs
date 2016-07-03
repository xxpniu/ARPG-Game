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
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DapperQ.Parser;
using System.Reflection;

namespace DapperQ.Sql
{
    internal abstract class SqlStatement : IDisposable
    {
        public SqlStatement()
        {
            Init();
        }
        ~SqlStatement()
        {
            Dispose();
        }

        public int Top { get; set; }
        public int Skip { get; set; }
        public Action Action { get; set; }
        public string Target { get; set; }
        public ResultType ResultType { get; set; }
        public List<string> IndexNames { get; set; }
        public List<string> Where { get; set; }
        public List<string> GroupBy { get; set; }
        public List<string> OrderBy { get; set; }
        public Dictionary<Type, string> From { get; set; }
        public Dictionary<string, string> Columns { get; set; }
        public Join Join { get; set; }
        public Dictionary<string, object> Settor { get; set; }
        public bool Distinct { get; set; }
        public override string ToString()
        { 
            return this.ToString(null);
        }
        public virtual string ToString(Type type)
        {
            switch (Action)
            {
                case Action.CreateIndex:
                    return GenCreateIndexStatement();
                case Action.DeleteSelect:
                    return GenDeleteStatement();
                case Action.DropIndex:
                    return GenDropIndexStatement();
                case Action.DropTable:
                    return GenDropTableStatement();
                case Action.Exists:
                    if (!From.ContainsKey(type))
                        From.Add(type, string.Format("[{0}]", Visitor.GetTableName(type)));
                    return GenExistsStatement();
                case Action.GetIndexes:
                    return GenGetIndexStatement();
                case Action.IndexExists:
                    return GenIndexExistsStatement();
                case Action.InsertSelect:
                    return GenInsertSelectStatement();
                case Action.Insert:
                    return GenInsertStatement();
                case Action.UpdateSelect:
                    return GenUpdateSelectStatement();
                default:
                    return GenSelectStatement(type);
            }
        }

        protected abstract string GenUpdateSelectStatement();

        protected abstract string GenInsertSelectStatement();

        protected abstract string GenInsertStatement();

        protected abstract string GenIndexExistsStatement();

        protected abstract string GenGetIndexStatement();

        protected abstract string GenExistsStatement();

        protected abstract string GenDropTableStatement();

        protected abstract string GenDropIndexStatement();

        protected abstract string GenDeleteStatement();

        protected abstract string GenCreateIndexStatement();
        protected abstract string GenSelectStatement(Type type);

        protected abstract string GenFromStatement();

        internal static long GetTimespan()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        public void Dispose()
        {
            Init();
        }

        private void Init()
        {
            Action = Action.Select;
            Where = new List<string>();
            GroupBy = new List<string>();
            OrderBy = new List<string>();
            From = new Dictionary<Type, string>();
            Columns = new Dictionary<string, string>();
            Settor = new Dictionary<string, object>();
            IndexNames = new List<string>();
            Join = null;
        }

        internal abstract void Union(Dictionary<int, SqlStatement> sqlList);
        protected string GetSelectColumns(Type type)
        {
            switch (ResultType)
            { 
                case ResultType.Count:
                    ResultType = ResultType.Columns;
                    return "COUNT(1) AS Count";
                case ResultType.Columns:
                    return (Distinct ? "DISTINCT " : string.Empty) + (Columns.Count < 1 ? 
                        (type == null ? "*" :
                        string.Join(", ", type.GetProperties().Where(o => o.GetCustomAttributes(false).Where(a => a.GetType() == typeof(Mapping.Exclude)).FirstOrDefault() == null).Select(o => TryAs(o)))) :
                        string.Join(", ", Columns.Select(o => Columns[o.Key]).ToArray()));
                default:
                    return string.Format("{0}({1})", ResultType.ToString(), Columns.First().Value);
            }
        }

        private string TryAs(PropertyInfo o)
        {
            var name = IParser.GetMemberName(o);
            if (name == o.Name)
                return string.Format("[{0}]", name);

            return string.Format("[{0}] AS [{1}]", name, o.Name);
        }

        protected virtual string DatetimeFormat(string sql)
        {
            throw new NotImplementedException();
        }

        protected virtual string SplitFrom(string fromAs)
        {
            return fromAs.Split(new string[] { " AS " }, StringSplitOptions.RemoveEmptyEntries)[0];
        }
    }
}
