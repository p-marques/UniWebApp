using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniWebApp.Data
{
    public class UniWebAppRepository : IUniWebAppRepository
    {
        private readonly AppDbContext _db;

        public UniWebAppRepository(AppDbContext db)
        {
            _db = db;
        }
    }
}
