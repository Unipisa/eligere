using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EligereES.Models.DB
{
    public partial class ESDB : DbContext
    {
        public ESDB()
        {
        }

        public ESDB(DbContextOptions<ESDB> options)
            : base(options)
        {
        }

        public virtual DbSet<BallotName> BallotName { get; set; }
        public virtual DbSet<Election> Election { get; set; }
        public virtual DbSet<ElectionRole> ElectionRole { get; set; }
        public virtual DbSet<ElectionStaff> ElectionStaff { get; set; }
        public virtual DbSet<ElectionType> ElectionType { get; set; }
        public virtual DbSet<EligibleCandidate> EligibleCandidate { get; set; }
        public virtual DbSet<IdentificationCommissionerAffinityRel> IdentificationCommissionerAffinityRel { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Party> Party { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PollingStationCommission> PollingStationCommission { get; set; }
        public virtual DbSet<PollingStationCommissioner> PollingStationCommissioner { get; set; }
        public virtual DbSet<PollingStationSystem> PollingStationSystem { get; set; }
        public virtual DbSet<Recognition> Recognition { get; set; }
        public virtual DbSet<RelPollingStationSystemPollingStationCommission> RelPollingStationSystemPollingStationCommission { get; set; }
        public virtual DbSet<RemoteIdentificationCommissioner> RemoteIdentificationCommissioner { get; set; }
        public virtual DbSet<TempCell> TempCell { get; set; }
        public virtual DbSet<TempComm> TempComm { get; set; }
        public virtual DbSet<TempEl> TempEl { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<UserLoginRequest> UserLoginRequest { get; set; }
        public virtual DbSet<Voter> Voter { get; set; }
        public virtual DbSet<VotingTicket> VotingTicket { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Database=ESDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BallotName>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BallotNameLabel)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.ElectionFk).HasColumnName("Election_FK");

                entity.Property(e => e.PartyFk).HasColumnName("Party_FK");

                entity.HasOne(d => d.ElectionFkNavigation)
                    .WithMany(p => p.BallotName)
                    .HasForeignKey(d => d.ElectionFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BallotName_ToElection");

                entity.HasOne(d => d.PartyFkNavigation)
                    .WithMany(p => p.BallotName)
                    .HasForeignKey(d => d.PartyFk)
                    .HasConstraintName("FK_BallotName_ToParty");
            });

            modelBuilder.Entity<Election>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Configuration).IsRequired();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.ElectionTypeFk).HasColumnName("ElectionType_FK");

                entity.Property(e => e.ElectorateListClosingDate).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.PollEndDate).HasColumnType("datetime");

                entity.Property(e => e.PollStartDate).HasColumnType("datetime");

                entity.HasOne(d => d.ElectionTypeFkNavigation)
                    .WithMany(p => p.Election)
                    .HasForeignKey(d => d.ElectionTypeFk)
                    .HasConstraintName("FK_Election_ToElectionType");
            });

            modelBuilder.Entity<ElectionRole>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Label)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ElectionStaff>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ElectionFk).HasColumnName("Election_FK");

                entity.Property(e => e.ElectionRoleFk).HasColumnName("ElectionRole_FK");

                entity.Property(e => e.PersonFk).HasColumnName("Person_FK");

                entity.HasOne(d => d.ElectionFkNavigation)
                    .WithMany(p => p.ElectionStaff)
                    .HasForeignKey(d => d.ElectionFk)
                    .HasConstraintName("FK_ElectionStaff_ToElection");

                entity.HasOne(d => d.ElectionRoleFkNavigation)
                    .WithMany(p => p.ElectionStaff)
                    .HasForeignKey(d => d.ElectionRoleFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ElectionStaff_ToElectionRole");

                entity.HasOne(d => d.PersonFkNavigation)
                    .WithMany(p => p.ElectionStaff)
                    .HasForeignKey(d => d.PersonFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ElectionStaff_ToPerson");
            });

            modelBuilder.Entity<ElectionType>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.DefaultConfiguration).IsRequired();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<EligibleCandidate>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BallotNameFk).HasColumnName("BallotName_FK");

                entity.Property(e => e.PersonFk).HasColumnName("Person_FK");

                entity.HasOne(d => d.BallotNameFkNavigation)
                    .WithMany(p => p.EligibleCandidate)
                    .HasForeignKey(d => d.BallotNameFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EligibleCandidate_ToBallotName");

                entity.HasOne(d => d.PersonFkNavigation)
                    .WithMany(p => p.EligibleCandidate)
                    .HasForeignKey(d => d.PersonFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EligibleCandidate_ToPerson");
            });

            modelBuilder.Entity<IdentificationCommissionerAffinityRel>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ElectionFk).HasColumnName("Election_FK");

                entity.Property(e => e.PersonFk).HasColumnName("Person_FK");

                entity.HasOne(d => d.ElectionFkNavigation)
                    .WithMany(p => p.IdentificationCommissionerAffinityRel)
                    .HasForeignKey(d => d.ElectionFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IdentificationCommissionerAffinityRel_Election");

                entity.HasOne(d => d.PersonFkNavigation)
                    .WithMany(p => p.IdentificationCommissionerAffinityRel)
                    .HasForeignKey(d => d.PersonFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IdentificationCommissionerAffinityRel_Person");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AccountProvider).IsRequired();

                entity.Property(e => e.LogEntry).IsRequired();

                entity.Property(e => e.PersonFk).HasColumnName("Person_FK");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.PersonFkNavigation)
                    .WithMany(p => p.Log)
                    .HasForeignKey(d => d.PersonFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Log_ToPerson");
            });

            modelBuilder.Entity<Party>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.PublicId).HasColumnName("PublicID");
            });

            modelBuilder.Entity<PollingStationCommission>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ElectionFk).HasColumnName("Election_FK");

                entity.Property(e => e.PresidentFk).HasColumnName("President_FK");

                entity.HasOne(d => d.ElectionFkNavigation)
                    .WithMany(p => p.PollingStationCommission)
                    .HasForeignKey(d => d.ElectionFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PollingStationCommission_ToElection");

                entity.HasOne(d => d.PresidentFkNavigation)
                    .WithMany(p => p.PollingStationCommission)
                    .HasForeignKey(d => d.PresidentFk)
                    .HasConstraintName("FK_PollingStationCommission_ToPollingStationCommissioner");
            });

            modelBuilder.Entity<PollingStationCommissioner>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.PersonFk).HasColumnName("Person_FK");

                entity.Property(e => e.PollingStationCommissionFk).HasColumnName("PollingStationCommission_FK");

                entity.HasOne(d => d.PersonFkNavigation)
                    .WithMany(p => p.PollingStationCommissioner)
                    .HasForeignKey(d => d.PersonFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PollingStationCommissioner_ToPerson");

                entity.HasOne(d => d.PollingStationCommissionFkNavigation)
                    .WithMany(p => p.PollingStationCommissioner)
                    .HasForeignKey(d => d.PollingStationCommissionFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PollingStationCommissioner_ToPollingStationCommission");
            });

            modelBuilder.Entity<PollingStationSystem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Ipaddress)
                    .HasMaxLength(50)
                    .HasColumnName("IPAddress");
            });

            modelBuilder.Entity<Recognition>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AccountProvider).IsRequired();

                entity.Property(e => e.Idexpiration)
                    .HasColumnType("datetime")
                    .HasColumnName("IDExpiration");

                entity.Property(e => e.Idnum).HasColumnName("IDNum");

                entity.Property(e => e.Idtype).HasColumnName("IDType");

                entity.Property(e => e.Otp).HasColumnName("OTP");

                entity.Property(e => e.RemoteIdentification)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.State).HasComment("0:Login;1:Recognized;2:Activated(byOTP)");

                entity.Property(e => e.UserId).IsRequired();

                entity.Property(e => e.Validity).HasColumnType("datetime");
            });

            modelBuilder.Entity<RelPollingStationSystemPollingStationCommission>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.PollingStationCommissionFk).HasColumnName("PollingStationCommission_FK");

                entity.Property(e => e.PollingStationSystemFk).HasColumnName("PollingStationSystem_FK");

                entity.HasOne(d => d.PollingStationCommissionFkNavigation)
                    .WithMany(p => p.RelPollingStationSystemPollingStationCommission)
                    .HasForeignKey(d => d.PollingStationCommissionFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RelPollingStationSystemPollingStationCommission_ToPollingStationCommission");

                entity.HasOne(d => d.PollingStationSystemFkNavigation)
                    .WithMany(p => p.RelPollingStationSystemPollingStationCommission)
                    .HasForeignKey(d => d.PollingStationSystemFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RelPollingStationSystemPollingStationCommission_ToPollingStationSystem");
            });

            modelBuilder.Entity<RemoteIdentificationCommissioner>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.PersonFk).HasColumnName("Person_FK");

                entity.Property(e => e.PollingStationCommissionFk).HasColumnName("PollingStationCommission_FK");

                entity.Property(e => e.VirtualRoom).IsRequired();

                entity.HasOne(d => d.PersonFkNavigation)
                    .WithMany(p => p.RemoteIdentificationCommissioner)
                    .HasForeignKey(d => d.PersonFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RemoteIdentificationCommissioner_ToPerson");

                entity.HasOne(d => d.PollingStationCommissionFkNavigation)
                    .WithMany(p => p.RemoteIdentificationCommissioner)
                    .HasForeignKey(d => d.PollingStationCommissionFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RemoteIdentificationCommissioner_ToPollingStationCommission");
            });

            modelBuilder.Entity<TempCell>(entity =>
            {
                entity.HasKey(e => e.Mat);

                entity.Property(e => e.Mat).HasMaxLength(50);

                entity.Property(e => e.Cell)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cell");

                entity.Property(e => e.Cellint)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("cellint");

                entity.Property(e => e.Cognome).IsRequired();
            });

            modelBuilder.Entity<TempComm>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Dipartimento)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PersonFk).HasColumnName("Person_FK");
            });

            modelBuilder.Entity<TempEl>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Dipartimento)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.LastLogin).HasColumnType("datetime");

                entity.Property(e => e.PersonFk).HasColumnName("PersonFK");

                entity.Property(e => e.Provider)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.PersonFkNavigation)
                    .WithMany(p => p.UserLogin)
                    .HasForeignKey(d => d.PersonFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserLogin_ToTable");
            });

            modelBuilder.Entity<UserLoginRequest>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Approval).HasColumnType("datetime");

                entity.Property(e => e.PersonFk).HasColumnName("PersonFK");

                entity.Property(e => e.Provider)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.PersonFkNavigation)
                    .WithMany(p => p.UserLoginRequest)
                    .HasForeignKey(d => d.PersonFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserLoginRequest_ToPerson");
            });

            modelBuilder.Entity<Voter>(entity =>
            {
                entity.HasIndex(e => e.Id, "IX_Voter");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ElectionFk).HasColumnName("Election_FK");

                entity.Property(e => e.PersonFk).HasColumnName("Person_FK");

                entity.Property(e => e.RecognitionFk).HasColumnName("Recognition_FK");

                entity.Property(e => e.Vote).HasColumnType("datetime");

                entity.Property(e => e.VotingTicketFk).HasColumnName("VotingTicket_FK");

                entity.HasOne(d => d.ElectionFkNavigation)
                    .WithMany(p => p.Voter)
                    .HasForeignKey(d => d.ElectionFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Voter_ToElection");

                entity.HasOne(d => d.PersonFkNavigation)
                    .WithMany(p => p.Voter)
                    .HasForeignKey(d => d.PersonFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Voter_ToPerson");

                entity.HasOne(d => d.RecognitionFkNavigation)
                    .WithMany(p => p.Voter)
                    .HasForeignKey(d => d.RecognitionFk)
                    .HasConstraintName("FK_Voter_ToRecognition");

                entity.HasOne(d => d.VotingTicketFkNavigation)
                    .WithMany(p => p.Voter)
                    .HasForeignKey(d => d.VotingTicketFk)
                    .HasConstraintName("FK_Voter_VotingTicket");
            });

            modelBuilder.Entity<VotingTicket>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.VoterFk).HasColumnName("Voter_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
