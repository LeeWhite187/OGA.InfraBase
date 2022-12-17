using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGA.InfraBase.Services
{
    /// <summary>
    /// Interface for the Config Service.
    /// Implementations of this, can access configuration that is stored in memory, file, database, or an API.
    /// </summary>
    public interface IConfigService
    {
        IEnumerable<OGA.DomainBase.Entities.ConfigElement_v1> GetAll();

        /// <summary>
        /// Returns the type of backing store: file, memory, database, REST, etc...
        /// </summary>
        string ProviderType { get; }

        int GetbyKey(string key, out bool val);
        int GetbyKey(string key, out int val);
        int GetbyKey(string key, out float val);
        int GetbyKey(string key, out string val);
        int GetbyKey(string key, out DateTime val);

        int SetValue(string key, bool val);
        int SetValue(string key, int val);
        int SetValue(string key, float val);
        int SetValue(string key, string val);
        int SetValue(string key, DateTime val);

        int Set_Defaults();

        void Delete(string key);
    }
}
