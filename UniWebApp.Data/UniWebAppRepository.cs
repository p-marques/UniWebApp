using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
            return await _db.AppEntities.Include(z => z.Type).Include(t => t.Fields).ToListAsync();
        }

        public async Task<AppEntity> GetEntityByIdAsync(int id, bool includeFields)
        {
            if (includeFields)
            {
                return await _db.AppEntities.Include(z => z.Type).Include(t => t.Fields).SingleOrDefaultAsync(x => x.Id == id);
            }

            return await _db.AppEntities.Include(z => z.Type).SingleOrDefaultAsync(x => x.Id == id);
        }

        public bool GetEntityExistsByName(string name)
        {
            bool result = false;
            var entities = _db.AppEntities.Include(x => x.Fields);
            foreach (var entity in entities)
            {
                if(entity.Fields.Where(t => t.Name == "Nome" && ((AppEntityDataFieldText)t).Value == name).Count() > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public void AddEntity(AppEntity newEntity)
        {
            _db.AppEntities.Add(newEntity);
        }

        public void UpdateEntity(AppEntity entity)
        {
            _db.AppEntities.Update(entity);
        }

        public void RemoveEntity(AppEntity entityToRemove)
        {
            _db.AppEntities.Remove(entityToRemove);
        }

        // Relations
        public async Task<List<AppEntityRelation>> GetEntityRelationsAsync(int entityId)
        {
            return await _db.AppEntityRelations.Include(y => y.Entity).Where(t => t.Entity.Id == entityId || t.relatedEntityId == entityId).ToListAsync();
        }

        // Data Field
        public async Task<List<AppEntityDataField>> GetDataFieldsByEntityAsync(int entityId)
        {
            return await _db.AppEntityFields.Include(x => x.Entity).Where(c => c.Entity.Id == entityId).ToListAsync();
        }

        public async Task<List<AppEntityDataFieldComboboxOption>> GetDataFieldComboboxOptionsAsync(int fieldId)
        {
            return await _db.AppEntityDataFieldComboboxOptions.Include(x => x.Combobox).Where(t => t.Combobox.Id == fieldId).ToListAsync();
        }

        public void RemoveDataFieldRange(ICollection<AppEntityDataField> fieldsToRemove)
        {
            _db.AppEntityFields.RemoveRange(fieldsToRemove);
        }

        public void RemoveDataFieldComboboxOptionsRange(ICollection<AppEntityDataFieldComboboxOption> options)
        {
            _db.AppEntityDataFieldComboboxOptions.RemoveRange(options);
        }

        // AppEntityType
        public async Task<List<AppEntityType>> GetAllEntityTypesAsync()
        {
            return await _db.AppEntityTypes.ToListAsync();
        }

        public async Task<AppEntityType> GetEntityTypeByIdAsync(int id)
        {
            return await _db.AppEntityTypes.FindAsync(id);
        }

        public async Task<AppEntityType> GetEntityTypeByNameAsync(string name)
        {
            return await _db.AppEntityTypes.FirstOrDefaultAsync(x => x.Name == name);
        }

        public void AddEntityType(AppEntityType newType)
        {
            _db.AppEntityTypes.Add(newType);
        }

        public void RemoveEntityType(AppEntityType typeToRemove)
        {
            _db.AppEntityTypes.Remove(typeToRemove);
        }

        // Save
        public async Task<bool> SaveChangesAsync()
        {
            return (await _db.SaveChangesAsync()) > 0;
        }
    }
}