using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OGA.InfraBase.Services
{
    public interface IConfigUpdateService
    {
        string Get_BuildData_Rev();
        void Set_BuildData_Rev(string revnumber);

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

        private readonly OGA.AppSettings.Writeable.JSONConfig.IWritableOptions<OGA.SharedKernel.Config.structs.Config_AppPaths_v2> _appSettings;

        private readonly OGA.AppSettings.Writeable.JSONConfig.IWritableOptions<OGA.SharedKernel.Config.structs.Config_BuildData> _builddata;

        private OGA.InfraBase.Services.IConfigService _confsvc;

        #endregion


        #region ctor / dtor

        public cConfigUpdateService(
                            OGA.AppSettings.Writeable.JSONConfig.IWritableOptions<OGA.SharedKernel.Config.structs.Config_AppPaths_v2> appSettings,
                            OGA.AppSettings.Writeable.JSONConfig.IWritableOptions<OGA.SharedKernel.Config.structs.Config_BuildData> builddata,
                            OGA.InfraBase.Services.IConfigService confsvc)
        {
            this._confsvc = confsvc;

            this._appSettings = appSettings;

            this._builddata = builddata;
        }

        #endregion


        public string Get_BuildData_Rev()
        {
            return this._builddata.Value.Source_Revision;
        }
        public void Set_BuildData_Rev(string rev)
        {
            this._builddata.Update(opt =>
            {
                opt.Source_Revision = rev.ToString();
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
