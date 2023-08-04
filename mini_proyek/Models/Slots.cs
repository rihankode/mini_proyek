namespace mini_proyek.Models
{
    public class Slots
    {
        public string? userIdentity { get; set; }

        public List<Dictionary<string, string>>? filter { get; set; }

        public string? id { get; set; }

        public string? slotId { get; set; }

        public int? index { get; set; }

        public int? perpage { get; set; }

        public string? status { get; set;}

    }
}
