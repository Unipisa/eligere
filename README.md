# Eligere

Eligere is a software intended as a contribution to digital democracy community in the belief that voting software should be open source. The software in this repo has been already used for casting more than eight thousand votes in University elections.

The software has been initially inspired as a web interface to use [ElectionGuard](https://github.com/Microsoft/ElectionGuard) SDK for implementing secure ballot boxes. The project has deviated into a system to support the full election process including the formation of electoral lists.

The codebase still needs heavy polishing and not all the features are yet exposed through the web interface.

We will keep updating this repository to make this project a successful contribution to an open democratic process.

## Status of the project
The system is based on two independent services implemented using .NET core 5.0 that interact through encrypted messages: one is called Election System (EligereES) and the other Voting System (EligereVS). The former is responsible to support the creation and management of multiple elections (as of today we supported 95 simultaneous elections), identify users, and generate voting tickets (digitally encrypted) used to access the voting system and proceed to vote as an anonymous user. The separate administration of the two system allows to enforce anonymous voting.

Currently we used Microsoft Identity to identify users, though the system is easily extensible with other authentication schemas. Identification of remote voters is performed through calls that are provided through web links, and the system attempts to balance the recognition process across a large number of operators (as of today we identified 1600 voters in a single day).

There are several changes to improve robustness and easy of install of the system, the final goal is to provide a containerized version that can be executed starting from public repositories so that it is possible to reduce internal attacks and ensure, as much as possible, the tracking between source code and binaries.

The ballot box implemented by the Voting System will be containerized first so that it will be possible to spawn multiple secure boxes simultaneously. Secure ballots will be also used to support (possibly remote) votes from assemblies for deliberation.

## What Eligere is and what it is not
Eligere has been designed to be as clean as possible to help code inspection. We favored declarative programming patterns to ensure that semantics of operations does not depend only by the system itself. Security is not only based on encryption techniques, procedures and processes are also crucial to ensure a secure voting process. Every design decision has been conceived to favor anonimity and compartimentalization of rights and privileges.

The focus of Eligere is to target the voting process (to form electoral lists and polling commissions) and safely (and anonymously) collect votes and providing counts. The specific rules of an election are out of the scope of the system in order to ensure simplicity of the system, key to the crucial tenet of being readable. 

The system is focusing on the processes and relies on external open source projects for core security primitives.

## Contribute to the project
Please feel free to contact us on GitHub to contribute to the project. We will promote an open development process through pull requests. Over time we will promote the formation of a board responsible for PR approval to ensure that the coding tenets needed to ensure the proper quality of the project.
