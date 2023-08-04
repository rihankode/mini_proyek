using mini_proyek.Models;

namespace mini_proyek.Interfaces
{
    public interface HistoryInterface
    {
        Dictionary<string, object> Get_data_Hostory(History request);

        Dictionary<string, object> Get_data_Hostor_by_idy(History request);
    }
}
