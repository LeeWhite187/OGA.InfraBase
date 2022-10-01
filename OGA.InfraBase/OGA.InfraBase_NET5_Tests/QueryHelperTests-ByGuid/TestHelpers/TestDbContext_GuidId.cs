﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGA.DomainBase_Tests
{
    public class TestDbContext_GuidId : DbContext
    {
        public TestDbContext_GuidId([NotNullAttribute] DbContextOptions options) : base(options)
        // public cDBDContext_Base() : base()
        {
        }

        public TestDbContext_GuidId() : base()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //// Iterate all assemblies, and search for types that implement IEntityTypeConfiguration, and register each one.
            //var asl = NETCore_Common.Process.cAssemblyHelper.Get_All_Assemblies();
            //foreach (var fff in asl)
            //{
            //    modelBuilder.ApplyConfigurationsFromAssembly(fff);
            //}

            //base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            //var entries = ChangeTracker
            //    .Entries()
            //    .Where(e => e.Entity is NETCore_Common.Entities.cStorageBaseEntity && (
            //            e.State == EntityState.Added
            //            || e.State == EntityState.Modified));

            //foreach (var entityEntry in entries)
            //{
            //    // Update the changed data for all entries...
            //    ((NETCore_Common.Entities.cStorageBaseEntity)entityEntry.Entity).ModifiedDateUTC = DateTime.Now.ToUniversalTime();

            //    // Set the creation data for any new entries...
            //    if (entityEntry.State == EntityState.Added)
            //    {
            //        ((NETCore_Common.Entities.cStorageBaseEntity)entityEntry.Entity).CreationDateUTC = DateTime.Now.ToUniversalTime();
            //    }
            //}

            return base.SaveChanges();
        }

        public DbSet<TestClass_GuidId> TestData { get; set; }
    }
}
