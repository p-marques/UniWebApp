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

        bool GetEntityExistsByName(string name);

        void AddEntity(AppEntity newEntity);

        void UpdateEntity(AppEntity entity);

        void RemoveEntity(AppEntity entityToRemove);

        // AppEntityRelation
        Task<List<AppEntityRelation>> GetEntityRelationsAsync(int entityId);

        // AppEntityDataField
        Task<List<AppEntityDataField>> GetDataFieldsByEntityAsync(int entityId);

        //void AddDataFieldToEntity(int EntityId, AppEntityDataField newField, bool addToAllEntitiesFromType);
        //void DeleteDataField(AppEntityDataField fieldToRemove);

        void RemoveDataFieldRange(ICollection<AppEntityDataField> fieldsToRemove);

        Task<List<AppEntityDataFieldComboboxOption>> GetDataFieldComboboxOptionsAsync(int fieldId);

        void RemoveDataFieldComboboxOptionsRange(ICollection<AppEntityDataFieldComboboxOption> options);

        // AppEntityType
        Task<List<AppEntityType>> GetAllEntityTypesAsync();

        Task<AppEntityType> GetEntityTypeByIdAsync(int id);

        Task<AppEntityType> GetEntityTypeByNameAsync(string name);

        void AddEntityType(AppEntityType newType);

        void RemoveEntityType(AppEntityType typeToRemove);

        // Save
        Task<bool> SaveChangesAsync();
    }
}