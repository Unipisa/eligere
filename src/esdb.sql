USE [master]
GO
/****** Object:  Database [ESDB]    Script Date: 29/09/2021 11:03:48 ******/
CREATE DATABASE [ESDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ESDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\ESDB.mdf' , SIZE = 204800KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ESDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\ESDB_log.ldf' , SIZE = 1187840KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ESDB] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ESDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ESDB] SET ANSI_NULL_DEFAULT ON 
GO
ALTER DATABASE [ESDB] SET ANSI_NULLS ON 
GO
ALTER DATABASE [ESDB] SET ANSI_PADDING ON 
GO
ALTER DATABASE [ESDB] SET ANSI_WARNINGS ON 
GO
ALTER DATABASE [ESDB] SET ARITHABORT ON 
GO
ALTER DATABASE [ESDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ESDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ESDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ESDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ESDB] SET CURSOR_DEFAULT  LOCAL 
GO
ALTER DATABASE [ESDB] SET CONCAT_NULL_YIELDS_NULL ON 
GO
ALTER DATABASE [ESDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ESDB] SET QUOTED_IDENTIFIER ON 
GO
ALTER DATABASE [ESDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ESDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ESDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ESDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ESDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ESDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ESDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ESDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ESDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ESDB] SET RECOVERY FULL 
GO
ALTER DATABASE [ESDB] SET  MULTI_USER 
GO
ALTER DATABASE [ESDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ESDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ESDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ESDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ESDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ESDB] SET QUERY_STORE = OFF
GO
USE [ESDB]
GO
/****** Object:  User [eligere_es]    Script Date: 29/09/2021 11:03:48 ******/
CREATE USER [eligere_es] FOR LOGIN [eligere_es] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [eligere_es]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [eligere_es]
GO
/****** Object:  UserDefinedFunction [dbo].[LPAD]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[LPAD]

               (@SourceString VARCHAR(MAX),

                @FinalLength  INT,

                @PadChar      CHAR(1))

RETURNS VARCHAR(MAX)

AS

  BEGIN

    RETURN

      (SELECT Replicate(@PadChar,@FinalLength - Len(@SourceString)) + @SourceString)

  END

GO
/****** Object:  Table [dbo].[BallotName]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BallotName](
	[Id] [uniqueidentifier] NOT NULL,
	[BallotNameLabel] [nvarchar](512) NOT NULL,
	[Election_FK] [uniqueidentifier] NOT NULL,
	[Party_FK] [uniqueidentifier] NULL,
	[SequenceOrder] [int] NULL,
	[IsCandidate] [bit] NULL,
 CONSTRAINT [PK_BallotName] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Election]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Election](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Configuration] [nvarchar](max) NOT NULL,
	[PollStartDate] [datetime] NOT NULL,
	[PollEndDate] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[ElectorateListClosingDate] [datetime] NULL,
	[ElectionType_FK] [uniqueidentifier] NULL,
	[PollingStationGroupId] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ElectionRole]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ElectionRole](
	[Id] [uniqueidentifier] NOT NULL,
	[Label] [varchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ElectionStaff]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ElectionStaff](
	[Id] [uniqueidentifier] NOT NULL,
	[Person_FK] [uniqueidentifier] NOT NULL,
	[ElectionRole_FK] [uniqueidentifier] NOT NULL,
	[Election_FK] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ElectionType]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ElectionType](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[DefaultConfiguration] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EligibleCandidate]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EligibleCandidate](
	[Id] [uniqueidentifier] NOT NULL,
	[Person_FK] [uniqueidentifier] NOT NULL,
	[BallotName_FK] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK__Eligible__3214EC072F15BA23] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentificationCommissionerAffinityRel]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentificationCommissionerAffinityRel](
	[Id] [uniqueidentifier] NOT NULL,
	[Election_FK] [uniqueidentifier] NOT NULL,
	[Person_FK] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_IdentificationCommissionerAffinityRel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [uniqueidentifier] NOT NULL,
	[Person_FK] [uniqueidentifier] NOT NULL,
	[AccountProvider] [nvarchar](max) NOT NULL,
	[UserId] [nvarchar](max) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[LogEntry] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Party]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Party](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](512) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Party] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[PublicID] [nvarchar](max) NULL,
	[BirthDate] [datetime] NULL,
	[BirthPlace] [nvarchar](max) NULL,
	[Attributes] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PollingStationCommission]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PollingStationCommission](
	[Id] [uniqueidentifier] NOT NULL,
	[Election_FK] [uniqueidentifier] NOT NULL,
	[Location] [nvarchar](max) NULL,
	[DigitalLocation] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[President_FK] [uniqueidentifier] NULL,
	[PollingStationGroupId] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PollingStationCommissioner]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PollingStationCommissioner](
	[Id] [uniqueidentifier] NOT NULL,
	[Person_FK] [uniqueidentifier] NOT NULL,
	[PollingStationCommission_FK] [uniqueidentifier] NOT NULL,
	[VirtualRoom] [nvarchar](max) NULL,
	[AvailableForRemoteRecognition] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PollingStationSystem]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PollingStationSystem](
	[Id] [uniqueidentifier] NOT NULL,
	[IPAddress] [nvarchar](50) NULL,
	[DigitalFootprint] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recognition]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recognition](
	[Id] [uniqueidentifier] NOT NULL,
	[Validity] [datetime] NULL,
	[OTP] [nvarchar](max) NULL,
	[IDNum] [nvarchar](max) NULL,
	[IDExpiration] [datetime] NULL,
	[IDType] [nvarchar](max) NULL,
	[AccountProvider] [nvarchar](max) NOT NULL,
	[UserId] [nvarchar](max) NOT NULL,
	[State] [int] NOT NULL,
	[RemoteIdentification] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RelPollingStationSystemPollingStationCommission]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RelPollingStationSystemPollingStationCommission](
	[Id] [uniqueidentifier] NOT NULL,
	[PollingStationCommission_FK] [uniqueidentifier] NOT NULL,
	[PollingStationSystem_FK] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RemoteIdentificationCommissioner]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RemoteIdentificationCommissioner](
	[Id] [uniqueidentifier] NOT NULL,
	[PollingStationCommission_FK] [uniqueidentifier] NOT NULL,
	[Person_FK] [uniqueidentifier] NOT NULL,
	[VirtualRoom] [nvarchar](max) NOT NULL,
	[AvailableForRemoteRecognition] [bit] NOT NULL,
 CONSTRAINT [PK_RemoteIdentificationCommissioner] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TempCell]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempCell](
	[Mat] [nvarchar](50) NOT NULL,
	[Cognome] [nvarchar](max) NOT NULL,
	[cell] [nvarchar](50) NOT NULL,
	[cellint] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TempCell] PRIMARY KEY CLUSTERED 
(
	[Mat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLogin]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLogin](
	[Id] [uniqueidentifier] NOT NULL,
	[Provider] [nvarchar](50) NOT NULL,
	[UserId] [nvarchar](max) NOT NULL,
	[PersonFK] [uniqueidentifier] NOT NULL,
	[LastLogin] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLoginRequest]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLoginRequest](
	[Id] [uniqueidentifier] NOT NULL,
	[Provider] [nvarchar](50) NOT NULL,
	[UserId] [nvarchar](max) NOT NULL,
	[PersonFK] [uniqueidentifier] NOT NULL,
	[Approved] [bit] NOT NULL,
	[Approval] [datetime] NULL,
	[Approver] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Voter]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Voter](
	[Id] [uniqueidentifier] NOT NULL,
	[Election_FK] [uniqueidentifier] NOT NULL,
	[Person_FK] [uniqueidentifier] NOT NULL,
	[Vote] [datetime] NULL,
	[Recognition_FK] [uniqueidentifier] NULL,
	[Dropped] [bit] NULL,
	[DropReason] [nvarchar](max) NULL,
	[VotingTicket_FK] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VotingTicket]    Script Date: 29/09/2021 11:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VotingTicket](
	[Id] [uniqueidentifier] NOT NULL,
	[Hash] [nvarchar](128) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Voter_FK] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_Voter]    Script Date: 29/09/2021 11:03:48 ******/
