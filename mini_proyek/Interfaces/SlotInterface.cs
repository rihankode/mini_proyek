using mini_proyek.Models;

namespace mini_proyek.Interfaces
{
    public interface SlotInterface
    {
        Dictionary<string, object> get_availibility(Slots request);

        Dictionary<string, object> get_slot(Slots request);

        Dictionary<string, object> get_checkout(Slots request);
    }
}
