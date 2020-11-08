using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ElectionGuard
{
    public class MediatorClient
    {
        private string baseUrl;
        private HttpClient client;

        public MediatorClient(string _baseUrl)
        {
            baseUrl = _baseUrl;
            client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));
        }

        private async Task<RespT> PostRequestAsync<RespT, ReqT>(string endpoint, ReqT ctxt)
        {
            return await RequestHelper.PostRequest<RespT, ReqT>(baseUrl, client, endpoint, ctxt);
        }

        private RespT PostRequest<RespT, ReqT>(string endpoint, ReqT ctxt)
        {
            var req = RequestHelper.PostRequest<RespT, ReqT>(baseUrl, client, endpoint, ctxt);
            req.Wait();
            return req.Result; 
        }

        private async Task<RespT> PostRequestAsync<RespT>(string endpoint)
        {
            return await RequestHelper.PostRequest<RespT>(baseUrl, client, endpoint);
        }

        private RespT PostRequest<RespT>(string endpoint)
        {
            var req = RequestHelper.PostRequest<RespT>(baseUrl, client, endpoint);
            req.Wait();
            return req.Result;
        }

        private async Task<RespT> GetRequestAsync<RespT>(string endpoint)
        {
            return await RequestHelper.GetRequest<RespT>(baseUrl, client, endpoint);
        }

        private RespT GetRequest<RespT>(string endpoint)
        {
            var req = RequestHelper.GetRequest<RespT>(baseUrl, client, endpoint);
            req.Wait();
            return req.Result;
        }

        public async Task<ElectionConstants> ElectionConstantsAsync()
        {
            return await GetRequestAsync<ElectionConstants>("/api/v1/election/constants");
        }

        public ElectionConstants ElectionConstants()
        {
            return GetRequest<ElectionConstants>("/api/v1/election/constants");
        }

        public async Task<CiphertextElectionContext> ElectionContextAsync(ElectionDescription description, string elgamal_public_key, int number_of_guardians, int quorum)
        {
            var ctxt = new ElectionContextRequest() 
            { 
                description = description, 
                elgamal_public_key = elgamal_public_key,
                number_of_guardians = number_of_guardians,
                quorum = quorum 
            };
            return await PostRequestAsync<CiphertextElectionContext, ElectionContextRequest>("/api/v1/election/context", ctxt);
        }

        public CiphertextElectionContext ElectionContext(ElectionDescription description, string elgamal_public_key, int number_of_guardians, int quorum)
        {
            var ctxt = new ElectionContextRequest()
            {
                description = description,
                elgamal_public_key = elgamal_public_key,
                number_of_guardians = number_of_guardians,
                quorum = quorum
            };
            return PostRequest<CiphertextElectionContext, ElectionContextRequest>("/api/v1/election/context", ctxt);
        }

        public async Task<ElectionJointKey> ElectionCombineAsync(string[] election_public_keys)
        {
            var ctxt = new CombineElectionKeysRequest()
            {
                election_public_keys = election_public_keys
            };
            return await PostRequestAsync<ElectionJointKey, CombineElectionKeysRequest>("/api/v1/key/election/combine", ctxt);
        }

        public ElectionJointKey ElectionCombine(string[] election_public_keys)
        {
            var ctxt = new CombineElectionKeysRequest()
            {
                election_public_keys = election_public_keys
            };
            return PostRequest<ElectionJointKey, CombineElectionKeysRequest>("/api/v1/key/election/combine", ctxt);
        }

        public async Task<CiphertextAcceptedBallot> BallotCastAsync(CiphertextBallot ballot, CiphertextElectionContext context, ElectionDescription description)
        {
            var ctxt = new AcceptBallotRequest()
            {
                ballot = ballot,
                context = context,
                description = description
            };
            return await PostRequestAsync<CiphertextAcceptedBallot, AcceptBallotRequest>("/api/v1/ballot/cast", ctxt);
        }

        public CiphertextAcceptedBallot BallotCast(CiphertextBallot ballot, CiphertextElectionContext context, ElectionDescription description)
        {
            var ctxt = new AcceptBallotRequest()
            {
                ballot = ballot,
                context = context,
                description = description
            };
            return PostRequest<CiphertextAcceptedBallot, AcceptBallotRequest>("/api/v1/ballot/cast", ctxt);
        }

        public async Task<CiphertextAcceptedBallot> BallotSpoilAsync(CiphertextBallot ballot, CiphertextElectionContext context, ElectionDescription description)
        {
            var ctxt = new AcceptBallotRequest()
            {
                ballot = ballot,
                context = context,
                description = description
            };
            return await PostRequestAsync<CiphertextAcceptedBallot, AcceptBallotRequest>("/api/v1/ballot/spoil", ctxt);
        }

        public CiphertextAcceptedBallot BallotSpoil(CiphertextBallot ballot, CiphertextElectionContext context, ElectionDescription description)
        {
            var ctxt = new AcceptBallotRequest()
            {
                ballot = ballot,
                context = context,
                description = description
            };
            return PostRequest<CiphertextAcceptedBallot, AcceptBallotRequest>("/api/v1/ballot/spoil", ctxt);
        }

        public async Task<Dictionary<string, Dictionary<string, PlaintextTallyContest>>> BallotDecryptAsync(CiphertextElectionContext context, CiphertextAcceptedBallot[] encrypted_ballots, Dictionary<string, BallotDecryptionShare[]> shares)
        {
            var ctxt = new DecryptBallotsRequest()
            {
                context = context,
                encrypted_ballots = encrypted_ballots,
                shares = shares
            };
            return await PostRequestAsync<Dictionary<string, Dictionary<string, PlaintextTallyContest>>, DecryptBallotsRequest>("/api/v1/ballot/decrypt", ctxt);
        }

        public Dictionary<string, Dictionary<string, PlaintextTallyContest>> BallotDecrypt(CiphertextElectionContext context, CiphertextAcceptedBallot[] encrypted_ballots, Dictionary<string, BallotDecryptionShare[]> shares)
        {
            var ctxt = new DecryptBallotsRequest()
            {
                context = context,
                encrypted_ballots = encrypted_ballots,
                shares = shares
            };
            return PostRequest<Dictionary<string, Dictionary<string, PlaintextTallyContest>>, DecryptBallotsRequest>("/api/v1/ballot/decrypt", ctxt);
        }

        public async Task<PublishedCiphertextTally> TallyAsync(CiphertextAcceptedBallot[] ballots, CiphertextElectionContext context, ElectionDescription description)
        {
            var ctxt = new StartTallyRequest()
            {
                ballots = ballots,
                context = context,
                description = description
            };
            return await PostRequestAsync<PublishedCiphertextTally, StartTallyRequest>("/api/v1/tally", ctxt);
        }

        public PublishedCiphertextTally Tally(CiphertextAcceptedBallot[] ballots, CiphertextElectionContext context, ElectionDescription description)
        {
            var ctxt = new StartTallyRequest()
            {
                ballots = ballots,
                context = context,
                description = description
            };
            return PostRequest<PublishedCiphertextTally, StartTallyRequest>("/api/v1/tally", ctxt);
        }

        public async Task<PublishedCiphertextTally> TallyAppendAsync(CiphertextAcceptedBallot[] ballots, CiphertextElectionContext context, ElectionDescription description, PublishedCiphertextTally encrypted_tally)
        {
            var ctxt = new AppendTallyRequest()
            {
                ballots = ballots,
                context = context,
                description = description,
                encrypted_tally = encrypted_tally
            };
            return await PostRequestAsync<PublishedCiphertextTally, AppendTallyRequest>("/api/v1/tally/append", ctxt);
        }

        public PublishedCiphertextTally TallyAppend(CiphertextAcceptedBallot[] ballots, CiphertextElectionContext context, ElectionDescription description, PublishedCiphertextTally encrypted_tally)
        {
            var ctxt = new AppendTallyRequest()
            {
                ballots = ballots,
                context = context,
                description = description,
                encrypted_tally = encrypted_tally
            };
            return PostRequest<PublishedCiphertextTally, AppendTallyRequest>("/api/v1/tally/append", ctxt);
        }

        public async Task<PublishedPlaintextTally> TallyDecryptAsync(CiphertextElectionContext context, ElectionDescription description, PublishedCiphertextTally encrypted_tally, Dictionary<string, TallyDecryptionShare> shares)
        {
            var ctxt = new DecryptTallyRequest()
            {
                context = context,
                description = description,
                encrypted_tally = encrypted_tally,
                shares = shares
            };
            return await PostRequestAsync<PublishedPlaintextTally, DecryptTallyRequest>("/api/v1/tally/decrypt", ctxt);
        }

        public PublishedPlaintextTally TallyDecrypt(CiphertextElectionContext context, ElectionDescription description, PublishedCiphertextTally encrypted_tally, Dictionary<string, TallyDecryptionShare> shares)
        {
            var ctxt = new DecryptTallyRequest()
            {
                context = context,
                description = description,
                encrypted_tally = encrypted_tally,
                shares = shares
            };
            return PostRequest<PublishedPlaintextTally, DecryptTallyRequest>("/api/v1/tally/decrypt", ctxt);
        }

        public async Task<EncryptBallotsResponse> BallotEncryptAsync(PlaintextBallot[] ballots, CiphertextElectionContext context, ElectionDescription description, string nonce, string seed_hash)
        {
            var ctxt = new EncryptBallotsRequest()
            {
                ballots = ballots,
                context = context,
                description = description,
                nonce = nonce,
                seed_hash = seed_hash
            };
            return await PostRequestAsync<EncryptBallotsResponse, EncryptBallotsRequest>("/api/v1/ballot/encrypt", ctxt);
        }

        public EncryptBallotsResponse BallotEncrypt(PlaintextBallot[] ballots, CiphertextElectionContext context, ElectionDescription description, string nonce, string seed_hash)
        {
            var ctxt = new EncryptBallotsRequest()
            {
                ballots = ballots,
                context = context,
                description = description,
                nonce = nonce,
                seed_hash = seed_hash
            };
            return PostRequest<EncryptBallotsResponse, EncryptBallotsRequest>("/api/v1/ballot/encrypt", ctxt);
        }

        public async Task<TrackerHash> TrackerHashAsync(string separator, string tracker_words)
        {
            var ctxt = new TrackerHashRequest()
            {
                seperator = separator,
                tracker_words = tracker_words
            };
            return await PostRequestAsync<TrackerHash, TrackerHashRequest>("/api/v1/tracker/hash", ctxt);
        }

        public TrackerHash TrackerHash(string separator, string tracker_words)
        {
            var ctxt = new TrackerHashRequest()
            {
                seperator = separator,
                tracker_words = tracker_words
            };
            return PostRequest<TrackerHash, TrackerHashRequest>("/api/v1/tracker/hash", ctxt);
        }

        public async Task<TrackerWords> TrackerWordsAsync(string separator, string tracker_hash)
        {
            var ctxt = new TrackerWordsRequest()
            {
                seperator = separator,
                tracker_hash = tracker_hash
            };
            return await PostRequestAsync<TrackerWords, TrackerWordsRequest>("/api/v1/tracker/words", ctxt);
        }

        public TrackerWords TrackerWords(string separator, string tracker_hash)
        {
            var ctxt = new TrackerWordsRequest()
            {
                seperator = separator,
                tracker_hash = tracker_hash
            };
            return PostRequest<TrackerWords, TrackerWordsRequest>("/api/v1/tracker/words", ctxt);
        }

        public async Task<string> PingAsync()
        {
            return await GetRequestAsync<string>("/api/v1/ping");
        }

        public string Ping()
        {
            return GetRequest<string>("/api/v1/ping");
        }
    }
}