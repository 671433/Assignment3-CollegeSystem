using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Wpf_Test2ScofFolding.Models;

namespace Wpf_Test2ScofFolding.Data;

public partial class ApplicationDBContext : DbContext
{
    public ApplicationDBContext()
    {
    }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("data source=dat154demo.database.windows.net;Initial Catalog=dat154;User ID=dat154_rw;Password=Svart_Katt;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Coursecode).HasName("pk_course");

            entity.Property(e => e.Coursecode).IsFixedLength();
            entity.Property(e => e.Semester).IsFixedLength();
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => new { e.Coursecode, e.Studentid }).HasName("pk_grade");

            entity.Property(e => e.Coursecode).IsFixedLength();
            entity.Property(e => e.Grade1).IsFixedLength();

            entity.HasOne(d => d.CoursecodeNavigation).WithMany(p => p.Grades)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_course");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_student");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Primary");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
