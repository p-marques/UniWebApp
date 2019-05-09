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
        public async Task<AppEntity> GetEntityByIdAsync(int id, bool includeFields)
        {
            var result = await _db.AppEntities.FindAsync(id);

            if (includeFields)
            {
                result.Fields = await GetDataFieldsByEntityAsync(id, false);
            }

            return result;
        }

        // Data Field
        public async Task<List<AppEntityDataField>> GetDataFieldsByEntityAsync(int entityId, bool majorFieldsOnly)
        {
            var result = _db.AppEntityFields.Include(x => x.Entity).Where(c => c.Entity.Id == entityId);

            if (majorFieldsOnly)
            {
                result = result.Where(v => v.Major == true);
            }

            return await result.ToListAsync();
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

        // DataFieldTemplate
        public async Task<DataFieldTemplate> GetDataFieldTemplateByIdAsync(int entityTypeId)
        {
            return await _db.DataFieldsTemplate.FindAsync(entityTypeId);
        }
        public async Task<DataFieldTemplate> GetDataFieldTemplateByNameAsync(int entityTypeId, string name)
        {
            return await _db.DataFieldsTemplate.Include(x => x.EntityType).Where(x => x.EntityType.Id == entityTypeId).SingleOrDefaultAsync(q => q.Name == name);
        }
        public void AddDataFieldTemplate(DataFieldTemplate newFieldTemplate)
        {
            _db.DataFieldsTemplate.Add(newFieldTemplate);
        }
        public void RemoveDataFieldTemplate(DataFieldTemplate fieldToDelete)
        {
            _db.DataFieldsTemplate.Remove(fieldToDelete);
        }

        // DataFieldTemplateComboboxOptions
        public async Task<List<DataFieldTemplateComboboxOption>> GetDataFieldTemplateComboboxOptionsAsync(int templateDataFieldId)
        {
            return await _db.DataFieldsTemplateComboboxOptions.Include(x => x.DataFieldTemplate).Where(op => op.DataFieldTemplate.Id == templateDataFieldId).ToListAsync();
        }
        public void RemoveDataFieldTemplateComboboxOptions(List<DataFieldTemplateComboboxOption> optionsToRemove)
        {
            _db.DataFieldsTemplateComboboxOptions.RemoveRange(optionsToRemove);
        }

        // Save
        public async Task<bool> SaveChangesAsync()
        {
            return (await _db.SaveChangesAsync()) > 0;
        }
    }
}
