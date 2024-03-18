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
    /// Wraps the base db context class to provide late-bound entity mapping and date setting on save.
    /// To, use, derive a context class from this base that includes your DbSet props. Then, derive a datastore-specific type from that for each storage type you have: PostGres, MSSQL, InMem, etc...
    /// NOTE: The OnModelCreating override, in this class, makes use of AssemblyHelper to prescreen and compose a list of assemblies where IEntityTypeConfiguration<> implementations can be found.
    /// NOTE: So, be sure, during early process startup, to set the AssemblyHelper_Base.AssemblyHelperRef with a valid AssemblyHelper instance.
    /// </summary>
    public class cDBDContext_Base : DbContext
    {
        #region Private Fields

        protected string _classname;
        public string DatabaseType = "";

        #endregion


        #region Public Properties

        /// <summary>
        /// Behavior property that affects whether or not, this class and derivatives will pull in all implementations of IEntityTypeConfiguration<>, from the process's referenced assemblies.
        /// If not aware, these are the custom mapping models that tell EF how to map entity properties to table columns, what value conversions to perform, etc.
        /// This property (behavior) is enabled by default.
        /// If you don't want this behavior to occur in your implementation, set this property to false in the constructor of the derived class.
        /// NOTE: Leaving this enabled doesn't cause trouble for stray mappings to be pulled into a database context, because only the mappings of a DbSet will be used.
        /// </summary>
        public bool Cfg_LoadAllMapConfigsfromAssemblies { get; set; } = true;

        #endregion


        #region Data Sets

        public DbSet<OGA.DomainBase.Entities.ConfigElement_v1> ConfigData { get; set; } = null!;

        #endregion


        #region ctor / dtor

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

        #endregion


        #region Public Methods

        /// <summary>
        /// Will set on save, the creationdate and modifieddate as needed for any entity types that derive from cStorageBaseEntity.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Will set on save, the creationdate and modifieddate as needed for any entity types that derive from cStorageBaseEntity.
        /// </summary>
        /// <returns></returns>
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

        #endregion


        #region Private Methods

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
        /// If this is the case, override this method (in your derived class) to be blank and not call the base, and assign individual value converters in the appropriate model builder instances.
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

        /// <summary>
        /// This override retrieves all the IEntityTypeConfiguration<> implementations in process assemblies, and makes them available as entity mappings.
        /// If you don't want this behavior, set Cfg_LoadAllMapConfigsfromAssemblies = false in your derived class' constructor.
        /// You can 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Search for IEntityTypeConfiguration<> implementations if required...
            if(Cfg_LoadAllMapConfigsfromAssemblies)
            {
                // Iterate all assemblies, and search for types that implement IEntityTypeConfiguration, and register each one.
                var asl = OGA.SharedKernel.Process.AssemblyHelper_Base.AssemblyHelperRef.Get_All_Assemblies();
                foreach (var fff in asl)
                {
                    modelBuilder.ApplyConfigurationsFromAssembly(fff);
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
