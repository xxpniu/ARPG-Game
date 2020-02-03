using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace XNet.Libs.Net
{

    public delegate bool EachCondition(Client client);

    /// <summary>
    /// 当前连接管理
    /// @author:xxp
    /// @date:2013/01/09
    /// </summary>
    public class ConnectionManager
    {
        private readonly ConcurrentDictionary<int, Client> Connections = new ConcurrentDictionary<int, Client>();
        private volatile int Index = 1;

        /// <summary>
        /// 当前连接数
        /// </summary>
        public int Count{get {return Connections.Count;}}

        /// <summary>
        /// 所有连接
        /// </summary>
        public IList<Client> AllConnections{get{ return Connections.Values.ToList(); }}

        /// <summary>
        ///  添加一个连接
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(Client client)
        {
            Connections.AddOrUpdate(client.ID, client,(key,old)=> { return client; });
        }

        /// <summary>
        /// 根据连接ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client GetClientByID(int id)
        {
            if (Connections.TryGetValue(id, out Client c)) return c;
            return null;
        }
        /// <summary>
        /// 删除一个连接
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(Client client)
        {
            RemoveClient(client.ID);
        }
        /// <summary>
        /// 根据iD删除一个
        /// </summary>
        /// <param name="id"></param>
        public bool RemoveClient(int id)
        {
            return Connections.TryRemove(id, out _);
        }

        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="action"></param>
        public void Each(EachCondition action)
        {
            var clients = AllConnections;
            foreach (var i in clients) if (action(i)) break;
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            Connections.Clear();
        }
      
        /// <summary>
        /// 存在
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool Exisit(Client client)
        {
            return Connections.ContainsKey(client.ID);
        }

        /// <summary>
        /// 创建一个连接
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public Client CreateClient(Socket socket, SocketServer server)
        {
            if (Index == int.MaxValue) Index = 1;
            var id = Index++;
            var client = new Client(server, socket, id);
            AddClient(client);
            client.LastMessageTime = DateTime.UtcNow;
            return client;
        }
    }
}
