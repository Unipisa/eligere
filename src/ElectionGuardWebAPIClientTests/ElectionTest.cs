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
            var ec = new ElectionContextRequest();
            ec.description = e;
            ec.elgamal_public_key = "123455";
            ec.number_of_guardians = 5;
            ec.quorum = 3;
            var pc = client.ElectionContext(ec);
            pc.Wait();
            Assert.AreEqual(new BigInteger(123455), pc.Result.elgamal_public_key);
        }

        [TestMethod]
        public void TestGuardianCreation()
        {
            var guardianApi = new GuardianClient("http://localhost:8001");
            var mediatorApi = new MediatorClient("http://localhost:8000");
            var edesc = JsonSerializer.Deserialize<ElectionDescription>(TestData.description);
            var guardians = new string[] { "A B", "C D", "E F", "G H", "I J" };
            var glist = new List<Guardian>();
            for (var i = 0; i < guardians.Length; i++)
            {
                var g = guardianApi.Guardian(new GuardianRequest() { id = guardians[i], sequence_order = i, number_of_guardians = 5, quorum = 3 });
                g.Wait();
                glist.Add(g.Result);
            }
            var pkeys = glist.ConvertAll(g => g.election_key_pair.public_key).ToArray();
            var elgpk = mediatorApi.ElectionCombine(new CombineElectionKeysRequest() { election_public_keys = pkeys });
            elgpk.Wait();
            var r = elgpk.Result;
            var ctxtr = mediatorApi.ElectionContext(new ElectionContextRequest() { description = edesc, elgamal_public_key = elgpk.Result.joint_key, number_of_guardians = 5, quorum = 3 });
            ctxtr.Wait();
            var ctxt = ctxtr.Result;

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
                var encr = mediatorApi.BallotEncrypt(new EncryptBallotsRequest() { ballots = s, context = ctxt, description = edesc, nonce = nonce, seed_hash = seed_hash });
                encr.Wait();
                var enc = encr.Result;
                encballots.AddRange(enc.encrypted_ballots);
                seed_hash = enc.next_seed_hash;
            }

            var spoiledballots = new List<CiphertextAcceptedBallot>();
            var castedballots = new List<CiphertextAcceptedBallot>();

            for (var i = 0; i < encballots.Count; i++)
            {
                if (i % 2 == 1)
                {
                    var spoilr = mediatorApi.BallotSpoil(new AcceptBallotRequest() { ballot = encballots[i], context = ctxt, description = edesc });
                    spoilr.Wait();
                    spoiledballots.Add(spoilr.Result);
                }
                else
                {
                    var castr = mediatorApi.BallotCast(new AcceptBallotRequest() { ballot = encballots[i], context = ctxt, description = edesc });
                    castr.Wait();
                    castedballots.Add(castr.Result);
                }
            }


            var starttallyr = mediatorApi.Tally(new StartTallyRequest() { ballots = castedballots.Take(1).ToArray(), context = ctxt, description = edesc });
            starttallyr.Wait();
            var current_tally = starttallyr.Result;
            var appendtallyr = mediatorApi.TallyAppend(new AppendTallyRequest() { ballots = castedballots.Skip(1).Take(1).ToArray(), context = ctxt, description = edesc, encrypted_tally = current_tally });
            appendtallyr.Wait();
            current_tally = appendtallyr.Result;

            var tallyShares = new Dictionary<string, TallyDecryptionShare>();
            foreach (var g in glist)
            {
                var decsr = guardianApi.TallyDecryptShare(new DecryptTallyShareRequest() { context = ctxt, description = edesc, encrypted_tally = current_tally, guardian = g });
                decsr.Wait();
                var share = decsr.Result;
                tallyShares.Add(g.id, share);
            }

            var dectr = mediatorApi.TallyDecrypt(new DecryptTallyRequest() { context = ctxt, description = edesc, encrypted_tally = current_tally, shares = tallyShares });
            dectr.Wait();
            var plainTally = dectr.Result;

            var ballotShares = new Dictionary<string, BallotDecryptionShare[]>();
            foreach (var g in glist)
            {
                var decbr = guardianApi.BallotDecryptShares(new DecryptBallotSharesRequest() { context = ctxt, encrypted_ballots = spoiledballots.ToArray(), guardian = g });
                decbr.Wait();
                var share = decbr.Result;
                ballotShares.Add(g.id, share.shares);
            }
            var decbar = mediatorApi.BallotDecrypt(new DecryptBallotsRequest() { context = ctxt, encrypted_ballots = spoiledballots.ToArray(), shares = ballotShares });
            decbar.Wait();
            var plainBallots = decbar.Result;
        }
    }
}
