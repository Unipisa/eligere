﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ElectionGuardWebAPIClientTests
{
    public static class TestData
    {
        public static string description = @"
{
    ""ballot_styles"": [
        {
            ""geopolitical_unit_ids"": [
                ""hamilton-county"",
                ""congress-district-7""
            ],
            ""object_id"": ""congress-district-7-hamilton-county""
        },
        {
            ""geopolitical_unit_ids"": [
                ""hamilton-county"",
                ""congress-district-7"",
                ""lacroix-township-precinct-1""
            ],
            ""object_id"": ""congress-district-7-lacroix""
        },
        {
            ""geopolitical_unit_ids"": [
                ""hamilton-county"",
                ""congress-district-7"",
                ""lacroix-township-precinct-1"",
                ""lacroix-exeter-utility-district""
            ],
            ""object_id"": ""congress-district-7-lacroix-exeter""
        },
        {
            ""geopolitical_unit_ids"": [
                ""hamilton-county"",
                ""congress-district-7"",
                ""arlington-township-precinct-1""
            ],
            ""object_id"": ""congress-district-7-arlington""
        },
        {
            ""geopolitical_unit_ids"": [
                ""hamilton-county"",
                ""congress-district-7"",
                ""arlington-township-precinct-1"",
                ""pismo-beach-school-district-precinct-1""
            ],
            ""object_id"": ""congress-district-7-arlington-pismo-beach""
        },
        {
            ""geopolitical_unit_ids"": [
                ""hamilton-county"",
                ""congress-district-7"",
                ""arlington-township-precinct-1"",
                ""somerset-school-district-precinct-1""
            ],
            ""object_id"": ""congress-district-7-arlington-somerset""
        },
        {
            ""geopolitical_unit_ids"": [
                ""hamilton-county"",
                ""congress-district-5""
            ],
            ""object_id"": ""congress-district-5-hamilton-county""
        },
        {
            ""geopolitical_unit_ids"": [
                ""hamilton-county"",
                ""congress-district-5"",
                ""lacroix-township-precinct-1""
            ],
            ""object_id"": ""congress-district-5-lacroix""
        },
        {
            ""geopolitical_unit_ids"": [
                ""hamilton-county"",
                ""congress-district-5"",
                ""harris-township""
            ],
            ""object_id"": ""congress-district-5-harris""
        },
        {
            ""geopolitical_unit_ids"": [
                ""hamilton-county"",
                ""congress-district-5"",
                ""arlington-township-precinct-1"",
                ""pismo-beach-school-district-precinct-1""
            ],
            ""object_id"": ""congress-district-5-arlington-pismo-beach""
        },
        {
            ""geopolitical_unit_ids"": [
                ""hamilton-county"",
                ""congress-district-5"",
                ""arlington-township-precinct-1"",
                ""somerset-school-district-precinct-1""
            ],
            ""object_id"": ""congress-district-5-arlington-somerset""
        }
    ],
    ""candidates"": [
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Joseph Barchi and Joseph Hallaren""
                    }
                ]
            },
            ""object_id"": ""barchi-hallaren"",
            ""party_id"": ""whig""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Adam Cramer and Greg Vuocolo""
                    }
                ]
            },
            ""object_id"": ""cramer-vuocolo"",
            ""party_id"": ""federalist""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Daniel Court and Amy Blumhardt""
                    }
                ]
            },
            ""object_id"": ""court-blumhardt"",
            ""party_id"": ""peoples""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Alvin Boone and James Lian""
                    }
                ]
            },
            ""object_id"": ""boone-lian"",
            ""party_id"": ""liberty""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Ashley Hildebrand-McDougall and James Garritty""
                    }
                ]
            },
            ""object_id"": ""hildebrand-garritty"",
            ""party_id"": ""constitution""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Martin Patterson and Clay Lariviere""
                    }
                ]
            },
            ""object_id"": ""patterson-lariviere"",
            ""party_id"": ""labor""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Charlene Franz""
                    }
                ]
            },
            ""object_id"": ""franz"",
            ""party_id"": ""federalist""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Gerald Harris""
                    }
                ]
            },
            ""object_id"": ""harris"",
            ""party_id"": ""peoples""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Linda Bargmann""
                    }
                ]
            },
            ""object_id"": ""bargmann"",
            ""party_id"": ""constitution""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Barbara Abcock""
                    }
                ]
            },
            ""object_id"": ""abcock"",
            ""party_id"": ""liberty""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Carrie Steel-Loy""
                    }
                ]
            },
            ""object_id"": ""steel-loy"",
            ""party_id"": ""labor""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Frederick Sharp""
                    }
                ]
            },
            ""object_id"": ""sharp"",
            ""party_id"": ""constitution""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Alex Wallace""
                    }
                ]
            },
            ""object_id"": ""wallace"",
            ""party_id"": ""independent""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Barbara Williams""
                    }
                ]
            },
            ""object_id"": ""williams"",
            ""party_id"": ""peoples""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Althea Sharp""
                    }
                ]
            },
            ""object_id"": ""sharp-althea"",
            ""party_id"": ""whig""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Douglas Alpern""
                    }
                ]
            },
            ""object_id"": ""alpern"",
            ""party_id"": ""federalist""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Ann Windbeck""
                    }
                ]
            },
            ""object_id"": ""windbeck"",
            ""party_id"": ""peoples""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Mike Greher""
                    }
                ]
            },
            ""object_id"": ""greher"",
            ""party_id"": ""constitution""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Patricia Alexander""
                    }
                ]
            },
            ""object_id"": ""alexander"",
            ""party_id"": ""whig""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Kenneth Mitchell""
                    }
                ]
            },
            ""object_id"": ""mitchell"",
            ""party_id"": ""federalist""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Stan Lee""
                    }
                ]
            },
            ""object_id"": ""lee"",
            ""party_id"": ""independent""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Henry Ash""
                    }
                ]
            },
            ""object_id"": ""ash"",
            ""party_id"": ""liberty""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Karen Kennedy""
                    }
                ]
            },
            ""object_id"": ""kennedy"",
            ""party_id"": ""independent""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Van Jackson""
                    }
                ]
            },
            ""object_id"": ""jackson"",
            ""party_id"": ""labor""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Debbie Brown""
                    }
                ]
            },
            ""object_id"": ""brown"",
            ""party_id"": ""peoples""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Joseph Teller""
                    }
                ]
            },
            ""object_id"": ""teller"",
            ""party_id"": ""peoples""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Greg Ward""
                    }
                ]
            },
            ""object_id"": ""ward"",
            ""party_id"": ""independent""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Lou Murphy""
                    }
                ]
            },
            ""object_id"": ""murphy"",
            ""party_id"": ""federalist""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Jane Newman""
                    }
                ]
            },
            ""object_id"": ""newman"",
            ""party_id"": ""whig""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Jack Callanann""
                    }
                ]
            },
            ""object_id"": ""callanann"",
            ""party_id"": ""labor""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Esther York""
                    }
                ]
            },
            ""object_id"": ""york"",
            ""party_id"": ""labor""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Glenn Chandler""
                    }
                ]
            },
            ""object_id"": ""chandler"",
            ""party_id"": ""labor""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Andrea Solis""
                    }
                ]
            },
            ""object_id"": ""solis"",
            ""party_id"": ""labor""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Amos Keller""
                    }
                ]
            },
            ""object_id"": ""keller"",
            ""party_id"": ""constitution""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Davitra Rangel""
                    }
                ]
            },
            ""object_id"": ""rangel"",
            ""party_id"": ""peoples""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Camille Argent""
                    }
                ]
            },
            ""object_id"": ""argent"",
            ""party_id"": ""liberty""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Chloe Witherspoon-Smithson""
                    }
                ]
            },
            ""object_id"": ""witherspoon-smithson"",
            ""party_id"": ""independent""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Clayton Bainbridge""
                    }
                ]
            },
            ""object_id"": ""bainbridge"",
            ""party_id"": ""peoples""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Charlene Hennessey""
                    }
                ]
            },
            ""object_id"": ""hennessey"",
            ""party_id"": ""whig""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Eric Savoy""
                    }
                ]
            },
            ""object_id"": ""savoy"",
            ""party_id"": ""labor""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Susan Tawa""
                    }
                ]
            },
            ""object_id"": ""tawa"",
            ""party_id"": ""constitution""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Mary Tawa""
                    }
                ]
            },
            ""object_id"": ""tawa-mary"",
            ""party_id"": ""independent""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Valarie Altman""
                    }
                ]
            },
            ""object_id"": ""altman"",
            ""party_id"": ""peoples""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Helen Moore""
                    }
                ]
            },
            ""object_id"": ""moore""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""John White""
                    }
                ]
            },
            ""object_id"": ""white""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""John Smallberries""
                    }
                ]
            },
            ""object_id"": ""smallberries""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""John Warfin""
                    }
                ]
            },
            ""object_id"": ""warfin""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Chris Norberg""
                    }
                ]
            },
            ""object_id"": ""norberg""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Abigail Parks""
                    }
                ]
            },
            ""object_id"": ""parks""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Harmony Savannah""
                    }
                ]
            },
            ""object_id"": ""savannah""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Buffy Summers""
                    }
                ]
            },
            ""object_id"": ""summers""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Cordelia Chase""
                    }
                ]
            },
            ""object_id"": ""chase""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Daniel Osborne""
                    }
                ]
            },
            ""object_id"": ""osborne""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Willow Rosenberg""
                    }
                ]
            },
            ""object_id"": ""rosenberg""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Anthony Stewart Head""
                    }
                ]
            },
            ""object_id"": ""head""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""James Marsters""
                    }
                ]
            },
            ""object_id"": ""marsters""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Write In Candidate""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Escribir en la candidata""
                    }
                ]
            },
            ""isWriteIn"": true,
            ""object_id"": ""write-in-1""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Write In Candidate""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Escribir en la candidata""
                    }
                ]
            },
            ""isWriteIn"": true,
            ""object_id"": ""write-in-2""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Write In Candidate""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Escribir en la candidata""
                    }
                ]
            },
            ""isWriteIn"": true,
            ""object_id"": ""write-in-3""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Retain""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Conservar""
                    }
                ]
            },
            ""object_id"": ""ozark-chief-justice-retain-demergue-affirmative""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Reject""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Rechazar""
                    }
                ]
            },
            ""object_id"": ""ozark-chief-justice-retain-demergue-negative""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Yes""
                    }
                ]
            },
            ""object_id"": ""exeter-utility-district-referendum-affirmative""
        },
        {
            ""ballot_name"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""No""
                    }
                ]
            },
            ""object_id"": ""exeter-utility-district-referendum-negative""
        }
    ],
    ""contact_information"": {
        ""address_line"": [
            ""1234 Paul Revere Run"",
            ""Hamilton, Ozark 99999""
        ],
        ""email"": [
            {
                ""annotation"": ""press"",
                ""value"": ""inquiries@hamilton.state.gov""
            },
            {
                ""annotation"": ""federal"",
                ""value"": ""commissioner@hamilton.state.gov""
            }
        ],
        ""name"": ""Hamilton State Election Commission"",
        ""phone"": [
            {
                ""annotation"": ""domestic"",
                ""value"": ""123-456-7890""
            },
            {
                ""annotation"": ""international"",
                ""value"": ""+1-123-456-7890""
            }
        ]
    },
    ""contests"": [
        {
            ""@type"": ""CandidateContest"",
            ""ballot_selections"": [
                {
                    ""candidate_id"": ""barchi-hallaren"",
                    ""object_id"": ""barchi-hallaren-selection"",
                    ""sequence_order"": 0
                },
                {
                    ""candidate_id"": ""cramer-vuocolo"",
                    ""object_id"": ""cramer-vuocolo-selection"",
                    ""sequence_order"": 1
                },
                {
                    ""candidate_id"": ""court-blumhardt"",
                    ""object_id"": ""court-blumhardt-selection"",
                    ""sequence_order"": 2
                },
                {
                    ""candidate_id"": ""boone-lian"",
                    ""object_id"": ""boone-lian-selection"",
                    ""sequence_order"": 3
                },
                {
                    ""candidate_id"": ""hildebrand-garritty"",
                    ""object_id"": ""hildebrand-garritty-selection"",
                    ""sequence_order"": 4
                },
                {
                    ""candidate_id"": ""patterson-lariviere"",
                    ""object_id"": ""patterson-lariviere-selection"",
                    ""sequence_order"": 5
                },
                {
                    ""candidate_id"": ""write-in"",
                    ""object_id"": ""write-in-selection-president"",
                    ""sequence_order"": 6
                }
            ],
            ""ballot_subtitle"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Vote for one""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Votar por uno""
                    }
                ]
            },
            ""ballot_title"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""President and Vice President of the United States""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Presidente y Vicepresidente de los Estados Unidos""
                    }
                ]
            },
            ""electoral_district_id"": ""hamilton-county"",
            ""name"": ""President and Vice President of the United States"",
            ""number_elected"": 1,
            ""object_id"": ""president-vice-president-contest"",
            ""sequence_order"": 0,
            ""vote_variation"": ""one_of_m"",
            ""votes_allowed"": 1
        },
        {
            ""@type"": ""CandidateContest"",
            ""ballot_selections"": [
                {
                    ""candidate_id"": ""franz"",
                    ""object_id"": ""franz-selection"",
                    ""sequence_order"": 0
                },
                {
                    ""candidate_id"": ""harris"",
                    ""object_id"": ""harris-selection"",
                    ""sequence_order"": 1
                },
                {
                    ""candidate_id"": ""bargmann"",
                    ""object_id"": ""bargmann-selection"",
                    ""sequence_order"": 2
                },
                {
                    ""candidate_id"": ""abcock"",
                    ""object_id"": ""abcock-selection"",
                    ""sequence_order"": 3
                },
                {
                    ""candidate_id"": ""steel-loy"",
                    ""object_id"": ""steel-loy-selection"",
                    ""sequence_order"": 4
                },
                {
                    ""candidate_id"": ""sharp"",
                    ""object_id"": ""sharp-selection"",
                    ""sequence_order"": 5
                },
                {
                    ""candidate_id"": ""wallace"",
                    ""object_id"": ""walace-selection"",
                    ""sequence_order"": 6
                },
                {
                    ""candidate_id"": ""williams"",
                    ""object_id"": ""williams-selection"",
                    ""sequence_order"": 7
                },
                {
                    ""candidate_id"": ""alpern"",
                    ""object_id"": ""alpern-selection"",
                    ""sequence_order"": 9
                },
                {
                    ""candidate_id"": ""windbeck"",
                    ""object_id"": ""windbeck-selection"",
                    ""sequence_order"": 10
                },
                {
                    ""candidate_id"": ""sharp-althea"",
                    ""object_id"": ""sharp-althea-selection"",
                    ""sequence_order"": 11
                },
                {
                    ""candidate_id"": ""greher"",
                    ""object_id"": ""greher-selection"",
                    ""sequence_order"": 12
                },
                {
                    ""candidate_id"": ""alexander"",
                    ""object_id"": ""alexander-selection"",
                    ""sequence_order"": 13
                },
                {
                    ""candidate_id"": ""mitchell"",
                    ""object_id"": ""mitchell-selection"",
                    ""sequence_order"": 14
                },
                {
                    ""candidate_id"": ""lee"",
                    ""object_id"": ""lee-selection"",
                    ""sequence_order"": 15
                },
                {
                    ""candidate_id"": ""ash"",
                    ""object_id"": ""ash-selection"",
                    ""sequence_order"": 16
                },
                {
                    ""candidate_id"": ""kennedy"",
                    ""object_id"": ""kennedy-selection"",
                    ""sequence_order"": 17
                },
                {
                    ""candidate_id"": ""jackson"",
                    ""object_id"": ""jackson-selection"",
                    ""sequence_order"": 18
                },
                {
                    ""candidate_id"": ""brown"",
                    ""object_id"": ""brown-selection"",
                    ""sequence_order"": 19
                },
                {
                    ""candidate_id"": ""teller"",
                    ""object_id"": ""teller-selection"",
                    ""sequence_order"": 20
                },
                {
                    ""candidate_id"": ""ward"",
                    ""object_id"": ""ward-selection"",
                    ""sequence_order"": 21
                },
                {
                    ""candidate_id"": ""murphy"",
                    ""object_id"": ""murphy-selection"",
                    ""sequence_order"": 22
                },
                {
                    ""candidate_id"": ""newman"",
                    ""object_id"": ""newman-selection"",
                    ""sequence_order"": 23
                },
                {
                    ""candidate_id"": ""callanann"",
                    ""object_id"": ""callanann-selection"",
                    ""sequence_order"": 24
                },
                {
                    ""candidate_id"": ""york"",
                    ""object_id"": ""york-selection"",
                    ""sequence_order"": 25
                },
                {
                    ""candidate_id"": ""chandler"",
                    ""object_id"": ""chandler-selection"",
                    ""sequence_order"": 26
                },
                {
                    ""candidate_id"": ""write-in"",
                    ""object_id"": ""write-in-selection-governor"",
                    ""sequence_order"": 27
                }
            ],
            ""ballot_subtitle"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Vote for one""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Votar por uno""
                    }
                ]
            },
            ""ballot_title"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Governor of the Commonwealth of Ozark""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Gobernador de la Mancomunidad de Ozark""
                    }
                ]
            },
            ""electoral_district_id"": ""hamilton-county"",
            ""name"": ""Governor of the Commonwealth of Ozark"",
            ""number_elected"": 1,
            ""object_id"": ""ozark-governor"",
            ""sequence_order"": 1,
            ""vote_variation"": ""one_of_m"",
            ""votes_allowed"": 1
        },
        {
            ""@type"": ""CandidateContest"",
            ""ballot_selections"": [
                {
                    ""candidate_id"": ""soliz"",
                    ""object_id"": ""soliz-selection"",
                    ""sequence_order"": 0
                },
                {
                    ""candidate_id"": ""keller"",
                    ""object_id"": ""keller-selection"",
                    ""sequence_order"": 1
                },
                {
                    ""candidate_id"": ""rengel"",
                    ""object_id"": ""rangel-selection"",
                    ""sequence_order"": 2
                },
                {
                    ""candidate_id"": ""argent"",
                    ""object_id"": ""argent-selection"",
                    ""sequence_order"": 3
                },
                {
                    ""candidate_id"": ""witherspoon-smithson"",
                    ""object_id"": ""witherspoon-smithson-selection"",
                    ""sequence_order"": 4
                },
                {
                    ""candidate_id"": ""write-in"",
                    ""object_id"": ""write-in-selection-us-congress-district-5"",
                    ""sequence_order"": 5
                }
            ],
            ""ballot_subtitle"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Vote for one""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Votar por uno""
                    }
                ]
            },
            ""ballot_title"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""House of Representatives Congressional District 5""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Cámara de Representantes de Distrito 5 del Congreso de Ozark""
                    }
                ]
            },
            ""electoral_district_id"": ""congress-district-5"",
            ""name"": ""Congressional District 5"",
            ""number_elected"": 1,
            ""object_id"": ""congress-district-5-contest"",
            ""sequence_order"": 2,
            ""vote_variation"": ""one_of_m"",
            ""votes_allowed"": 1
        },
        {
            ""@type"": ""CandidateContest"",
            ""ballot_selections"": [
                {
                    ""candidate_id"": ""bainbridge"",
                    ""object_id"": ""bainbridge-selection"",
                    ""sequence_order"": 0
                },
                {
                    ""candidate_id"": ""hennessey"",
                    ""object_id"": ""hennessey-selection"",
                    ""sequence_order"": 1
                },
                {
                    ""candidate_id"": ""savoy"",
                    ""object_id"": ""savoy-selection"",
                    ""sequence_order"": 2
                },
                {
                    ""candidate_id"": ""tawa"",
                    ""object_id"": ""tawa-selection"",
                    ""sequence_order"": 3
                },
                {
                    ""candidate_id"": ""tawa-mary"",
                    ""object_id"": ""tawa-mary-selection"",
                    ""sequence_order"": 4
                },
                {
                    ""candidate_id"": ""write-in"",
                    ""object_id"": ""write-in-selection-us-congress-district-7"",
                    ""sequence_order"": 5
                }
            ],
            ""ballot_subtitle"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Vote for one""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Votar por uno""
                    }
                ]
            },
            ""ballot_title"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""House of Representatives Ozark Congressional District 7""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Cámara de Representantes de Distrito 7 del Congreso de Ozark""
                    }
                ]
            },
            ""electoral_district_id"": ""congress-district-7"",
            ""name"": ""Congressional District 7"",
            ""number_elected"": 1,
            ""object_id"": ""congress-district-7-contest"",
            ""sequence_order"": 3,
            ""vote_variation"": ""one_of_m"",
            ""votes_allowed"": 1
        },
        {
            ""@type"": ""CandidateContest"",
            ""ballot_selections"": [
                {
                    ""candidate_id"": ""moore"",
                    ""object_id"": ""moore-selection"",
                    ""sequence_order"": 0
                },
                {
                    ""candidate_id"": ""white"",
                    ""object_id"": ""white-selection"",
                    ""sequence_order"": 1
                },
                {
                    ""candidate_id"": ""smallberries"",
                    ""object_id"": ""smallberries-selection"",
                    ""sequence_order"": 2
                },
                {
                    ""candidate_id"": ""warfin"",
                    ""object_id"": ""warfin-selection"",
                    ""sequence_order"": 3
                },
                {
                    ""candidate_id"": ""norberg"",
                    ""object_id"": ""norberg-selection"",
                    ""sequence_order"": 4
                },
                {
                    ""candidate_id"": ""parks"",
                    ""object_id"": ""parks-selection"",
                    ""sequence_order"": 5
                },
                {
                    ""candidate_id"": ""savannah"",
                    ""object_id"": ""savannah-selection"",
                    ""sequence_order"": 6
                },
                {
                    ""candidate_id"": ""write-in-1"",
                    ""object_id"": ""write-in-selection-1-pismo-beach-school-board"",
                    ""sequence_order"": 7
                },
                {
                    ""candidate_id"": ""write-in-2"",
                    ""object_id"": ""write-in-selection-2-pismo-beach-school-board"",
                    ""sequence_order"": 8
                },
                {
                    ""candidate_id"": ""write-in-3"",
                    ""object_id"": ""write-in-selection-3-pismo-beach-school-board"",
                    ""sequence_order"": 9
                }
            ],
            ""ballot_subtitle"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Vote for up to 3""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Vote por hasta 3""
                    }
                ]
            },
            ""ballot_title"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Pismo Beach School Board""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Junta Escolar de Pismo Beach""
                    }
                ]
            },
            ""electoral_district_id"": ""pismo-beach-school-district-precinct-1"",
            ""name"": ""Pismo Beach School Board"",
            ""number_elected"": 3,
            ""object_id"": ""pismo-beach-school-board-contest"",
            ""sequence_order"": 4,
            ""vote_variation"": ""n_of_m"",
            ""votes_allowed"": 3
        },
        {
            ""@type"": ""CandidateContest"",
            ""ballot_selections"": [
                {
                    ""candidate_id"": ""summers"",
                    ""object_id"": ""summers-selection"",
                    ""sequence_order"": 0
                },
                {
                    ""candidate_id"": ""chase"",
                    ""object_id"": ""chase-selection"",
                    ""sequence_order"": 1
                },
                {
                    ""candidate_id"": ""osborne"",
                    ""object_id"": ""osborne-selection"",
                    ""sequence_order"": 2
                },
                {
                    ""candidate_id"": ""rosenberg"",
                    ""object_id"": ""rosenberg-selection"",
                    ""sequence_order"": 3
                },
                {
                    ""candidate_id"": ""head"",
                    ""object_id"": ""head-selection"",
                    ""sequence_order"": 4
                },
                {
                    ""candidate_id"": ""marsters"",
                    ""object_id"": ""marsters-selection"",
                    ""sequence_order"": 5
                },
                {
                    ""candidate_id"": ""write-in-1"",
                    ""object_id"": ""write-in-selection-1-somerset-school-board"",
                    ""sequence_order"": 6
                },
                {
                    ""candidate_id"": ""write-in-2"",
                    ""object_id"": ""write-in-selection-2-somerset-school-board"",
                    ""sequence_order"": 7
                }
            ],
            ""ballot_subtitle"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Vote for up to 2""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Vote por hasta 2""
                    }
                ]
            },
            ""ballot_title"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Pismo Beach School Board""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Junta Escolar de Somerset""
                    }
                ]
            },
            ""electoral_district_id"": ""somerset-school-district-precinct-1"",
            ""name"": ""Somerset School Board"",
            ""number_elected"": 2,
            ""object_id"": ""somerset-school-board-contest"",
            ""sequence_order"": 5,
            ""vote_variation"": ""n_of_m"",
            ""votes_allowed"": 2
        },
        {
            ""@type"": ""ReferendumContest"",
            ""ballot_selections"": [
                {
                    ""candidate_id"": ""ozark-chief-justice-retain-demergue-affirmative"",
                    ""object_id"": ""ozark-chief-justice-retain-demergue-affirmative-selection"",
                    ""sequence_order"": 0
                },
                {
                    ""candidate_id"": ""ozark-chief-justice-retain-demergue-negative"",
                    ""object_id"": ""ozark-chief-justice-retain-demergue-negative-selection"",
                    ""sequence_order"": 1
                }
            ],
            ""ballot_subtitle"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Choose 'Accept' or 'Reject'""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Elija 'Aceptar' o 'Rechazar'""
                    }
                ]
            },
            ""ballot_title"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Retain Robert Demergue as Chief Justice?""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""¿Retener a Robert Demergue como Presidente del Tribunal Supremo?""
                    }
                ]
            },
            ""electoral_district_id"": ""arlington-township-precinct-1"",
            ""name"": ""Retain Robert Demergue as Chief Justice?"",
            ""number_elected"": 1,
            ""object_id"": ""arlington-chief-justice-retain-demergue"",
            ""sequence_order"": 6,
            ""vote_variation"": ""one_of_m"",
            ""votes_allowed"": 1
        },
        {
            ""@type"": ""ReferendumContest"",
            ""ballotDescription"": {
                ""text"": [
                    {
                        ""value"": ""The Executive Board of the Exeter Utility district adopted Ordinance No. 970 concerning voter approval of its regular property tax levy. This proposition would maintain police, fire, park and mandated community services by increasing the regular property tax levy rate above the limit factor by $0.20/$1,000 assessed value to a maximum rate of $0.83712/$1,000 assessed valuation for collection in 2020, set a 5% limit factor for each year 2021-2025, use the 2025 levy amount as the base to compute subsequent levy limits, and exempt low income seniors and disabled; as set forth in Ordinance No. 970."",
                        ""language"": ""en""
                    },
                    {
                        ""value"": ""La Junta Ejecutiva del distrito de Servicios Públicos de Exeter adoptó la Ordenanza No. 970 sobre la aprobación de los votantes de su recaudación de impuestos de propiedad regulatoria. Esta propuesta mantendría a la policía, los bomberos, los parques y los servicios comunitarios obligatorios al aumentar la tasa de recaudación del impuesto a la propiedad regular de por encima del factor límite en un valor tasado de $ 0.20 / $ 1,000 a una tasa máxima de $ 0.83712 / $ 1,000 de tasación tasada para cobrar en 2020, establecer un 5% factor límite para cada año 2021-2025, use el monto del impuesto 2025 como base para calcular los límites de impuestos posteriores, y exima a las personas de la tercera edad de bajos ingresos y discapacitadas; como se establece en la Ordenanza No. 970."",
                        ""language"": ""es""
                    }
                ]
            },
            ""ballot_selections"": [
                {
                    ""candidate_id"": ""exeter-utility-district-referendum-affirmative"",
                    ""object_id"": ""exeter-utility-district-referendum-affirmative-selection"",
                    ""sequence_order"": 0
                },
                {
                    ""candidate_id"": ""exeter-utility-district-referendum-negative"",
                    ""object_id"": ""exeter-utility-district-referendum-selection"",
                    ""sequence_order"": 1
                }
            ],
            ""ballot_subtitle"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Should this Proposition be approved?""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Uno""
                    }
                ]
            },
            ""ballot_title"": {
                ""text"": [
                    {
                        ""language"": ""en"",
                        ""value"": ""Levy Lift to Maintain Public Safety and Other Core Utility Services""
                    },
                    {
                        ""language"": ""es"",
                        ""value"": ""Levy Lift para Mantener la Seguridad Pública y Otros Servicios Básicos""
                    }
                ]
            },
            ""electoral_district_id"": ""lacroix-exeter-utility-district"",
            ""name"": ""Capital Projects Levy"",
            ""number_elected"": 1,
            ""object_id"": ""exeter-utility-district-referendum-contest"",
            ""sequence_order"": 7,
            ""vote_variation"": ""one_of_m"",
            ""votes_allowed"": 1
        }
    ],
    ""election_scope_id"": ""hamilton-county-general-election"",
    ""end_date"": ""2020-03-01T20:00:00-05:00"",
    ""geopolitical_units"": [
        {
            ""contact_information"": {
                ""address_line"": [
                    ""1234 Samuel Adams Way"",
                    ""Hamilton, Ozark 99999""
                ],
                ""email"": [
                    {
                        ""annotation"": ""inquiries"",
                        ""value"": ""inquiries@hamiltoncounty.gov""
                    }
                ],
                ""name"": ""Hamilton County Clerk"",
                ""phone"": [
                    {
                        ""annotation"": ""domestic"",
                        ""value"": ""123-456-7890""
                    }
                ]
            },
            ""name"": ""Hamilton County"",
            ""object_id"": ""hamilton-county"",
            ""type"": ""county""
        },
        {
            ""contact_information"": {
                ""address_line"": [
                    ""1234 Somerville Gateway"",
                    ""Medford, Ozark 999999""
                ],
                ""email"": [
                    {
                        ""annotation"": ""inquiries"",
                        ""value"": ""inquiries@congressional-district-5.gov""
                    }
                ],
                ""name"": ""Medford Town Hall"",
                ""phone"": [
                    {
                        ""annotation"": ""domestic"",
                        ""value"": ""123-456-7890""
                    }
                ]
            },
            ""name"": ""Congressional District 5"",
            ""object_id"": ""congress-district-5"",
            ""type"": ""congressional""
        },
        {
            ""contact_information"": {
                ""address_line"": [
                    ""1234 Somerville Gateway"",
                    ""Arlington, Ozark 999999""
                ],
                ""email"": [
                    {
                        ""annotation"": ""inquiries"",
                        ""value"": ""inquiries@congressional-district-7.gov""
                    }
                ],
                ""name"": ""Arlington Town Hall"",
                ""phone"": [
                    {
                        ""annotation"": ""domestic"",
                        ""value"": ""123-456-7890""
                    }
                ]
            },
            ""name"": ""Congressional District 7"",
            ""object_id"": ""congress-district-7"",
            ""type"": ""congressional""
        },
        {
            ""contact_information"": {
                ""address_line"": [
                    ""1234 Thorton Drive"",
                    ""LaCroix, Ozark 99999""
                ],
                ""email"": [
                    {
                        ""annotation"": ""inquiries"",
                        ""value"": ""inquiries@lacrox.gov""
                    }
                ],
                ""name"": ""LaCroix Town Hall"",
                ""phone"": [
                    {
                        ""annotation"": ""domestic"",
                        ""value"": ""123-456-7890""
                    }
                ]
            },
            ""name"": ""LaCroix Township Precinct 1"",
            ""object_id"": ""lacroix-township-precinct-1"",
            ""type"": ""precinct""
        },
        {
            ""contact_information"": {
                ""address_line"": [
                    ""1234 Watt Drive"",
                    ""LaCroix, Ozark 99999""
                ],
                ""email"": [
                    {
                        ""annotation"": ""inquiries"",
                        ""value"": ""inquiries@exeter-utility.com""
                    }
                ],
                ""name"": ""Exeter Utility District coordinator"",
                ""phone"": [
                    {
                        ""annotation"": ""domestic"",
                        ""value"": ""123-456-7890""
                    }
                ]
            },
            ""name"": ""Exeter Utility District"",
            ""object_id"": ""lacroix-exeter-utility-district"",
            ""type"": ""utility""
        },
        {
            ""contact_information"": {
                ""address_line"": [
                    ""1234 Pahk Avenue"",
                    ""Arlinton, Ozark 99999""
                ],
                ""email"": [
                    {
                        ""annotation"": ""inquiries"",
                        ""value"": ""inquiries@arlington-township.gov""
                    }
                ],
                ""name"": ""Arlington Town Hall"",
                ""phone"": [
                    {
                        ""annotation"": ""domestic"",
                        ""value"": ""123-456-7890""
                    }
                ]
            },
            ""name"": ""Arlington Township Precinct 1"",
            ""object_id"": ""arlington-township-precinct-1"",
            ""type"": ""precinct""
        },
        {
            ""contact_information"": {
                ""address_line"": [
                    ""1234 Pismo Beach Elementary"",
                    ""Arlington, Ozark 99999""
                ],
                ""email"": [
                    {
                        ""annotation"": ""inquiries"",
                        ""value"": ""inquiries@pismo-beach-school.edu""
                    }
                ],
                ""name"": ""Pismo Beah Elementary"",
                ""phone"": [
                    {
                        ""annotation"": ""domestic"",
                        ""value"": ""123-456-7890""
                    }
                ]
            },
            ""name"": ""Pismo Beach School District Precinct 1"",
            ""object_id"": ""pismo-beach-school-district-precinct-1"",
            ""type"": ""school""
        },
        {
            ""contact_information"": {
                ""address_line"": [
                    ""1234 Somerset Avenue"",
                    ""Arlinton, Ozark 99999""
                ],
                ""email"": [
                    {
                        ""annotation"": ""inquiries"",
                        ""value"": ""inquiries@somerset-elementary.edu""
                    }
                ],
                ""name"": ""Someset Elementary"",
                ""phone"": [
                    {
                        ""annotation"": ""domestic"",
                        ""value"": ""123-456-7890""
                    }
                ]
            },
            ""name"": ""Somerset School District"",
            ""object_id"": ""somerset-school-district-precinct-1"",
            ""type"": ""school""
        },
        {
            ""contact_information"": {
                ""address_line"": [
                    ""1234 Pahk Avenue"",
                    ""Harris, Ozark 99999""
                ],
                ""email"": [
                    {
                        ""annotation"": ""inquiries"",
                        ""value"": ""inquiries@harris-township.gov""
                    }
                ],
                ""name"": ""harris Town Hall"",
                ""phone"": [
                    {
                        ""annotation"": ""domestic"",
                        ""value"": ""123-456-7890""
                    }
                ]
            },
            ""name"": ""Harris Township"",
            ""object_id"": ""harris-township"",
            ""type"": ""township""
        }
    ],
    ""name"": {
        ""text"": [
            {
                ""language"": ""en"",
                ""value"": ""Hamiltion County General Election""
            },
            {
                ""language"": ""es"",
                ""value"": ""Elección general del condado de Hamilton""
            }
        ]
    },
    ""parties"": [
        {
            ""abbreviation"": ""WHI"",
            ""ballot_name"": {
                ""text"": []
            },
            ""color"": ""AAAAAA"",
            ""logo_uri"": ""http://some/path/to/whig.svg"",
            ""name"": {
                ""text"": [
                    {
                        ""value"": ""Whig Party"",
                        ""language"": ""en""
                    }
                ]
            },
            ""object_id"": ""whig""
        },
        {
            ""abbreviation"": ""FED"",
            ""ballot_name"": {
                ""text"": []
            },
            ""color"": ""BBBBBB"",
            ""logo_uri"": ""http://some/path/to/federalist.svg"",
            ""name"": {
                ""text"": [
                    {
                        ""value"": ""Federalist Party"",
                        ""language"": ""en""
                    }
                ]
            },
            ""object_id"": ""federalist""
        },
        {
            ""abbreviation"": ""PPL"",
            ""ballot_name"": {
                ""text"": []
            },
            ""color"": ""CCCCCC"",
            ""logo_uri"": ""http://some/path/to/people-s.svg"",
            ""name"": {
                ""text"": [
                    {
                        ""value"": ""People's Party"",
                        ""language"": ""en""
                    }
                ]
            },
            ""object_id"": ""peoples""
        },
        {
            ""abbreviation"": ""LIB"",
            ""ballot_name"": {
                ""text"": []
            },
            ""color"": ""DDDDDD"",
            ""logo_uri"": ""http://some/path/to/liberty.svg"",
            ""name"": {
                ""text"": [
                    {
                        ""value"": ""Liberty Party"",
                        ""language"": ""en""
                    }
                ]
            },
            ""object_id"": ""liberty""
        },
        {
            ""abbreviation"": ""CONST"",
            ""ballot_name"": {
                ""text"": []
            },
            ""color"": ""EEEEEE"",
            ""logo_uri"": ""http://some/path/to/democratic-repulbican.svg"",
            ""name"": {
                ""text"": [
                    {
                        ""value"": ""Constitution Party"",
                        ""language"": ""en""
                    }
                ]
            },
            ""object_id"": ""constitution""
        },
        {
            ""abbreviation"": ""LBR"",
            ""ballot_name"": {
                ""text"": []
            },
            ""color"": ""FFFFFF"",
            ""logo_uri"": ""http://some/path/to/laobr.svg"",
            ""name"": {
                ""text"": [
                    {
                        ""value"": ""Labor Party"",
                        ""language"": ""en""
                    }
                ]
            },
            ""object_id"": ""labor""
        },
        {
            ""abbreviation"": ""IND"",
            ""ballot_name"": {
                ""text"": []
            },
            ""color"": ""000000"",
            ""logo_uri"": ""http://some/path/to/independent.svg"",
            ""name"": {
                ""text"": [
                    {
                        ""value"": ""Independent"",
                        ""language"": ""en""
                    }
                ]
            },
            ""object_id"": ""independent""
        }
    ],
    ""start_date"": ""2020-03-01T08:00:00-05:00"",
    ""type"": ""general""
}";

        public static string ballotData = @"
{
    ""ballot_style"": ""congress-district-5-hamilton-county"",
    ""contests"": [
        {
            ""ballot_selections"": [
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""barchi-hallaren-selection"",
                    ""vote"": ""True""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""cramer-vuocolo-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""court-blumhardt-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""boone-lian-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""hildebrand-garritty-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""patterson-lariviere-selection"",
                    ""vote"": ""False""
                }
            ],
            ""object_id"": ""president-vice-president-contest""
        },
        {
            ""ballot_selections"": [
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""franz-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""harris-selection"",
                    ""vote"": ""True""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""sharp-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""alpern-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""windbeck-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""greher-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""alexander-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""mitchell-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""lee-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""ash-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""brown-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""murphy-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""york-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""write-in-selection-governor"",
                    ""vote"": ""False""
                }
            ],
            ""object_id"": ""ozark-governor""
        },
        {
            ""ballot_selections"": [
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""soliz-selection"",
                    ""vote"": ""True""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""keller-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""rangel-selection"",
                    ""vote"": ""False""
                },
                {
                    ""is_placeholder_selection"": false,
                    ""object_id"": ""write-in-selection-us-congress-district-5"",
                    ""vote"": ""False""
                }
            ],
            ""object_id"": ""congress-district-5-contest""
        }
    ],
    ""object_id"": ""TEMPLATE_BALLOT""
}
";
    }
}
