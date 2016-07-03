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
using System.Linq.Expressions;
using System.Reflection;
using System.Data;
using System.Text.RegularExpressions;
using DapperQ.Parser;
using DapperQ.Sql;
using DapperQ.Factory;
using Dapper;

namespace DapperQ
{
    internal class Visitor : ExpressionVisitor, IDisposable
	{
        Dictionary<int, SqlStatement> Union;
        public DbType DbType { get; private set; }

        internal Visitor(DbType dbType)
        {
            DbType = dbType;
            Current = SqlStatementFactory.Create(DbType);
            Union = new Dictionary<int, SqlStatement>();
        }
        SqlStatement Current { get; set; }
        internal List<string> IndexNames
        {
            get { return Current.IndexNames; }
        }
        internal Dictionary<Type, string> From
        {
            get { return Current.From; }
        }
        internal void Settor(PropertyInfo prop, ConstantExpression value)
        {
            if (prop == null)
                return;

            var name = IParser.GetMemberAlias(prop, IParser.GetMemberName(prop));
            if (!string.IsNullOrWhiteSpace(name) && !Current.Settor.ContainsKey(name))
                Current.Settor.Add(name, IParser.GetValue(value));
        }
        internal void Settor(MemberInfo member, ConstantExpression value)
        {
            var name = IParser.GetMemberAlias(member, IParser.GetMemberName(member));
            if (!string.IsNullOrWhiteSpace(name) && !Current.Settor.ContainsKey(name))
                Current.Settor.Add(name, IParser.GetValue(value));
        }
        internal void SetAction(Action ac)
        {
            Current.Action = ac;
        }

        protected override Expression VisitNew(NewExpression nex)
        {
            foreach (var arg in nex.Arguments)
            {
                if (arg.NodeType != ExpressionType.MemberAccess)
                    continue;

                if (!arg.Type.Equals(typeof(string)) &&
                    !arg.Type.BaseType.Equals(typeof(System.ValueType)))
                    continue;

                var exclude = Attribute.GetCustomAttribute(((MemberExpression)arg).Member, typeof(Mapping.Exclude));
                if (exclude != null)
                    continue;

                var key = Parser.GetMemberName((MemberExpression)arg);
                var val = string.Format("[{0}].[{1}]", Parser.GetMemberPrefix((MemberExpression)arg), key);

                if (!Current.Columns.ContainsKey(key))
                    Current.Columns.Add(key, val);
            }

            return base.VisitNew(nex);
        }
        protected override Expression VisitParameter(ParameterExpression p)
        {
            var t = p.Type;
            if (t.GetGenericArguments().Length > 1)
                return base.VisitParameter(p);

            if(!Current.From.ContainsKey(t))
                Current.From.Add(t, string.Format("[{0}] AS [{1}]", GetTableName(t), p));

            return base.VisitParameter(p);
        }
        protected override MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
        {
            var settor = new Settor();
            if (assignment.Expression.NodeType != ExpressionType.Constant)
            {
                LambdaExpression lambda = Expression.Lambda(assignment.Expression);
                Delegate fn = lambda.Compile();
                var val = fn.DynamicInvoke(null);
                settor.Stack(assignment.Member.Name, ConstantExpression.Constant(val));
            }
            else
                settor.Stack(assignment.Member.Name, ((ConstantExpression)assignment.Expression));

            var item = ((KeyValuePair<string, object>)settor.Context);
            if(!Current.Settor.ContainsKey(item.Key))
                Current.Settor.Add(item.Key, item.Value);

            return base.VisitMemberAssignment(assignment);
        }
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Arguments.Count < 1)
                return m;

