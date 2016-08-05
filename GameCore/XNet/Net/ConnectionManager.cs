using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace XNet.Libs.Net
{
    /// <summary>
    /// 当前连接管理
    /// @author:xxp
    /// @date:2013/01/09
    /// </summary>
    public class ConnectionManager
    {
        private Dictionary<int, Client> Connections = new Dictionary<int, Client>();

        private object SyncRoot = new object();

        /// <summary>
        /// 所有连接
        /// </summary>
        public List<Client> AllConnections
        {
            get
            {
                lock (SyncRoot)
                {
                    return Connections.Values.ToList();
                }
            }
        }
        /// <summary>
        ///  添加一个连接
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(Client client)
        {
            lock (SyncRoot)
            {
                if (Connections.ContainsKey(client.ID))
                {
                    Connections[client.ID] = client;
                }
                else
                {
                    Connections.Add(client.ID, client);
                }
            }
        }
        /// <summary>
        /// 根据连接ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client GetClientByID(int id)
        {
            lock (SyncRoot)
            {
                Client client;
                Connections.TryGetValue(id, out client);
                return client;
            }
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
        public void RemoveClient(int id)
        {
            lock (SyncRoot)
            {
                Connections.Remove(id);
            }
        }
        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="action"></param>
        public void Each(Action<Client> action)
        {
            var clients = AllConnections;
            foreach (var i in clients)
            { action(i); }
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            lock (SyncRoot)
            {
                Connections.Clear();
            }
        }
        /// <summary>
        /// 当前连接数
        /// </summary>
        public int Count
        {
            get
            {
                lock (SyncRoot)
                {
                    return Connections.Count;
                }
            }
        }

        private volatile int Index = 1;
        /// <summary>
        /// 创建一个连接
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public Client CreateClient(Socket socket, SocketServer server)
        {
            if (Index == int.MaxValue)
                Index = 1;
            var id = Index++;
            var client = new Client(server, socket, id);
            AddClient(client);
			client.LastMessageTime = DateTime.UtcNow;
            return client;
        }
    }
}
