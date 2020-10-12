USE [master]
GO
/****** Object:  Database [ESDB]    Script Date: 05/10/2020 17:22:20 ******/
CREATE DATABASE [ESDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ElectionESdb', FILENAME = N'C:\Users\ciste\OneDrive - University of Pisa\PC\Documenti\Dev@Unipi\eligere\src\EligereES\Data\ElectionESdb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ElectionESdb_log', FILENAME = N'C:\Users\ciste\OneDrive - University of Pisa\PC\Documenti\Dev@Unipi\eligere\src\EligereES\Data\ElectionESdb.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
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
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [ESDB]
GO
/****** Object:  Table [dbo].[Election]    Script Date: 05/10/2020 17:22:20 ******/
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
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ElectionRole]    Script Date: 05/10/2020 17:22:20 ******/
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
/****** Object:  Table [dbo].[ElectionStaff]    Script Date: 05/10/2020 17:22:20 ******/
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
/****** Object:  Table [dbo].[ElectionType]    Script Date: 05/10/2020 17:22:20 ******/
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
/****** Object:  Table [dbo].[EligibleCandidate]    Script Date: 05/10/2020 17:22:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EligibleCandidate](
	[Id] [uniqueidentifier] NOT NULL,
	[Person_FK] [uniqueidentifier] NOT NULL,
	[Election_FK] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 05/10/2020 17:22:20 ******/
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
/****** Object:  Table [dbo].[Person]    Script Date: 05/10/2020 17:22:20 ******/
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
/****** Object:  Table [dbo].[PollingStationCommission]    Script Date: 05/10/2020 17:22:20 ******/
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
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PollingStationCommissioner]    Script Date: 05/10/2020 17:22:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PollingStationCommissioner](
	[Id] [uniqueidentifier] NOT NULL,
	[Person_FK] [uniqueidentifier] NOT NULL,
	[PollingStationCommission_FK] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PollingStationSystem]    Script Date: 05/10/2020 17:22:20 ******/
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
/****** Object:  Table [dbo].[Recognition]    Script Date: 05/10/2020 17:22:20 ******/
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
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RelPollingStationSystemPollingStationCommission]    Script Date: 05/10/2020 17:22:20 ******/
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
/****** Object:  Table [dbo].[UserLogin]    Script Date: 05/10/2020 17:22:20 ******/
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
/****** Object:  Table [dbo].[Voter]    Script Date: 05/10/2020 17:22:20 ******/
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
	[TicketId] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Election] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Election] ADD  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[ElectionRole] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[ElectionStaff] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[ElectionType] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Log] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Log] ADD  DEFAULT (getdate()) FOR [TimeStamp]
GO
ALTER TABLE [dbo].[Person] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[PollingStationCommission] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[PollingStationSystem] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Recognition] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Recognition] ADD  DEFAULT ((0)) FOR [State]
GO
ALTER TABLE [dbo].[RelPollingStationSystemPollingStationCommission] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[UserLogin] ADD  DEFAULT (newid()) FOR [Id]
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
ALTER TABLE [dbo].[EligibleCandidate]  WITH CHECK ADD  CONSTRAINT [FK_EligibleCandidate_ToElection] FOREIGN KEY([Election_FK])
REFERENCES [dbo].[Election] ([Id])
GO
ALTER TABLE [dbo].[EligibleCandidate] CHECK CONSTRAINT [FK_EligibleCandidate_ToElection]
GO
ALTER TABLE [dbo].[EligibleCandidate]  WITH CHECK ADD  CONSTRAINT [FK_EligibleCandidate_ToPerson] FOREIGN KEY([Person_FK])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[EligibleCandidate] CHECK CONSTRAINT [FK_EligibleCandidate_ToPerson]
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
ALTER TABLE [dbo].[UserLogin]  WITH CHECK ADD  CONSTRAINT [FK_UserLogin_ToTable] FOREIGN KEY([PersonFK])
REFERENCES [dbo].[Person] ([Id])
GO
ALTER TABLE [dbo].[UserLogin] CHECK CONSTRAINT [FK_UserLogin_ToTable]
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:Login;1:Recognized;2:Activated(byOTP)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Recognition', @level2type=N'COLUMN',@level2name=N'State'
GO
USE [master]
GO
ALTER DATABASE [ESDB] SET  READ_WRITE 
GO
