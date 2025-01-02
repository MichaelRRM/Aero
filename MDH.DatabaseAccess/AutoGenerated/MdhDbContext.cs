using System;
using System.Collections.Generic;
using MDH.DatabaseAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace MDH.DatabaseAccess;

public partial class MdhDbContext : DbContext
{
    public MdhDbContext(DbContextOptions<MdhDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DealDatum> DealData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DealDatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("deal_data");

            entity.Property(e => e.DataCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("data_code");
            entity.Property(e => e.DataValueBit).HasColumnName("data_value_bit");
            entity.Property(e => e.DataValueInt).HasColumnName("data_value_int");
            entity.Property(e => e.DataValueNum)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("data_value_num");
            entity.Property(e => e.DataValueTxt)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("data_value_txt");
            entity.Property(e => e.DealId).HasColumnName("deal_id");
            entity.Property(e => e.ValueDate).HasColumnName("value_date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
