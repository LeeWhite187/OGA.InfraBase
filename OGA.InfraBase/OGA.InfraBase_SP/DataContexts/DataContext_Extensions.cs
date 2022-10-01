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
        public static T GetStoreConfiguration<T>(this OGA.InfraBase.DataContexts.cDBDContext_Base db, string key)
        {
            var sc = db.Set<OGA.DomainBase.Entities.ConfigElement_v1>().Find(key);
            if (sc == null) return default(T);

            var value = sc.Value;
            var tc = TypeDescriptor.GetConverter(typeof(T));
            try
            {
                var convertedValue = (T)tc.ConvertFromString(value);
                return convertedValue;
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }

        public static void SetStoreConfiguration(this OGA.InfraBase.DataContexts.cDBDContext_Base db, string key, object value)
        {
            var sc = db.Set<OGA.DomainBase.Entities.ConfigElement_v1>().Find(key);
            if (sc == null)
            {
                sc = new OGA.DomainBase.Entities.ConfigElement_v1 { Key = key };
                db.Set<OGA.DomainBase.Entities.ConfigElement_v1>().Add(sc);
            }

            sc.Value = value == null ? null : value.ToString();
        }
    }
}