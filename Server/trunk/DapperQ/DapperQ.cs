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
using System.Threading.Tasks;
using DapperQ.Parser;
using DapperQ.Factory;
using Dapper;

namespace DapperQ
{
    public static class DapperQ
    {
        /// <summary>
        /// 创建索引
        /// （Create Index）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="dq">DapperQ</param>
        /// <param name="indexName">索引名称数组（Index names）</param>
        public static void CreateIndex<T>(this DapperQ<T> dq, params string[] keyNames) where T : class, new()
        {
            dq.Execute(Action.CreateIndex, typeof(T), keyNames);
        }

        /// <summary>
        /// 移除表
        /// （Drop table）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="dq">DapperQ</param>
        public static void Drop<T>(this DapperQ<T> dq) where T : class, new()
        {
            dq.Execute(Action.DropTable, typeof(T));
        }

        /// <summary>
        /// 删除所有索引[未实现]
        /// （Remove all index [invalid]）
        /// </summary>
        public static void DropAllIndexes<T>(this DapperQ<T> dq) where T : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除索引
        /// （Drop Index）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="dq">DapperQ</param>
        /// <param name="indexName">索引名称数组（Index names）</param>
        public static void DropIndex<T>(this DapperQ<T> dq, params string[] indexNames) where T : class, new()
        {
            dq.Execute(Action.DropIndex, typeof(T), indexNames);
        }

        /// <summary>
        /// 移除索引
        /// （Drop Index）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="dq">DapperQ</param>
        /// <param name="indexName">索引名称（Index name）</param>
        public static void DropIndexByName<T>(this DapperQ<T> dq, string indexName) where T : class, new()
        {
            dq.Execute(Action.DropIndex, typeof(T), new string[] { indexName });
        }

        /// <summary>
        /// 检查Lambda表达式查询指定的数据是否存在
        /// （Check data is exists by lambda）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="dq">DapperQ</param>
        /// <param name="where">Lambda条件表达（Where condition by lambda）</param>
        /// <returns>true or false</returns>
        public static bool Exists<T>(this DapperQ<T> dq, Expression<Func<T, bool>> where) where T : class, new()
        {
            return dq.AsQueryable().Where(where).Exists();
        }

        /// <summary>
        /// 获取指定表的索引名称
        /// （Get index names for table）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="dq">DapperQ</param>
        /// <returns>索引名称数组（Index names）</returns>
        public static string[] GetIndexes<T>(this DapperQ<T> dq) where T : class, new()
        {
            var result = dq.Execute(Action.GetIndexes, typeof(T));
            if(((IList)result).Count==0)
                return null;

            var list = new List<string>();
            foreach (var item in ((IList)result))
            {
                var dict = item as IDictionary<string, object>;
                list.Add(dict.First().Value.ToString());
            }

            return list.ToArray();
        }

        /// <summary>
        /// 检查索引是否存在
        /// （Index exists）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="dq">DapperQ</param>
        /// <param name="indexName">索引名称（Index name）</param>
        /// <returns>True or false</returns>
        public static bool IndexExistsByName<T>(this DapperQ<T> dq, string indexName) where T : class, new()
        {
            return dq.Execute(Action.IndexExists, typeof(T), indexName);
        }

        /// <summary>
        /// 插入数据到数据库
        /// （Insert data in database）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="dq">DapperQ</param>
        /// <param name="info">实体（Entity）</param>
        public static void Insert<T>(this DapperQ<T> dq, object info) where T : class, new()
        {
            dq.Execute(Action.Insert, info);
        }

        /// <summary>
        /// 更新Lambda表达式查询指定的数据
        /// （Update data in database by lambda）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="dq">DapperQ</param>
        /// <param name="data">Lambda数据表达式（Assignment by lambda）</param>
        /// <param name="where">Lambda条件表达（Where condition by lambda）</param>
        public static void Update<T>(this DapperQ<T> dq, Expression<Func<T, T>> data, Expression<Func<T, bool>> where) where T : class, new()
        {
            dq.AsQueryable().Where(where).Update(data);
        }

