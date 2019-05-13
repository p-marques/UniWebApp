using System.Collections.Generic;
using System.Threading.Tasks;
using UniWebApp.Core;

namespace UniWebApp.Data
{
    public interface IUniWebAppRepository
    {
        // AppEntity
        Task<List<AppEntity>> GetAllEntitiesAsync();

        //Task<List<AppEntity>> GetEntitiesByTypeAsync(int entityTypeId);
        Task<AppEntity> GetEntityByIdAsync(int id, bool includeFields);

        void AddEntity(AppEntity newEntity);
        //void DeleteEntity(AppEntity entityToRemove);

        // AppEntityDataField
        Task<List<AppEntityDataField>> GetDataFieldsByEntityAsync(int entityId);

        //void AddDataFieldToEntity(int EntityId, AppEntityDataField newField, bool addToAllEntitiesFromType);
        //void DeleteDataField(AppEntityDataField fieldToRemove);

        // AppEntityType
        Task<List<AppEntityType>> GetAllEntityTypesAsync(bool includeTemplateFields);

        Task<AppEntityType> GetEntityTypeByIdAsync(int id);

        Task<AppEntityType> GetEntityTypeByNameAsync(string name);

        void AddEntityType(AppEntityType newType);

        void RemoveEntityType(AppEntityType typeToRemove);

        // DataFieldTemplate
        Task<List<DataFieldTemplate>> GetEntityTypeTemplateFieldsAsync(int entityTypeId);

        Task<DataFieldTemplate> GetDataFieldTemplateByIdAsync(int id);

        Task<DataFieldTemplate> GetDataFieldTemplateByNameAsync(int entityTypeId, string name);

        void AddDataFieldTemplate(DataFieldTemplate newFieldTemplate);

        void RemoveDataFieldTemplate(DataFieldTemplate fieldToDelete);

        // DataFieldTemplateComboboxOption
        Task<List<DataFieldTemplateComboboxOption>> GetDataFieldTemplateComboboxOptionsAsync(int templateDataFieldId);

        void RemoveDataFieldTemplateComboboxOptions(List<DataFieldTemplateComboboxOption> optionsToRemove);

        // Save
        Task<bool> SaveChangesAsync();
    }
}