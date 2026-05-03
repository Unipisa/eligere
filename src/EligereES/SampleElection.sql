SET DATEFORMAT ymd;

declare @StartDate datetime
declare @EndDate datetime
declare @ClosingDate datetime
declare @PresidentID uniqueidentifier
declare @Member uniqueidentifier

set @StartDate = '2024-5-5 9:00'
set @EndDate = '2024-5-6 19:00'
set @ClosingDate = '2024-5-3'
set @PresidentID = '4EECBCD0-8AEA-4133-960C-D494D961390E'
set @Member = '71624860-6F2A-44CD-B6A3-F2126A25AD36'

-- Cleanup
update Voter set VotingTicket_FK=null where Election_FK in ('fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '7aecf054-7dad-44a5-bcaa-bc1332a405d0')
delete from VotingTicket where Voter_FK in (select id from Voter where Election_FK in ('fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '7aecf054-7dad-44a5-bcaa-bc1332a405d0'))
delete from Voter where Election_FK in ('fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '7aecf054-7dad-44a5-bcaa-bc1332a405d0')
update PollingStationCommission set President_FK=null where Election_FK in ('fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '7aecf054-7dad-44a5-bcaa-bc1332a405d0')
delete from PollingStationCommissioner where PollingStationCommission_FK in (select id from PollingStationCommission where Election_FK in ('fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '7aecf054-7dad-44a5-bcaa-bc1332a405d0'))
delete from PollingStationCommission where Election_FK in ('fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '7aecf054-7dad-44a5-bcaa-bc1332a405d0')
delete from EligibleCandidate where BallotName_FK in (select id from BallotName where Election_FK in ('fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '7aecf054-7dad-44a5-bcaa-bc1332a405d0'))
delete from BallotName where Election_FK in ('fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '7aecf054-7dad-44a5-bcaa-bc1332a405d0')
delete from Party where Election_FK in ('fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '7aecf054-7dad-44a5-bcaa-bc1332a405d0')
delete from Election where id in ('fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '7aecf054-7dad-44a5-bcaa-bc1332a405d0')

delete from Person where id in (
'a581f294-04f7-4454-98cb-f8af6e4b87f0', '366dbd37-1b78-4558-8777-307f0e559620', 'b3a8db1d-17b4-4dcf-b0d6-5e39d7904237', '4ed8db8e-f9a9-488c-94ba-0de89a93c180','f1af5e59-1ef4-43fa-bdcc-116f317f7f0e',
'01c31088-fa22-434a-a71c-deab3684a6b3', '0e350d4e-d3eb-4fa8-b34f-3194ffa97bad', '5ebd8ca6-23c6-4520-b75d-998e92704ab3', '4f008b55-51de-412d-bde8-7a34903d67cf', 'e80b5090-bdb5-49bf-bb01-bf18a7758bef'
)

delete from Person where id in (
'aeb2f2d0-9dc5-4e4e-b1f3-efbc9603b523', '9f26f565-fef5-468f-89d3-c045e29a6abf', 'ba2f5cb3-6f9c-49e5-9586-4747abdfd342', '0f50609a-17ba-48ff-afe5-0acd37e36283', 'b4985928-ef2c-4c11-91ea-c87c5a2e1db7',
'546f45b2-4cab-4b54-8755-01f3442e0e23', '09af055f-66bb-4cab-a545-237bed512d8f', '7f858cac-f8f5-499c-ae75-f8d2878cab80', '2322ccfe-0ca9-4d6b-94f2-f3e7f038cc56', 'c6c5c31d-6dc2-4c93-9345-9b7c0f0936e4'
)

delete from Person where id in (
'cfb575f9-3446-4090-814b-b4b3b41aaaf2',
'82da810f-7522-4657-86a8-3d48f8b2e783',
'94c185cf-fdb2-4d3e-b5ca-4c897d0e2c68',
'8f8747bb-9771-4f87-b4b3-f4cc3daae34f'
)

delete from Person where id in (
'110c293d-f7ac-4122-a123-7f10c17ea840',
'2aa6ed86-f86c-4b9d-b952-1f1c90945905',
'7ef9d13e-d8f3-45e5-a4a5-eae63ab2f33f',
'22e8f94e-35e8-4e7f-964a-113a93543749'
)

-- Insert

-- Marvel
insert into Person (Id, FirstName, LastName, PublicID, BirthDate, BirthPlace, Attributes) values
('a581f294-04f7-4454-98cb-f8af6e4b87f0', 'Tony', 'Stark', 'STRTNY63C06Z404B', '1963-3-6', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('366dbd37-1b78-4558-8777-307f0e559620', 'Bruce', 'Banner', 'BNNBRC62E12Z404P', '1962-5-12', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('b3a8db1d-17b4-4dcf-b0d6-5e39d7904237', 'Thor', 'Odinson', 'DNSTHR62M09Z404C', '1962-8-9', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('4ed8db8e-f9a9-488c-94ba-0de89a93c180', 'Steve', 'Rogers', 'RGRSTV41C17Z404V', '1941-3-17', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('f1af5e59-1ef4-43fa-bdcc-116f317f7f0e', 'Stephen', 'Strange', 'STRSPH63L14Z404M', '1963-7-14', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('01c31088-fa22-434a-a71c-deab3684a6b3', 'Ororo', 'Munroe', 'MNRRRO64C50Z404V', '1964-3-10', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('0e350d4e-d3eb-4fa8-b34f-3194ffa97bad', 'Jean', 'Gray', 'GRYJNE63P41Z404B', '1963-9-1', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('5ebd8ca6-23c6-4520-b75d-998e92704ab3', 'Natasha', 'Romanoff', 'RMNNSH64D45Z404C', '1964-4-5', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('4f008b55-51de-412d-bde8-7a34903d67cf', 'Carol', 'Danvers', 'DNVCRL68C50Z404Y', '1968-3-10', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('e80b5090-bdb5-49bf-bb01-bf18a7758bef', 'Jennifer', 'Walters', 'WLTJNF80B44Z404A', '1980-2-4', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}')

-- DC
insert into Person (Id, FirstName, LastName, PublicID, BirthDate, BirthPlace, Attributes) values
('aeb2f2d0-9dc5-4e4e-b1f3-efbc9603b523', 'Bruce', 'Wayne', 'WYNBRC39E05Z404C', '1939-5-5', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('9f26f565-fef5-468f-89d3-c045e29a6abf', 'Barry', 'Allen', 'LLNBRY56R07Z404Z', '1956-10-7', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('ba2f5cb3-6f9c-49e5-9586-4747abdfd342', 'Clark', 'Kent', 'KNTCRK38H07Z404L', '1938-6-7', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('0f50609a-17ba-48ff-afe5-0acd37e36283', 'Hal', 'Jordan', 'JRDHLA59R09Z404U', '1959-10-9', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('b4985928-ef2c-4c11-91ea-c87c5a2e1db7', 'Arthur', 'Curry', 'CRRRHR41S03Z404X', '1941-11-3', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('546f45b2-4cab-4b54-8755-01f3442e0e23', 'Diana', 'Prince', 'PRNDNI41T57Z404E', '1941-12-17', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('09af055f-66bb-4cab-a545-237bed512d8f', 'Kara', 'Zor-El', 'ZRLKRA59E41Z404K', '1959-5-1', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('7f858cac-f8f5-499c-ae75-f8d2878cab80', 'Selina', 'Kyle', 'KYLSLN40D53Z404G', '1940-4-13', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('2322ccfe-0ca9-4d6b-94f2-f3e7f038cc56', 'Dinah', 'Drake', 'DRKDNH47M43Z404O', '1947-8-3', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('c6c5c31d-6dc2-4c93-9345-9b7c0f0936e4', 'Shiera', 'Sanders', 'SNDSHR40A46Z404A', '1940-1-6', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}')

-- Harry Potter
insert into Person (Id, FirstName, LastName, PublicID, BirthDate, BirthPlace, Attributes) values
('cfb575f9-3446-4090-814b-b4b3b41aaaf2', 'Harry', 'Potter', 'PTTHRY80L31Z114G', '1980-7-31', 'UK', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('82da810f-7522-4657-86a8-3d48f8b2e783', 'Hermione', 'Granger', 'GRNHMN79P59Z114Y', '1979-9-19', 'UK', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('94c185cf-fdb2-4d3e-b5ca-4c897d0e2c68', 'Ronald', 'Weasley', 'WSLRLD80C01Z114X', '1980-3-1', 'UK', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('8f8747bb-9771-4f87-b4b3-f4cc3daae34f', 'Pansy', 'Parkinson', 'PRKPSY80B47Z114S', '1980-2-7', 'UK', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}')

-- Star Wars
insert into Person (Id, FirstName, LastName, PublicID, BirthDate, BirthPlace, Attributes) values
('110c293d-f7ac-4122-a123-7f10c17ea840', 'Luke', 'Skywalker', 'SKYLKU51P25Z404Y', '1951-9-25', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('2aa6ed86-f86c-4b9d-b952-1f1c90945905', 'Han', 'Solo', 'SLOHNA42E10Z404W', '1942-5-10', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('7ef9d13e-d8f3-45e5-a4a5-eae63ab2f33f', 'Leia', 'Organa', 'RGNLEI56R61Z404I', '1956-10-21', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}'),
('22e8f94e-35e8-4e7f-964a-113a93543749', 'Mon', 'Mothma', 'MTHMNO33B53Z404J', '1933-2-13', 'USA', '{"Role":"Demo","CompanyId":null,"Login":null,"Mobile":null}')

-- Election

insert into Election (Id, Name, Description, Configuration, PollStartDate, PollEndDate, ElectorateListClosingDate, ElectionType_FK, PollingStationGroupId) values 
('fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'Elezione di test per le liste', 'Elezione di test per le liste', '{"Notes":null,"ValidityQuorum":0.1,"ValidityQuorumType":1,"ElectionQuorum":0.1,"ElectionQuorumType":1,"RoundQuorum":0,"RoundQuorumType":1,"WeightedVoters":false,"NumPreferences":1,"HasCandidates":true,"EligibleSeats":2,"CandidatesType":3,"IdentificationType":3,"SamplingRate":0,"NumPartyPreferences":1,"ActiveForStronglyAuthenticatedUsers":true,"NoNullVote":true}', @StartDate, @EndDate, @ClosingDate, '1E08193F-B8B2-4D28-9EE3-015349DB4D0C', '8c952746-001a-4b7c-9c16-19c5dfc974a4'),
('7aecf054-7dad-44a5-bcaa-bc1332a405d0', 'Elezione di test per candidati singoli', 'Elezione di test per candidati singoli', '{"Notes":null,"ValidityQuorum":0.1,"ValidityQuorumType":1,"ElectionQuorum":0.1,"ElectionQuorumType":1,"RoundQuorum":0,"RoundQuorumType":1,"WeightedVoters":false,"NumPreferences":1,"HasCandidates":true,"EligibleSeats":2,"CandidatesType":1,"IdentificationType":3,"SamplingRate":0,"NumPartyPreferences":0, "ActiveForStronglyAuthenticatedUsers": true, "NoNullVote": true }', @StartDate, @EndDate, @ClosingDate, '1E08193F-B8B2-4D28-9EE3-015349DB4D0C', '8c952746-001a-4b7c-9c16-19c5dfc974a4')

-- Commission

insert into PollingStationCommission (Id, Election_FK, Location, DigitalLocation, Description, President_FK, PollingStationGroupId) values
('1c2bdc6e-baba-4e67-9daa-32b8a7cae013', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', null, null, 'Commissione di seggio - test', null, '8c952746-001a-4b7c-9c16-19c5dfc974a4'),
('d3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', '7aecf054-7dad-44a5-bcaa-bc1332a405d0', null, null, 'Commissione di seggio - test', null, '8c952746-001a-4b7c-9c16-19c5dfc974a4')

insert into PollingStationCommissioner (Id, Person_FK, PollingStationCommission_FK) values
('99a56871-dc3c-48c8-aec0-3729a7c6f0e1', @PresidentID, '1c2bdc6e-baba-4e67-9daa-32b8a7cae013'),
('70d63842-c052-4cd8-9314-052bb5a9742f', @PresidentID, 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f'),
('a6504063-b034-4caa-8798-6bb8833fc4ec', @Member, '1c2bdc6e-baba-4e67-9daa-32b8a7cae013'),
('dd00a03c-ee30-4b62-bbde-34c52caf6481', @Member, 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f')

update PollingStationCommission set President_FK='99a56871-dc3c-48c8-aec0-3729a7c6f0e1' where Id='1c2bdc6e-baba-4e67-9daa-32b8a7cae013'
update PollingStationCommission set President_FK='70d63842-c052-4cd8-9314-052bb5a9742f' where Id='d3704fe5-ca37-4a0d-8bf8-4d0c517aa77f'

-- Candidates

insert into Party (Id, Name, Description, Election_FK, LogoUri) values
('d3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', 'Marvel', null, 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'https://logodownload.org/wp-content/uploads/2017/05/marvel-logo-0.png'),
('1c9a9a10-92f9-4bf9-8825-c828cc7274cf', 'DC', null, 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'https://logos-download.com/wp-content/uploads/2019/01/DC_Comics_Logo_stars.png')

insert into BallotName (Id, BallotNameLabel, Election_FK, Party_FK, IsCandidate, SequenceOrder) values
('61574ee7-1b22-4a6f-a28c-7393f45058b4', 'DC', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '1c9a9a10-92f9-4bf9-8825-c828cc7274cf', 0, 1),
('737b6e41-ee52-490f-b0fd-b81ab6d65892', 'DC - Bruce Wayne', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '1c9a9a10-92f9-4bf9-8825-c828cc7274cf', 1, 2),
('56bc8ebe-dc0a-4848-875e-5381d0f2ef1e', 'DC - Barry Allen', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '1c9a9a10-92f9-4bf9-8825-c828cc7274cf', 1, 3),
('651877e2-e614-43f2-8055-dda512802c45', 'DC - Clark Kent', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '1c9a9a10-92f9-4bf9-8825-c828cc7274cf', 1, 4),
('4d54d1d4-9271-4d24-8b3c-0921737ee7ff', 'DC - Hal Jordan', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '1c9a9a10-92f9-4bf9-8825-c828cc7274cf', 1, 5),
('db9b684c-9c88-4346-8850-978a37e46951', 'DC - Arthur Curry', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '1c9a9a10-92f9-4bf9-8825-c828cc7274cf', 1, 6),
('9a71007f-4356-4822-9243-c1bc7885cfd1', 'DC - Diana Prince', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '1c9a9a10-92f9-4bf9-8825-c828cc7274cf', 1, 7),
('21090dbf-e9bb-40ce-a8cc-51d09179be86', 'DC - Kara Zor-El', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '1c9a9a10-92f9-4bf9-8825-c828cc7274cf', 1, 8),
('7226fe44-47fe-4189-8999-745c47065b83', 'DC - Selina Kyle', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '1c9a9a10-92f9-4bf9-8825-c828cc7274cf', 1, 9),
('584f5824-4376-4e2b-9f79-027c0a7aad44', 'DC - Dinah Drake', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '1c9a9a10-92f9-4bf9-8825-c828cc7274cf', 1, 10),
('bb1d4858-c798-4f62-9386-d88a7f59e396', 'DC - Shiera Sanders', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', '1c9a9a10-92f9-4bf9-8825-c828cc7274cf', 1, 11),
('636660c0-846f-4f9f-8e74-54401ff14dc5', 'Marvel', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', 0, 12),
('bf5e1656-b46b-4ac5-bd06-de6ddefacf08', 'Marvel - Tony Stark', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', 1, 13),
('4defea9b-624f-45a8-b37d-d8270d534108', 'Marvel - Bruce Banner', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', 1, 14),
('ed318cd3-f42c-4f4b-98cc-66cef2e08761', 'Marvel - Thor Odinson', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', 1, 15),
('a0f16f7f-ceb3-4b9a-a0e5-17e9fdc44a4f', 'Marvel - Steve Rogers', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', 1, 16),
('2d73c7c2-0f22-4f83-ab3c-54b4e559f127', 'Marvel - Stephen Strange', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', 1, 17),
('0b265d28-e438-41e9-811f-e0260a5bba3c', 'Marvel - Ororo Munroe', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', 1, 18),
('8355bea1-42ab-4307-a138-91635f9fb32a', 'Marvel - Jean Gray', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', 1, 19),
('5937a3e1-dff3-466e-85e7-d0d5842b65f0', 'Marvel - Natasha Romanoff', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', 1, 20),
('f5a2588d-0dcb-419f-a580-5167c9e0c3d2', 'Marvel - Carol Danvers', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', 1, 21),
('10b9cd50-5a07-4008-b84b-9ecd1627b245', 'Marvel - Jennifer Walters', 'fd934b31-d03c-4541-94db-8dd5f4ef8ee8', 'd3704fe5-ca37-4a0d-8bf8-4d0c517aa77f', 1, 22)

insert into EligibleCandidate (Id, BallotName_FK, Person_FK) values 
('ea27e37e-2998-4cbb-a0cb-6f0ad46e6859', '737b6e41-ee52-490f-b0fd-b81ab6d65892', 'aeb2f2d0-9dc5-4e4e-b1f3-efbc9603b523'),
('a931a542-ad78-4d4a-a0e6-bd95c476d6d2', '56bc8ebe-dc0a-4848-875e-5381d0f2ef1e', '9f26f565-fef5-468f-89d3-c045e29a6abf'),
('811ce32e-075b-4f92-b34d-891230f6cb55', '651877e2-e614-43f2-8055-dda512802c45', 'ba2f5cb3-6f9c-49e5-9586-4747abdfd342'),
('0427831a-04c2-4d85-aab9-f1ab58be71c6', '4d54d1d4-9271-4d24-8b3c-0921737ee7ff', '0f50609a-17ba-48ff-afe5-0acd37e36283'),
('ccbfe787-ecd4-492c-8865-95798116c45a', 'db9b684c-9c88-4346-8850-978a37e46951', 'b4985928-ef2c-4c11-91ea-c87c5a2e1db7'),
('ef2909d8-67da-4d7b-8a6a-978c81e130b7', '9a71007f-4356-4822-9243-c1bc7885cfd1', '546f45b2-4cab-4b54-8755-01f3442e0e23'),
('a7e2ba5e-2245-4571-983f-275e5a482716', '21090dbf-e9bb-40ce-a8cc-51d09179be86', '09af055f-66bb-4cab-a545-237bed512d8f'),
('1596e5e8-e743-4155-b123-f6043ea0d7c7', '7226fe44-47fe-4189-8999-745c47065b83', '7f858cac-f8f5-499c-ae75-f8d2878cab80'),
('a3039b88-fbfe-4db8-aaa7-015ceb6721db', '584f5824-4376-4e2b-9f79-027c0a7aad44', '2322ccfe-0ca9-4d6b-94f2-f3e7f038cc56'),
('9904b1bf-3479-4d5b-8366-831ede669eee', 'bb1d4858-c798-4f62-9386-d88a7f59e396', 'c6c5c31d-6dc2-4c93-9345-9b7c0f0936e4'),
('92e47a11-1e3e-487a-8a5e-67189facf392', 'bf5e1656-b46b-4ac5-bd06-de6ddefacf08', 'a581f294-04f7-4454-98cb-f8af6e4b87f0'),
('a3afa929-256f-43df-80f9-08666e7b1d4a', '4defea9b-624f-45a8-b37d-d8270d534108', '366dbd37-1b78-4558-8777-307f0e559620'),
('d9638e0b-96c0-4640-bed7-e773a4a7befc', 'ed318cd3-f42c-4f4b-98cc-66cef2e08761', 'b3a8db1d-17b4-4dcf-b0d6-5e39d7904237'),
('93f79db1-fca9-4aee-b0e2-9dde20d87958', 'a0f16f7f-ceb3-4b9a-a0e5-17e9fdc44a4f', '4ed8db8e-f9a9-488c-94ba-0de89a93c180'),
('e7b7190e-2c00-4867-9d0c-d1b2b152b48f', '2d73c7c2-0f22-4f83-ab3c-54b4e559f127', 'f1af5e59-1ef4-43fa-bdcc-116f317f7f0e'),
('37e00aff-ebcc-478f-8665-1d084488dbc7', '0b265d28-e438-41e9-811f-e0260a5bba3c', '01c31088-fa22-434a-a71c-deab3684a6b3'),
('a583b053-a1c6-4e80-801f-015e5375ade1', '8355bea1-42ab-4307-a138-91635f9fb32a', '0e350d4e-d3eb-4fa8-b34f-3194ffa97bad'),
('052801f8-37cb-4c1d-90e9-ccf2f08d61bc', '5937a3e1-dff3-466e-85e7-d0d5842b65f0', '5ebd8ca6-23c6-4520-b75d-998e92704ab3'),
('1b14abe6-caaa-43c8-86d8-5c749dca5ed6', 'f5a2588d-0dcb-419f-a580-5167c9e0c3d2', '4f008b55-51de-412d-bde8-7a34903d67cf'),
('1c1c6fd4-0279-4eec-a0a9-8d78c3916fd1', '10b9cd50-5a07-4008-b84b-9ecd1627b245', 'e80b5090-bdb5-49bf-bb01-bf18a7758bef')

insert into BallotName (Id, BallotNameLabel, Election_FK, Party_FK, IsCandidate, SequenceOrder) values
('3d294fe7-5495-4f12-a08d-047101cd39f3', 'Harry Potter', '7aecf054-7dad-44a5-bcaa-bc1332a405d0', null, null, null),
('d4ade2df-6f88-4ba7-af88-781124b2ccdd', 'Hermione Granger', '7aecf054-7dad-44a5-bcaa-bc1332a405d0', null, null, null),
('03d1e5a0-5263-4e45-9fc9-4ae0086da98b', 'Ronald Weasley', '7aecf054-7dad-44a5-bcaa-bc1332a405d0', null, null, null),
('d91c52a2-6649-4b50-b328-e18e69d3a880', 'Pansy Parkinson', '7aecf054-7dad-44a5-bcaa-bc1332a405d0', null, null, null),
('f3559dfe-b332-41fd-b5dd-987d3673cc84', 'Luke Skywalker', '7aecf054-7dad-44a5-bcaa-bc1332a405d0', null, null, null),
('a8c5504a-a5b0-48f7-9eb8-1eef1d74878b', 'Han Solo', '7aecf054-7dad-44a5-bcaa-bc1332a405d0', null, null, null),
('7e7bad08-62b5-4cdd-8ed4-88b867c12786', 'Leia Organa', '7aecf054-7dad-44a5-bcaa-bc1332a405d0', null, null, null),
('ebc2119a-fb05-4808-9437-2f5ac2ad4f50', 'Mon Mothma', '7aecf054-7dad-44a5-bcaa-bc1332a405d0', null, null, null)

insert into EligibleCandidate (Id, BallotName_FK, Person_FK) values 
('1ad173d4-c079-447e-8f6f-ee18788dbd9a', '3d294fe7-5495-4f12-a08d-047101cd39f3', 'cfb575f9-3446-4090-814b-b4b3b41aaaf2'),
('bbf5b68a-911a-4686-b674-8e9662eafb89', 'd4ade2df-6f88-4ba7-af88-781124b2ccdd', '82da810f-7522-4657-86a8-3d48f8b2e783'),
('3c708701-6f73-4aaf-8d5c-34986f264357', '03d1e5a0-5263-4e45-9fc9-4ae0086da98b', '94c185cf-fdb2-4d3e-b5ca-4c897d0e2c68'),
('56cdbdfb-75f3-4ecb-a5cd-f3e0585817eb', 'd91c52a2-6649-4b50-b328-e18e69d3a880', '8f8747bb-9771-4f87-b4b3-f4cc3daae34f'),
('0476e0c3-7bcf-4050-bef1-b0a00cdb3149', 'f3559dfe-b332-41fd-b5dd-987d3673cc84', '110c293d-f7ac-4122-a123-7f10c17ea840'),
('11147d71-f7e1-4722-a822-cada68228a65', 'a8c5504a-a5b0-48f7-9eb8-1eef1d74878b', '2aa6ed86-f86c-4b9d-b952-1f1c90945905'),
('5ff9561a-076a-4d33-abd8-af72bbec5bd4', '7e7bad08-62b5-4cdd-8ed4-88b867c12786', '7ef9d13e-d8f3-45e5-a4a5-eae63ab2f33f'),
('57b07317-bce7-4115-8a2d-cae3cd9d0fea', 'ebc2119a-fb05-4808-9437-2f5ac2ad4f50', '22e8f94e-35e8-4e7f-964a-113a93543749')
