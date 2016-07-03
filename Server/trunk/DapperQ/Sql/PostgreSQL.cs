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
    internal class PostgreSQL : MySql
    {
        protected override string GenIndexExistsStatement()
        {
            return string.Format("SELECT 1 FROM (SELECT NULL AS TABLE_CAT, n.nspname AS TABLE_SCHEM, ct.relname AS TABLE_NAME,  NOT i.indisunique AS NON_UNIQUE, NULL AS INDEX_QUALIFIER, ci.relname AS INDEX_NAME, CASE i.indisclustered WHEN true THEN 1 ELSE CASE am.amname WHEN 'hash' THEN 2 ELSE 3 END END AS TYPE, am.amname as index_method, (i.keys).n AS ORDINAL_POSITION,  pg_catalog.pg_get_indexdef(ci.oid, (i.keys).n, false) AS COLUMN_NAME,  CASE am.amcanorder WHEN true THEN CASE i.indoption[(i.keys).n - 1] & 1  WHEN 1 THEN 'D' ELSE 'A' END ELSE NULL END AS ASC_OR_DESC,  ci.reltuples AS CARDINALITY, ci.relpages AS PAGES,  pg_catalog.pg_get_expr(i.indpred, i.indrelid) AS FILTER_CONDITION FROM  pg_catalog.pg_class ct JOIN pg_catalog.pg_namespace n ON  (ct.relnamespace = n.oid) JOIN (SELECT i.indexrelid, i.indrelid,  i.indoption, i.indisunique, i.indisclustered, i.indpred, i.indexprs,  information_schema._pg_expandarray(i.indkey) AS keys FROM  pg_catalog.pg_index i) i ON (ct.oid = i.indrelid) JOIN  pg_catalog.pg_class ci ON (ci.oid = i.indexrelid) JOIN pg_catalog.pg_am am ON (ci.relam = am.oid) WHERE true AND  ct.relname = 'tableName' ORDER BY NON_UNIQUE, TYPE, INDEX_NAME, ORDINAL_POSITION) AS S WHERE " +
                "table_name='{0}'", From.First().Value);
        }

        protected override string GenGetIndexStatement()
        {
            return string.Format("SELECT index_name AS idx_name FROM (SELECT NULL AS TABLE_CAT, n.nspname AS TABLE_SCHEM, ct.relname AS TABLE_NAME,  NOT i.indisunique AS NON_UNIQUE, NULL AS INDEX_QUALIFIER, ci.relname AS INDEX_NAME, CASE i.indisclustered WHEN true THEN 1 ELSE CASE am.amname WHEN 'hash' THEN 2 ELSE 3 END END AS TYPE, am.amname as index_method, (i.keys).n AS ORDINAL_POSITION,  pg_catalog.pg_get_indexdef(ci.oid, (i.keys).n, false) AS COLUMN_NAME,  CASE am.amcanorder WHEN true THEN CASE i.indoption[(i.keys).n - 1] & 1  WHEN 1 THEN 'D' ELSE 'A' END ELSE NULL END AS ASC_OR_DESC,  ci.reltuples AS CARDINALITY, ci.relpages AS PAGES,  pg_catalog.pg_get_expr(i.indpred, i.indrelid) AS FILTER_CONDITION FROM  pg_catalog.pg_class ct JOIN pg_catalog.pg_namespace n ON  (ct.relnamespace = n.oid) JOIN (SELECT i.indexrelid, i.indrelid,  i.indoption, i.indisunique, i.indisclustered, i.indpred, i.indexprs,  information_schema._pg_expandarray(i.indkey) AS keys FROM  pg_catalog.pg_index i) i ON (ct.oid = i.indrelid) JOIN  pg_catalog.pg_class ci ON (ci.oid = i.indexrelid) JOIN pg_catalog.pg_am am ON (ci.relam = am.oid) WHERE true AND  ct.relname = 'tableName' ORDER BY NON_UNIQUE, TYPE, INDEX_NAME, ORDINAL_POSITION) AS S WHERE " +
                "table_name='{0}'", From.First().Value);
        }
        protected override string EnsureKeyword(string sql)
        {
            Regex regex = new Regex(@"[\[\]]", RegexOptions.Multiline | RegexOptions.Compiled);
            sql = regex.Replace(sql, "");
            return sql;
        }
    }
}
