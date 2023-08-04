using mini_proyek.Models;

namespace mini_proyek.Interfaces
{
    public interface AreaInterfaces
    {
        Dictionary<string, object> Get_data_Area(Area request);

        Dictionary<string, object> Create_data_area(Area request);

        Dictionary<string, object> Update_Data_area(Area request);

        Dictionary<string, object> Delete_area(Area request);

        Dictionary<string, object> getDataById(Area request);
    }
}
