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

        public async Task<Guardian> GuardianAsync(AuxiliaryKeyPair auxiliary_key_pair, ElectionKeyPair election_key_pair, string id, int number_of_guardians, int quorum, int sequence_order)
        {
            var ctxt = new GuardianRequest()
            {
                auxiliary_key_pair = auxiliary_key_pair,
                election_key_pair = election_key_pair,
                id = id,
                number_of_guardians = number_of_guardians,
                quorum = quorum,
                sequence_order = sequence_order
            };
            return await PostRequestAsync<Guardian, GuardianRequest>("/api/v1/guardian", ctxt);
        }

        public Guardian Guardian(AuxiliaryKeyPair auxiliary_key_pair, ElectionKeyPair election_key_pair, string id, int number_of_guardians, int quorum, int sequence_order)
        {
            var ctxt = new GuardianRequest()
            {
                auxiliary_key_pair = auxiliary_key_pair,
                election_key_pair = election_key_pair,
                id = id,
                number_of_guardians = number_of_guardians,
                quorum = quorum,
                sequence_order = sequence_order
            };
            return PostRequest<Guardian, GuardianRequest>("/api/v1/guardian", ctxt);
        }

        public async Task<GuardianBackup> GuardianBackupAsync(AuxiliaryPublicKey[] auxiliary_public_keys, ElectionPolynomial election_polynomial, string guardian_id, bool override_rsa, int quorum)
        {
            var ctxt = new GuardianBackupRequest()
            {
                auxiliary_public_keys = auxiliary_public_keys,
                election_polynomial = election_polynomial,
                guardian_id = guardian_id,
                override_rsa = override_rsa,
                quorum = quorum
            };
            return await PostRequestAsync<GuardianBackup, GuardianBackupRequest>("/api/v1/guardian/backup", ctxt);
        }

        public GuardianBackup GuardianBackup(AuxiliaryPublicKey[] auxiliary_public_keys, ElectionPolynomial election_polynomial, string guardian_id, bool override_rsa, int quorum)
        {
            var ctxt = new GuardianBackupRequest()
            {
                auxiliary_public_keys = auxiliary_public_keys,
                election_polynomial = election_polynomial,
                guardian_id = guardian_id,
                override_rsa = override_rsa,
                quorum = quorum
            };
            return PostRequest<GuardianBackup, GuardianBackupRequest>("/api/v1/guardian/backup", ctxt);
        }

        public async Task<ElectionPartialKeyVerification> GuardianBackupVerifyAsync(AuxiliaryKeyPair auxiliary_key_pair, ElectionPartialKeyBackup election_partial_key_backup, bool override_rsa, string verifier_id)
        {
            var ctxt = new BackupVerificationRequest()
            {
                auxiliary_key_pair = auxiliary_key_pair,
                election_partial_key_backup = election_partial_key_backup,
                override_rsa = override_rsa,
                verifier_id = verifier_id
            };
            return await PostRequestAsync<ElectionPartialKeyVerification, BackupVerificationRequest>("/api/v1/guardian/backup/verify", ctxt);
        }

        public ElectionPartialKeyVerification GuardianBackupVerify(AuxiliaryKeyPair auxiliary_key_pair, ElectionPartialKeyBackup election_partial_key_backup, bool override_rsa, string verifier_id)
        {
            var ctxt = new BackupVerificationRequest()
            {
                auxiliary_key_pair = auxiliary_key_pair,
                election_partial_key_backup = election_partial_key_backup,
                override_rsa = override_rsa,
                verifier_id = verifier_id
            };
            return PostRequest<ElectionPartialKeyVerification, BackupVerificationRequest>("/api/v1/guardian/backup/verify", ctxt);
        }

        public async Task<ElectionPartialKeyChallenge> GuardianChallengeAsync(ElectionPartialKeyBackup election_partial_key_backup, ElectionPolynomial election_polynomial)
        {
            var ctxt = new BackupChallengeRequest()
            {
                election_partial_key_backup = election_partial_key_backup,
                election_polynomial = election_polynomial
            };
            return await PostRequestAsync<ElectionPartialKeyChallenge, BackupChallengeRequest>("/api/v1/guardian/challenge", ctxt);
        }

        public ElectionPartialKeyChallenge GuardianChallenge(ElectionPartialKeyBackup election_partial_key_backup, ElectionPolynomial election_polynomial)
        {
            var ctxt = new BackupChallengeRequest()
            {
                election_partial_key_backup = election_partial_key_backup,
                election_polynomial = election_polynomial
            };
            return PostRequest<ElectionPartialKeyChallenge, BackupChallengeRequest>("/api/v1/guardian/challenge", ctxt);
        }

        public async Task<ElectionPartialKeyVerification> GuardianChallengeVerifyAsync(ElectionPartialKeyChallenge election_partial_key_challenge, string verifier_id)
        {
            var ctxt = new ChallengeVerificationRequest()
            {
                election_partial_key_challenge = election_partial_key_challenge,
                verifier_id = verifier_id
            };
            return await PostRequestAsync<ElectionPartialKeyVerification, ChallengeVerificationRequest>("/api/v1/guardian/challenge/verify", ctxt);
        }

        public ElectionPartialKeyVerification GuardianChallengeVerify(ElectionPartialKeyChallenge election_partial_key_challenge, string verifier_id)
        {
            var ctxt = new ChallengeVerificationRequest()
            {
                election_partial_key_challenge = election_partial_key_challenge,
                verifier_id = verifier_id
            };
            return PostRequest<ElectionPartialKeyVerification, ChallengeVerificationRequest>("/api/v1/guardian/challenge/verify", ctxt);
        }

        public async Task<ElectionKeyPair> KeyElectionGenerateAsync(ElectionPartialKeyChallenge election_partial_key_challenge, string verifier_id)
        {
            var ctxt = new ElectionKeyPairRequest()
            {
                election_partial_key_challenge = election_partial_key_challenge,
                verifier_id = verifier_id
            };
            return await PostRequestAsync<ElectionKeyPair, ElectionKeyPairRequest>("/api/v1/key/election/generate", ctxt);
        }

        public ElectionKeyPair KeyElectionGenerate(ElectionPartialKeyChallenge election_partial_key_challenge, string verifier_id)
        {
            var ctxt = new ElectionKeyPairRequest()
            {
                election_partial_key_challenge = election_partial_key_challenge,
                verifier_id = verifier_id
            };
            return PostRequest<ElectionKeyPair, ElectionKeyPairRequest>("/api/v1/key/election/generate", ctxt);
        }

        public async Task<AuxiliaryKeyPair> KeyAuxiliaryGenerateAsync()
        {
            return await PostRequestAsync<AuxiliaryKeyPair>("/api/v1/key/auxiliary/generate");
        }

        public AuxiliaryKeyPair KeyAuxiliaryGenerate()
        {
            return PostRequest<AuxiliaryKeyPair>("/api/v1/key/auxiliary/generate");
        }

        public async Task<DecryptBallotSharesResponse> BallotDecryptSharesAsync(CiphertextElectionContext context, CiphertextAcceptedBallot[] encrypted_ballots, Guardian guardian)
        {
            var ctxt = new DecryptBallotSharesRequest()
            {
                context = context,
                encrypted_ballots = encrypted_ballots,
                guardian = guardian
            };
            return await PostRequestAsync<DecryptBallotSharesResponse, DecryptBallotSharesRequest>("/api/v1/ballot/decrypt-shares", ctxt);
        }

        public DecryptBallotSharesResponse BallotDecryptShares(CiphertextElectionContext context, CiphertextAcceptedBallot[] encrypted_ballots, Guardian guardian)
        {
            var ctxt = new DecryptBallotSharesRequest()
            {
                context = context,
                encrypted_ballots = encrypted_ballots,
                guardian = guardian
            };
            return PostRequest<DecryptBallotSharesResponse, DecryptBallotSharesRequest>("/api/v1/ballot/decrypt-shares", ctxt);
        }

        public async Task<TallyDecryptionShare> TallyDecryptShareAsync(CiphertextElectionContext context, ElectionDescription description, PublishedCiphertextTally encrypted_tally, Guardian guardian)
        {
            var ctxt = new DecryptTallyShareRequest()
            {
                context = context,
                description = description,
                encrypted_tally = encrypted_tally,
                guardian = guardian
            };
            return await PostRequestAsync<TallyDecryptionShare, DecryptTallyShareRequest>("/api/v1/tally/decrypt-share", ctxt);
        }

        public TallyDecryptionShare TallyDecryptShare(CiphertextElectionContext context, ElectionDescription description, PublishedCiphertextTally encrypted_tally, Guardian guardian)
        {
            var ctxt = new DecryptTallyShareRequest()
            {
                context = context,
                description = description,
                encrypted_tally = encrypted_tally,
                guardian = guardian
            };
            return PostRequest<TallyDecryptionShare, DecryptTallyShareRequest>("/api/v1/tally/decrypt-share", ctxt);
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