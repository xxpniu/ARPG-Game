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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DapperQ.Parser
{
    internal abstract class IParser : IDisposable
    {
        const string DatatimeFormat = "yyyy/MM/dd HH:mm:ss";
        internal virtual void Stack(string name, object value) { }
        internal virtual void Stack(string name, ConstantExpression constant) { }
        internal virtual void Stack(string format, MemberExpression member, ConstantExpression constant) { }
        internal virtual void Stack(string expString) { }
        internal virtual void Stack(Relation condition) { }
        internal virtual void Stack(Expression exp) { }
        internal virtual void Stack(Expression left, string op, Expression right) { }

        internal abstract object Context { get; }
        public abstract void Dispose();

        internal static string GetValue(ConstantExpression constant)
        {
            if (constant.Value == null)
                return "Null";

            if (constant.Type.Equals(typeof(string)))
                return string.Format("'{0}'", constant.Value.ToString().Replace("'", "’"));

            if (constant.Type.Equals(typeof(DateTime)))
                return string.Format("#{0}#", Convert.ToDateTime(constant.Value).ToString(IParser.DatatimeFormat));

            if (constant.Type.Equals(typeof(bool)))
                return Convert.ToInt16(constant.Value).ToString();

            return constant.Value.ToString();
        }

        internal virtual string GetMemberName(MemberExpression m)
        {
            Func<string> get = () =>
            {
                var val = GetMemberAlias(m, m.Member.Name);
                if (m.Type.IsGenericType)
                {
                    var tmp = val.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length > 1)
                    {
                        var memberName = GetMemberAlias(m, tmp[tmp.Length - 1]);
                        val = string.Format("{0}.{1}", tmp[tmp.Length - 2], memberName);
                    }
                }
                return val;
            };
            return MemberCache.Find(m.GetHashCode(), get());
        }

        internal static string GetMemberName(PropertyInfo prop)
        {
            if (prop == null)
                return string.Empty;

            Func<string> get = () =>
            {
                if (prop.GetCustomAttributes(false).Where(o=>o.GetType() == typeof(Mapping.Exclude)).FirstOrDefault() != null)
                    return string.Empty;

                var attr = prop.GetCustomAttributes(false).Where(o => o.GetType() == typeof(Mapping.Exclude)).FirstOrDefault();
                if (attr == null)
                    return prop.Name;

                return ((Mapping.Fieldname)attr).Name;
            };
            return MemberCache.Find(prop.GetHashCode(), get());
        }

        internal static string GetMemberName(MemberInfo member)
        {
            Func<string> get = () =>
            {
                if (HasExclude(member))
                    return string.Empty;

                var attr = member.GetCustomAttributes(false).Where(a => a.GetType() == typeof(Mapping.Fieldname)).FirstOrDefault();
                if (attr == null)
                    return member.Name;

                return ((Mapping.Fieldname)attr).Name;
            };
            return MemberCache.Find(member.GetHashCode(), get());
        }


        internal virtual string GetMemberPrefix(Expression m)
        {
            if (m == null)
                return string.Empty;

            var val = m.ToString();
            var tmp = val.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (tmp.Length > 1)
            {
                var regex = new Regex("<>", RegexOptions.Compiled);
                var count = regex.Matches(val).Count;
                if (count > 1 && tmp.Length % 2 != 0)
                    return tmp[tmp.Length - 1];

                return tmp[tmp.Length - 2];
            }
            return val;
        }

        internal static string GetMemberAlias(MemberExpression m, string defaultName)
        {
            return GetMemberAlias(m.Member, defaultName);
        }

        internal static string GetMemberAlias(PropertyInfo prop, string defaultName)
        {
            if (prop == null)
                return defaultName;

            var attr = prop.GetCustomAttributes(false).Where(a => a.GetType() == typeof(Mapping.Fieldname)).FirstOrDefault();
            if (attr != null)
                return (attr as Mapping.Fieldname).Name;

            return defaultName;
        }

        internal static string GetMemberAlias(MemberInfo member, string defaultName)
        {
            var attr = Attribute.GetCustomAttribute(member, typeof(Mapping.Fieldname));
            if (attr != null && attr is Mapping.Fieldname)
                return (attr as Mapping.Fieldname).Name;

            return defaultName;
        }

        internal static bool HasExclude(MemberInfo member)
        {
            var exclude = Attribute.GetCustomAttribute(member, typeof(Mapping.Exclude));
            if (exclude != null)
                return true;
            else
                return false;
        }
    }
}
