using System;
using System.Text.Json.Serialization;
using System.Numerics;
using System.Collections.Generic;

namespace ElectionGuard
{
    public class ElectionConstants
    {
        public BigInteger cofactor { get; set; }
        public BigInteger generator { get; set; }
        public BigInteger large_prime { get; set; }
        public BigInteger small_prime { get; set; }
    }

    public class ElectionContextRequest
    {
        public ElectionDescription description { get; set; }
        public string elgamal_public_key { get; set; }
        public int number_of_guardians { get; set; }
        public int quorum { get; set; }
    }

    public class LocalizedText
    {
        public string value { get; set; }
        public string language { get; set; }
    }

    public class ListOfLocalizedText
    {
        public LocalizedText[] text { get; set; }
    }

    public class PartyDescription
    {
        public string object_id { get; set; }
        public string abbreviation { get; set; }
        public string color { get; set; }
        public string logo_uri { get; set; }
        public ListOfLocalizedText name { get; set; }

    }

    public class AnnotatedText
    {
        public string annotation { get; set; }
        public string value { get; set; }
    }

    public class ContactInformation
    {
        public string[] address_line { get; set; }
        public string name { get; set; }
        public AnnotatedText[] email { get; set; }
        public AnnotatedText[] phone { get; set; }
    }

    public class BallotStyle
    {
        public string object_id { get; set; }
        public string[] geopolitical_unit_ids { get; set; }
    }

    public class BallotName
    {
        public LocalizedText[] text { get; set; }
        public string party_id { get; set; }
    }

    public class Candidate
    {
        public string object_id { get; set; }
        public BallotName ballot_name { get; set; }
    }

    public class GeoPoliticalUnit
    {
        public string object_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public ContactInformation contact_information { get; set; }
    }

    public class BallotSelection
    {
        public string object_id { get; set; }
        public int sequence_order { get; set; }
        public string candidate_id { get; set; }
    }

    public class Contest
    {
        [JsonPropertyName("@type")]
        public string type { get; set; }
        public string object_id { get; set; }
        public int sequence_order { get; set; }
        public BallotSelection[] ballot_selections { get; set; }
        public ListOfLocalizedText ballot_title { get; set; }
        public ListOfLocalizedText ballot_subtitle { get; set; }
        public string vote_variation { get; set; }
        public string electoral_district_id { get; set; }
        public string name { get; set; }
        public string[] primary_party_ids { get; set; }
        public int number_elected { get; set; }
        public int votes_allowed { get; set; }
    }

