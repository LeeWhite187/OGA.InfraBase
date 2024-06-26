﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGA.InfraBase.Services
{
    /// <summary>
    /// Configuration Service Class with an EF-based backing store.
    /// This class .
    /// It can also be used for testing other services
    /// </summary>
    public class ConfigService_DB : IConfigService
    {
        static private string _classname = typeof(ConfigService_Mem).Name;

        private OGA.InfraBase.DataContexts.cDBDContext_Base _context;

        /// <summary>
        /// Returns the type of backing store: file, memory, database, REST, etc...
        /// </summary>
        public string ProviderType { get => "database"; }


        public ConfigService_DB(OGA.InfraBase.DataContexts.cDBDContext_Base context)
        {
            _context = context;
        }

        public IEnumerable<OGA.DomainBase.Entities.ConfigElement_v1> GetAll()
        {
            return _context.ConfigData;
        }

        // Returns the following:
        //  1 if found
        //  0 if not found
        //  -1 for errors
        public int GetbyKey(string key, out bool val)
        {
            try
            {
                var ce = _context.ConfigData.Find(key);
                if (ce == null)
                {
                    // Not found.

                    // Return unknown...
                    val = false;
                    return 0;
                }
                // If here, we got a value.

                // Get its value...
                if (ce.Value.ToLower() == "true")
                    val = true;
                if (ce.Value.ToLower() == "false")
                    val = false;
                else
                {
                    val = false;
                    return -1;
                }

                return 1;
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(GetbyKey)} - " +
                        "encountered an exception.");

                val = false;
                return -10;
            }
        }
        // Returns the following:
        //  1 if found
        //  0 if not found
        //  -1 for errors
        public int GetbyKey(string key, out int val)
        {
            try
            {
                var ce = _context.ConfigData.Find(key);
                if (ce == null)
                {
                    // Not found.

                    // Return unknown...
                    val = -9999;
                    return 0;
                }
                // If here, we got a value.

                try
                {
                    // Get its value...
                    val = Convert.ToInt32(ce.Value);
                    return 1;
                }
                catch (Exception)
                {
                    val = -9999;
                    return -1;
                }
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(GetbyKey)} - " +
                        "encountered an exception.");

                val = -9999;
                return -10;
            }
        }
        // Returns the following:
        //  1 if found
        //  0 if not found
        //  -1 for errors
        public int GetbyKey(string key, out float val)
        {
            try
            {
                var ce = _context.ConfigData.Find(key);
                if (ce == null)
                {
                    // Not found.

                    // Return unknown...
                    val = -9999.0f;
                    return 0;
                }
                // If here, we got a value.

                try
                {
                    // Get its value...
                    val = Convert.ToSingle(ce.Value);
                    return 1;
                }
                catch (Exception)
                {
                    val = -9999.0f;
                    return -1;
                }
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(GetbyKey)} - " +
                        "encountered an exception.");

                val = -9999.0f;
                return -10;
            }
        }
        // Returns the following:
        //  1 if found
        //  0 if not found
        //  -1 for errors
        public int GetbyKey(string key, out string val)
        {
            try
            {
                var ce = _context.ConfigData.Find(key);
                if (ce == null)
                {
                    // Not found.

                    // Return unknown...
                    val = "";
                    return 0;
                }
                // If here, we got a value.

                try
                {
                    // Get its value...
                    val = ce.Value;
                    return 1;
                }
                catch (Exception)
                {
                    val = "";
                    return -1;
                }
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(GetbyKey)} - " +
                        "encountered an exception.");

                val = "";
                return -10;
            }
        }
        // Returns the following:
        //  1 if found
        //  0 if not found
        //  -1 for errors
        public int GetbyKey(string key, out DateTime val)
        {
            try
            {
                var ce = _context.ConfigData.Find(key);
                if (ce == null)
                {
                    // Not found.

                    // Return unknown...
                    val = new DateTime();
                    return 0;
                }
                // If here, we got a value.

                try
                {
                    // Get its value...
                    val = DateTime.Parse(ce.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
                    return 1;
                }
                catch (Exception)
                {
                    val = new DateTime();
                    return -1;
                }
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(GetbyKey)} - " +
                        "encountered an exception.");

                val = new DateTime();
                return -10;
            }
        }

        public int SetValue(string key, bool val)
        {
            try
            {
                // See if the value is currently stored...
                var ce = _context.ConfigData.Find(key);
                if (ce == null)
                {
                    // Not found.

                    // We will add it to the database.
                    ce = new OGA.DomainBase.Entities.ConfigElement_v1();
                    ce.DataType = "bool";
                    ce.Key = key;
                    ce.Value = val.ToString();

                    this._context.ConfigData.Add(ce);

                    return 1;
                }
                else
                {
                    // If here, the key exists.
                    // We will update it instead.

                    ce.Value = val.ToString();
                    this._context.ConfigData.Update(ce);
                }

                _context.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(SetValue)} - " +
                        "encountered an exception.");

                return -10;
            }
        }
        public int SetValue(string key, int val)
        {
            try
            {
                // See if the value is currently stored...
                var ce = _context.ConfigData.Find(key);
                if (ce == null)
                {
                    // Not found.

                    // We will add it to the database.
                    ce = new OGA.DomainBase.Entities.ConfigElement_v1();
                    ce.DataType = "int";
                    ce.Key = key;
                    ce.Value = val.ToString();

                    this._context.ConfigData.Add(ce);
                }
                else
                {
                    // If here, the key exists.
                    // We will update it instead.

                    ce.Value = val.ToString();
                    this._context.ConfigData.Update(ce);
                }

                _context.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(SetValue)} - " +
                        "encountered an exception.");

                return -10;
            }
        }
        public int SetValue(string key, float val)
        {
            try
            {
                // See if the value is currently stored...
                var ce = _context.ConfigData.Find(key);
                if (ce == null)
                {
                    // Not found.

                    // We will add it to the database.
                    ce = new OGA.DomainBase.Entities.ConfigElement_v1();
                    ce.DataType = "float";
                    ce.Key = key;
                    ce.Value = val.ToString();

                    this._context.ConfigData.Add(ce);

                    return 1;
                }
                else
                {
                    // If here, the key exists.
                    // We will update it instead.

                    ce.Value = val.ToString();
                    this._context.ConfigData.Update(ce);
                }

                _context.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(SetValue)} - " +
                        "encountered an exception.");

                return -10;
            }
        }
        public int SetValue(string key, string val)
        {
            try
            {
                // See if the value is currently stored...
                var ce = _context.ConfigData.Find(key);
                if (ce == null)
                {
                    // Not found.

                    // We will add it to the database.
                    ce = new OGA.DomainBase.Entities.ConfigElement_v1();
                    ce.DataType = "string";
                    ce.Key = key;
                    ce.Value = val;

                    this._context.ConfigData.Add(ce);

                    return 1;
                }
                else
                {
                    // If here, the key exists.
                    // We will update it instead.

                    ce.Value = val;
                    this._context.ConfigData.Update(ce);
                }

                _context.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(SetValue)} - " +
                        "encountered an exception.");

                return -10;
            }
        }
        public int SetValue(string key, DateTime val)
        {
            try
            {
                /* Uses a special roundtrip serializer format for datatimes.
                 * Taken from here:
                 * https://markheath.net/post/roundtrip-serialization-of-datetimes-in
                 *  Console.WriteLine("Local: {0}", DateTime.Now.ToString("O"));
                 *  Console.WriteLine("UTC: {0}", DateTime.UtcNow.ToString("O"));
                 */

                // See if the value is currently stored...
                var ce = _context.ConfigData.Find(key);
                if (ce == null)
                {
                    // Not found.

                    // We will add it to the database.
                    ce = new OGA.DomainBase.Entities.ConfigElement_v1();
                    ce.DataType = "DateTime";
                    ce.Key = key;
                    ce.Value = val.ToUniversalTime().ToString("O");

                    this._context.ConfigData.Add(ce);
                }
                else
                {
                    // If here, the key exists.
                    // We will update it instead.

                    ce.Value = val.ToUniversalTime().ToString("O");
                    this._context.ConfigData.Update(ce);
                }

                _context.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(SetValue)} - " +
                        "encountered an exception.");

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
                this._context.SaveChanges();

                return 1;
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(Set_Defaults)} - " +
                        "encountered an exception.");

                return -10;
            }
        }

#if NET5
        #nullable enable
#endif
        protected virtual object? Get_Default(string key)
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
                var cd = _context.ConfigData.Find(key);
                if (cd != null)
                {
                    _context.ConfigData.Remove(cd);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                OGA.SharedKernel.Logging_Base.Logger_Ref?.Error(e, $"{_classname}:{nameof(Delete)} - " +
                        "encountered an exception.");

                return;
            }
        }
    }
}
