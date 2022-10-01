using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OGA.InfraBase.Services
{
    public interface IConfigUpdateService
    {
        int Get_BuildData_Rev();
        void Set_BuildData_Rev(int revnumber);

        string Get_RepoType();
        void Set_RepoType(string repotype);

        string Get_RepoURL();
        void Set_RepoURL(string url);

        string Get_SolutionSubfolder();
        void Set_SolutionSubfolder(string url);
    }

    public class cConfigUpdateService : IConfigUpdateService
    {
        #region Private Fields

        private readonly NETCore_Common.Config.WriteableJSONConfig.IWritableOptions<OGA.SharedKernel.Config.structs.cConfig_AppPaths> _appSettings;

        private readonly NETCore_Common.Config.WriteableJSONConfig.IWritableOptions<OGA.SharedKernel.Config.structs.cConfig_BuildData> _builddata;

        private OGA.InfraBase.Services.IDBConfigService _confsvc;

        #endregion


        #region ctor / dtor

        public cConfigUpdateService(
                            NETCore_Common.Config.WriteableJSONConfig.IWritableOptions<OGA.SharedKernel.Config.structs.cConfig_AppPaths> appSettings,
                            NETCore_Common.Config.WriteableJSONConfig.IWritableOptions<OGA.SharedKernel.Config.structs.cConfig_BuildData> builddata,
                            OGA.InfraBase.Services.IDBConfigService confsvc)
        {
            this._confsvc = confsvc;

            this._appSettings = appSettings;

            this._builddata = builddata;
        }

        #endregion


        public int Get_BuildData_Rev()
        {
            try
            {
                return Convert.ToInt32(this._builddata.Value.Source_Revision);
            }
            catch (Exception e)
            {
                return -9999;
            }
        }
        public void Set_BuildData_Rev(int revnumber)
        {
            this._builddata.Update(opt =>
            {
                opt.Source_Revision = revnumber.ToString();
            });
        }

        public string Get_RepoType()
        {
            return this._builddata.Value.RepoType;
        }
        public void Set_RepoType(string repotype)
        {
            this._builddata.Update(opt =>
            {
                opt.RepoType = repotype;
            });
        }

        public string Get_RepoURL()
        {
            return this._builddata.Value.Source_URL;
        }
        public void Set_RepoURL(string url)
        {
            this._builddata.Update(opt =>
            {
                opt.Source_URL = url;
            });
        }

        public string Get_SolutionSubfolder()
        {
            return this._builddata.Value.Source_SolutionSubFolder;
        }
        public void Set_SolutionSubfolder(string folder)
        {
            this._builddata.Update(opt =>
            {
                opt.Source_SolutionSubFolder = folder;
            });
        }

        #region Private Methods

        #endregion
    }
}
