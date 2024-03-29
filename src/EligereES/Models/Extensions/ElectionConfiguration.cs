﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EligereES.Models.Extensions
{
    public enum QuorumType
    {
        NoQuorum = 0,
        PotentialVoters = 1,
        ActualVoters = 2
    }

    public enum CandidatesType
    {
        Implicit = 0,
        OnlyExplicit = 1,
        ImplicitAndExplicit = 2,
        Party=3
    }

    public enum IdentificationType
    {
        Individual = 0,
        Public = 1,
        Sampling = 2,
        IndividualAndSPID = 3
    }

    // This class is used to define configuration of a specific election (i.e. Quorum, ecc)
    // It will be serialized into a json column inside the database to ensure that DB schema will not explode over time
    public class ElectionConfiguration
    {
        public string ToJson()
        {
            return JsonSerializer.Serialize<ElectionConfiguration>(this);
        }

        public static ElectionConfiguration FromJson(string data)
        {
            return JsonSerializer.Deserialize<ElectionConfiguration>(data);
        }

        public string Notes { get; set; }

        public double ValidityQuorum { get; set; }

        public QuorumType ValidityQuorumType { get; set; }

        public double ElectionQuorum { get; set; }

        public QuorumType ElectionQuorumType { get; set; }

        public double RoundQuorum { get; set; }

        public QuorumType RoundQuorumType { get; set; }

        public bool WeightedVoters { get; set; }

        public int NumPreferences { get; set; }

        // Used to have joint or disjoint vote, used only with Party Candidate type
        public int NumPartyPreferences { get; set; }

        // Deprecated
        public bool HasCandidates { get; set; }

        public CandidatesType CandidatesType { get; set; }

        public IdentificationType IdentificationType { get; set; }

        // 0 means as much as possible with recognizers
        public double SamplingRate { get; set; }

        public int EligibleSeats { get; set; }

        // if true the election card will only contains blank vote not null vote option
        public bool NoNullVote { get; set; }

        public bool ActiveForStronglyAuthenticatedUsers { get; set; }
    }
}
