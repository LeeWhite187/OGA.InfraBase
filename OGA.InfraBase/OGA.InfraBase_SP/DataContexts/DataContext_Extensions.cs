using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OGA.InfraBase.DataContexts
{
    /*  Data Context Extension for Key Value Pair Storage
        This class used to extend a Data Context so that it can provide storage for key-value pairs.
        This particular instance uses the storage entity:

            public class cStorageBaseEntity
            {
                public DateTime CreationDate { get; set; }
                public DateTime ModifiedDate { get; set; }
            }
     
        Taken from here: https://stackoverflow.com/questions/25420477/entity-framework-can-i-map-a-class-to-a-key-value-table 
     */
    /* Sample usage as follows:
        using (var db = new AppContext())
        {
            db.SetStoreConfiguration("DefaultpartnerID", 1);
            db.SaveChanges();
        }

        using (var db = new AppContext())
        {
            var defaultpartnerID = db.GetStoreConfiguration<int>("DefaultpartnerID");
            db.SaveChanges();
        }
     */

    public static class StoreConfigurationExtension
    {
        public static T? GetStoreConfiguration<T>(this OGA.InfraBase.DataContexts.cDBDContext_Base db, string key)
        {
            try
            {
                // Attempt to retrieve the value object...
                var sc = db.Set<OGA.DomainBase.Entities.ConfigElement_v1>().Find(key);
                // Ensure it isn't null...
                if (sc == null)
                    return default(T);

                // Get the friendly type string...
                string objtype = OGA.SharedKernel.Serialization.Serialization_Helper.GetType_forSerialization(typeof(T));

                // Check that the type is a match...
                if (sc.DataType != objtype)
                {
                    // Cannot hydrate type.

                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Error($"StoreConfigurationExtension:{nameof(GetStoreConfiguration)} - " +
                        $"Encountered a datatype mismatch. Generic type is ({objtype}), but stored type is ({(sc?.DataType ?? "")}).");

                    return default(T);
                }
                // The types match.
                // We can convert.

                // Get the raw string...
                var val = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(sc.Value);
                if(val == null)
                {
                    // Was null.

                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Error($"StoreConfigurationExtension:{nameof(GetStoreConfiguration)} - " +
                        $"Failed to deserialize value. Generic type is ({typeof(T).Name}), but stored type is ({(sc?.DataType ?? "")}).");

                    return default(T);
                }
                // Got a value.

                return val;
            }
            catch (NotSupportedException nse)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(nse,
                    $"StoreConfigurationExtension:{nameof(GetStoreConfiguration)} - " +
                    $"NotSupportedException occurred while trying to retrieve value for key: {(key ?? "")}.");

                return default(T);
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e,
                    $"StoreConfigurationExtension:{nameof(GetStoreConfiguration)} - " +
                    $"Exception occurred while trying to retrieve value for key: {(key ?? "")}.");

                return default(T);
            }
        }

        public static void SetStoreConfiguration(this OGA.InfraBase.DataContexts.cDBDContext_Base db, string key, object value)
        {
            try
            {
                // Do nothing if key is empty...
                if (string.IsNullOrEmpty(key))
                    return;

                // Ensure the given value is not null...
                if(value == null)
                {
                    OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(
                        $"StoreConfigurationExtension:{nameof(GetStoreConfiguration)} - " +
                        $"Given value is null. Cannot store value for key: {(key ?? "")}.");

                    return;
                }

                // Get the object's type...
                string objtype = OGA.SharedKernel.Serialization.Serialization_Helper.GetType_forSerialization(value);

                // Locate the entry...
                var sc = db.Set<OGA.DomainBase.Entities.ConfigElement_v1>().Find(key);
                // Check if we found anything...
                if (sc == null)
                {
                    // Not found.
                    // We will create the entry.

                    // Create a new entry...
                    sc = new OGA.DomainBase.Entities.ConfigElement_v1();
                    sc.Value = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                    sc.DataType = objtype;
                    sc.Key = key;
                    db.Set<OGA.DomainBase.Entities.ConfigElement_v1>().Add(sc);

                    return;
                }
                // Found it.

                sc.Value = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                sc.DataType = OGA.SharedKernel.Serialization.Serialization_Helper.GetType_forSerialization(value);

                db.Set<OGA.DomainBase.Entities.ConfigElement_v1>().Update(sc);
            }
            catch (NotSupportedException nse)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(nse,
                    $"StoreConfigurationExtension:{nameof(GetStoreConfiguration)} - " +
                    $"NotSupportedException occurred while trying to retrieve value for key: {(key ?? "")}.");

                return;
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e,
                    $"StoreConfigurationExtension:{nameof(GetStoreConfiguration)} - " +
                    $"Exception occurred while trying to retrieve value for key: {(key ?? "")}.");

                return;
            }
        }
    }
}