            bool isUnion = false;
            this.Visit(m.Arguments[0]);
            switch (m.Method.Name)
            {
                case "Count":
                    if (m.Method.DeclaringType == typeof(System.Linq.Queryable))
                        Current.ResultType = ResultType.Count;
                        
                    return m;
                case "Sum":
                case "Min":
                case "Max":
                    if (m.Method.DeclaringType != typeof(System.Linq.Queryable))
                        return AddToColumns(m);

                    return AddToColumns(m, (ResultType)Enum.Parse(typeof(ResultType), m.Method.Name));
                case "Average":
                    if (m.Method.DeclaringType != typeof(System.Linq.Queryable))
                        return AddToColumns(m, "Avg");

                    return AddToColumns(m, ResultType.Average);
                case "Update":
                    break;
                case "Where":
                    Parser = new Where();
                    break;
                case "ThenBy":
                case "OrderBy":
                    Parser = new Asc();
                    break;
                case "ThenByDescending":
                case "OrderByDescending":
                    Parser = new Desc();
                    break;
                case "Union":
                    Union.Add(Current.GetHashCode(), Current);
                    Current = SqlStatementFactory.Create(DbType);
                    Parser = null;
                    isUnion = true;
                    break;
                case "SelectMany":
                case "Select":
                    Parser = new Column();
                    break;
                case "StartsWith":
                    this.Visit(m.Object);
                    Parser.Stack("{0} LIKE '{1}%'",
                        (MemberExpression) m.Object,
                        GetConstant(m.Arguments[0]));
                    return m;
                case "Contains":
                    this.Visit(m.Object);
                    Parser.Stack("{0} LIKE '%{1}%'", 
                        (MemberExpression) m.Object,
                        GetConstant(m.Arguments[0]));
                    return m;
                case "EndsWith":
                    Parser.Stack("{0} LIKE '%{1}'", 
                        (MemberExpression) m.Object,
                        GetConstant(m.Arguments[0]));
                    this.Visit(m.Object);
                    return m;
                case "First":
                case "FirstOrDefault":
                    Current.Top = 1;
                    var t = m.Arguments[0].Type.GetGenericArguments()[0];
                    if(!Current.From.ContainsKey(t))
                        Current.From.Add(t, string.Format("[{0}]", GetTableName(t)));
                    return m;
                case "Take":
                    Current.Top = Numbic.GetInstance().Stack(m.Arguments[1]).Value;
                    return m;
                case "Skip":
                    Current.Skip = Numbic.GetInstance().Stack(m.Arguments[1]).Value;
                    return m;
                case "Distinct":
                    Current.Distinct = true;
                    return m;
                case "GroupBy":
                    Parser = new Parser.Group();
                    break;
                case "Join":
                case "GroupJoin":
                    Current.Join = Join.GetInstance(m.Method.Name).Stack(m);
                    return m;
                default:
                    return m;
            }

            LambdaExpression lambda = Expression.Lambda(StripQuotes(m.Arguments[1]));
            this.Visit(lambda.Body);
            ComputeStatement();

            if (isUnion)
            {
                Union.Add(Current.GetHashCode(), Current);
                Current = SqlStatementFactory.Create(DbType);
                Parser = null;
            }

            return m;
        }

        private static ConstantExpression GetConstant(Expression exp)
        {
            return (exp.NodeType == ExpressionType.MemberAccess ?
                                    Expression.Constant(Expression.Lambda(exp).Compile().DynamicInvoke()) :
                                    (ConstantExpression)exp);
        }

#if CSHARP30
        private Expression AddToColumns(MethodCallExpression m)
        {
            return AddToColumns(m, null);
        }
        private Expression AddToColumns(MethodCallExpression m, string functionName)
#else
        private Expression AddToColumns(MethodCallExpression m, string functionName = null)
