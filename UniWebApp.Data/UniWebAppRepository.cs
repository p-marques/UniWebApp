using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniWebApp.Core;

namespace UniWebApp.Data
{
    public class UniWebAppRepository : IUniWebAppRepository
    {
        private readonly AppDbContext _db;

        public UniWebAppRepository(AppDbContext db)
        {
            _db = db;
        }

        // AppEntity
        public async Task<List<AppEntity>> GetAllEntitiesAsync()
        {
            return await _db.AppEntities.ToListAsync();
        }

        // AppEntityType
        public void AddEntityType(AppEntityType newType)
        {
            _db.AppEntityTypes.Add(newType);
        }

        // Save
        public async Task<bool> SaveChangesAsync()
        {
            return (await _db.SaveChangesAsync()) > 0;
        }
    }
}
