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
            Assert.AreEqual("pong", client.Ping());
        }

        [TestMethod]
        public void PingGuardian()
        {
            var client = new GuardianClient("http://localhost:8001");
            Assert.AreEqual("pong", client.Ping());
        }
    }
}
