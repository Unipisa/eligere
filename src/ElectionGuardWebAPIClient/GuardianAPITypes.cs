using System;
using System.Numerics;

namespace ElectionGuard
{
    public class ElGamalKeyPair
    {
        public BigInteger secret_key { get; set; }
        public BigInteger public_key { get; set; }
    }

    public class ElectionPolynomial
    {
        public BigInteger[] coefficients { get; set; }
        public BigInteger[] coefficient_commitments { get; set; }
        public SchnorrProof[] coefficient_proofs { get; set; }
    }

    public class ElectionKeyPair
    {
        public ElGamalKeyPair key_pair { get; set; }
        public SchnorrProof proof { get; set; }
        public ElectionPolynomial polynomial { get; set; }
    }

    public class AuxiliaryKeyPair
    {
        public BigInteger secret_key { get; set; }
        public BigInteger public_key { get; set; }
    }

    public class GuardianRequest
    {
        public string id { get; set; }
        public int sequence_order { get; set; }
        public int number_of_guardians { get; set; }
        public int quorum { get; set; }
        public ElectionKeyPair election_key_pair { get; set; }
        public AuxiliaryKeyPair auxiliary_key_pair { get; set; }

    }

    public class Guardian
    {
        public string id { get; set; }
        public int sequence_order { get; set; }
        public int number_of_guardians { get; set; }
        public int quorum { get; set; }
        public ElectionKeyPair election_key_pair { get; set; }
        public AuxiliaryKeyPair auxiliary_key_pair { get; set; }
    }

    public class AuxiliaryPublicKey
    {
        public string owner_id { get; set; }
        public int sequence_order { get; set; }
        public string key { get; set; }
    }

    public class GuardianBackupRequest
    {
        public string guardian_id { get; set; }
        public int quorum { get; set; }
        public ElectionPolynomial election_polynomial { get; set; }
        public AuxiliaryPublicKey[] auxiliary_public_keys { get; set; }
        public bool override_rsa { get; set; }
    }

    public class ElectionPartialKeyBackup
    {
        public string owner_id { get; set; }
        public string designated_id { get; set; }
        public int designated_sequence_order { get; set; }
        public string encrypted_value { get; set; }
        public BigInteger[] coefficient_commitments { get; set; }
        public SchnorrProof[] coefficient_proofs { get; set; }
    }

    public class GuardianBackup
    {
        public string id { get; set; }
        public ElectionPartialKeyBackup[] election_partial_key_backups { get; set; }
    }

    public class ElectionPartialKeyVerification
    {
        public string owner_id { get; set; }
        public string designated_id { get; set; }
        public string verifier_id { get; set; }
        public bool verified { get; set; }
    }

    public class BackupVerificationRequest
    {
        public string verifier_id { get; set; }
        public ElectionPartialKeyBackup election_partial_key_backup { get; set; }
        public AuxiliaryKeyPair auxiliary_key_pair { get; set; }
        public bool override_rsa { get; set; }
    }

    public class BackupChallengeRequest
    {
        public ElectionPartialKeyBackup election_partial_key_backup { get; set; }
        public ElectionPolynomial election_polynomial { get; set; }

    }

    public class ElectionPartialKeyChallenge
    {
        public string owner_id { get; set; }
        public string designated_id { get; set; }
        public int designated_sequence_order { get; set; }
        public BigInteger value { get; set; }
        public BigInteger[] coefficient_commitments { get; set; }
        public SchnorrProof[] coefficient_proofs { get; set; }
    }

    public class ElectionKeyPairRequest
    {
        public string verifier_id { get; set; }
        public ElectionPartialKeyChallenge election_partial_key_challenge { get; set; }
    }

    public class DecryptBallotSharesRequest
    {
        public CiphertextAcceptedBallot[] encrypted_ballots { get; set; }
        public Guardian guardian { get; set; }
        public CiphertextElectionContext context { get; set; }
    }

    public class DecryptBallotSharesResponse
    {
        public BallotDecryptionShare[] shares { get; set; }
    }

    public class ChallengeVerificationRequest
    {
        public string verifier_id { get; set; }
        public ElectionPartialKeyChallenge election_partial_key_challenge { get; set; }
    }

    public class DecryptTallyShareRequest
    {
        public PublishedCiphertextTally encrypted_tally { get; set; }
        public Guardian guardian { get; set; }
        public ElectionDescription description { get; set; }
        public CiphertextElectionContext context { get; set; }
    }
}