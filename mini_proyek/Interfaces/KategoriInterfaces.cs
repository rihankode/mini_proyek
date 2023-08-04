using mini_proyek.Models;

namespace mini_proyek.Interfaces
{
    public interface KategoriInterfaces
    {
        Dictionary<string, object> Get_kategori(Kategori request);

        Dictionary<string, object> Create_data_kategori(Kategori request);

        Dictionary<string, object> Update_Data_katgeori(Kategori request);

        Dictionary<string, object> Delete_data(Kategori request);

        Dictionary<string, object> getDataById(Kategori request);
    }
}
