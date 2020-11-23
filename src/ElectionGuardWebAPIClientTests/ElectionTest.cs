using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElectionGuard;
using System.Text.Json;
using System.Numerics;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ElectionGuardWebAPIClientTests
{
    [TestClass]
    public class ElectionTest
    {
        [TestMethod]
        public void ElectionContextTest()
        {
            var client = new MediatorClient("http://localhost:8000");
            var e = JsonSerializer.Deserialize<ElectionDescription>(TestData.description);
            Assert.AreEqual("123455", client.ElectionContext(e, "123455", 5, 3).elgamal_public_key);
        }

        [TestMethod]
        public void TestGuardianCreation()
        {
            var guardianApi = new GuardianClient("http://localhost:8001");
            var mediatorApi = new MediatorClient("http://localhost:8000");
            var edesc = JsonSerializer.Deserialize<ElectionDescription>(TestData.description2);
            var guardians = new string[] { "A B", "C D", "E F", "G H", "I J" };
            var glist = new List<Guardian>();
            for (var i = 0; i < guardians.Length; i++)
            {
                glist.Add(guardianApi.Guardian(null, null, guardians[i], 5, 3, i));
            }
            var pkeys = glist.ConvertAll(g => g.election_key_pair.public_key).ToArray();
            var r = mediatorApi.ElectionCombine(pkeys);
            var ctxt = mediatorApi.ElectionContext(edesc, r.joint_key, 5, 3);

            var ballots = new PlaintextBallot[4];
            for (int i = 0; i< ballots.Length; i++)
            {
                ballots[i] = JsonSerializer.Deserialize<PlaintextBallot>(TestData.ballotData);
                ballots[i].object_id = $"ballot-{i}";
            }

            var nonce = "110191403412906482859082647039385908787148325839889522238592336039604240167009";
            var seed_hash = "110191403412906482859082647039385908787148325839889522238592336039604240167009";

            var segments = new[] { ballots[0..2], ballots[2..4]  };

            var encballots = new List<CiphertextBallot>();

            foreach (var s in segments)
            {
                var enc = mediatorApi.BallotEncrypt(s, ctxt, edesc, nonce, seed_hash);
                encballots.AddRange(enc.encrypted_ballots);
                seed_hash = enc.next_seed_hash;
            }

            var spoiledballots = new List<CiphertextAcceptedBallot>();
            var castedballots = new List<CiphertextAcceptedBallot>();

            for (var i = 0; i < encballots.Count; i++)
            {
                if (i % 2 == 1)
                {
                    spoiledballots.Add(mediatorApi.BallotSpoil(encballots[i], ctxt, edesc));
                }
                else
                {
                    castedballots.Add(mediatorApi.BallotCast(encballots[i], ctxt, edesc));
                }
            }


            var current_tally = mediatorApi.Tally(castedballots.Take(1).ToArray(), ctxt, edesc);
            current_tally = mediatorApi.TallyAppend(castedballots.Skip(1).Take(1).ToArray(), ctxt, edesc, current_tally);

            var tallyShares = new Dictionary<string, TallyDecryptionShare>();
            foreach (var g in glist)
            {
                tallyShares.Add(g.id, guardianApi.TallyDecryptShare(ctxt, edesc, current_tally, g));
            }

            var plainTally = mediatorApi.TallyDecrypt(ctxt, edesc, current_tally, tallyShares);

            var ballotShares = new Dictionary<string, BallotDecryptionShare[]>();
            foreach (var g in glist)
            {
                var share = guardianApi.BallotDecryptShares(ctxt, spoiledballots.ToArray(), g);
                ballotShares.Add(g.id, share.shares);
            }
            var plainBallots = mediatorApi.BallotDecrypt(ctxt, spoiledballots.ToArray(), ballotShares);
        }
    }
}
