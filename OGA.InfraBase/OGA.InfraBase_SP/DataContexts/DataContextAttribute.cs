using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGA.InfraBase.DataContexts
{
    /// <summary>
    /// Marker attribute that allows us to locate data contexts and identify their backend types.
    /// All data contexts derived from cDBDContext_Base should be flagged with this attribute for easy location.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DataContextAttribute : Attribute
    {
        /// <summary>
        /// Proper Name of the provider.
        /// </summary>
        public string Provider { get; internal set; }
        /// <summary>
        /// Provider Name. Should match the command line argument.
        /// </summary>
        public string ShortName { get; internal set; }
        /// <summary>
        /// Name used by the configuration section.
        /// </summary>
        public string ConfigSectionName { get; internal set; }

        // Disable the default constructor...
        private DataContextAttribute()
        {
        }

        public DataContextAttribute(string provider, string shortname, string configname)
        {
            this.Provider = provider;
            this.ShortName = shortname;
            this.ConfigSectionName = configname;
        }
    }
}
