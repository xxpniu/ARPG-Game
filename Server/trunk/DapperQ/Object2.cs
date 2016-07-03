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
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;

namespace DapperQ
{
    public class Object2<T>
    {
        public static T ToAnonymousObject(object entity)
        {
            var properties = typeof(T).GetProperties();
            object[] datas = new object[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                var propertyInfo = properties[i];
                var entityMember = entity.GetType().GetProperty(propertyInfo.Name);
                if (entityMember == null)
                    continue;

                datas[i] = entityMember.GetAccessors()[0].Invoke(entity, null);
            }
            return (T)typeof(T).GetConstructor(typeof(T).GetGenericArguments()).Invoke(datas);
        }

        public static IEnumerable<T> ToAnonymousList(IEnumerable result)
        {
            List<T> list = new List<T>();
            foreach (var obj in ((IList)result).Cast<object>())
                list.Add(ToAnonymousObject(obj));

            return list;
        }
    }
}
