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
        public async Task<List<AppEntity>> GetAllEntitiesAsync(bool includeFields = false)
        {
            if (includeFields)
            {
                return await _db.AppEntities.Include(z => z.Type).Include(t => t.Fields).ToListAsync();
            }
            return await _db.AppEntities.Include(z => z.Type).ToListAsync();
        }

        public async Task<AppEntity> GetEntityByIdAsync(int id, bool includeFields)
        {
            if (includeFields)
            {
                return await _db.AppEntities.Include(z => z.Type).Include(t => t.Fields).SingleOrDefaultAsync(x => x.Id == id);
            }

            return await _db.AppEntities.Include(z => z.Type).SingleOrDefaultAsync(x => x.Id == id);
        }

        public void AddEntity(AppEntity newEntity)
        {
            _db.AppEntities.Add(newEntity);
        }

        public void RemoveEntity(AppEntity entityToRemove)
        {
            _db.AppEntities.Remove(entityToRemove);
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
        public async Task<List<AppEntityType>> GetAllEntityTypesAsync(bool includeTemplateFields)
        {
            if (includeTemplateFields)
            {
                return await _db.AppEntityTypes.Include(d => d.TemplateFields).ToListAsync();
            }

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
        public async Task<List<DataFieldTemplate>> GetEntityTypeTemplateFieldsAsync(int entityTypeId)
        {
            return await _db.DataFieldsTemplate.Include(t => t.EntityType).Include(y => y.ComboboxOptions).Where(x => x.EntityType.Id == entityTypeId).ToListAsync();
        }


        public async Task<DataFieldTemplate> GetDataFieldTemplateByIdAsync(int id)
        {
            return await _db.DataFieldsTemplate.FindAsync(id);
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