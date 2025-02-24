﻿using Aero.MDH.DatabaseAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Aero.MDH.DatabaseAccess;

public partial class MdhDbContext : DbContext
{

    public virtual DbSet<Codification> Codifications { get; set; }

    public virtual DbSet<CompanyDatum> CompanyData { get; set; }

    public virtual DbSet<CompanyIdMaster> CompanyIdMasters { get; set; }

    public virtual DbSet<DealDatum> DealData { get; set; }

    public virtual DbSet<DealIdMaster> DealIdMasters { get; set; }

    public virtual DbSet<GlobalIdMaster> GlobalIdMasters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Codification>(entity =>
        {
            entity.HasKey(e => new { e.GlobalId, e.CodificationCode, e.VersionId });

            entity.ToTable("codification");

            entity.Property(e => e.GlobalId).HasColumnName("global_id");
            entity.Property(e => e.CodificationCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codification_code");
            entity.Property(e => e.VersionId).HasColumnName("version_id");
            entity.Property(e => e.AuditCreateDate)
                .HasColumnType("datetime")
                .HasColumnName("audit_create_date");
            entity.Property(e => e.AuditCreateUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("audit_create_user");
            entity.Property(e => e.AuditUpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("audit_update_date");
            entity.Property(e => e.AuditUpdateUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("audit_update_user");
            entity.Property(e => e.CodeNum).HasColumnName("code_num");
            entity.Property(e => e.CodeTxt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code_txt");

            entity.HasOne(d => d.Global).WithMany(p => p.Codifications)
                .HasForeignKey(d => d.GlobalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("codification_codification__fk");
        });

        modelBuilder.Entity<CompanyDatum>(entity =>
        {
            entity.HasKey(e => new { e.CompanyId, e.ValueDate, e.DataCode });

            entity.ToTable("company_data");

            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.ValueDate).HasColumnName("value_date");
            entity.Property(e => e.DataCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("data_code");
            entity.Property(e => e.AuditCreateDate)
                .HasColumnType("datetime")
                .HasColumnName("audit_create_date");
            entity.Property(e => e.AuditCreateUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("audit_create_user");
            entity.Property(e => e.AuditUpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("audit_update_date");
            entity.Property(e => e.AuditUpdateUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("audit_update_user");
            entity.Property(e => e.DataValueBit).HasColumnName("data_value_bit");
            entity.Property(e => e.DataValueDate).HasColumnName("data_value_date");
            entity.Property(e => e.DataValueInt).HasColumnName("data_value_int");
            entity.Property(e => e.DataValueTxt)
                .HasMaxLength(5000)
                .IsUnicode(false)
                .HasColumnName("data_value_txt");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyData)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_data_fk");
        });

        modelBuilder.Entity<CompanyIdMaster>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK_company");

            entity.ToTable("company_id_master");

            entity.Property(e => e.CompanyId)
                .ValueGeneratedNever()
                .HasColumnName("company_id");
            entity.Property(e => e.AuditCreateDate)
                .HasColumnType("datetime")
                .HasColumnName("audit_create_date");
            entity.Property(e => e.AuditCreateUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("audit_create_user");
            entity.Property(e => e.AuditUpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("audit_update_date");
            entity.Property(e => e.AuditUpdateUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("audit_update_user");
            entity.Property(e => e.CompanyType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("company_type");

            entity.HasOne(d => d.Company).WithOne(p => p.CompanyIdMaster)
                .HasForeignKey<CompanyIdMaster>(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("company_id_master__fk");
        });

        modelBuilder.Entity<DealDatum>(entity =>
        {
            entity.HasKey(e => new { e.DealId, e.ValueDate, e.DataCode });

            entity.ToTable("deal_data");

            entity.Property(e => e.DealId).HasColumnName("deal_id");
            entity.Property(e => e.ValueDate).HasColumnName("value_date");
            entity.Property(e => e.DataCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("data_code");
            entity.Property(e => e.AuditCreateDate)
                .HasColumnType("datetime")
                .HasColumnName("audit_create_date");
            entity.Property(e => e.AuditCreateUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("audit_create_user");
            entity.Property(e => e.AuditUpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("audit_update_date");
            entity.Property(e => e.AuditUpdateUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("audit_update_user");
            entity.Property(e => e.DataValueBit).HasColumnName("data_value_bit");
            entity.Property(e => e.DataValueDate).HasColumnName("data_value_date");
            entity.Property(e => e.DataValueInt).HasColumnName("data_value_int");
            entity.Property(e => e.DataValueTxt)
                .HasMaxLength(5000)
                .IsUnicode(false)
                .HasColumnName("data_value_txt");

            entity.HasOne(d => d.Deal).WithMany(p => p.DealData)
                .HasForeignKey(d => d.DealId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("deal_data_fk");
        });

        modelBuilder.Entity<DealIdMaster>(entity =>
        {
            entity.HasKey(e => e.DealId).HasName("PK_deal");

            entity.ToTable("deal_id_master");

            entity.Property(e => e.DealId)
                .ValueGeneratedNever()
                .HasColumnName("deal_id");
            entity.Property(e => e.AuditCreateDate)
                .HasColumnType("datetime")
                .HasColumnName("audit_create_date");
            entity.Property(e => e.AuditCreateUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("audit_create_user");
            entity.Property(e => e.AuditUpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("audit_update_date");
            entity.Property(e => e.AuditUpdateUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("audit_update_user");

            entity.HasOne(d => d.Deal).WithOne(p => p.DealIdMaster)
                .HasForeignKey<DealIdMaster>(d => d.DealId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("deal_id_master__fk");
        });

        modelBuilder.Entity<GlobalIdMaster>(entity =>
        {
            entity.HasKey(e => e.GlobalId);

            entity.ToTable("global_id_master");

            entity.Property(e => e.GlobalId).HasColumnName("global_id");
            entity.Property(e => e.AuditCreateDate)
                .HasColumnType("datetime")
                .HasColumnName("audit_create_date");
            entity.Property(e => e.AuditCreateUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("audit_create_user");
            entity.Property(e => e.AuditUpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("audit_update_date");
            entity.Property(e => e.AuditUpdateUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("audit_update_user");
            entity.Property(e => e.ObjectType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("object_type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
