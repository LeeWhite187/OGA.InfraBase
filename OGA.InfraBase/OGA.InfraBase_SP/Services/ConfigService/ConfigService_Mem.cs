using OGA.DomainBase.Entities;
using OGA.InfraBase.DataContexts;
using OGA.SharedKernel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGA.InfraBase.Services
{
    /// <summary>
    /// Configuration Service Class with no persistent backing store.
    /// This class can be used as a caching service, and for holding configuration that was passed to it at runtime.
    /// It can also be used for testing other services
    /// </summary>
    public class ConfigService_Mem : IConfigService
    {
        static private string _classname = typeof(ConfigService_Mem).Name;

        private Dictionary<string, ConfigElement_v1> _mockdata;


        /// <summary>
        /// Returns the type of backing store: file, memory, database, REST, etc...
        /// </summary>
        public string ProviderType { get => "memory"; }

        public ConfigService_Mem()
        {
            _mockdata = new Dictionary<string, ConfigElement_v1>();
        }

        public IEnumerable<ConfigElement_v1> GetAll()
        {
            return _mockdata.Values.ToList();
        }

        public int GetbyKey(string key, out bool val)
        {
            try
            {
                if(!_mockdata.TryGetValue(key, out ConfigElement_v1 configElement_v))
                {
                    val = false;
                    return 0;
                }

                if (configElement_v.Value.ToLower() == "true")
                {
                    val = true;
                }

                if (configElement_v.Value.ToLower() == "false")
                {
                    val = false;
                    return 1;
                }

                val = false;
                return -1;
            }
            catch (Exception exception)
            {
                Logging_Base.Logger_Ref?.Error(exception, $"{_classname}:{nameof(GetbyKey)} - encountered an exception.");
                val = false;
                return -10;
            }
        }

        public int GetbyKey(string key, out int val)
        {
            try
            {
                if(!_mockdata.TryGetValue(key, out ConfigElement_v1 configElement_v))
                {
                    val = -9999;
                    return 0;
                }

                try
                {
                    val = Convert.ToInt32(configElement_v.Value);
                    return 1;
                }
                catch (Exception)
                {
                    val = -9999;
                    return -1;
                }
            }
            catch (Exception exception)
            {
                Logging_Base.Logger_Ref?.Error(exception, $"{_classname}:{nameof(GetbyKey)} - encountered an exception.");
                val = -9999;
                return -10;
            }
        }

        public int GetbyKey(string key, out float val)
        {
            try
            {
                if(!_mockdata.TryGetValue(key, out ConfigElement_v1 configElement_v))
                {
                    val = -9999f;
                    return 0;
                }

                try
                {
                    val = Convert.ToSingle(configElement_v.Value);
                    return 1;
                }
                catch (Exception)
                {
                    val = -9999f;
                    return -1;
                }
            }
            catch (Exception exception)
            {
                Logging_Base.Logger_Ref?.Error(exception, $"{_classname}:{nameof(GetbyKey)} - encountered an exception.");
                val = -9999f;
                return -10;
            }
        }

        public int GetbyKey(string key, out string val)
        {
            try
            {
                if(!_mockdata.TryGetValue(key, out ConfigElement_v1 configElement_v))
                {
                    val = "";
                    return 0;
                }

                try
                {
                    val = configElement_v.Value;
                    return 1;
                }
                catch (Exception)
                {
                    val = "";
                    return -1;
                }
            }
            catch (Exception exception)
            {
                Logging_Base.Logger_Ref?.Error(exception, $"{_classname}:{nameof(GetbyKey)} - encountered an exception.");
                val = "";
                return -10;
            }
        }

        public int GetbyKey(string key, out DateTime val)
        {
            try
            {
                if(!_mockdata.TryGetValue(key, out ConfigElement_v1 configElement_v))
                {
                    val = default(DateTime);
                    return 0;
                }

                try
                {
                    val = DateTime.Parse(configElement_v.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
                    return 1;
                }
                catch (Exception)
                {
                    val = default(DateTime);
                    return -1;
                }
            }
            catch (Exception exception)
            {
                Logging_Base.Logger_Ref?.Error(exception, $"{_classname}:{nameof(GetbyKey)} - encountered an exception.");
                val = default(DateTime);
                return -10;
            }
        }

        public int SetValue(string key, bool val)
        {
            try
            {
                if(!_mockdata.TryGetValue(key, out ConfigElement_v1 configElement_v))
                {
                    configElement_v = new ConfigElement_v1();
                    configElement_v.DataType = "bool";
                    configElement_v.Key = key;
                    configElement_v.Value = val.ToString();
                    _mockdata.Add(key, configElement_v);
                    return 1;
                }

                configElement_v.Value = val.ToString();
                _mockdata[key] = configElement_v;
                return 1;
            }
            catch (Exception exception)
            {
                Logging_Base.Logger_Ref?.Error(exception, $"{_classname}:SetValue - encountered an exception.");
                return -10;
            }
        }

        public int SetValue(string key, int val)
        {
            try
            {
                if(!_mockdata.TryGetValue(key, out ConfigElement_v1 configElement_v))
                {
                    configElement_v = new ConfigElement_v1();
                    configElement_v.DataType = "int";
                    configElement_v.Key = key;
                    configElement_v.Value = val.ToString();
                    _mockdata.Add(key, configElement_v);
                }
                else
                {
                    configElement_v.Value = val.ToString();
                    _mockdata.Add(key, configElement_v);
                }

                return 1;
            }
            catch (Exception exception)
            {
                Logging_Base.Logger_Ref?.Error(exception, $"{_classname}:{nameof(SetValue)} - encountered an exception.");
                return -10;
            }
        }

        public int SetValue(string key, float val)
        {
            try
            {
                if(!_mockdata.TryGetValue(key, out ConfigElement_v1 configElement_v))
                {
                    configElement_v = new ConfigElement_v1();
                    configElement_v.DataType = "float";
                    configElement_v.Key = key;
                    configElement_v.Value = val.ToString();
                    _mockdata.Add(key, configElement_v);
                    return 1;
                }

                configElement_v.Value = val.ToString();
                _mockdata[key] = configElement_v;
                return 1;
            }
            catch (Exception exception)
            {
                Logging_Base.Logger_Ref?.Error(exception, $"{_classname}:{nameof(SetValue)} - encountered an exception.");
                return -10;
            }
        }

        public int SetValue(string key, string val)
        {
            try
            {
                if(!_mockdata.TryGetValue(key, out ConfigElement_v1 configElement_v))
                {
                    configElement_v = new ConfigElement_v1();
                    configElement_v.DataType = "string";
                    configElement_v.Key = key;
                    configElement_v.Value = val;
                    _mockdata.Add(key, configElement_v);
                    return 1;
                }

                configElement_v.Value = val;
                _mockdata[key] = configElement_v;
                return 1;
            }
            catch (Exception exception)
            {
                Logging_Base.Logger_Ref?.Error(exception, $"{_classname}:{nameof(SetValue)} - encountered an exception.");
                return -10;
            }
        }

        public int SetValue(string key, DateTime val)
        {
            try
            {
                if(!_mockdata.TryGetValue(key, out ConfigElement_v1 configElement_v))
                {
                    configElement_v = new ConfigElement_v1();
                    configElement_v.DataType = "DateTime";
                    configElement_v.Key = key;
                    configElement_v.Value = val.ToUniversalTime().ToString("O");
                    _mockdata.Add(key, configElement_v);
                }
                else
                {
                    configElement_v.Value = val.ToUniversalTime().ToString("O");
                    _mockdata[key] = configElement_v;
                }

                return 1;
            }
            catch (Exception exception)
            {
                Logging_Base.Logger_Ref?.Error(exception, $"{_classname}:{nameof(SetValue)} - encountered an exception.");
                return -10;
            }
        }

        public virtual int Set_Defaults()
        {
            try
            {
                //string keyname = "";
                //int intval = 0;
                //string stringval = "";

                // Add defaults like this...
                //{
                //    keyname = DBConfig_Constants.CONST_ConfigKey_AuthToken_ExpiryDuration;
                //    if (GetbyKey(keyname, out intval) != 1)
                //    {
                //        // not set.
                //        // Set it now...
                //        int val = ((int)Get_Default(keyname));
                //        SetValue(keyname, val);
                //    }
                //}

                // Add more default settings here...


                // Tell the dbcontext to save updates...
                //this._context.SaveChanges();

                return 1;
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(Set_Defaults)} - " +
                        "encountered an exception.");

                return -10;
            }
        }
        protected virtual object Get_Default(string key)
        {
            return null;
            // Add defaults like this....
            //if (key.ToLower() == DBConfig_Constants.CONST_ConfigKey_AuthToken_ExpiryDuration)
            //{
            //    // Default token expiry to 15 minutes...
            //    return 5;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public void Delete(string key)
        {
            try
            {
                if(_mockdata.TryGetValue(key, out ConfigElement_v1 configElement_v))
                {
                    _mockdata.Remove(key);
                }
            }
            catch (Exception exception)
            {
                Logging_Base.Logger_Ref?.Error(exception, $"{_classname}:{nameof(Delete)} - encountered an exception.");
            }
        }
    }
}
