using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace campsApi
{
    public partial class campsContext : DbContext
    {
        public campsContext()
        {
        }

        public campsContext(DbContextOptions<campsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CampUsers> CampUsers { get; set; }
        public virtual DbSet<Camps> Camps { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Speakers> Speakers { get; set; }
        public virtual DbSet<Users> Users { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseNpgsql("Host=localhost;Database=camps;Username=postgres;Password=tim");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CampUsers>(entity =>
            {
                entity.ToTable("camp_users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.FirstName).HasColumnName("first_name");

                entity.Property(e => e.LastName).HasColumnName("last_name");

                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
            });

            modelBuilder.Entity<Camps>(entity =>
            {
                entity.ToTable("camps");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EndDate).HasColumnName("end_date");

                entity.Property(e => e.Events).HasColumnName("events");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.StartDate).HasColumnName("start_date");

                entity.HasOne(d => d.EventsNavigation)
                    .WithMany(p => p.Camps)
                    .HasForeignKey(d => d.Events)
                    .HasConstraintName("camps_events_fkey");
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.ToTable("events");

                entity.HasIndex(e => e.Id)
                    .HasName("events_id_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('talks_id_seq'::regclass)");

                entity.Property(e => e.Speaker).HasColumnName("speaker");

                entity.Property(e => e.Venue).HasColumnName("venue");

                entity.Property(e => e.NameOfEvent).HasColumnName("NameOfEvent");

                entity.HasOne(d => d.SpeakerNavigation)
                    .WithMany(p => p.Events)
                    .HasForeignKey(d => d.Speaker)
                    .HasConstraintName("events_speaker_fkey");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("roles");

                entity.HasIndex(e => e.Id)
                    .HasName("roles_id_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Role).HasColumnName("role");
            });

            modelBuilder.Entity<Speakers>(entity =>
            {
                entity.ToTable("speakers");

                entity.HasIndex(e => e.Id)
                    .HasName("speakers_id_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Fullname).HasColumnName("fullname");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.Fullname).HasColumnName("fullname");

                entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");

                entity.Property(e => e.Passwordsalt).HasColumnName("passwordsalt");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.Token).HasColumnName("token");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Role)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("users_role_fkey");
            });

            modelBuilder.HasSequence("talks_id_seq");
        }
    }
}
