using mini_proyek.Models;

namespace mini_proyek.Interfaces
{
    public interface SlotInterface
    {
        Dictionary<string, object> get_availibility(Kategori request);
    }
}
