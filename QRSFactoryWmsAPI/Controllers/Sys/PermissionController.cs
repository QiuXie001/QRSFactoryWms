using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Json;

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class PermissionController : BaseController
    {
        [HttpGet]
        public string GetPermissions()
        {
            SysRoleMenu a = null;
            return a.ToJson();
        }
    }
}
