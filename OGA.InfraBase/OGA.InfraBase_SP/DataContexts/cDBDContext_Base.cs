using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace OGA.InfraBase.DataContexts
{
    public class cDBDContext_Base : DbContext
    {
        protected string _classname;
        public string DatabaseType = "";

        public cDBDContext_Base([NotNullAttribute] DbContextOptions options) : base(options)
        // public cDBDContext_Base() : base()
        {
            OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("cDBDContext_Base:DataContext - started.");

            this._classname = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;

            OGA.SharedKernel.Logging_Base.Logger_Ref?.Debug(
                "NLog injected into " + _classname);

            OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("cDBDContext_Base:DataContext - completed.");
        }
        public cDBDContext_Base() : base()
        {
            OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("cDBDContext_Base:DataContext - started.");

            this._classname = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;

            OGA.SharedKernel.Logging_Base.Logger_Ref?.Debug(
                "NLog injected into " + _classname);

            OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("cDBDContext_Base:DataContext - completed.");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Iterate all assemblies, and search for types that implement IEntityTypeConfiguration, and register each one.
            var asl = NETCore_Common.Process.cAssemblyHelper.Get_All_Assemblies();
            foreach (var fff in asl)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(fff);
            }

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is NETCore_Common.Entities.cStorageBaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                // Update the changed data for all entries...
                ((NETCore_Common.Entities.cStorageBaseEntity)entityEntry.Entity).ModifiedDateUTC = DateTime.Now.ToUniversalTime();

                // Set the creation data for any new entries...
                if (entityEntry.State == EntityState.Added)
                {
                    ((NETCore_Common.Entities.cStorageBaseEntity)entityEntry.Entity).CreationDateUTC = DateTime.Now.ToUniversalTime();
                }
            }

            return base.SaveChanges();
        }

        public DbSet<OGA.DomainBase.Entities.ConfigElement_v1> ConfigData { get; set; }
    }
}
