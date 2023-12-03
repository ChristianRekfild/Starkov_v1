using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace Starkov_v1;

public partial class StarkovContext : DbContext
{
    public StarkovContext()
    {
    }

    public StarkovContext(DbContextOptions<StarkovContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<JobTitle> JobTitles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connString = string.Empty;

        XmlDocument xDoc = new XmlDocument();
        string path = Directory.GetCurrentDirectory() + "\\Config\\Configuration.xml";
        xDoc.Load(path);

        XmlElement? xRoot = xDoc.DocumentElement;
        if (xRoot != null)
        {
            // обход всех узлов в корневом элементе
            foreach (XmlElement xnode in xRoot)
            {
                // получаем атрибут name
                XmlNode? attr = xnode.Attributes.GetNamedItem("ConnectionString");
                connString = attr?.Value;

                int a = 0;
                a++;
            }
        }

                optionsBuilder.UseNpgsql(connString);
    }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Department_pkey");

            entity.ToTable("Department");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);

            entity.HasOne(d => d.Manager).WithMany(p => p.Departments)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("Manager");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("Parent");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Employee_pkey");

            entity.ToTable("Employee");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Fullname).HasMaxLength(100);
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(150);

            entity.HasOne(d => d.JobTitle).WithMany(p => p.Employees)
                .HasForeignKey(d => d.JobTitleId)
                .HasConstraintName("JobTitle");
        });

        modelBuilder.Entity<JobTitle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("JobTitle_pkey");

            entity.ToTable("JobTitle");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
