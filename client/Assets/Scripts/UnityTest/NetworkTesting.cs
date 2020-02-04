using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Networking;
using XNet.Libs.Net;
using Proto.BattleServerService;
using XNet.Libs.Utility;

namespace Tests
{
    public class NetworkTesting
    {

        private class UnityLoger : Loger
        {
            #region implemented abstract members of Loger
            public override void WriteLog(DebugerLog log)
            {
                switch (log.Type)
                {
                    case LogerType.Error:
                        Debug.LogError(log);
                        break;
                    case LogerType.Log:
                        Debug.Log(log);
                        break;
                    case LogerType.Waring:
                    case LogerType.Debug:
                        Debug.LogWarning(log);
                        break;
                }

            }
            #endregion
        }

        // A Test behaves as an ordinary method
        [Test]
        public void NetworkTestingSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        [Timeout(3000)]
        public IEnumerator NetworkTestingWithEnumeratorPasses()
        {

            Debuger.Loger = new UnityLoger();
            var requestHandler = new RequestHandle<BattleServerService>();
            var server = new SocketServer(new ConnectionManager(), 12000)
            {
                HandlerManager = requestHandler
            };

            server.Start();

            //yield return new WaitUntil(()=>server)


            var client = new  RequestClient<TaskHandler>("127.0.0.1",2000);
            client.Connect();
            yield return new WaitUntil(() => client.IsConnect);

            var requ = JoinBattle.CreateQuery();
            yield return requ
                .Send(client, new Proto.C2B_JoinBattle { AccountUuid = "222", Session = "11" });

            client.Disconnect();
            server.Stop();

        }
    }
}
