using DB.Models;

namespace IServices.Wms
{
    public interface IWms_InvmovedetailService : IBaseService<WmsInvmovedetail>
    {
        Task<string> PageList(string pid);
    }
}