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
using System.Threading.Tasks;

namespace DapperQ.Parser
{
    internal class Where : IParser
    {
        StringBuilder sb;
        public Where()
        {
            sb = new StringBuilder();
        }
        internal override object Context { get { return sb.ToString(); } }

        public override void Dispose()
        {
            sb = null;
        }
        internal override void Stack(string expString)
        {
            sb.Append(expString);
        }
        internal override void Stack(string format, MemberExpression member, ConstantExpression constant)
        {
            if (HasExclude(member.Member))
                return;

            sb.AppendFormat(format, string.Format("[{0}].[{1}]", GetMemberPrefix(member.Expression), GetMemberName(member)), constant.Value);
        }
        internal override void Stack(Relation condition)
        {
            sb.AppendFormat(" {0} ", condition.ToString());
        }
        internal override void Stack(Expression left, string op, Expression right)
        {
            bool hasLeftValue = (left.ToString().IndexOf("value") > -1 || left.NodeType == ExpressionType.Call);
            bool hasRightValue = (right.ToString().IndexOf("value") > -1 || left.NodeType == ExpressionType.Call);
            if (hasLeftValue || hasRightValue)
            {
                if (hasLeftValue && hasRightValue)
                {
                    Stack(Expression.Constant(Expression.Lambda(left).Compile().DynamicInvoke()), op,
                        Expression.Constant(Expression.Lambda(right).Compile().DynamicInvoke()));
                    return;
                }

                if (!hasLeftValue && hasRightValue)
                {
                    Stack(left, op, Expression.Constant(Expression.Lambda(right).Compile().DynamicInvoke()));
                    return;
                }

                if (hasLeftValue && !hasRightValue)
                {
                    Stack(Expression.Constant(Expression.Lambda(left).Compile().DynamicInvoke()), op, right);
                    return;
                }
            }

            if (right.NodeType == ExpressionType.MemberAccess &&
                left.NodeType == ExpressionType.MemberAccess)
            {
                var lm = (MemberExpression)left;
                var rm = (MemberExpression)right;
                if (HasExclude(lm.Member) || HasExclude(rm.Member))
                    return;

                sb.Append(string.Format("{0}{1}{2}",
                    string.Format("[{0}].[{1}]", GetMemberPrefix(lm.Expression), GetMemberName(lm)), op,
                    string.Format("[{0}].[{1}]", GetMemberPrefix(rm.Expression), GetMemberName(rm))));

                return;
            }

            if (right.NodeType == ExpressionType.MemberAccess &&
                left.NodeType == ExpressionType.Constant)
            {
                var m = (MemberExpression)right;
                if (HasExclude(m.Member))
                    return;

                sb.Append(string.Format("{0}{1}{2}",
                    GetValue((ConstantExpression)left), op,
                    string.Format("[{0}].[{1}]", GetMemberPrefix(m.Expression), GetMemberName(m))
                    ));

                return;
            }

            if (right.NodeType == ExpressionType.Constant &&
                left.NodeType == ExpressionType.MemberAccess)
            {
                var m = (MemberExpression)left;
                if (HasExclude(m.Member))
                    return;

                sb.Append(string.Format("{0}{1}{2}",
                    string.Format("[{0}].[{1}]", GetMemberPrefix(m.Expression), GetMemberName(m)), op,
                    GetValue((ConstantExpression)right)));

                return;
            }

            if (right.NodeType == ExpressionType.Constant &&
                left.NodeType == ExpressionType.Constant)
            {
                var m = (MemberExpression)left;
                sb.Append(string.Format("{0}{1}{2}",
                    GetValue((ConstantExpression)left), op,
                    GetValue((ConstantExpression)right)));

                return;
            }
        }
    }
}
