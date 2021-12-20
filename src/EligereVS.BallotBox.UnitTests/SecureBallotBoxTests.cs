using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace EligereVS.BallotBox.UnitTests
{
    [TestClass]
    public class SecureBallotBoxTests
    {
        private ServiceProvider _services;

        public SecureBallotBoxTests()
        {
            var sc = new ServiceCollection();
            sc.AddDataProtection();
            _services = sc.BuildServiceProvider();
        }
        [TestMethod]
        public void CreateBallotBox()
        {
            var protection = _services.GetDataProtectionProvider();
            var basedir = Path.GetTempPath();
            var secureBBox = SecureBallotBox.CreateSecureBallotBox(protection, basedir, "SecureBallotBoxTest");
            Assert.IsNotNull(secureBBox);
            secureBBox.Close();
        }
        [TestMethod]
        public void TestKeys()
        {
            var protection = _services.GetDataProtectionProvider();
            var basedir = Path.GetTempPath();
            var secureBBox = SecureBallotBox.CreateSecureBallotBox(protection, basedir, "SecureBallotBoxTest");
            Assert.IsNotNull(secureBBox);
            var keys = secureBBox.GetGuardianKeys(5, 5);
            Assert.IsNotNull(keys);
            Assert.AreEqual(5, keys.Length);
            Assert.AreNotEqual(null, secureBBox.Tally(keys));
            secureBBox.Close();
        }
    }
}
