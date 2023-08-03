


using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace mini_proyek.Models
{
    public class DbContex : DbContext
    {
        public DbContex(DbContextOptions<DbContex> options) : base(options)
        {
        }
    }
}
