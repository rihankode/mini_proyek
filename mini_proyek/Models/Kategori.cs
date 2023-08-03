namespace mini_proyek.Models
{
    public class Kategori
    {
        public List<Dictionary<string,string>>? data { get; set; }

        public List<Dictionary<string, string>>? filter { get; set; }

        public string? id { get; set; }

        public int? index { get; set; }

        public int? perpage { get; set; }
    }

    
}