    public class ElectionDescription
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string type { get; set; } // Check
        public string election_scope_id { get; set; } // Check
        public PartyDescription[] parties { get; set; }
        public ContactInformation contact_information { get; set; }
        public ListOfLocalizedText name { get; set; }
        public Candidate[] candidates { get; set; }
        public GeoPoliticalUnit[] geopolitical_units { get; set; }
        public Contest[] contests { get; set; }
        public BallotStyle[] ballot_styles { get; set; }
    }

    public class CiphertextElectionContext
    {
        public int number_of_guardians { get; set; }
        public int quorum { get; set; }
        public BigInteger elgamal_public_key { get; set; }
        public BigInteger description_hash { get; set; }
        public BigInteger crypto_base_hash { get; set; }
        public BigInteger crypto_extended_base_hash { get; set; }
    }

    public class SchnorrProof
    {
        public BigInteger public_key { get; set; }
        public BigInteger commitment { get; set; }
        public BigInteger challenge { get; set; }
        public BigInteger response { get; set; }
        public string usage { get; set; } // Check
    }

    public class ElectionPublicKey
    {
        public string owner_id { get; set; }
        public SchnorrProof proof { get; set; }
        public BigInteger key { get; set; }

    }

    public class ElectionJointKey
    {
        public string joint_key { get; set; }
    }

    public class CombineElectionKeysRequest
    {
        public ElectionPublicKey[] election_public_keys { get; set; }
    }

    public class CiphertextBallotSelection
    {
        public BigInteger description_hash { get; set; }
        public ElGamalCiphertext ciphertext { get; set; }
        public BigInteger crypto_hash { get; set; }
        public bool is_placeholder_selection { get; set; }
        public BigInteger? nonce { get; set; }
        public DisjunctiveChaumPedersenProof proof { get; set; }
        public ElGamalCiphertext extended_data { get; set; }
    }

    public class ConstantChaumPedersenProof
    {
        public BigInteger pad { get; set; }
        public BigInteger data { get; set; }
        public BigInteger challenge { get; set; }
        public BigInteger response { get; set; }
        public int constant { get; set; }
        public string usage { get; set; }
    }

    public class DisjunctiveChaumPedersenProof
    {
        public BigInteger proof_zero_pad { get; set; }
        public BigInteger proof_zero_data { get; set; }
        public BigInteger proof_one_pad { get; set; }
        public BigInteger proof_one_data { get; set; }
        public BigInteger proof_zero_challenge { get; set; }
        public BigInteger proof_one_challenge { get; set; }
        public BigInteger challenge { get; set; }
        public BigInteger proof_zero_response { get; set; }
        public BigInteger proof_one_response { get; set; }
        public string usage { get; set; }
    }

    public class CiphertextBallotContest
    {
        public BigInteger description_hash { get; set; }
        public CiphertextBallotSelection[] ballot_selections { get; set; }
        public BigInteger crypto_hash { get; set; }
        public BigInteger? nonce { get; set; }
        public ConstantChaumPedersenProof? proof { get; set; }
    }

    public class CiphertextBallot
    {
        public string ballot_style { get; set; }
        public BigInteger description_hash { get; set; }
        public BigInteger previous_tracking_hash { get; set; }
        public CiphertextBallotContest[] contests { get; set; }
        public BigInteger? tracking_hash { get; set; }
        public int timestamp { get; set; }
        public BigInteger crypto_hash { get; set; }
        public BigInteger? nonce { get; set; }
    }

    public class AcceptBallotRequest
    {
        public ElectionDescription description { get; set; }
        public CiphertextElectionContext context { get; set; }
        public CiphertextBallot ballot { get; set; }
    }

    public enum BallotBoxState
    {
        CAST = 1,
        SPOILED = 2,
        UNKNOWN = 999
    }

    public class CiphertextAcceptedBallot
    {
        public BallotBoxState state { get; set; }
    }

    public class ChaumPedersenProof
    {
        public BigInteger pad { get; set; }
        public BigInteger data { get; set; }
        public BigInteger challenge { get; set; }
        public BigInteger response { get; set; }
        public string usage { get; set; }
    }

    public class CiphertextCompensatedDecryptionSelection
    {
        public string guardian_id { get; set; }
        public string missing_guardian_id { get; set; }
        public BigInteger description_hash { get; set; }
        public BigInteger share { get; set; }
        public BigInteger recovery_key { get; set; }
        public ChaumPedersenProof proof { get; set; }
    }

    public class CiphertextDecryptionSelection
    {
        public string guardian_id { get; set; }
        public BigInteger description_hash { get; set; }
        public BigInteger share { get; set; }
        public ChaumPedersenProof proof { get; set; }
        public Dictionary<string, CiphertextCompensatedDecryptionSelection> recovered_parts { get; set; }
    }

    public class CiphertextDecryptionContest
    {
        public string guardian_id { get; set; }
        public BigInteger description_hash { get; set; }
        public Dictionary<string, CiphertextDecryptionSelection> selections { get; set; }
    }

    public class BallotDecryptionShare
    {
        public string guardian_id { get; set; }
        public BigInteger public_key { get; set; }
        public string ballot_id { get; set; }
        public Dictionary<string, CiphertextDecryptionContest> contests { get; set; }
    }

    public class DecryptBallotsRequest
    {
        public CiphertextAcceptedBallot[] encrypted_ballots { get; set; }
        public System.Collections.Generic.Dictionary<string, BallotDecryptionShare[]> shares { get; set; }
        public CiphertextElectionContext context { get; set; }
    }

    public class ElGamalCiphertext
    {
        public BigInteger pad { get; set; }
        public BigInteger data { get; set; }
    }

    public class PlaintextTallySelection
    {
        public int tally { get; set; }
        public BigInteger value { get; set; }
        public ElGamalCiphertext message { get; set; }
        public CiphertextDecryptionSelection[] shares { get; set; }
    }

    public class PlaintextTallyContest
    {
        public Dictionary<string, PlaintextTallySelection> selections { get; set; }
    }

    public class StartTallyRequest
    {
        public CiphertextAcceptedBallot[] ballots { get; set; }
        public ElectionDescription description { get; set; }
        public CiphertextElectionContext context { get; set; }
    }

    public class CiphertextTallySelection
    {
        public BigInteger description_hash { get; set; }
        public ElGamalCiphertext ciphertext { get; set; }
    }

    public class CiphertextTallyContest
    {
        public BigInteger description_hash { get; set; }
        public Dictionary<string, CiphertextTallySelection> tally_selections { get; set; }
    }

    public class PublishedCiphertextTally
    {
        public Dictionary<string, CiphertextTallyContest> cast { get; set; }
    }

    public class AppendTallyRequest
    {
        public PublishedCiphertextTally encrypted_tally { get; set; }
    }

    public class TallyDecryptionShare
    {
        public string guardian_id { get; set; }
        public BigInteger public_key { get; set; }
        public Dictionary<string, CiphertextDecryptionContest> contests { get; set; }
        public Dictionary<string, BallotDecryptionShare> spoiled_ballots { get; set; }
    }

    public class DecryptTallyRequest
    {
        public PublishedCiphertextTally encrypted_tally { get; set; }
        public Dictionary<string, TallyDecryptionShare> shares { get; set; }
        public ElectionDescription description { get; set; }
        public CiphertextElectionContext context { get; set; }
    }

    public class PublishedPlaintextTally
    {
        public Dictionary<string, PlaintextTallyContest> contests { get; set; }
    }

    public class ExtendedData
    {
        public string value { get; set; }
        public int length { get; set; }
    }

    public class PlaintextBallot
    {
        public string vote { get; set; }
        public bool is_placeholder_selection { get; set; }
        public ExtendedData extended_data { get; set; }
    }

    public class EncryptBallotsRequest
    {
        public PlaintextBallot[] ballots { get; set; }
        public string seed_hash { get; set; }
        public string nonce { get; set; }
        public ElectionDescription description { get; set; }
        public CiphertextElectionContext context { get; set; }
    }
    public class EncryptBallotsResponse
    {
        public PlaintextBallot[] encrypted_ballots { get; set; }
        public string next_seed_hash { get; set; }
    }

    public class TrackerHashRequest
    {
        public string tracker_words { get; set; }
        public string seperator { get; set; }
    }

    public class TrackerHash
    {
        public string tracker_hash { get; set; }
    }

    public class TrackerWordsRequest
    {
        public string tracker_hash { get; set; }
        public string seperator { get; set; }
    }

    public class TrackerWords
    {
        public string tracker_words { get; set; }
    }
 }