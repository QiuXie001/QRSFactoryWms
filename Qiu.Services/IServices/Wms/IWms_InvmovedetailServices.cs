using DB.Models;

namespace IServices
{
    public interface IWms_InvmovedetailServices : IBaseService<WmsInvmovedetail>
    {
        string PageList(string pid);
    }
}