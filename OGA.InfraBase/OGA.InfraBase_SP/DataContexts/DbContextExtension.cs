using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OGA.InfraBase.DataContexts
{
    public static class DbContextExtension
    {
        public static int AllMigrationsApplied(this OGA.InfraBase.DataContexts.cDBDContext_Base context)
        {
            try
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:AllMigrationsApplied - triggered.");

                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:AllMigrationsApplied - getting a list of migrations in the database...");

                var rephist = context.GetService<IHistoryRepository>();

                // Get a list of migrations from the database...
                var applied = rephist.GetAppliedMigrations().Select(m => m.MigrationId);

                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:AllMigrationsApplied - have a list of migrations in the database.");

                // Push applied migrations to the log...
                {
                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:AllMigrationsApplied - dumping database migrations to log...");

                    int index = 0;
                    string msg = "";
                    foreach (var s in applied)
                    {
                        msg = "     Database Migration (" + index.ToString() + ") : Migration ID = " + s + ";\r\n";
                        index++;
                    }
                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info(msg);

                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:AllMigrationsApplied - end of database migrations list.");
                }

                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:AllMigrationsApplied - getting a list of migrations in the codebase...");

                // Get a list of codebase assemblies containing database migrations...
                var migassy = context.GetService<IMigrationsAssembly>();
                // Get a list of migrations in the codebase...
                var total = migassy.Migrations.Select(m => m.Key);

                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:AllMigrationsApplied - have a list of migrations across the codebase.");

                // Push code-side migrations to the log...
                {
                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:AllMigrationsApplied - dumping codebase migrations to log...");

                    int index = 0;
                    string msg = "";
                    foreach (var s in migassy.Migrations)
                    {
                        msg = "     Codebase Migration (" + index.ToString() + ") : Migration Key = " + s.Key + "; " + "Migration Class = " + s.Value.FullName + ";\r\n";
                        index++;
                    }
                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info(msg);

                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:AllMigrationsApplied - end of codebase migrations list.");
                }

                // Checking if the database has all migrations...
                bool res = total.Except(applied).Any();
                if(res)
                {
                    // There are migrations in the codebase that aren't yet applied to the database.

                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:AllMigrationsApplied - codebase has migrations that aren't yet applied to the database.");

                    return 0;
                }
                // All codebase migrations have in the database.

                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:AllMigrationsApplied - all codebase migrations are in the database.");

                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:AllMigrationsApplied - completed.");

                return 1;

                //return !total.Except(applied).Any();
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, "DbContextExtension:AllMigrationsApplied - exception occurred.");

                return -2;
            }
        }
        public static int Get_Pending_Migrations(this OGA.InfraBase.DataContexts.cDBDContext_Base context, out List<string> pending_migrations)
        {
            pending_migrations = new List<string>();

            try
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - triggered.");

                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - getting a list of migrations in the database...");

                var rephist = context.GetService<IHistoryRepository>();

                // Get a list of migrations from the database...
                var applied = rephist.GetAppliedMigrations().Select(m => m.MigrationId);

                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - have a list of migrations in the database.");

                // Push applied migrations to the log...
                {
                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - dumping database migrations to log...");

                    int index = 0;
                    string msg = "";
                    foreach (var s in applied)
                    {
                        msg = "     Database Migration (" + index.ToString() + ") : Migration ID = " + s + ";\r\n";
                        index++;
                    }
                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info(msg);

                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - end of database migrations list.");
                }

                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - getting a list of migrations in the codebase...");

                // Get a list of codebase assemblies containing database migrations...
                var migassy = context.GetService<IMigrationsAssembly>();
                // Get a list of migrations in the codebase...
                var total = migassy.Migrations.Select(m => m.Key);

                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - have a list of migrations across the codebase.");

                // Push code-side migrations to the log...
                {
                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - dumping codebase migrations to log...");

                    int index = 0;
                    string msg = "";
                    foreach (var s in migassy.Migrations)
                    {
                        msg = "     Codebase Migration (" + index.ToString() + ") : Migration Key = " + s.Key + "; " + "Migration Class = " + s.Value.FullName + ";\r\n";
                        index++;
                    }
                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info(msg);

                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - end of codebase migrations list.");
                }

                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - checking for missing migrations...");

                // Get a list of what's not been applied to the database...
                var pending = total.Except(applied);
                if(pending == null)
                {
                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - the database contains all codebase migrations.");
                }
                else
                {
                    if (pending.Count() == 0)
                    {
                        OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - the database contains all codebase migrations.");
                    }
                    else
                    {
                        // There is at least one outstanding migration the database does not have.
                        // Dump a list of pending migrations to the log...
                        {
                            string msg = "Data is missing the following migrations:\r\n";
                            int index = 0;
                            foreach (var s in migassy.Migrations)
                            {
                                // See if this migration is in the applied list...
                                if(!applied.Contains(s.Key))
                                {
                                    // The 
                                    msg = "     Codebase Migration (" + index.ToString() + ") : Migration Key = " + s.Key + "; " + "Migration Class = " + s.Value.FullName + ";\r\n";
                                    index++;
                                }
                            }
                            OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(msg);
                        }

                        // Push the pending list to our output...
                        foreach(var s in pending)
                        {
                            pending_migrations.Add(s);
                        }
                    }
                }

                OGA.SharedKernel.Logging_Base.Logger_Ref?.Info("DbContextExtension:Get_Pending_Migrations - completed.");

                return 1;

                //return !total.Except(applied).Any();
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, "DbContextExtension:Get_Pending_Migrations - exception occurred.");

                return -2;
            }
        }
    }
}