        /// <summary>
        /// 删除Lambda表达式查询指定的数据
        /// （Remove data in database by lambda）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="dq">DapperQ</param>
        /// <param name="where">Lambda条件表达（Where condition by lambda）</param>
        public static void Delete<T>(this DapperQ<T> dq, Expression<Func<T, bool>> where) where T : class, new()
        {
            dq.AsQueryable().Where(where).Delete();
        }
    }

    public static class Queryable
    {
        /// <summary>
        /// 插入Linq查询指定的数据
        /// （Insert data in database by queryable）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="query">IQueryable</param>
        public static void Insert<T>(this IQueryable<object> query)
        {
            (query.Provider as QueryProvider).Execute(Action.InsertSelect, query.Expression, typeof(T));
        }

        /// <summary>
        /// 更新Linq查询指定的数据
        /// （Update data in database by queryable）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="query">IQueryable</param>
        /// <param name="info">实体（Entity）</param>
        public static void Update<T>(this IQueryable<T> query, T info)
        {
            query.Update<T>(o => info, info);
        }

        /// <summary>
        /// 更新Linq查询指定的数据
        /// （Update data in database by queryable）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="query">IQueryable</param>
        /// <param name="data">Lambda数据表达式（Assignment by lambda）</param>
        public static void Update<T>(this IQueryable<T> query, Expression<Func<T, T>> dataExp)
        {
            query.Update(dataExp, null);
        }

        private static void Update<T>(this IQueryable<T> query, Expression<Func<T, T>> dataExp, object data)
        {
            (query.Provider as QueryProvider).Execute(Action.UpdateSelect, query.Expression, dataExp, data);
        }

        /// <summary>
        /// 检查Linq查询指定的数据是否存在
        /// （Query data is exists）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="query">IQueryable</param>
        /// <param name="data">Lambda数据表达式（Assignment by lambda）</param>
        public static bool Exists<T>(this IQueryable<T> query)
        {
            return (query.Provider as QueryProvider).Execute(Action.Exists, query.Expression);
        }

        /// <summary>
        /// 删除Linq查询指定的数据
        /// （Remove data in database by queryable）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="query">IQueryable</param>
        public static void Delete<T>(this IQueryable<T> query)
        {
            (query.Provider as QueryProvider).Execute(Action.DeleteSelect, query.Expression);
        }

        /// <summary>
        /// 将Queryable转为SQL语句
        /// （To sql statement）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="query">IQueryable</param>
        /// <param name="ac">
        /// DapperQ.Action [Enum]
        /// (Select|InsertSelect|UpdateSelect|DeleteSelect|IndexExists|Exists)
        /// </param>
        /// <param name="data">Lambda数据表达式（Assignment by lambda）</param>
        /// <returns>SQL语句（Sql statement）</returns>
#if CSHARP30
        public static string ToString<T>(this IQueryable<T> query, Action ac)
        {
            return query.Tostring<T>(ac, null);
        }
        public static string ToString<T>(this IQueryable<T> query, Action ac, Expression<Func<T, T>> data)
#else
        public static string ToString<T>(this IQueryable<T> query, Action ac, Expression<Func<T, T>> data = null)
#endif
        {
            return (query.Provider as QueryProvider).GetQueryText<T>(query.Expression, ac, data, typeof(T));
        }
        
        /// <summary>
        /// 将Queryable转为SQL语句
        /// （To sql statement）
        /// </summary>
        /// <typeparam name="T">泛型类型（Model anonymous type）</typeparam>
        /// <param name="query">IQueryable</param>
        /// <param name="ac">
        /// DapperQ.Action [Enum]
        /// (Select|InsertSelect|UpdateSelect|DeleteSelect|IndexExists|Exists)
        /// </param>
        /// <returns>SQL语句（Sql statement）</returns>
        public static string ToString<T>(this IQueryable<object> query, Action ac)
        {
            return (query.Provider as QueryProvider).GetQueryText<object>(query.Expression, ac, null, typeof(T));
        }
    }
}
