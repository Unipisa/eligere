using System;
using System.Collections.Generic;
using System.Text;

namespace ElectionGuardWebAPIClientTests
{
    public static class TestData
    {
        public static string description2 = @"
{""start_date"":null,""end_date"":null,""type"":null,""election_scope_id"":null,""parties"":null,""contact_information"":null,""name"":null,""candidates"":[{""object_id"":""d211c530-c324-4731-9c8e-01307243cbea"",""ballot_name"":{""text"":[{""value"":""GUEORGUIEV VLADIMIR SIMEONOV"",""language"":""it""}],""party_id"":""""}},{""object_id"":""0f0f8f4b-aaf2-41f1-8427-027958d023dd"",""ballot_name"":{""text"":[{""value"":""VICARI ETTORE"",""language"":""it""}],""party_id"":""""}},{""object_id"":""b577b4f5-4736-4a34-b307-03daa952709d"",""ballot_name"":{""text"":[{""value"":""RICCI LAURA EMILIA MARIA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""8d085f70-0c33-46c0-aec0-052637caa329"",""ballot_name"":{""text"":[{""value"":""DANELUTTO MARCO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""67f0fd80-88a1-464f-87fb-0cee4cc9adba"",""ballot_name"":{""text"":[{""value"":""BROGI ANTONIO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""e59fa7d1-6109-40cd-ab55-0d636785391e"",""ballot_name\
"":{""text"":[{""value"":""MARTELLI BRUNO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""6ece7ec5-f717-4466-8c84-1137b740f069"",""ballot_name"":{""text"":[{""value"":""LOMBARDO DAVIDE"",""language"":""it""}],""party_id"":""""}},{""object_id"":""26b3c18f-c9f7-45cb-b04d-166dd964c4f0"",""ballot_name"":{""text"":[{""value"":""DI NASSO MAURO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""13401d69-e506-4fa5-9e81-16defc8ebf9f"",""ballot_name"":{""text"":[{""value"":""PISANTI NADIA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""29be0f31-2254-49f2-98dc-19ebd34107e7"",""ballot_name"":{""text"":[{""value"":""TALPO MATTIA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""9edab0fe-493f-46ac-b9dc-1e008325519b"",""ballot_name"":{""text"":[{""value"":""SCUTELLA\\u0027 MARIA GRAZIA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""ccb1671c-0976-4af7-a7bb-200aacf20f5f"",""ballot_name"":{""text"":[{""value"":""NOVAGA MATTEO"",""language"":""it""}],""party_id"":""""}}
,{""object_id"":""47182783-1801-4603-8486-23a27bfecbde"",""ballot_name"":{""text"":[{""value"":""PAOLINI EMANUELE"",""language"":""it""}],""party_id"":""""}},{""object_id"":""4dcf2de8-73c9-42f7-bcc9-24b0988a475d"",""ballot_name"":{""text"":[{""value"":""CORRADINI ANDREA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""614a1fcf-2866-4d11-a487-31e42fdbfff3"",""ballot_name"":{""text"":[{""value"":""COGLIATI ALBERTO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""c2440c86-07b4-46ab-8d45-39860341b178"",""ballot_name"":{""text"":[{""value"":""DI MARTINO PIETRO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""f157a042-9b9d-4f9c-913b-3a038e4e2d36"",""ballot_name"":{""text"":[{""value"":""PEDRESCHI DINO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""40b525b3-b85a-4dbb-a8cc-3af708128b8b"",""ballot_name"":{""text"":[{""value"":""POLONI FEDERICO GIOVANNI"",""language"":""it""}],""party_id"":""""}},{""object_id"":""874f8ea6-c931-4b9f-8297-3b317b3840ee"",""ballot_name"":{""
text"":[{""value"":""TREVISAN DARIO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""d89db868-ed4f-4419-9714-408b02bfe823"",""ballot_name"":{""text"":[{""value"":""FRANCIOSI MARCO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""55e90938-a0dd-4d50-97fa-4281fb8d1e71"",""ballot_name"":{""text"":[{""value"":""NOCCETTI SABRINA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""677bb8c7-595f-45bb-809c-45329651edff"",""ballot_name"":{""text"":[{""value"":""BERSELLI LUIGI CARLO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""9bf2b6ad-0d8f-4ea3-aade-49a024057759"",""ballot_name"":{""text"":[{""value"":""MEINI BEATRICE"",""language"":""it""}],""party_id"":""""}},{""object_id"":""6457ab4a-89f5-40fb-b62e-4f10ae55a709"",""ballot_name"":{""text"":[{""value"":""GALLI LAURA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""7d768d89-e6e7-4946-87cf-4fe9bc48cff8"",""ballot_name"":{""text"":[{""value"":""GADDUCCI FABIO"",""language"":""it""}],""party_id"":""""}},{""object
_id"":""a505d45c-5de5-4980-91da-50e3ab96b715"",""ballot_name"":{""text"":[{""value"":""GHIMENTI MARCO GIPO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""fe291611-13cb-431e-8f27-54261ac50eec"",""ballot_name"":{""text"":[{""value"":""FERRARI GIAN-LUIGI"",""language"":""it""}],""party_id"":""""}},{""object_id"":""914997a8-b2de-406e-a9ac-5456dd7940bf"",""ballot_name"":{""text"":[{""value"":""LISCA PAOLO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""a54da3f6-50f7-430b-9e14-5a95781b0849"",""ballot_name"":{""text"":[{""value"":""PASSACANTANDO MAURO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""05e03343-668e-48ea-836b-5ae498618019"",""ballot_name"":{""text"":[{""value"":""SALVETTI MARIO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""ff27d21c-81e0-4c62-8e4f-5c4a9b9da4c2"",""ballot_name"":{""text"":[{""value"":""BERARDUCCI ALESSANDRO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""3801a1e5-6342-432b-862a-5ce855572a17"",""ballot_name"":{""text"":[{""
value"":""BIGI GIANCARLO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""11795dfb-d99a-4a8b-a3ff-5ed8d446c4f1"",""ballot_name"":{""text"":[{""value"":""BRUNI ROBERTO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""e9f02f7b-5d38-492e-adfe-60e7a82856c8"",""ballot_name"":{""text"":[{""value"":""ROMITO MARCO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""b02d829b-eba0-4a89-a805-622688054c21"",""ballot_name"":{""text"":[{""value"":""PAPPALARDO MASSIMO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""97570864-1947-48d8-aa1a-626abc5c0a71"",""ballot_name"":{""text"":[{""value"":""GOBBINO MASSIMO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""ab29aa1e-ec58-4f49-97bc-64829498380f"",""ballot_name"":{""text"":[{""value"":""SIRBU ALINA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""13a4a5f8-b208-46e9-94a2-65dd49bcc01f"",""ballot_name"":{""text"":[{""value"":""MONREALE ANNA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""2ed56442-0a
7a-4627-9750-68ad3e197747"",""ballot_name"":{""text"":[{""value"":""ABATE MARCO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""b8d29548-7d7e-45c4-af5e-6f8a4dc1f71d"",""ballot_name"":{""text"":[{""value"":""GROSSI ROBERTO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""406ec522-b594-4481-94d9-6fbb4cef1c4a"",""ballot_name"":{""text"":[{""value"":""BANDINI ANDREA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""2a2453b6-ec40-4834-b70f-769825ec675a"",""ballot_name"":{""text"":[{""value"":""GHISI MARINA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""45e627a3-9dc8-4256-b4e5-772ece091ecf"",""ballot_name"":{""text"":[{""value"":""ROSONE GIOVANNA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""ebc897ea-e874-4342-ac2c-79a81041daa1"",""ballot_name"":{""text"":[{""value"":""PARDINI RITA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""5ada52ff-8c5b-4ed8-aefc-7a362793f2bd"",""ballot_name"":{""text"":[{""value"":""MELANI VALERIO"",""language"":""i
t""}],""party_id"":""""}},{""object_id"":""43bcde8c-4617-4988-bd69-7c14843ee2d0"",""ballot_name"":{""text"":[{""value"":""GORI ROBERTA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""11ceb1a9-e9a9-448c-b5ce-7e41ff89bcb7"",""ballot_name"":{""text"":[{""value"":""GAIFFI GIOVANNI"",""language"":""it""}],""party_id"":""""}},{""object_id"":""2410621b-92a9-4ef2-8bc2-7eda0019bf66"",""ballot_name"":{""text"":[{""value"":""MASTROENI GIANDOMENICO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""d07e1519-25b8-4b45-90ed-8859b9aea7f7"",""ballot_name"":{""text"":[{""value"":""PAGANELLI FEDERICA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""bb94aaff-ea0b-452a-963b-88e2f30a80f6"",""ballot_name"":{""text"":[{""value"":""PANDOLFI LUCA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""5c910438-fc94-45b7-908d-8a4f6a48b07e"",""ballot_name"":{""text"":[{""value"":""MICHELI ALESSIO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""090b3352-4e55-48b0-88a6-9095c420efb3"",""
ballot_name"":{""text"":[{""value"":""GHELLI GIORGIO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""8215c95a-8187-4f4f-bff6-92716debc344"",""ballot_name"":{""text"":[{""value"":""CHIODAROLI ELISABETTA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""db447a79-b6c9-4c5b-903c-93610ce37294"",""ballot_name"":{""text"":[{""value"":""CINI MARCO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""843a9468-86d3-47ef-b960-94544baf89d1"",""ballot_name"":{""text"":[{""value"":""GEMIGNANI LUCA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""d21587b5-ce55-4096-8963-9acc5aa3b824"",""ballot_name"":{""text"":[{""value"":""GRONCHI GIOVANNI FEDERICO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""ccda058b-290b-41be-b0d2-9c5a071adfc0"",""ballot_name"":{""text"":[{""value"":""GUIDOTTI RICCARDO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""91626a50-1a83-45b9-acb5-9e9803a12e5a"",""ballot_name"":{""text"":[{""value"":""BONCHI FILIPPO"",""language"":""it""}],\
""party_id"":""""}},{""object_id"":""9a62be31-780d-474c-9775-a0aaad5cd984"",""ballot_name"":{""text"":[{""value"":""RUGGIERI SALVATORE"",""language"":""it""}],""party_id"":""""}},{""object_id"":""26f8dfb4-acfc-4d05-90ea-a11d86d3698d"",""ballot_name"":{""text"":[{""value"":""GALLICCHIO CLAUDIO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""c4ff5b89-e4b7-4481-a274-a15b03459ac9"",""ballot_name"":{""text"":[{""value"":""MARO\\u0027 STEFANO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""ee41c1d1-2bab-46ef-98fc-a252964271d2"",""ballot_name"":{""text"":[{""value"":""SEMINI LAURA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""260f90b6-8d11-4736-b57c-a2a2a60aa95f"",""ballot_name"":{""text"":[{""value"":""PETRONIO CARLO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""19a45c95-3318-4104-8180-a3356fb67a07"",""ballot_name"":{""text"":[{""value"":""FERRAGINA PAOLO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""7510e577-15b8-43e9-9dcb-a3a47bc0a552"",""ballot
_name"":{""text"":[{""value"":""BUTTAZZO GIUSEPPE"",""language"":""it""}],""party_id"":""""}},{""object_id"":""2ded8684-3735-4846-8239-abb816748ca0"",""ballot_name"":{""text"":[{""value"":""MILAZZO PAOLO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""501ef6f7-4398-4a2e-94a8-ad122830727d"",""ballot_name"":{""text"":[{""value"":""DEL CORSO GIANNA MARIA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""ae53572e-2f8e-48d6-8344-afc96b44b129"",""ballot_name"":{""text"":[{""value"":""DEL CORSO ILARIA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""217ab58e-33cb-45b3-9b77-b4590b0e042d"",""ballot_name"":{""text"":[{""value"":""BODEI CHIARA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""74a73e91-38e3-479c-b50e-b7db6cfb35c7"",""ballot_name"":{""text"":[{""value"":""VISCIGLIA NICOLA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""e4dd129e-cbec-4b24-a9b5-be6c0d5c9212"",""ballot_name"":{""text"":[{""value"":""BENEDETTI RICCARDO"",""language"":""it""}],""party_
id"":""""}},{""object_id"":""0c385840-8c0d-4c94-aebd-c2a921cfa761"",""ballot_name"":{""text"":[{""value"":""BACCAGLINI-FRANK ANNA ETHELWYN"",""language"":""it""}],""party_id"":""""}},{""object_id"":""c0ba68c7-4bda-4169-a866-c357ce8c8df9"",""ballot_name"":{""text"":[{""value"":""BOITO PAOLA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""f716f9cc-c088-4c90-ba01-c36aa0b4a023"",""ballot_name"":{""text"":[{""value"":""GUIDI BARBARA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""38b10b7e-8e65-4bb8-8aaa-cf2bcc5bec8f"",""ballot_name"":{""text"":[{""value"":""MENCAGLI GABRIELE"",""language"":""it""}],""party_id"":""""}},{""object_id"":""15f7a782-a340-4f24-b0d7-d394df66fa68"",""ballot_name"":{""text"":[{""value"":""SZAMUELY TAMAS"",""language"":""it""}],""party_id"":""""}},{""object_id"":""011cfbf7-cd1a-404d-aa20-d433562793a0"",""ballot_name"":{""text"":[{""value"":""MAZZEI DANIELE"",""language"":""it""}],""party_id"":""""}},{""object_id"":""b886ffaf-f0b8-47f4-9f4f-d6d4abe03e7e"",""ballot_nam
e"":{""text"":[{""value"":""CHESSA STEFANO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""583cb5b3-d058-4a71-ad37-d85921111c74"",""ballot_name"":{""text"":[{""value"":""MAGNANI VALENTINO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""ac436317-33e1-48f8-8332-dc232a4b0bc7"",""ballot_name"":{""text"":[{""value"":""GERVASI VINCENZO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""95d5b319-1329-47a0-8486-df6516922fc7"",""ballot_name"":{""text"":[{""value"":""FRIGERIO ROBERTO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""2ca99839-1cec-41d2-a952-e109f2be6241"",""ballot_name"":{""text"":[{""value"":""MAMINO MARCELLO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""76c5c0ff-8add-4c76-81fc-e4db5427396e"",""ballot_name"":{""text"":[{""value"":""VENTURINI ROSSANO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""f9711b75-0085-4927-8864-e544629d457e"",""ballot_name"":{""text"":[{""value"":""DEL CORSO ILARIA"",""language"":""it""}],""party_id"":""
""}},{""object_id"":""e60aa534-49c3-4fb4-b6f0-e93bf5beae41"",""ballot_name"":{""text"":[{""value"":""BARSANTI MICHELE"",""language"":""it""}],""party_id"":""""}},{""object_id"":""bc933194-112a-476a-a39d-e9a03fac6a92"",""ballot_name"":{""text"":[{""value"":""ACETO LIDIA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""3dc93471-491d-41e1-aaeb-ea129ee46966"",""ballot_name"":{""text"":[{""value"":""TOMMEI GIACOMO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""abfc0262-4a62-45d2-8568-ed218e04d040"",""ballot_name"":{""text"":[{""value"":""PRATELLI ALDO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""8cc2696c-7e7a-418d-9c7d-edd83d19463d"",""ballot_name"":{""text"":[{""value"":""SALA FRANCESCO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""345b2981-d2f1-4174-a422-ee6b8b943148"",""ballot_name"":{""text"":[{""value"":""ROBOL LEONARDO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""e4a3c84c-3cd8-44f7-8112-eee684e1d257"",""ballot_name"":{""text"":[{""value\
"":""MAFFEI ANDREA"",""language"":""it""}],""party_id"":""""}},{""object_id"":""e8daa994-8733-491c-989e-f1921ae909e4"",""ballot_name"":{""text"":[{""value"":""BACCIU DAVIDE"",""language"":""it""}],""party_id"":""""}},{""object_id"":""2601cdd6-03c5-4444-bd3e-f96065e30013"",""ballot_name"":{""text"":[{""value"":""BONANNO CLAUDIO"",""language"":""it""}],""party_id"":""""}},{""object_id"":""4cdce580-1f0d-43f7-8fc5-ff191e846004"",""ballot_name"":{""text"":[{""value"":""ALBERTI GIOVANNI"",""language"":""it""}],""party_id"":""""}},{""object_id"":""8acd088d-b23f-4595-b28f-ff5d39bc316f"",""ballot_name"":{""text"":[{""value"":""BIGI GIANCARLO"",""language"":""it""}],""party_id"":""""}}],""geopolitical_units"":null,""contests"":[{""@type"":null,""object_id"":""a3268b82-bed5-4880-a092-625da4175d9b"",""sequence_order"":0,""ballot_selections"":[{""object_id"":""bb94aaff-ea0b-452a-963b-88e2f30a80f6"",""sequence_order"":0,""candidate_id"":""bb94aaff-ea0b-452a-963b-88e2f30a80f6""}],""ballot_title"":null,""ballot_subtitle"":null
,""vote_variation"":null,""electoral_district_id"":""https://eligere.unipi.it"",""name"":""Senato Accademico - Direttore Settore Culturale 1"",""primary_party_ids"":null,""number_elected"":1,""votes_allowed"":1,""extensions"":{""HasCandidates"":""True"",""PollStartDate"":""20/11/2020 09:00:00"",""PollEndDate"":""26/11/2020 18:00:00""}},{""@type"":null,""object_id"":""3b0ef483-6f89-4b70-8493-636208357e9b"",""sequence_order"":1,""ballot_selections"":[{""object_id"":""3801a1e5-6342-432b-862a-5ce855572a17"",""sequence_order"":0,""candidate_id"":""3801a1e5-6342-432b-862a-5ce855572a17""},{""object_id"":""f9711b75-0085-4927-8864-e544629d457e"",""sequence_order"":1,""candidate_id"":""f9711b75-0085-4927-8864-e544629d457e""},{""object_id"":""0f0f8f4b-aaf2-41f1-8427-027958d023dd"",""sequence_order"":2,""candidate_id"":""0f0f8f4b-aaf2-41f1-8427-027958d023dd""}],""ballot_title"":null,""ballot_subtitle"":null,""vote_variation"":null,""electoral_district_id"":""https://eligere.unipi.it"",""name"":""Senato Accademico - Rappre
sentanti Settore Culturale 1"",""primary_party_ids"":null,""number_elected"":2,""votes_allowed"":1,""extensions"":{""HasCandidates"":""True"",""PollStartDate"":""20/11/2020 09:00:00"",""PollEndDate"":""26/11/2020 18:00:00""}},{""@type"":null,""object_id"":""1363ea36-f6d6-4a22-9feb-c86e95a976fc"",""sequence_order"":2,""ballot_selections"":[{""object_id"":""db447a79-b6c9-4c5b-903c-93610ce37294"",""sequence_order"":0,""candidate_id"":""db447a79-b6c9-4c5b-903c-93610ce37294""},{""object_id"":""55e90938-a0dd-4d50-97fa-4281fb8d1e71"",""sequence_order"":1,""candidate_id"":""55e90938-a0dd-4d50-97fa-4281fb8d1e71""}],""ballot_title"":null,""ballot_subtitle"":null,""vote_variation"":null,""electoral_district_id"":""https://eligere.unipi.it"",""name"":""Collegio di disciplina - Ricercatori a tempo indeterminato"",""primary_party_ids"":null,""number_elected"":3,""votes_allowed"":1,""extensions"":{""HasCandidates"":""True"",""PollStartDate"":""20/11/2020 09:00:00"",""PollEndDate"":""26/11/2020 18:00:00""}},{""@type"":null,""
object_id"":""374f096d-2e38-4264-a1da-fe96d3856a24"",""sequence_order"":3,""ballot_selections"":[{""object_id"":""2ed56442-0a7a-4627-9750-68ad3e197747"",""sequence_order"":0,""candidate_id"":""2ed56442-0a7a-4627-9750-68ad3e197747""},{""object_id"":""bc933194-112a-476a-a39d-e9a03fac6a92"",""sequence_order"":1,""candidate_id"":""bc933194-112a-476a-a39d-e9a03fac6a92""},{""object_id"":""4cdce580-1f0d-43f7-8fc5-ff191e846004"",""sequence_order"":2,""candidate_id"":""4cdce580-1f0d-43f7-8fc5-ff191e846004""},{""object_id"":""0c385840-8c0d-4c94-aebd-c2a921cfa761"",""sequence_order"":3,""candidate_id"":""0c385840-8c0d-4c94-aebd-c2a921cfa761""},{""object_id"":""e8daa994-8733-491c-989e-f1921ae909e4"",""sequence_order"":4,""candidate_id"":""e8daa994-8733-491c-989e-f1921ae909e4""},{""object_id"":""406ec522-b594-4481-94d9-6fbb4cef1c4a"",""sequence_order"":5,""candidate_id"":""406ec522-b594-4481-94d9-6fbb4cef1c4a""},{""object_id"":""e60aa534-49c3-4fb4-b6f0-e93bf5beae41"",""sequence_order"":6,""candidate_id"":""e60aa534-49c3-4f
b4-b6f0-e93bf5beae41""},{""object_id"":""e4dd129e-cbec-4b24-a9b5-be6c0d5c9212"",""sequence_order"":7,""candidate_id"":""e4dd129e-cbec-4b24-a9b5-be6c0d5c9212""},{""object_id"":""ff27d21c-81e0-4c62-8e4f-5c4a9b9da4c2"",""sequence_order"":8,""candidate_id"":""ff27d21c-81e0-4c62-8e4f-5c4a9b9da4c2""},{""object_id"":""677bb8c7-595f-45bb-809c-45329651edff"",""sequence_order"":9,""candidate_id"":""677bb8c7-595f-45bb-809c-45329651edff""},{""object_id"":""8acd088d-b23f-4595-b28f-ff5d39bc316f"",""sequence_order"":10,""candidate_id"":""8acd088d-b23f-4595-b28f-ff5d39bc316f""},{""object_id"":""217ab58e-33cb-45b3-9b77-b4590b0e042d"",""sequence_order"":11,""candidate_id"":""217ab58e-33cb-45b3-9b77-b4590b0e042d""},{""object_id"":""c0ba68c7-4bda-4169-a866-c357ce8c8df9"",""sequence_order"":12,""candidate_id"":""c0ba68c7-4bda-4169-a866-c357ce8c8df9""},{""object_id"":""2601cdd6-03c5-4444-bd3e-f96065e30013"",""sequence_order"":13,""candidate_id"":""2601cdd6-03c5-4444-bd3e-f96065e30013""},{""object_id"":""91626a50-1a83-45b9-acb5-9e98
03a12e5a"",""sequence_order"":14,""candidate_id"":""91626a50-1a83-45b9-acb5-9e9803a12e5a""},{""object_id"":""67f0fd80-88a1-464f-87fb-0cee4cc9adba"",""sequence_order"":15,""candidate_id"":""67f0fd80-88a1-464f-87fb-0cee4cc9adba""},{""object_id"":""11795dfb-d99a-4a8b-a3ff-5ed8d446c4f1"",""sequence_order"":16,""candidate_id"":""11795dfb-d99a-4a8b-a3ff-5ed8d446c4f1""},{""object_id"":""7510e577-15b8-43e9-9dcb-a3a47bc0a552"",""sequence_order"":17,""candidate_id"":""7510e577-15b8-43e9-9dcb-a3a47bc0a552""},{""object_id"":""b886ffaf-f0b8-47f4-9f4f-d6d4abe03e7e"",""sequence_order"":18,""candidate_id"":""b886ffaf-f0b8-47f4-9f4f-d6d4abe03e7e""},{""object_id"":""8215c95a-8187-4f4f-bff6-92716debc344"",""sequence_order"":19,""candidate_id"":""8215c95a-8187-4f4f-bff6-92716debc344""},{""object_id"":""614a1fcf-2866-4d11-a487-31e42fdbfff3"",""sequence_order"":20,""candidate_id"":""614a1fcf-2866-4d11-a487-31e42fdbfff3""},{""object_id"":""4dcf2de8-73c9-42f7-bcc9-24b0988a475d"",""sequence_order"":21,""candidate_id"":""4dcf2de8-73c9-
42f7-bcc9-24b0988a475d""},{""object_id"":""8d085f70-0c33-46c0-aec0-052637caa329"",""sequence_order"":22,""candidate_id"":""8d085f70-0c33-46c0-aec0-052637caa329""},{""object_id"":""501ef6f7-4398-4a2e-94a8-ad122830727d"",""sequence_order"":23,""candidate_id"":""501ef6f7-4398-4a2e-94a8-ad122830727d""},{""object_id"":""ae53572e-2f8e-48d6-8344-afc96b44b129"",""sequence_order"":24,""candidate_id"":""ae53572e-2f8e-48d6-8344-afc96b44b129""},{""object_id"":""c2440c86-07b4-46ab-8d45-39860341b178"",""sequence_order"":25,""candidate_id"":""c2440c86-07b4-46ab-8d45-39860341b178""},{""object_id"":""26b3c18f-c9f7-45cb-b04d-166dd964c4f0"",""sequence_order"":26,""candidate_id"":""26b3c18f-c9f7-45cb-b04d-166dd964c4f0""},{""object_id"":""19a45c95-3318-4104-8180-a3356fb67a07"",""sequence_order"":27,""candidate_id"":""19a45c95-3318-4104-8180-a3356fb67a07""},{""object_id"":""fe291611-13cb-431e-8f27-54261ac50eec"",""sequence_order"":28,""candidate_id"":""fe291611-13cb-431e-8f27-54261ac50eec""},{""object_id"":""d89db868-ed4f-4419-9714
-408b02bfe823"",""sequence_order"":29,""candidate_id"":""d89db868-ed4f-4419-9714-408b02bfe823""},{""object_id"":""95d5b319-1329-47a0-8486-df6516922fc7"",""sequence_order"":30,""candidate_id"":""95d5b319-1329-47a0-8486-df6516922fc7""},{""object_id"":""7d768d89-e6e7-4946-87cf-4fe9bc48cff8"",""sequence_order"":31,""candidate_id"":""7d768d89-e6e7-4946-87cf-4fe9bc48cff8""},{""object_id"":""11ceb1a9-e9a9-448c-b5ce-7e41ff89bcb7"",""sequence_order"":32,""candidate_id"":""11ceb1a9-e9a9-448c-b5ce-7e41ff89bcb7""},{""object_id"":""6457ab4a-89f5-40fb-b62e-4f10ae55a709"",""sequence_order"":33,""candidate_id"":""6457ab4a-89f5-40fb-b62e-4f10ae55a709""},{""object_id"":""26f8dfb4-acfc-4d05-90ea-a11d86d3698d"",""sequence_order"":34,""candidate_id"":""26f8dfb4-acfc-4d05-90ea-a11d86d3698d""},{""object_id"":""843a9468-86d3-47ef-b960-94544baf89d1"",""sequence_order"":35,""candidate_id"":""843a9468-86d3-47ef-b960-94544baf89d1""},{""object_id"":""ac436317-33e1-48f8-8332-dc232a4b0bc7"",""sequence_order"":36,""candidate_id"":""ac436317-
33e1-48f8-8332-dc232a4b0bc7""},{""object_id"":""090b3352-4e55-48b0-88a6-9095c420efb3"",""sequence_order"":37,""candidate_id"":""090b3352-4e55-48b0-88a6-9095c420efb3""},{""object_id"":""a505d45c-5de5-4980-91da-50e3ab96b715"",""sequence_order"":38,""candidate_id"":""a505d45c-5de5-4980-91da-50e3ab96b715""},{""object_id"":""2a2453b6-ec40-4834-b70f-769825ec675a"",""sequence_order"":39,""candidate_id"":""2a2453b6-ec40-4834-b70f-769825ec675a""},{""object_id"":""97570864-1947-48d8-aa1a-626abc5c0a71"",""sequence_order"":40,""candidate_id"":""97570864-1947-48d8-aa1a-626abc5c0a71""},{""object_id"":""43bcde8c-4617-4988-bd69-7c14843ee2d0"",""sequence_order"":41,""candidate_id"":""43bcde8c-4617-4988-bd69-7c14843ee2d0""},{""object_id"":""d21587b5-ce55-4096-8963-9acc5aa3b824"",""sequence_order"":42,""candidate_id"":""d21587b5-ce55-4096-8963-9acc5aa3b824""},{""object_id"":""b8d29548-7d7e-45c4-af5e-6f8a4dc1f71d"",""sequence_order"":43,""candidate_id"":""b8d29548-7d7e-45c4-af5e-6f8a4dc1f71d""},{""object_id"":""d211c530-c324-4731
-9c8e-01307243cbea"",""sequence_order"":44,""candidate_id"":""d211c530-c324-4731-9c8e-01307243cbea""},{""object_id"":""f716f9cc-c088-4c90-ba01-c36aa0b4a023"",""sequence_order"":45,""candidate_id"":""f716f9cc-c088-4c90-ba01-c36aa0b4a023""},{""object_id"":""ccda058b-290b-41be-b0d2-9c5a071adfc0"",""sequence_order"":46,""candidate_id"":""ccda058b-290b-41be-b0d2-9c5a071adfc0""},{""object_id"":""914997a8-b2de-406e-a9ac-5456dd7940bf"",""sequence_order"":47,""candidate_id"":""914997a8-b2de-406e-a9ac-5456dd7940bf""},{""object_id"":""6ece7ec5-f717-4466-8c84-1137b740f069"",""sequence_order"":48,""candidate_id"":""6ece7ec5-f717-4466-8c84-1137b740f069""},{""object_id"":""e4a3c84c-3cd8-44f7-8112-eee684e1d257"",""sequence_order"":49,""candidate_id"":""e4a3c84c-3cd8-44f7-8112-eee684e1d257""},{""object_id"":""583cb5b3-d058-4a71-ad37-d85921111c74"",""sequence_order"":50,""candidate_id"":""583cb5b3-d058-4a71-ad37-d85921111c74""},{""object_id"":""2ca99839-1cec-41d2-a952-e109f2be6241"",""sequence_order"":51,""candidate_id"":""2ca9
9839-1cec-41d2-a952-e109f2be6241""},{""object_id"":""c4ff5b89-e4b7-4481-a274-a15b03459ac9"",""sequence_order"":52,""candidate_id"":""c4ff5b89-e4b7-4481-a274-a15b03459ac9""},{""object_id"":""e59fa7d1-6109-40cd-ab55-0d636785391e"",""sequence_order"":53,""candidate_id"":""e59fa7d1-6109-40cd-ab55-0d636785391e""},{""object_id"":""2410621b-92a9-4ef2-8bc2-7eda0019bf66"",""sequence_order"":54,""candidate_id"":""2410621b-92a9-4ef2-8bc2-7eda0019bf66""},{""object_id"":""011cfbf7-cd1a-404d-aa20-d433562793a0"",""sequence_order"":55,""candidate_id"":""011cfbf7-cd1a-404d-aa20-d433562793a0""},{""object_id"":""9bf2b6ad-0d8f-4ea3-aade-49a024057759"",""sequence_order"":56,""candidate_id"":""9bf2b6ad-0d8f-4ea3-aade-49a024057759""},{""object_id"":""5ada52ff-8c5b-4ed8-aefc-7a362793f2bd"",""sequence_order"":57,""candidate_id"":""5ada52ff-8c5b-4ed8-aefc-7a362793f2bd""},{""object_id"":""38b10b7e-8e65-4bb8-8aaa-cf2bcc5bec8f"",""sequence_order"":58,""candidate_id"":""38b10b7e-8e65-4bb8-8aaa-cf2bcc5bec8f""},{""object_id"":""5c910438-fc94
-45b7-908d-8a4f6a48b07e"",""sequence_order"":59,""candidate_id"":""5c910438-fc94-45b7-908d-8a4f6a48b07e""},{""object_id"":""2ded8684-3735-4846-8239-abb816748ca0"",""sequence_order"":60,""candidate_id"":""2ded8684-3735-4846-8239-abb816748ca0""},{""object_id"":""13a4a5f8-b208-46e9-94a2-65dd49bcc01f"",""sequence_order"":61,""candidate_id"":""13a4a5f8-b208-46e9-94a2-65dd49bcc01f""},{""object_id"":""ccb1671c-0976-4af7-a7bb-200aacf20f5f"",""sequence_order"":62,""candidate_id"":""ccb1671c-0976-4af7-a7bb-200aacf20f5f""},{""object_id"":""d07e1519-25b8-4b45-90ed-8859b9aea7f7"",""sequence_order"":63,""candidate_id"":""d07e1519-25b8-4b45-90ed-8859b9aea7f7""},{""object_id"":""47182783-1801-4603-8486-23a27bfecbde"",""sequence_order"":64,""candidate_id"":""47182783-1801-4603-8486-23a27bfecbde""},{""object_id"":""b02d829b-eba0-4a89-a805-622688054c21"",""sequence_order"":65,""candidate_id"":""b02d829b-eba0-4a89-a805-622688054c21""},{""object_id"":""ebc897ea-e874-4342-ac2c-79a81041daa1"",""sequence_order"":66,""candidate_id"":\
""ebc897ea-e874-4342-ac2c-79a81041daa1""},{""object_id"":""a54da3f6-50f7-430b-9e14-5a95781b0849"",""sequence_order"":67,""candidate_id"":""a54da3f6-50f7-430b-9e14-5a95781b0849""},{""object_id"":""f157a042-9b9d-4f9c-913b-3a038e4e2d36"",""sequence_order"":68,""candidate_id"":""f157a042-9b9d-4f9c-913b-3a038e4e2d36""},{""object_id"":""260f90b6-8d11-4736-b57c-a2a2a60aa95f"",""sequence_order"":69,""candidate_id"":""260f90b6-8d11-4736-b57c-a2a2a60aa95f""},{""object_id"":""13401d69-e506-4fa5-9e81-16defc8ebf9f"",""sequence_order"":70,""candidate_id"":""13401d69-e506-4fa5-9e81-16defc8ebf9f""},{""object_id"":""40b525b3-b85a-4dbb-a8cc-3af708128b8b"",""sequence_order"":71,""candidate_id"":""40b525b3-b85a-4dbb-a8cc-3af708128b8b""},{""object_id"":""abfc0262-4a62-45d2-8568-ed218e04d040"",""sequence_order"":72,""candidate_id"":""abfc0262-4a62-45d2-8568-ed218e04d040""},{""object_id"":""b577b4f5-4736-4a34-b307-03daa952709d"",""sequence_order"":73,""candidate_id"":""b577b4f5-4736-4a34-b307-03daa952709d""},{""object_id"":""345b2981
-d2f1-4174-a422-ee6b8b943148"",""sequence_order"":74,""candidate_id"":""345b2981-d2f1-4174-a422-ee6b8b943148""},{""object_id"":""e9f02f7b-5d38-492e-adfe-60e7a82856c8"",""sequence_order"":75,""candidate_id"":""e9f02f7b-5d38-492e-adfe-60e7a82856c8""},{""object_id"":""45e627a3-9dc8-4256-b4e5-772ece091ecf"",""sequence_order"":76,""candidate_id"":""45e627a3-9dc8-4256-b4e5-772ece091ecf""},{""object_id"":""9a62be31-780d-474c-9775-a0aaad5cd984"",""sequence_order"":77,""candidate_id"":""9a62be31-780d-474c-9775-a0aaad5cd984""},{""object_id"":""8cc2696c-7e7a-418d-9c7d-edd83d19463d"",""sequence_order"":78,""candidate_id"":""8cc2696c-7e7a-418d-9c7d-edd83d19463d""},{""object_id"":""05e03343-668e-48ea-836b-5ae498618019"",""sequence_order"":79,""candidate_id"":""05e03343-668e-48ea-836b-5ae498618019""},{""object_id"":""9edab0fe-493f-46ac-b9dc-1e008325519b"",""sequence_order"":80,""candidate_id"":""9edab0fe-493f-46ac-b9dc-1e008325519b""},{""object_id"":""ee41c1d1-2bab-46ef-98fc-a252964271d2"",""sequence_order"":81,""candidate_i
d"":""ee41c1d1-2bab-46ef-98fc-a252964271d2""},{""object_id"":""ab29aa1e-ec58-4f49-97bc-64829498380f"",""sequence_order"":82,""candidate_id"":""ab29aa1e-ec58-4f49-97bc-64829498380f""},{""object_id"":""15f7a782-a340-4f24-b0d7-d394df66fa68"",""sequence_order"":83,""candidate_id"":""15f7a782-a340-4f24-b0d7-d394df66fa68""},{""object_id"":""29be0f31-2254-49f2-98dc-19ebd34107e7"",""sequence_order"":84,""candidate_id"":""29be0f31-2254-49f2-98dc-19ebd34107e7""},{""object_id"":""3dc93471-491d-41e1-aaeb-ea129ee46966"",""sequence_order"":85,""candidate_id"":""3dc93471-491d-41e1-aaeb-ea129ee46966""},{""object_id"":""874f8ea6-c931-4b9f-8297-3b317b3840ee"",""sequence_order"":86,""candidate_id"":""874f8ea6-c931-4b9f-8297-3b317b3840ee""},{""object_id"":""76c5c0ff-8add-4c76-81fc-e4db5427396e"",""sequence_order"":87,""candidate_id"":""76c5c0ff-8add-4c76-81fc-e4db5427396e""},{""object_id"":""74a73e91-38e3-479c-b50e-b7db6cfb35c7"",""sequence_order"":88,""candidate_id"":""74a73e91-38e3-479c-b50e-b7db6cfb35c7""}],""ballot_title"":nu
ll,""ballot_subtitle"":null,""vote_variation"":null,""electoral_district_id"":""https://eligere.unipi.it"",""name"":""Commissione scientifica Area 1"",""primary_party_ids"":null,""number_elected"":-1,""votes_allowed"":2,""extensions"":{""HasCandidates"":""False"",""PollStartDate"":""20/11/2020 09:00:00"",""PollEndDate"":""26/11/2020 18:00:00""}}],""ballot_styles"":[{""object_id"":""39cf9c73-cc7c-4ea6-b458-04d182e75bdd"",""geopolitical_unit_ids"":null}]}
";

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
