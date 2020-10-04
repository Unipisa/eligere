using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ElectionGuard
{
   public class GuardianClient
    {
        private string baseUrl;
        private HttpClient client;

        public GuardianClient(string _baseUrl)
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

        public async Task<Guardian> Guardian(GuardianRequest ctxt)
        {
            return await PostRequest<Guardian, GuardianRequest>("/api/v1/guardian", ctxt);
        }

        public async Task<GuardianBackup> GuardianBackup(GuardianBackupRequest ctxt)
        {
            return await PostRequest<GuardianBackup, GuardianBackupRequest>("/api/v1/guardian/backup", ctxt);
        }

        public async Task<ElectionPartialKeyVerification> GuardianBackupVerify(BackupVerificationRequest ctxt)
        {
            return await PostRequest<ElectionPartialKeyVerification, BackupVerificationRequest>("/api/v1/guardian/backup/verify", ctxt);
        }

        public async Task<ElectionPartialKeyChallenge> GuardianChallenge(BackupChallengeRequest ctxt)
        {
            return await PostRequest<ElectionPartialKeyChallenge, BackupChallengeRequest>("/api/v1/guardian/challenge", ctxt);
        }

        public async Task<ElectionPartialKeyVerification> GuardianChallengeVerify(ChallengeVerificationRequest ctxt)
        {
            return await PostRequest<ElectionPartialKeyVerification, ChallengeVerificationRequest>("/api/v1/guardian/challenge/verify", ctxt);
        }

        public async Task<ElectionKeyPair> KeyElectionGenerate(ElectionKeyPairRequest ctxt)
        {
            return await PostRequest<ElectionKeyPair, ElectionKeyPairRequest>("/api/v1/key/election/generate", ctxt);
        }

        public async Task<AuxiliaryKeyPair> KeyAuxiliaryGenerate()
        {
            return await PostRequest<AuxiliaryKeyPair>("/api/v1/key/auxiliary/generate");
        }
        public async Task<DecryptBallotSharesResponse> BallotDecryptShares(DecryptBallotSharesRequest ctxt)
        {
            return await PostRequest<DecryptBallotSharesResponse, DecryptBallotSharesRequest>("/api/v1/ballot/decrypt-shares", ctxt);
        }

        public async Task<TallyDecryptionShare> TallyDecryptShare(DecryptTallyShareRequest ctxt)
        {
            return await PostRequest<TallyDecryptionShare, DecryptTallyShareRequest>("/api/v1/tally/decrypt-share", ctxt);
        }

        public async Task<string> Ping()
        {
            return await GetRequest<string>("/api/v1/ping");
        }
    }
}