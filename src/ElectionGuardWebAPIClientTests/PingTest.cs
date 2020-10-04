using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElectionGuard;

namespace ElectionGuardWebAPIClientTests
{
    [TestClass]
    public class PingTest
    {
        [TestMethod]
        public void PingMediator()
        {
            var client = new MediatorClient("http://localhost:8000");
            var resp = client.Ping();
            resp.Wait();
            Assert.AreEqual("pong", resp.Result);
        }

        [TestMethod]
        public void PingGuardian()
        {
            var client = new GuardianClient("http://localhost:8001");
            var resp = client.Ping();
            resp.Wait();
            Assert.AreEqual("pong", resp.Result);
        }
    }
}
