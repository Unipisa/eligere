using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        public virtual DbSet<Election> Election { get; set; }
        public virtual DbSet<ElectionRole> ElectionRole { get; set; }
        public virtual DbSet<ElectionStaff> ElectionStaff { get; set; }
        public virtual DbSet<ElectionType> ElectionType { get; set; }
        public virtual DbSet<EligibleCandidate> EligibleCandidate { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PollingStationCommission> PollingStationCommission { get; set; }
        public virtual DbSet<PollingStationCommissioner> PollingStationCommissioner { get; set; }
        public virtual DbSet<PollingStationSystem> PollingStationSystem { get; set; }
        public virtual DbSet<Recognition> Recognition { get; set; }
        public virtual DbSet<RelPollingStationSystemPollingStationCommission> RelPollingStationSystemPollingStationCommission { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<Voter> Voter { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;Database=ESDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Election>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

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

                entity.Property(e => e.ElectionFk).HasColumnName("Election_FK");

                entity.Property(e => e.PersonFk).HasColumnName("Person_FK");

                entity.HasOne(d => d.ElectionFkNavigation)
                    .WithMany(p => p.EligibleCandidate)
                    .HasForeignKey(d => d.ElectionFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EligibleCandidate_ToElection");

                entity.HasOne(d => d.PersonFkNavigation)
                    .WithMany(p => p.EligibleCandidate)
                    .HasForeignKey(d => d.PersonFk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EligibleCandidate_ToPerson");
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

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

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
                    .HasColumnName("IPAddress")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Recognition>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AccountProvider).IsRequired();

                entity.Property(e => e.Idexpiration)
                    .HasColumnName("IDExpiration")
                    .HasColumnType("datetime");

                entity.Property(e => e.Idnum).HasColumnName("IDNum");

                entity.Property(e => e.Idtype).HasColumnName("IDType");

                entity.Property(e => e.Otp).HasColumnName("OTP");

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

            modelBuilder.Entity<Voter>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ElectionFk).HasColumnName("Election_FK");

                entity.Property(e => e.PersonFk).HasColumnName("Person_FK");

                entity.Property(e => e.RecognitionFk).HasColumnName("Recognition_FK");

                entity.Property(e => e.Vote).HasColumnType("datetime");

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
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
