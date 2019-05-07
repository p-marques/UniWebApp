using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        //void AddEntity(AppEntity newEntity);
        //void DeleteEntity(AppEntity entityToRemove);

        //// AppEntityDataField
        Task<List<AppEntityDataField>> GetDataFieldsByEntityAsync(int entityId, bool majorFieldsOnly);
        //void AddDataFieldToEntity(int EntityId, AppEntityDataField newField, bool addToAllEntitiesFromType);
        //void DeleteDataField(AppEntityDataField fieldToRemove);

        //// AppEntityType
        Task<List<AppEntityType>> GetAllEntityTypesAsync();
        Task<AppEntityType> GetEntityTypeByIdAsync(int id);
        Task<AppEntityType> GetEntityTypeByNameAsync(string name);
        void AddEntityType(AppEntityType newType);
        void RemoveEntityType(AppEntityType typeToRemove);

        //// Save
        Task<bool> SaveChangesAsync();
    }
}
