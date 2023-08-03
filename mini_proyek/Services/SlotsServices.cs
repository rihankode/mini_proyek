using Microsoft.Data.SqlClient;
using mini_proyek.Interfaces;
using mini_proyek.Models;
using System.Data;

namespace mini_proyek.Services
{
    public class SlotsServices : SlotInterface
    {
        private DbContex _context;

        private IConfiguration _configuration;
        public SlotsServices(DbContex context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public Dictionary<string, object> get_availibility(Kategori request)
        {
            var res = new Dictionary<string, object>();

            using (SqlConnection con = new SqlConnection(_configuration.GetSection("ConnectionString").Value))
            {


               

            }

            return res;
        }
    }
}
