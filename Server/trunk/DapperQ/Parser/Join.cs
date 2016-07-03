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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DapperQ.Parser
{
    internal class Join : IParser
    {
        internal const string LEFT = "LEFT JOIN";
        internal const string INNER = "INNER JOIN";
        Dictionary<string, string> dictAs;
        Dictionary<string, string> dictFrom;
        Dictionary<string, int> countor;
        Dictionary<string, On> dictOn;
        Dictionary<string, List<PropertyInfo>> propList;
        static readonly object lockor = new object();
        static volatile Join instance = null;
        internal string Mode {get;set;}

        Join(string joinMode)
        {
            Mode = (joinMode == "GroupJoin" ? LEFT : INNER);
            dictAs = new Dictionary<string, string>();
            countor = new Dictionary<string, int>();
            dictOn = new Dictionary<string, On>();
            dictFrom = new Dictionary<string, string>();
            propList = new Dictionary<string, List<PropertyInfo>>();
        }

        internal override object Context { get { return instance; } }

        public override string ToString()
        {
            var stacked = new List<string>();
            var sb = new StringBuilder();
            foreach (var item in dictOn)
            {
                if (!stacked.Contains(item.Value.a) && !stacked.Contains(item.Value.b))
                {
                    var a = item.Value.a;
                    var b = item.Value.b;
                    if (countor[b] > countor[a])
                    {
                        a = item.Value.b;
                        b = item.Value.a;
                    }
                    sb.AppendFormat(" {0} {3} {1} ON {2} ",
                        string.Format("[{0}] AS [{1}]", dictAs[a], a),
                        string.Format("[{0}] AS [{1}]", dictAs[b], b),
                        item.Value.exp, Mode);
                }

                if (stacked.Contains(item.Value.a) && !stacked.Contains(item.Value.b))
                {
                    sb.AppendFormat(" {2} {0} ON {1} ",
                        string.Format("[{0}] AS [{1}]", dictAs[item.Value.b], item.Value.b),
                        item.Value.exp, Mode);
                }

                if (!stacked.Contains(item.Value.a) && stacked.Contains(item.Value.b))
                {
                    sb.AppendFormat(" {2} {0} ON {1} ",
                        string.Format("[{0}] AS [{1}]", dictAs[item.Value.a], item.Value.a),
                        item.Value.exp, Mode);
                }

                if (stacked.Contains(item.Value.a) && stacked.Contains(item.Value.b))
                {
                    sb.AppendFormat(" AND {1} ", item.Value.exp);
                }
                stacked.Add(item.Value.a);
                stacked.Add(item.Value.b);
            }
            
            return sb.ToString();
        }

        public Stack GetStack()
        {
            var retval = new Stack();
            var stacked = new List<string>();
            foreach (var item in dictOn)
            {
                if (!stacked.Contains(item.Value.a) && !stacked.Contains(item.Value.b))
                {
                    var a = item.Value.a;
                    var b = item.Value.b;
                    if (countor[b] > countor[a])
                    {
                        a = item.Value.b;
                        b = item.Value.a;
                    }
                    retval.Push(new On { a = a, b = b, table1 = dictAs[a], table2 = dictAs[b], exp = item.Value.exp });
                }

                if (stacked.Contains(item.Value.a) && !stacked.Contains(item.Value.b))
                {
                    retval.Push(new On { a = item.Value.b, b = string.Empty, table1 = dictAs[item.Value.b], table2 = string.Empty, exp = item.Value.exp });
                }

                if (!stacked.Contains(item.Value.a) && stacked.Contains(item.Value.b))
                {
                    retval.Push(new On { a = item.Value.a, b = string.Empty, table1 = dictAs[item.Value.a], table2 = string.Empty, exp = item.Value.exp });
                }
            }
            return retval;
        }

        public override void Dispose()
        {
            countor = new Dictionary<string,int>();
            dictAs = new Dictionary<string,string>();
            dictOn = new Dictionary<string,On>();
            dictFrom = new Dictionary<string, string>();
            propList = new Dictionary<string, List<PropertyInfo>>();
        }

        void FillPropertyInfoList(string key, Type type)
        {
            if(!propList.ContainsKey(key))
            {
                List<PropertyInfo> props = new List<PropertyInfo>();
                props.AddRange(type.GetProperties());
                propList.Add(key, props);
            }
        }

        internal new Join Stack(Expression exp)
        {
            var call = exp as MethodCallExpression;
            Regex regex = new Regex(@"([\w\d]+)\s*=\>\s*([\w\d]\.[\w\d]+)", RegexOptions.Compiled);
            foreach (var arg in call.Arguments)
            {
                if (arg.NodeType != ExpressionType.Quote)
                    continue;

                var text = arg.ToString();
                if (regex.IsMatch(text))
                {
                    var type = arg.Type.GetGenericArguments()[0].GetGenericArguments()[0];
                    var tb = Visitor.GetTableName(type);
                    var als = regex.Match(text).Result("$1");
                    FillPropertyInfoList(als, type);

                    if (!dictFrom.ContainsKey(tb))
                        dictFrom.Add(tb, als);

                    if (!dictAs.ContainsKey(als))
                        dictAs.Add(als, tb);
                }
            }

            regex = new Regex(@"([\w\d])\.([\w\d]+)\s*,\s*[\w\d]+\s*=\>\s*([\w\d])\.([\w\d]+)", RegexOptions.Compiled);
            var matchs = regex.Matches(exp.ToString());
            if (matchs == null)
                return instance;

            foreach (Match m in matchs)
            {
                var as1 = m.Result("$1");
                var fld1 = m.Result("$2");
                var p1 = propList.Where(o => o.Key == as1).First().Value.Where(o => o.Name == fld1).FirstOrDefault();
                fld1 = (p1 != null ? GetMemberName(p1) : fld1);
                var as2 = m.Result("$3");
                var fld2 = m.Result("$4");
                var p2 = propList.Where(o => o.Key == as2).First().Value.Where(o => o.Name == fld2).FirstOrDefault();
                fld2 = (p2 != null ? GetMemberName(p2) : fld2);
                var on = new On
                {
                    a = m.Result("$1"),
                    b = m.Result("$3"),
                    exp = string.Format("[{0}].[{1}]=[{2}].[{3}]", as1, fld1, as2, fld2)
                };

                if (!dictOn.ContainsKey(on.exp))
                {
                    dictOn.Add(on.exp, on);

                    if (!countor.ContainsKey(on.a))
                        countor.Add(on.a, 0);

                    if (!countor.ContainsKey(on.b))
                        countor.Add(on.b, 0);

                    countor[on.a]++;
                    countor[on.b]++;
                }
            }

            return instance;
        }

        internal string GetAlias(string tableName)
        {
            if (!dictFrom.ContainsKey(tableName))
                return string.Empty;

            return dictFrom[tableName];
        }

        internal static Join GetInstance(string joinMode)
        {
            if (instance != null)
            {
                if (instance.Mode != (joinMode == "GroupJoin" ? LEFT : INNER))
                    instance.Dispose();
                else
                    return instance;
            }
            
            lock (lockor)
            {
                instance = new Join(joinMode);
            }
            return instance;
        }
    }

    internal struct On
    {
        internal string a;
        internal string b;
        internal string table1;
        internal string table2;
        internal string exp;
    }
}
