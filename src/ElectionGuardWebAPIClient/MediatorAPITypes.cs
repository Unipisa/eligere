using System;
using System.Text.Json.Serialization;
using System.Numerics;
using System.Collections.Generic;
using System.Resources;

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

    public enum VoteVariationType
    {
        unknown = 0,
        one_of_m = 1,
        approval = 2,
        borda = 3,
        cumulative = 4,
        majority = 5,
        n_of_m = 6,
        plurality = 7,
        proportional = 8,
        range = 9,
        rcv = 10,
        super_majority = 11,
        other = 12
    }

    public class SelectionDescription
    {
        public string object_id { get; set; }
        public string candidate_id { get; set; }
        public int sequence_order { get; set; }
    }

    public class Language
    {
        public string value { get; set; }
        public string language { get; set; }
    }

    public class InternationalizedText
    {
        public Language[] text { get; set; }
    }

    public class ContestDescriptionWithPlaceholders
    {
        public string object_id { get; set; }
        public string electoral_distric_id { get; set; }
        public int sequence_order { get; set; }
        public VoteVariationType vote_variation { get; set; }
        public int number_elected { get; set; }
        public int? votes_allowed { get; set; }
        public string name { get; set; }
        public SelectionDescription[] ballot_selections { get; set; }
        public InternationalizedText ballot_title { get; set; }
        public InternationalizedText ballot_subtitle { get; set; }
    }

    public class InternalElectionDescription
    {
        public ElectionDescription description { get; set; }
        public GeoPoliticalUnit[] geopolitical_units { get; set; }
        public ContestDescriptionWithPlaceholders[] contests { get; set; }
        public BallotStyle[] ballot_styles { get; set; }
        public string description_hash { get; set; }
    }

    public class CiphertextElectionContext
    {
        public int number_of_guardians { get; set; }
        public int quorum { get; set; }
        public string elgamal_public_key { get; set; }
        public string description_hash { get; set; }
        public string crypto_base_hash { get; set; }
        public string crypto_extended_base_hash { get; set; }
    }

    public class SchnorrProof
    {
        public string public_key { get; set; }
        public string commitment { get; set; }
        public string challenge { get; set; }
        public string response { get; set; }
        public string usage { get; set; } // Check
    }

    public class ElectionPublicKey
    {
        public string owner_id { get; set; }
        public SchnorrProof proof { get; set; }
        public string key { get; set; }

    }

    public class ElectionJointKey
    {
        public string joint_key { get; set; }
    }

    public class CombineElectionKeysRequest
    {
        public string[] election_public_keys { get; set; }
    }

    public class CiphertextBallotSelection
    {
        public string object_id { get; set; }
        public string description_hash { get; set; }
        public ElGamalCiphertext ciphertext { get; set; }
        public string crypto_hash { get; set; }
        public bool is_placeholder_selection { get; set; }
        public string nonce { get; set; }
        public DisjunctiveChaumPedersenProof proof { get; set; }
        public ElGamalCiphertext extended_data { get; set; }
    }

    public class ConstantChaumPedersenProof
    {
        public string name { get; set; }
        public string pad { get; set; }
        public string data { get; set; }
        public string challenge { get; set; }
        public string response { get; set; }
        public int constant { get; set; }
        public string usage { get; set; }
    }

    public class DisjunctiveChaumPedersenProof
    {
        public string name { get; set; }
        public string proof_zero_pad { get; set; }
        public string proof_zero_data { get; set; }
        public string proof_one_pad { get; set; }
        public string proof_one_data { get; set; }
        public string proof_zero_challenge { get; set; }
        public string proof_one_challenge { get; set; }
        public string challenge { get; set; }
        public string proof_zero_response { get; set; }
        public string proof_one_response { get; set; }
        public string usage { get; set; }
    }

    public class CiphertextBallotContest
    {
        public string object_id { get; set; }
        public string description_hash { get; set; }
        public CiphertextBallotSelection[] ballot_selections { get; set; }
        public string crypto_hash { get; set; }
        public string nonce { get; set; }
        public ConstantChaumPedersenProof? proof { get; set; }
    }

    public class CiphertextBallot
    {
        public string object_id { get; set; }
        public string ballot_style { get; set; }
        public string description_hash { get; set; }
        public string previous_tracking_hash { get; set; }
        public CiphertextBallotContest[] contests { get; set; }
        public string tracking_hash { get; set; }
        public int timestamp { get; set; }
        public string crypto_hash { get; set; }
        public string nonce { get; set; }
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

    public class CiphertextAcceptedBallot : CiphertextBallot
    {
        // Of type BallotBoxState
        public string state { get; set; }
    }

    public class ChaumPedersenProof
    {
        public string pad { get; set; }
        public string data { get; set; }
        public string challenge { get; set; }
        public string response { get; set; }
        public string usage { get; set; }
    }

    public class CiphertextCompensatedDecryptionSelection
    {
        public string object_id { get; set; }
        public string guardian_id { get; set; }
        public string missing_guardian_id { get; set; }
        public string description_hash { get; set; }
        public string share { get; set; }
        public string recovery_key { get; set; }
        public ChaumPedersenProof proof { get; set; }
    }

    public class CiphertextDecryptionSelection
    {
        public string object_id { get; set; }
        public string guardian_id { get; set; }
        public string description_hash { get; set; }
        public string share { get; set; }
        public ChaumPedersenProof proof { get; set; }
        public Dictionary<string, CiphertextCompensatedDecryptionSelection> recovered_parts { get; set; }
    }

    public class CiphertextDecryptionContest
    {
        public string object_id { get; set; }
        public string guardian_id { get; set; }
        public string description_hash { get; set; }
        public Dictionary<string, CiphertextDecryptionSelection> selections { get; set; }
    }

    public class BallotDecryptionShare
    {
        public string guardian_id { get; set; }
        public string public_key { get; set; }
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
        public string pad { get; set; }
        public string data { get; set; }
    }

    public class PlaintextTallySelection
    {
        public string object_id { get; set; }
        public int tally { get; set; }
        public string value { get; set; }
        public ElGamalCiphertext message { get; set; }
        public CiphertextDecryptionSelection[] shares { get; set; }
    }

    public class PlaintextTallyContest
    {
        public string object_id { get; set; }
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
        public string object_id { get; set; }
        public string description_hash { get; set; }
        public ElGamalCiphertext ciphertext { get; set; }
    }

    public class CiphertextTallyContest
    {
        public string object_id { get; set; }
        public string description_hash { get; set; }
        public Dictionary<string, CiphertextTallySelection> tally_selections { get; set; }
    }

    public class PublishedCiphertextTally
    {
        public string object_id { get; set; }
        public Dictionary<string, CiphertextTallyContest> cast { get; set; }
    }

    public class AppendTallyRequest : StartTallyRequest
    {
        public PublishedCiphertextTally encrypted_tally { get; set; }
    }

    public class TallyDecryptionShare
    {
        public string guardian_id { get; set; }
        public string public_key { get; set; }
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

    public class PlainTextBallotSelection
    {
        public string object_id { get; set; }
        public string vote { get; set; }
        public bool is_placeholder_selection { get; set; }
        public ExtendedData extended_data { get; set; }
    }

    public class PlainTextBallotContest
    {
        public string object_id { get; set; }
        public PlainTextBallotSelection[] ballot_selections { get; set; }
    }

    public class PlaintextBallot
    {
        public string object_id { get; set; }
        public string ballot_style { get; set; }
        public PlainTextBallotContest[] contests { get; set; }
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
        public CiphertextBallot[] encrypted_ballots { get; set; }
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