CREATE NONCLUSTERED INDEX [IX_Voter] ON [dbo].[Voter]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BallotName] ADD  CONSTRAINT [DF_BallotName_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Election] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Election] ADD  CONSTRAINT [DF__Election__Active__5165187F]  DEFAULT ((0)) FOR [Active]
GO
ALTER TABLE [dbo].[ElectionRole] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[ElectionStaff] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[ElectionType] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[IdentificationCommissionerAffinityRel] ADD  CONSTRAINT [DF_IdentificationCommissionerAffinityRel_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Log] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Log] ADD  DEFAULT (getdate()) FOR [TimeStamp]
GO
ALTER TABLE [dbo].[Party] ADD  CONSTRAINT [DF_Party_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Person] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[PollingStationCommission] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[PollingStationCommissioner] ADD  CONSTRAINT [DF_PollingStationCommissioner_AvailableForRemoteRecognition]  DEFAULT ((0)) FOR [AvailableForRemoteRecognition]
GO
ALTER TABLE [dbo].[PollingStationSystem] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Recognition] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Recognition] ADD  DEFAULT ((0)) FOR [State]
GO
ALTER TABLE [dbo].[Recognition] ADD  CONSTRAINT [DF_Recognition_RemoteIdentification]  DEFAULT ((1)) FOR [RemoteIdentification]
GO
ALTER TABLE [dbo].[RelPollingStationSystemPollingStationCommission] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[UserLogin] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[UserLoginRequest] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[UserLoginRequest] ADD  DEFAULT ((0)) FOR [Approved]
GO
ALTER TABLE [dbo].[VotingTicket] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[BallotName]  WITH CHECK ADD  CONSTRAINT [FK_BallotName_ToElection] FOREIGN KEY([Election_FK])
REFERENCES [dbo].[Election] ([Id])
GO
ALTER TABLE [dbo].[BallotName] CHECK CONSTRAINT [FK_BallotName_ToElection]
GO
ALTER TABLE [dbo].[BallotName]  WITH CHECK ADD  CONSTRAINT [FK_BallotName_ToParty] FOREIGN KEY([Party_FK])
REFERENCES [dbo].[Party] ([Id])
GO
ALTER TABLE [dbo].[BallotName] CHECK CONSTRAINT [FK_BallotName_ToParty]
GO
ALTER TABLE [dbo].[Election]  WITH CHECK ADD  CONSTRAINT [FK_Election_ToElectionType] FOREIGN KEY([ElectionType_FK])
REFERENCES [dbo].[ElectionType] ([Id])
GO
ALTER TABLE [dbo].[Election] CHECK CONSTRAINT [FK_Election_ToElectionType]
GO
ALTER TABLE [dbo].[ElectionStaff]  WITH CHECK ADD  CONSTRAINT [FK_ElectionStaff_ToElection] FOREIGN KEY([Election_FK])
REFERENCES [dbo].[Election] ([Id])
GO
ALTER TABLE [dbo].[ElectionStaff] CHECK CONSTRAINT [FK_ElectionStaff_ToElection]
GO
ALTER TABLE [dbo].[ElectionStaff]  WITH CHECK ADD  CONSTRAINT [FK_ElectionStaff_ToElectionRole] FOREIGN KEY([ElectionRole_FK])
REFERENCES [dbo].[ElectionRole] ([Id])
GO
ALTER TABLE [dbo].[ElectionStaff] CHECK CONSTRAINT [FK_ElectionStaff_ToElectionRole]
GO
ALTER TABLE [dbo].[ElectionStaff]  WITH CHECK ADD  CONSTRAINT [FK_ElectionStaff_ToPerson] FOREIGN KEY([Person_FK])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[ElectionStaff] CHECK CONSTRAINT [FK_ElectionStaff_ToPerson]
GO
ALTER TABLE [dbo].[EligibleCandidate]  WITH CHECK ADD  CONSTRAINT [FK_EligibleCandidate_ToBallotName] FOREIGN KEY([BallotName_FK])
REFERENCES [dbo].[BallotName] ([Id])
GO
ALTER TABLE [dbo].[EligibleCandidate] CHECK CONSTRAINT [FK_EligibleCandidate_ToBallotName]
GO
ALTER TABLE [dbo].[EligibleCandidate]  WITH CHECK ADD  CONSTRAINT [FK_EligibleCandidate_ToPerson] FOREIGN KEY([Person_FK])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[EligibleCandidate] CHECK CONSTRAINT [FK_EligibleCandidate_ToPerson]
GO
ALTER TABLE [dbo].[IdentificationCommissionerAffinityRel]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationCommissionerAffinityRel_Election] FOREIGN KEY([Election_FK])
REFERENCES [dbo].[Election] ([Id])
GO
ALTER TABLE [dbo].[IdentificationCommissionerAffinityRel] CHECK CONSTRAINT [FK_IdentificationCommissionerAffinityRel_Election]
GO
ALTER TABLE [dbo].[IdentificationCommissionerAffinityRel]  WITH CHECK ADD  CONSTRAINT [FK_IdentificationCommissionerAffinityRel_Person] FOREIGN KEY([Person_FK])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[IdentificationCommissionerAffinityRel] CHECK CONSTRAINT [FK_IdentificationCommissionerAffinityRel_Person]
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_Log_ToPerson] FOREIGN KEY([Person_FK])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_Log_ToPerson]
GO
ALTER TABLE [dbo].[PollingStationCommission]  WITH CHECK ADD  CONSTRAINT [FK_PollingStationCommission_ToElection] FOREIGN KEY([Election_FK])
REFERENCES [dbo].[Election] ([Id])
GO
ALTER TABLE [dbo].[PollingStationCommission] CHECK CONSTRAINT [FK_PollingStationCommission_ToElection]
GO
ALTER TABLE [dbo].[PollingStationCommission]  WITH CHECK ADD  CONSTRAINT [FK_PollingStationCommission_ToPollingStationCommissioner] FOREIGN KEY([President_FK])
REFERENCES [dbo].[PollingStationCommissioner] ([Id])
GO
ALTER TABLE [dbo].[PollingStationCommission] CHECK CONSTRAINT [FK_PollingStationCommission_ToPollingStationCommissioner]
GO
ALTER TABLE [dbo].[PollingStationCommissioner]  WITH CHECK ADD  CONSTRAINT [FK_PollingStationCommissioner_ToPerson] FOREIGN KEY([Person_FK])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[PollingStationCommissioner] CHECK CONSTRAINT [FK_PollingStationCommissioner_ToPerson]
GO
ALTER TABLE [dbo].[PollingStationCommissioner]  WITH CHECK ADD  CONSTRAINT [FK_PollingStationCommissioner_ToPollingStationCommission] FOREIGN KEY([PollingStationCommission_FK])
REFERENCES [dbo].[PollingStationCommission] ([Id])
GO
ALTER TABLE [dbo].[PollingStationCommissioner] CHECK CONSTRAINT [FK_PollingStationCommissioner_ToPollingStationCommission]
GO
ALTER TABLE [dbo].[RelPollingStationSystemPollingStationCommission]  WITH CHECK ADD  CONSTRAINT [FK_RelPollingStationSystemPollingStationCommission_ToPollingStationCommission] FOREIGN KEY([PollingStationCommission_FK])
REFERENCES [dbo].[PollingStationCommission] ([Id])
GO
ALTER TABLE [dbo].[RelPollingStationSystemPollingStationCommission] CHECK CONSTRAINT [FK_RelPollingStationSystemPollingStationCommission_ToPollingStationCommission]
GO
ALTER TABLE [dbo].[RelPollingStationSystemPollingStationCommission]  WITH CHECK ADD  CONSTRAINT [FK_RelPollingStationSystemPollingStationCommission_ToPollingStationSystem] FOREIGN KEY([PollingStationSystem_FK])
REFERENCES [dbo].[PollingStationSystem] ([Id])
GO
ALTER TABLE [dbo].[RelPollingStationSystemPollingStationCommission] CHECK CONSTRAINT [FK_RelPollingStationSystemPollingStationCommission_ToPollingStationSystem]
GO
ALTER TABLE [dbo].[RemoteIdentificationCommissioner]  WITH CHECK ADD  CONSTRAINT [FK_RemoteIdentificationCommissioner_ToPerson] FOREIGN KEY([Person_FK])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[RemoteIdentificationCommissioner] CHECK CONSTRAINT [FK_RemoteIdentificationCommissioner_ToPerson]
GO
ALTER TABLE [dbo].[RemoteIdentificationCommissioner]  WITH CHECK ADD  CONSTRAINT [FK_RemoteIdentificationCommissioner_ToPollingStationCommission] FOREIGN KEY([PollingStationCommission_FK])
REFERENCES [dbo].[PollingStationCommission] ([Id])
GO
ALTER TABLE [dbo].[RemoteIdentificationCommissioner] CHECK CONSTRAINT [FK_RemoteIdentificationCommissioner_ToPollingStationCommission]
GO
ALTER TABLE [dbo].[UserLogin]  WITH CHECK ADD  CONSTRAINT [FK_UserLogin_ToTable] FOREIGN KEY([PersonFK])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[UserLogin] CHECK CONSTRAINT [FK_UserLogin_ToTable]
GO
ALTER TABLE [dbo].[UserLoginRequest]  WITH CHECK ADD  CONSTRAINT [FK_UserLoginRequest_ToPerson] FOREIGN KEY([PersonFK])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[UserLoginRequest] CHECK CONSTRAINT [FK_UserLoginRequest_ToPerson]
GO
ALTER TABLE [dbo].[Voter]  WITH CHECK ADD  CONSTRAINT [FK_Voter_ToElection] FOREIGN KEY([Election_FK])
REFERENCES [dbo].[Election] ([Id])
GO
ALTER TABLE [dbo].[Voter] CHECK CONSTRAINT [FK_Voter_ToElection]
GO
ALTER TABLE [dbo].[Voter]  WITH CHECK ADD  CONSTRAINT [FK_Voter_ToPerson] FOREIGN KEY([Person_FK])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[Voter] CHECK CONSTRAINT [FK_Voter_ToPerson]
GO
ALTER TABLE [dbo].[Voter]  WITH CHECK ADD  CONSTRAINT [FK_Voter_ToRecognition] FOREIGN KEY([Recognition_FK])
REFERENCES [dbo].[Recognition] ([Id])
GO
ALTER TABLE [dbo].[Voter] CHECK CONSTRAINT [FK_Voter_ToRecognition]
GO
ALTER TABLE [dbo].[Voter]  WITH CHECK ADD  CONSTRAINT [FK_Voter_Voter] FOREIGN KEY([Id])
REFERENCES [dbo].[Voter] ([Id])
GO
ALTER TABLE [dbo].[Voter] CHECK CONSTRAINT [FK_Voter_Voter]
GO
ALTER TABLE [dbo].[Voter]  WITH NOCHECK ADD  CONSTRAINT [FK_Voter_VotingTicket] FOREIGN KEY([VotingTicket_FK])
REFERENCES [dbo].[VotingTicket] ([Id])
GO
ALTER TABLE [dbo].[Voter] CHECK CONSTRAINT [FK_Voter_VotingTicket]
GO
ALTER TABLE [dbo].[Election]  WITH CHECK ADD  CONSTRAINT [CK_Election_Configuration] CHECK  ((isjson([Configuration])=(1)))
GO
ALTER TABLE [dbo].[Election] CHECK CONSTRAINT [CK_Election_Configuration]
GO
ALTER TABLE [dbo].[ElectionType]  WITH CHECK ADD  CONSTRAINT [CK_ElectionType_DefaultConfiguration] CHECK  ((isjson([DefaultConfiguration])=(1)))
GO
ALTER TABLE [dbo].[ElectionType] CHECK CONSTRAINT [CK_ElectionType_DefaultConfiguration]
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [CK_Log_LogEntry] CHECK  ((isjson([LogEntry])=(1)))
GO
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [CK_Log_LogEntry]
GO
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [CK_Person_Attributes] CHECK  ((isjson([Attributes])=(1)))
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [CK_Person_Attributes]
GO
ALTER TABLE [dbo].[VotingTicket]  WITH CHECK ADD  CONSTRAINT [CK_VotingTicket_Content] CHECK  ((isjson([Content])=(1)))
GO
ALTER TABLE [dbo].[VotingTicket] CHECK CONSTRAINT [CK_VotingTicket_Content]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:Login;1:Recognized;2:Activated(byOTP)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recognition', @level2type=N'COLUMN',@level2name=N'State'
GO
USE [master]
GO
ALTER DATABASE [ESDB] SET  READ_WRITE 
GO