#endif
        {
            var exclude = Attribute.GetCustomAttribute(((MemberExpression)m.Arguments[0]).Member, typeof(Mapping.Exclude));
            if (exclude != null)
                return m;

            var key = Parser.GetMemberName((MemberExpression)m.Arguments[0]);
            Parser.Stack(string.IsNullOrWhiteSpace(functionName) ? m.Method.Name : functionName);
            if (!Current.Columns.ContainsKey(key))
                Current.Columns.Add(key, Parser.Context.ToString());
            else
                Current.Columns[key] = Parser.Context.ToString();

            return m;
        }

        private Expression AddToColumns(MethodCallExpression m, ResultType resultType)
        {
            if (m.Arguments.Count < 2)
                return m;

            var member = ((MemberExpression)((LambdaExpression)((UnaryExpression)m.Arguments[1]).Operand).Body);
            var exclude = Attribute.GetCustomAttribute(member.Member, typeof(Mapping.Exclude));
            if (exclude != null)
                return m;

            var memberName = Parser.GetMemberName(member);
            Current.ResultType = resultType;
            if (!Current.Columns.ContainsKey(memberName))
                Current.Columns.Add(memberName, string.Format("`{0}`", memberName));
            else
                Current.Columns[memberName] = string.Format("`{0}`", memberName);

            return m;
        }

        private void ComputeStatement()
        {
            if (Parser != null)
            {
                if (Parser.GetType().Equals(typeof(Where)))
                {
                    var content = Parser.Context.ToString();
                    if (!Current.Where.Contains(content))
                        Current.Where.Add(content);
                }
                else if (Parser.GetType().Equals(typeof(Asc)) ||
                         Parser.GetType().Equals(typeof(Desc)))
                {
                    var content = Parser.Context.ToString();
                    if (!Current.OrderBy.Contains(content))
                        Current.OrderBy.Add(content);
                }
            }
        }
        protected override Expression VisitMember(MemberExpression m)
        {
            if (Parser != null)
                Parser.Stack(m);

            return base.VisitMember(m);
        }
        protected override Expression VisitBinary(BinaryExpression b)
        {
            var op = string.Empty;
            var left = this.Visit(b.Left);
            switch (b.NodeType)
            {
                case ExpressionType.AndAlso:
                    Parser.Stack(Relation.AND);
                    break;
                case ExpressionType.OrElse:
                    Parser.Stack(Relation.OR);
                    break;
                case ExpressionType.Or:
                    op = "|";
                    break;
                case ExpressionType.And:
                    op = "&";
                    break;
                case ExpressionType.Equal:
                    op = "=";
                    break;
                case ExpressionType.NotEqual:
                    op = "<>";
                    break;
                case ExpressionType.LessThan:
                    op = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    op = "<=";
                    break;
                case ExpressionType.GreaterThan:
                    op = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    op = ">=";
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }
            var right = this.Visit(b.Right);

            if (b.NodeType != ExpressionType.AndAlso &&
                b.NodeType != ExpressionType.OrElse)
            {
                if (left.ToString() == "String.Empty")
                    left = Expression.Constant(string.Empty);

                if (right.ToString() == "String.Empty")
                    right = Expression.Constant(string.Empty);

                Parser.Stack(left, op, right);
            }

            return b;
        }

        public override string ToString()
        {
            return ToString(null);
        }
#if CSHARP30
        public override string ToString()
        {
            return ToString(null);
        }
        public string ToString(Type type)
#else
        public string ToString(Type type = null)
#endif
        {
            if (type != null && 
                (Current.Action == Action.InsertSelect || 
                Current.Action == Action.Insert || 
                Current.Action == Action.UpdateSelect))
            {
                if (From.Count > 0 && From.ContainsKey(type))
                    Current.Target = From[type];
                else
                    Current.Target = Visitor.GetTableName(type);
            }
            else if (Current.Action == Action.Select && Union.Count > 0)
                Current.Union(Union);

            return string.Format("{0};\r\n", Current.ToString(type));
        }

        public void Dispose()
        {
            if (Parser != null)
                Parser.Dispose();

            Parser = null;

            if (Current != null)
                Current.Dispose();

            Union = new Dictionary<int,SqlStatement>();
        }

        internal IParser Parser { get; set; }
        protected static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }
        internal static string GetTableName(Type t)
        {
            Func<string> get = () =>
            {
                var attr = Attribute.GetCustomAttribute(t, typeof(Mapping.Tablename));
                if (attr != null && attr is Mapping.Tablename)
                    return (attr as Mapping.Tablename).Name;

                return t.Name;
            };
            return MemberCache.Find(t.GetHashCode(), get());
        }
    }
}
