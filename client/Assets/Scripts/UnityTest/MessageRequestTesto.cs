using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Proto.LoginServerService;
using UnityEngine;
using UnityEngine.TestTools;
using XNet.Libs.Net;

namespace Tests
{
    public class MessageRequestTesto
    {
        // A Test behaves as an ordinary method
        [Test]
        public void MessageRequestTestoSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator MessageRequestTestoWithEnumeratorPasses()
        {

            bool isConnecting = true;

            var client = new RequestClient<TaskHandler>("127.0.0.1", 1700)
            {
                OnConnectCompleted = (s) =>
                {
                    isConnecting = false;
                }
            };
            client.Connect();

            while (isConnecting) yield return null;
            Assert.IsTrue(client.IsConnect);
            var query = Reg.CreateQuery();
            yield return query
                .Send(client, new Proto.C2L_Reg { UserName = "admin", Password = "123456", Version = 1 });
            Assert.IsTrue(query.QueryRespons.Code.IsOk());

            var logQuery = Login.CreateQuery();
            yield return logQuery
                .Send(client, new Proto.C2L_Login { Password = "123456", UserName = "admin", Version = 1 });
            Assert.IsTrue(logQuery.QueryRespons.Code.IsOk());

            yield return null;
        }
    }
}
