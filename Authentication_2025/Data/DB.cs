using Authentication_2025.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication_2025.Data
{
    public class DB : DbContext
    {

        public DB()
        {
            
        }

        public DB(DbContextOptions<DB> options) : base(options)  
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Fruit_Table> fruits { get; set; }
        public DbSet<LoginLogoutiInfo> loginlogoutiinfos { get; set; }
    }
}
