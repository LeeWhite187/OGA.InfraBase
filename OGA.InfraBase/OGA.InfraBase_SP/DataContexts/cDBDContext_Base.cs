using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;

namespace OGA.InfraBase.DataContexts
{
    /// <summary>
    /// 
    /// </summary>
    public class cDBDContext_Base : DbContext
    {
        protected string _classname;
        public string DatabaseType = "";

        public cDBDContext_Base([NotNullAttribute] DbContextOptions options) : base(options)
        // public cDBDContext_Base() : base()
        {
            OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("cDBDContext_Base:DataContext - started.");

            this._classname = System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? "";

            OGA.SharedKernel.Logging_Base.Logger_Ref?.Debug(
                "NLog injected into " + _classname);

            OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("cDBDContext_Base:DataContext - completed.");
        }
        public cDBDContext_Base() : base()
        {
            OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("cDBDContext_Base:DataContext - started.");

            this._classname = System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? "";

            OGA.SharedKernel.Logging_Base.Logger_Ref?.Debug(
                "NLog injected into " + _classname);

            OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("cDBDContext_Base:DataContext - completed.");
        }

#if NET5
        // NOTE: The convention converter in this block is only available in EF6 and forward.
        // So, EF5 usage will require individual value converters for each DateTime property in the model builder logic of each entity.
#else
        // NOTE: The convention converter in this block is only available in EF6 and forward.
        // So, EF5 usage will require individual value converters for each DateTime property in the model builder logic of each entity.


        /// <summary>
        /// This was added to globally retrieve all stored DateTime properties with their UTC flag set.
        /// If your implementation of classes has a mix of UTC and local time properties, you will need to be
        /// more surgical, and use individual value converters instead of this override.
        /// If this is the case, comment out this method, and assign individual value converters in the appropriate model builder instances.
        /// See this usage wiki: https://oga.atlassian.net/wiki/spaces/~311198967/pages/66322433/EF+Working+with+DateTime
        /// </summary>
        /// <param name="configurationBuilder"></param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<DateTime>()
                .HaveConversion<DateTimeUTCConverter>();
        }
#endif

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Iterate all assemblies, and search for types that implement IEntityTypeConfiguration, and register each one.
            var asl = OGA.SharedKernel.Process.AssemblyHelper_Base.AssemblyHelperRef.Get_All_Assemblies();
            foreach (var fff in asl)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(fff);
            }

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is OGA.DomainBase.Entities.cStorageBaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                // Update the changed data for all entries...
                ((OGA.DomainBase.Entities.cStorageBaseEntity)entityEntry.Entity).ModifiedDateUTC = DateTime.Now.ToUniversalTime();

                // Set the creation data for any new entries...
                if (entityEntry.State == EntityState.Added)
                {
                    ((OGA.DomainBase.Entities.cStorageBaseEntity)entityEntry.Entity).CreationDateUTC = DateTime.Now.ToUniversalTime();
                }
            }

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is OGA.DomainBase.Entities.cStorageBaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                // Update the changed data for all entries...
                ((OGA.DomainBase.Entities.cStorageBaseEntity)entityEntry.Entity).ModifiedDateUTC = DateTime.Now.ToUniversalTime();

                // Set the creation data for any new entries...
                if (entityEntry.State == EntityState.Added)
                {
                    ((OGA.DomainBase.Entities.cStorageBaseEntity)entityEntry.Entity).CreationDateUTC = DateTime.Now.ToUniversalTime();
                }
            }

            return base.SaveChanges();
        }

        public DbSet<OGA.DomainBase.Entities.ConfigElement_v1> ConfigData { get; set; } = null!;
    }
}
