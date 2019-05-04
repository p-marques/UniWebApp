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
        //Task<AppEntity> GetEntityAsync(int id, bool includeMajorFields, bool includeOtherFields);
        //void AddEntity(AppEntity newEntity);
        //void DeleteEntity(AppEntity entityToRemove);

        //// AppEntityDataField
        //Task<List<AppEntityDataField>> GetDataFieldsByEntityAsync(int EntityId, bool majorFieldsOnly);
        //void AddDataFieldToEntity(int EntityId, AppEntityDataField newField, bool addToAllEntitiesFromType);
        //void DeleteDataField(AppEntityDataField fieldToRemove);

        //// AppEntityType
        //Task<List<AppEntityType>> GetAllEntityTypesAsync();
        void AddEntityType(AppEntityType newType);

        //// Save
        Task<bool> SaveChangesAsync();
    }
}
