using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCS_CORE.Domain
{
    public class MasterEntity
    {
        [Column("id")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("create_datetime")]
        [Required]
        public DateTime CreateDatetime { get; set; } = System.DateTime.Now;

        [Column("create_user_id")]
        [Required]
        [StringLength(10)]
        public string CreateUserId { get; set; }

        [Column("update_datetime")]
        [Required]
        public DateTime UpdateDatetime { get; set; } = System.DateTime.Now;

        [Column("update_user_id")]
        [Required]
        [StringLength(10)]
        public string UpdateUserId { get; set; } 
    }
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication6.Models.Db;

public partial class DemoContext : DbContext
{
    public DemoContext()
    {
    }

    public DemoContext(DbContextOptions<DemoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleUser> RoleUsers { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<YourTableName> YourTableNames { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-GRR6EAN;Initial Catalog=demo;Persist Security Info=True;User ID=sa; Password=123456;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Role1).HasName("PK_SP_Role");

            entity.ToTable("Role");

            entity.Property(e => e.Role1)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Createat)
                .HasColumnType("datetime")
                .HasColumnName("createat");
            entity.Property(e => e.Createby)
                .HasMaxLength(50)
                .HasColumnName("createby");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<RoleUser>(entity =>
        {
            entity.HasKey(e => new { e.RoleName, e.Username });

            entity.ToTable("RoleUser");

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserLogi__3213E83F0588545E");

            entity.ToTable("UserLogin");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Firstname)
                .HasMaxLength(100)
                .HasColumnName("firstname");
            entity.Property(e => e.Grade)
                .HasMaxLength(50)
                .HasColumnName("grade");
            entity.Property(e => e.Group)
                .HasMaxLength(50)
                .HasColumnName("group");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Picture)
                .HasMaxLength(200)
                .HasColumnName("picture");
            entity.Property(e => e.Shift)
                .HasMaxLength(50)
                .HasColumnName("shift");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<YourTableName>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__YourTabl__3214EC07DA7FEA32");

            entity.ToTable("YourTableName");

            entity.Property(e => e.ApprovedBy).HasMaxLength(50);
            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.CheckedBy).HasMaxLength(50);
            entity.Property(e => e.CheckedDate).HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DrawingRev).HasMaxLength(50);
            entity.Property(e => e.IssuedBy).HasMaxLength(50);
            entity.Property(e => e.IssuedDate).HasColumnType("datetime");
            entity.Property(e => e.ManagementId)
                .HasMaxLength(50)
                .HasColumnName("ManagementID");
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.No).HasMaxLength(50);
            entity.Property(e => e.PartName).HasMaxLength(50);
            entity.Property(e => e.PartNo).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

