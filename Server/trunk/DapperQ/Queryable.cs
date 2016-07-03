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
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperQ.Parser;
using DapperQ.Factory;
using DapperQ.Mapping;
using System.Runtime.CompilerServices;



namespace DapperQ
{
    public class Queryable<T> : 
        IQueryable<T>, 
        IQueryable, 
        IEnumerable<T>, 
        IEnumerable, 
        IOrderedQueryable<T>, 
        IOrderedQueryable
    {
        QueryProvider provider;
        Expression expression;

        public Queryable(QueryProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            this.provider = provider;
            this.expression = Expression.Constant(this);
        }

        internal Queryable(QueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }

            this.provider = provider;
            this.expression = expression;
        }

        Expression IQueryable.Expression
        {
            get { return this.expression; }
        }

        Type IQueryable.ElementType
        {
            get { return typeof(T); }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return this.provider; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var result = this.provider.Execute(this.expression);
            if (TypeSystem.IsAnonymousType(typeof(T)))
                return Object2<T>.ToAnonymousList(result as IEnumerable).GetEnumerator();

            return ((IEnumerable<T>)result).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.provider.Execute(this.expression)).GetEnumerator();
        }

        public override string ToString()
        {
            return this.provider.GetQueryText(this.expression);
        }
    }

    public abstract class QueryProvider : IQueryProvider
    {
        protected QueryProvider() { }
        
        IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression)
        {
            return new Queryable<S>(this, expression);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(Queryable<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        S IQueryProvider.Execute<S>(Expression expression)  
        {
            object obj = default(S);
            if (expression.NodeType == ExpressionType.Call)
            {
                var call = ((MethodCallExpression)expression).Method.Name;

                if ((call == "First" || call == "FirstOrDefault"))
                {
                    var data = this.Execute<S>(expression);
                    var list = (IList)data;
                    if (list.Count > 0)
                        return (S)list[0];
                    else
                        return default(S);
                }

                var result = this.Execute<DapperResult>(expression);

                if (call == ResultType.Count.ToString())
                    obj = ((DapperResult)((IList)result)[0]).Count;

                else if (call == ResultType.Average.ToString())
                    obj = ((DapperResult)((IList)result)[0]).Average;

                else if (call == ResultType.Max.ToString())
                    obj = ((DapperResult)((IList)result)[0]).Max;

                else if (call == ResultType.Min.ToString())
                    obj = ((DapperResult)((IList)result)[0]).Min;

                else if (call == ResultType.Sum.ToString())
                    obj = ((DapperResult)((IList)result)[0]).Sum;
            }

            return (S)obj;
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return this.Execute(expression);
        }

        public abstract string GetQueryText(Expression expression);
#if CSHARP30
        public abstract string GetQueryText<T1>(Expression expression, Action ac);
        public abstract string GetQueryText<T1>(Expression expression, Action ac, Expression<Func<T1, T1>> exp);
        public abstract string GetQueryText<T1>(Expression expression, Action ac, Expression<Func<T1, T1>> exp, Type type);
        internal abstract object Execute(Action action, Expression exp_where);
        internal abstract object Execute(Action action, Expression exp_where, Expression exp_values);
        internal abstract object Execute(Action action, Expression exp_where, Expression exp_values, object data);
        internal abstract object Execute(Action action, Type type);
        internal abstract object Execute(Action action, Type type, string[] keyNames);
#else
        public abstract string GetQueryText<T1>(Expression expression, Action ac, Expression<Func<T1, T1>> exp = null, Type type = null);
        internal abstract object Execute(Action action, Expression exp_where, Expression exp_values = null, object data = null);
        internal abstract object Execute(Action action, Type type = null, string[] keyNames = null);
#endif
        public abstract object Execute(Expression expression);
        internal abstract object Execute<S>(Expression expression);
        internal abstract bool Execute(Action action, Expression expression);
        internal abstract long Execute(Action action, object data);
        internal abstract object Execute(Action action, Expression expression, Type type);
        internal abstract bool Execute(Action action, Type type, string indexName);
    }

    internal static class TypeSystem
    {
        internal static bool IsAnonymousType(Type type)
        {
            if (!type.IsGenericType) return false; 
            
            if ((type.Attributes & TypeAttributes.NotPublic) != TypeAttributes.NotPublic) 
                return false; 
            
            if (!Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)) 
                return false; 
            
            return type.Name.Contains("AnonymousType");
        }
        internal static Type GetElementType(Type seqType)
        {
            Type ienum = FindIEnumerable(seqType);
            if (ienum == null) return seqType;
            return ienum.GetGenericArguments()[0];
        }

        private static Type FindIEnumerable(Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
                return null;

            if (seqType.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());

            if (seqType.IsGenericType)
            {
                foreach (Type arg in seqType.GetGenericArguments())
                {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType))
                    {
                        return ienum;
                    }
                }
            }

            Type[] ifaces = seqType.GetInterfaces();
            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (Type iface in ifaces)
                {
                    Type ienum = FindIEnumerable(iface);
                    if (ienum != null) return ienum;
                }
            }

            if (seqType.BaseType != null && seqType.BaseType != typeof(object))
            {
                return FindIEnumerable(seqType.BaseType);
            }
            return null;
        }

    }

    public class DapperQ<T> : QueryProvider, IDisposable where T : class, new()
    {
        static readonly object lockObj = new object();
        static volatile Dictionary<string, DataSource> dict = new Dictionary<string, DataSource>();

        public DataSource DataSource { get; private set; }
        public bool OpenTransaction { get; set; }
        public DapperQ()
        {
            DataSource = GetDataSource();
            if (DataSource == null)
                throw new NotImplementedException();

            Connection = ConnectionFactory.Create(DataSource);
            Visitor = new Visitor(DataSource.DbType);

            OpenTransaction = false;
        }

        public DapperQ(IDbConnection conn)
        {
            Connection = conn;
            DataSource = GetDataSource();
            DbType? dbType = null;
            if (string.IsNullOrWhiteSpace(Connection.ConnectionString) && DataSource != null)
            {
                Connection.ConnectionString = DataSource.ConnectionString;
                dbType = DataSource.DbType;
            }
            Visitor = new Visitor(DataSource.GetDbType(conn.ToString(), (dbType == null ? DbType.Mysql : (DbType)dbType), conn.ConnectionString));

            OpenTransaction = false;
        }


        public override string GetQueryText(Expression expression)
        {
            return this.GetQueryText<EmptyClass>(expression, Action.Select);
        }
#if CSHARP30
        public override string GetQueryText<T1>(Expression expression, Action ac)
        {
            return GetQueryText<T1>(expression, ac, null, null);
        }
        public override string GetQueryText<T1>(Expression expression, Action ac, Expression<Func<T1, T1>> exp)
        {
            return GetQueryText<T1>(expression, ac, exp, null);
        }
        public override string GetQueryText<T1>(Expression expression, Action ac, Expression<Func<T1, T1>> exp, Type type)
#else
        public override string GetQueryText<T1>(Expression expression, Action ac, Expression<Func<T1, T1>> exp = null, Type type = null)
#endif
        {
            Visitor.SetAction(ac);
            Visitor.Visit(Expression.Lambda(expression).Body);
            switch (ac)
            { 
                case Action.UpdateSelect:
                    Visitor.Visit(exp);
                    FillSettor(exp);
                    break;
                case Action.InsertSelect:
                    return Visitor.ToString(type);
            }
            var sql = Visitor.ToString(typeof(T));
            Visitor.Dispose();

            return sql;
        }

        private object SQLExecute<S>(Action ac, string sql)
        {
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();

            switch (ac)
            { 
                case Action.CreateIndex:
                case Action.DeleteSelect:
                case Action.DropIndex:
                case Action.DropTable:
                case Action.InsertSelect:
                case Action.UpdateSelect:
                case Action.Insert:
                    try
                    {
                        return Connection.Execute(sql, null, (OpenTransaction && Transaction == null ?
                            (Transaction = Connection.BeginTransaction()) :
                             Transaction));
                    }
                    catch(DataException e)
                    {
                        if (Transaction != null)
                            Transaction.Rollback();

                        throw e;
                    }
                case Action.GetIndexes:
                    return Connection.Query(sql);
                case Action.IndexExists:
                case Action.Exists:
                case Action.Select:
                default:
                    return Connection.Query<S>(sql);
            }
        }

        public override object Execute(Expression expression)
        {
            Visitor.Visit(Expression.Lambda(expression).Body);
            var sql = Visitor.ToString(typeof(T));
            return SQLExecute<T>(Action.Select, sql);
        }
        internal override object Execute<S>(Expression expression)
        {
            Visitor.Visit(Expression.Lambda(expression).Body);
            var sql = Visitor.ToString(typeof(S));

            return SQLExecute<S>(Action.Select, sql);
        }
        public Queryable<T> AsQueryable()
        {
            return new Queryable<T>(this);
        }

        public IDbConnection Connection { get; private set; }
        public IDbTransaction Transaction { get; private set; }

        public void Commit()
        {
            if (Transaction != null)
            {
                try { Transaction.Commit(); }
                catch { }
                Transaction.Dispose();
                Transaction = null;
            }
        }

        internal Visitor Visitor { get; set; }
#if CSHARP30
        internal override object Execute(Action action, Expression exp_where)
        {
            return Execute(action, exp_where, null);
        }
        internal override object Execute(Action action, Expression exp_where, Expression exp_values)
        {
            return Execute(action, exp_where, exp_values, null);
        }
        internal override object Execute(Action action, Expression exp_where, Expression exp_values, object data)
#else
        internal override object Execute(Action action, Expression exp_where, Expression exp_values = null, object data = null)
#endif
        {
            Visitor.SetAction(action);
            if (exp_where != null && exp_where.NodeType == ExpressionType.Call)
                Visitor.Visit(Expression.Lambda(exp_where).Body);

            FillSettor(exp_values, data);
            var sql = Visitor.ToString(typeof(T));
            return SQLExecute<T>(action, sql);
        }
#if CSHARP30
        private void FillSettor(Expression exp_values)
        {
            FillSettor(exp_values, null);
        }
        private void FillSettor(Expression exp_values, object data)
#else
        private void FillSettor(Expression exp_values, object data = null)
#endif
        {
            if (exp_values != null)
            {
                if (((LambdaExpression)exp_values).Body.NodeType == ExpressionType.New &&
                    TypeSystem.IsAnonymousType(((LambdaExpression)exp_values).Body.Type))
                {
                    var n = ((LambdaExpression)exp_values).Body as NewExpression;
                    for (var i = 0; i < n.Members.Count; i++)
                        Visitor.Settor(n.Members[i], (ConstantExpression)n.Arguments[i]);
                }

                else if (((LambdaExpression)exp_values).Body.NodeType == ExpressionType.MemberInit)
                    Visitor.Visit(((LambdaExpression)exp_values).Body);

                else if (((LambdaExpression)exp_values).Body.NodeType == ExpressionType.MemberAccess)
                {
                    var body = ((LambdaExpression)exp_values).Body;
                    var props = body.Type.GetProperties();
                    foreach (var p in props)
                        Visitor.Settor(p, Expression.Constant(p.GetGetMethod().Invoke(data, null)));
                }
            }
        }
        delegate int Gettor();
        internal override long Execute(Action action, object data)
        {
            Visitor.SetAction(action);
            if (action == Action.Insert)
            {
                foreach (var prop in data.GetType().GetProperties())
                    Visitor.Settor(typeof(T).GetProperty(prop.Name), Expression.Constant(prop.GetAccessors()[0].Invoke(data, null)));
            }
            var sql = Visitor.ToString(typeof(T));

            var result = SQLExecute<T>(action, sql);

            return Convert.ToInt64(result);
        }
        internal override bool Execute(Action action, Expression expression)
        {
            Visitor.SetAction(action);
            Visitor.Visit(Expression.Lambda(expression).Body);
            var sql = Visitor.ToString(expression.Type.GetGenericArguments()[0]);
            var result = SQLExecute<T>(action, sql);
            if (result.GetType() == typeof(int))
                return Convert.ToInt32(result) > 0 ? true : false;

            return (result == null || ((IList)result).Count == 0) ? false : true;
        }
        internal override object Execute(Action action, Expression expression, Type type)
        {
            Visitor.SetAction(action);
            Visitor.Visit(Expression.Lambda(expression).Body);
            if (action == Action.InsertSelect)
            {
                if (expression.NodeType == ExpressionType.Lambda &&
                ((LambdaExpression)expression).Body.NodeType == ExpressionType.MemberAccess)
                {
                    var member = ((LambdaExpression)expression).Body as MemberExpression;
                    Visitor.Visit(member);
                }
            }

            var sql = Visitor.ToString(type);
            return SQLExecute<T>(action, sql);
        }

#if CSHARP30
        internal override object Execute(Action action)
        {
            return Execute(action, null, null);
        }
        internal override object Execute(Action action, Type type)
        {
            return Execute(action, type, null);
        }
        internal override object Execute(Action action, Type type, string[] keyNames)
#else
        internal override object Execute(Action action, Type type = null, string[] keyNames = null)
#endif
        {
            Visitor.SetAction(action);
            if (keyNames != null)
            {
                Visitor.IndexNames.Clear();
                Visitor.IndexNames.AddRange(keyNames);
            }
            Visitor.From.Add(type, Visitor.GetTableName(type));
            var sql = Visitor.ToString(type);
            return SQLExecute<T>(action, sql);
        }

        internal override bool Execute(Action action, Type type, string indexName)
        {
            Visitor.SetAction(action);
            Visitor.IndexNames.Clear();
            Visitor.IndexNames.Add(indexName);
            Visitor.From.Add(type, Visitor.GetTableName(type));
            var sql = Visitor.ToString(type);
            var result = SQLExecute<T>(action, sql);
            if (result.GetType() == typeof(int))
                return Convert.ToInt32(result) > 0 ? true : false;

            return (result == null || ((IList)result).Count == 0) ? false : true;
        }
        private DataSource GetDataSource()
        {
            var dataSource = Attribute.GetCustomAttribute(typeof(T), typeof(DataSource));
            if (dataSource == null)
                return null;

            var classFullName = typeof(T).FullName;
            if (!dict.ContainsKey(classFullName))
            {
                lock (lockObj)
                {
                    if (!dict.ContainsKey(classFullName))
                        dict[classFullName] = dataSource as DataSource;
                }
            }

            return dict[classFullName];
        }

        public void Dispose()
        {
            try
            {
                if (Transaction != null)
                    Transaction.Commit();
            }
            catch { }

            if (Connection.State == ConnectionState.Open ||
                Connection.State == ConnectionState.Connecting)
                Connection.Close();

            Connection.Dispose();
        }
    }

    internal class EmptyClass { }
}
