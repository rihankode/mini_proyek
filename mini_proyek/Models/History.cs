namespace mini_proyek.Models
{
    public class History
    {
        public List<Dictionary<string, string>>? filter { get; set; }

        public string? userid { get; set; }

        public int? index { get; set; }

        public int? perpage { get; set; }
    }
}
