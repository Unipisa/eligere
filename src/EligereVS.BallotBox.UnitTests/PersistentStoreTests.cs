using Microsoft.VisualStudio.TestTools.UnitTesting;
using EligereVS.BallotBox;
using System.IO;

namespace EligereVS.BallotBox.UnitTests
{
    [TestClass]
    public class PersistentStoreTests
    {
        [TestMethod]
        public void StoreName()
        {
            var basedir = Path.GetTempPath();
            var boxpath = Path.Combine(basedir, "TestBallotBox");
            var boxstorepath = Path.Combine(boxpath, "BallotBox");
            if (Directory.Exists(boxpath))
                Directory.Delete(boxpath, true);

            PersistentStore persistentStore = new(basedir, "TestBallotBox", "BallotBox");
            persistentStore.Open();
            Assert.IsTrue(Directory.Exists(boxpath), $"Failed to create ballotbox directory {boxpath}");
            Assert.IsTrue(Directory.Exists(boxstorepath), $"Failed to create store directory {boxstorepath}");
            persistentStore.Close();
        }

        [TestMethod]
        public void MultipleStores()
        {
            var basedir = Path.GetTempPath();
            var boxpath = Path.Combine(basedir, "TestBallotBox");
            var boxstorepath1 = Path.Combine(boxpath, "BallotBox1");
            var boxstorepath2 = Path.Combine(boxpath, "BallotBox2");
            if (Directory.Exists(boxpath))
                Directory.Delete(boxpath, true);

            PersistentStore persistentStore = new(basedir, "TestBallotBox", "BallotBox1", "BallotBox2");
            persistentStore.Open();
            Assert.IsTrue(Directory.Exists(boxpath), $"Failed to create ballotbox directory {boxpath}");
            Assert.IsTrue(Directory.Exists(boxstorepath1), $"Failed to create store directory {boxstorepath1}");
            Assert.IsTrue(Directory.Exists(boxstorepath2), $"Failed to create store directory {boxstorepath2}");
            persistentStore.Close();
        }

        [TestMethod]
        public void WriteMultipleStores()
        {
            var basedir = Path.GetTempPath();
            var boxpath = Path.Combine(basedir, "TestBallotBox");
            var boxstorepath1 = Path.Combine(boxpath, "BallotBox1");
            var boxstorepath2 = Path.Combine(boxpath, "BallotBox2");
            if (Directory.Exists(boxpath))
                Directory.Delete(boxpath, true);

            PersistentStore persistentStore = new(basedir, "TestBallotBox", "BallotBox1", "BallotBox2");
            persistentStore.Open();

            persistentStore["BallotBox1"].Put("Key1", "Test write bbox1");
            persistentStore["BallotBox2"].Put("Key1", "Test write bbox2");

            Assert.AreEqual("Test write bbox1", persistentStore["BallotBox1"].Get("Key1"));
            Assert.AreEqual("Test write bbox2", persistentStore["BallotBox2"].Get("Key1"));
            persistentStore.Close();
        }
    }
}