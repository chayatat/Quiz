using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace QuizWebAPI.Models
{
    public partial class quizdbContext : DbContext
    {
        public quizdbContext()
        {
        }

        public quizdbContext(DbContextOptions<quizdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblChoice> TblChoices { get; set; }
        public virtual DbSet<TblQuestion> TblQuestions { get; set; }
        public virtual DbSet<TblUser> TblUsers { get; set; }
        public virtual DbSet<TblUserScore> TblUserScores { get; set; }
        public virtual DbSet<TblUsergroup> TblUsergroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=quizdb;Trusted_Connection=True;");
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Thai_CI_AS");

            modelBuilder.Entity<TblChoice>(entity =>
            {
                entity.HasKey(e => e.ChoiceId)
                    .HasName("choice_PK");

                entity.ToTable("tbl_choice");

                entity.Property(e => e.ChoiceId).HasColumnName("choice_id");

                entity.Property(e => e.ChoiceName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("choice_name");

                entity.Property(e => e.ChoiceScore).HasColumnName("choice_score");

                entity.Property(e => e.ChoiceSort).HasColumnName("choice_sort");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TblChoices)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_choice_FK");
            });

            modelBuilder.Entity<TblQuestion>(entity =>
            {
                entity.HasKey(e => e.QuestionId)
                    .HasName("question_PK");

                entity.ToTable("tbl_question");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.QuestionName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("question_name");

                entity.Property(e => e.QuestionSort).HasColumnName("question_sort");

                entity.Property(e => e.UsergroupId).HasColumnName("usergroup_id");

                entity.HasOne(d => d.Usergroup)
                    .WithMany(p => p.TblQuestions)
                    .HasForeignKey(d => d.UsergroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_question_FK");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("user_PK");

                entity.ToTable("tbl_user");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("user_status")
                    .IsFixedLength(true);

                entity.Property(e => e.UserTotalScore).HasColumnName("user_total_score");

                entity.Property(e => e.UsergroupId).HasColumnName("usergroup_id");

                entity.HasOne(d => d.Usergroup)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.UsergroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_user_FK");
            });

            modelBuilder.Entity<TblUserScore>(entity =>
            {
                entity.HasKey(e => e.UserScoreId)
                    .HasName("user_score_PK");

                entity.ToTable("tbl_user_score");

                entity.Property(e => e.UserScoreId).HasColumnName("user_score_id");

                entity.Property(e => e.ChoiceId).HasColumnName("choice_id");

                entity.Property(e => e.ChoiceScore).HasColumnName("choice_score");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserScoreStatus)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("user_score_status")
                    .IsFixedLength(true);

                entity.Property(e => e.UsergroupId).HasColumnName("usergroup_id");

                entity.Property(e => e.UsergroupName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("usergroup_name");

                entity.HasOne(d => d.Choice)
                    .WithMany(p => p.TblUserScores)
                    .HasForeignKey(d => d.ChoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_user_score_FK_3");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TblUserScores)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_user_score_FK_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserScores)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_user_score_FK");

                entity.HasOne(d => d.Usergroup)
                    .WithMany(p => p.TblUserScores)
                    .HasForeignKey(d => d.UsergroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tbl_user_score_FK_1");
            });

            modelBuilder.Entity<TblUsergroup>(entity =>
            {
                entity.HasKey(e => e.UsergroupId)
                    .HasName("usergroup_PK");

                entity.ToTable("tbl_usergroup");

                entity.Property(e => e.UsergroupId).HasColumnName("usergroup_id");

                entity.Property(e => e.UsergroupName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("usergroup_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
