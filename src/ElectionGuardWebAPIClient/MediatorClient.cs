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
        private async Task<RespT> PostRequest<RespT, ReqT>(string endpoint, ReqT ctxt)
        {
            return await RequestHelper.PostRequest<RespT, ReqT>(baseUrl, client, endpoint, ctxt);
        }

        private async Task<RespT> PostRequest<RespT>(string endpoint)
        {
            return await RequestHelper.PostRequest<RespT>(baseUrl, client, endpoint);
        }

        private async Task<RespT> GetRequest<RespT>(string endpoint)
        {
            return await RequestHelper.GetRequest<RespT>(baseUrl, client, endpoint);
        }

        public async Task<ElectionConstants> ElectionConstants()
        {
            return await GetRequest<ElectionConstants>("/api/v1/election/constants");
        }

        public async Task<CiphertextElectionContext> ElectionContext(ElectionContextRequest ctxt)
        {
            return await PostRequest<CiphertextElectionContext, ElectionContextRequest>("/api/v1/election/context", ctxt);
        }

        public async Task<ElectionJointKey> ElectionCombine(CombineElectionKeysRequest ctxt)
        {
            return await PostRequest<ElectionJointKey, CombineElectionKeysRequest>("/api/v1/key/election/combine", ctxt);
        }

        public async Task<CiphertextAcceptedBallot> BallotCast(AcceptBallotRequest ctxt)
        {
            return await PostRequest<CiphertextAcceptedBallot, AcceptBallotRequest>("/api/v1/ballot/cast", ctxt);
        }

        public async Task<CiphertextAcceptedBallot> BallotSpoil(AcceptBallotRequest ctxt)
        {
            return await PostRequest<CiphertextAcceptedBallot, AcceptBallotRequest>("/api/v1/ballot/spoil", ctxt);
        }

        public async Task<Dictionary<string, Dictionary<string, PlaintextTallyContest>>> BallotDecrypt(DecryptBallotsRequest ctxt)
        {
            return await PostRequest<Dictionary<string, Dictionary<string, PlaintextTallyContest>>, DecryptBallotsRequest>("/api/v1/ballot/decrypt", ctxt);
        }

        public async Task<PublishedCiphertextTally> Tally(StartTallyRequest ctxt)
        {
            return await PostRequest<PublishedCiphertextTally, StartTallyRequest>("/api/v1/tally", ctxt);
        }

        public async Task<PublishedCiphertextTally> TallyAppend(AppendTallyRequest ctxt)
        {
            return await PostRequest<PublishedCiphertextTally, AppendTallyRequest>("/api/v1/tally/append", ctxt);
        }

        public async Task<PublishedPlaintextTally> TallyDecrypt(DecryptTallyRequest ctxt)
        {
            return await PostRequest<PublishedPlaintextTally, DecryptTallyRequest>("/api/v1/tally/decrypt", ctxt);
        }

        public async Task<EncryptBallotsResponse> BallotEncrypt(EncryptBallotsRequest ctxt)
        {
            return await PostRequest<EncryptBallotsResponse, EncryptBallotsRequest>("/api/v1/ballot/encrypt", ctxt);
        }

        public async Task<TrackerHash> TrackerHash(TrackerHashRequest ctxt)
        {
            return await PostRequest<TrackerHash, TrackerHashRequest>("/api/v1/tracker/hash", ctxt);
        }

        public async Task<TrackerWords> TrackerWords(TrackerWordsRequest ctxt)
        {
            return await PostRequest<TrackerWords, TrackerWordsRequest>("/api/v1/tracker/words", ctxt);
        }

        public async Task<string> Ping()
        {
            return await GetRequest<string>("/api/v1/ping");
        }
    }
}