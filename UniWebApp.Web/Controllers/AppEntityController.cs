using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniWebApp.Core;
using UniWebApp.Data;
using UniWebApp.Web.Models;

namespace UniWebApp.Web.Controllers
{
    [Route("api/entities")]
    [ApiController]
    public class AppEntityController : ControllerBase
    {
        private readonly IUniWebAppRepository _repo;

        public AppEntityController(IUniWebAppRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAppEntities(bool includeFields = false)
        {
            try
            {
                var modelResults = new List<AppEntityModel>();
                var results = await _repo.GetAllEntitiesAsync(includeFields);
                foreach (var entity in results)
                {
                    AppEntityModel model = new AppEntityModel()
                    {
                        Id = entity.Id,
                        TypeId = entity.Type.Id,
                        TypeName = entity.Type.Name
                    };

                    if (includeFields)
                    {
                        model.Fields = new List<AppEntityDataFieldModel>();
                        foreach (var field in entity.Fields)
                        {
                            var fieldModel = new AppEntityDataFieldModel()
                            {
                                FieldId = field.Id,
                                Name = field.Name
                            };

                            switch (field.GetType().Name)
                            {
                                case "AppEntityDataFieldText":
                                    fieldModel.FieldType = DataFieldTypeEnum.Text;
                                    fieldModel.TextValue = ((AppEntityDataFieldText)field).Value;
                                    break;
                                case "AppEntityDataFieldNumber":
                                    fieldModel.FieldType = DataFieldTypeEnum.Number;
                                    fieldModel.NumberValue = ((AppEntityDataFieldNumber)field).Value;
                                    break;
                                case "AppEntityDataFieldDate":
                                    fieldModel.FieldType = DataFieldTypeEnum.Date;
                                    fieldModel.DateValue = ((AppEntityDataFieldDate)field).Value;
                                    break;
                                case "AppEntityDataFieldCombobox":
                                    fieldModel.FieldType = DataFieldTypeEnum.Combobox;
                                    fieldModel.ComboboxSelected = ((AppEntityDataFieldCombobox)field).SelectedOption;
                                    fieldModel.ComboboxOptions = new List<string>();
                                    foreach (var option in await _repo.GetDataFieldComboboxOptionsAsync(fieldModel.FieldId))
                                    {
                                        fieldModel.ComboboxOptions.Add(option.Name);
                                    }
                                    break;
                                case "AppEntityDataFieldBoolean":
                                    fieldModel.FieldType = DataFieldTypeEnum.Boolean;
                                    fieldModel.BooleanValue = ((AppEntityDataFieldBoolean)field).Value;
                                    break;
                            }

                            model.Fields.Add(fieldModel);
                        }
                    }

                    modelResults.Add(model);
                }                

                return Ok(new ApiResponse<List<AppEntityModel>>(StatusCodes.Status200OK, "OK", modelResults));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetEntity(int id, bool includeFields = false)
        {
            try
            {
                var entity = await _repo.GetEntityByIdAsync(id, includeFields);
                if (entity == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro! Essa entidade não existe."));
                }

                var modelResult = new AppEntityModel()
                {
                    Id = entity.Id,
                    TypeId = entity.Type.Id,
                    TypeName = entity.Type.Name
                };

                if (includeFields)
                {
                    modelResult.Fields = new List<AppEntityDataFieldModel>();
                    foreach (var field in entity.Fields)
                    {
                        var fieldModel = new AppEntityDataFieldModel()
                        {
                            FieldId = field.Id,
                            Name = field.Name
                        };

                        switch (field.GetType().Name)
                        {
                            case "AppEntityDataFieldText":
                                fieldModel.FieldType = DataFieldTypeEnum.Text;
                                fieldModel.TextValue = ((AppEntityDataFieldText)field).Value;
                                break;
                            case "AppEntityDataFieldNumber":
                                fieldModel.FieldType = DataFieldTypeEnum.Number;
                                fieldModel.NumberValue = ((AppEntityDataFieldNumber)field).Value;
                                break;
                            case "AppEntityDataFieldDate":
                                fieldModel.FieldType = DataFieldTypeEnum.Date;
                                fieldModel.DateValue = ((AppEntityDataFieldDate)field).Value;
                                break;
                            case "AppEntityDataFieldCombobox":
                                fieldModel.FieldType = DataFieldTypeEnum.Combobox;
                                fieldModel.ComboboxSelected = ((AppEntityDataFieldCombobox)field).SelectedOption;
                                fieldModel.ComboboxOptions = new List<string>();
                                foreach (var option in await _repo.GetDataFieldComboboxOptionsAsync(fieldModel.FieldId))
                                {
                                    fieldModel.ComboboxOptions.Add(option.Name);
                                }
                                break;
                            case "AppEntityDataFieldBoolean":
                                fieldModel.FieldType = DataFieldTypeEnum.Boolean;
                                fieldModel.BooleanValue = ((AppEntityDataFieldBoolean)field).Value;
                                break;
                        }

                        modelResult.Fields.Add(fieldModel);
                    }
                }

                return Ok(new ApiResponse<AppEntityModel>(StatusCodes.Status200OK, "Ok", modelResult));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddEntity(NewAppEntityModel model)
        {
            try
            {
                if (model.TypeId <= 0 || model.Fields == null || model.Fields.Count == 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro! Verifique o modelo e tente novamente."));
                }

                AppEntity newEntity = new AppEntity();
                newEntity.Type = await _repo.GetEntityTypeByIdAsync(model.TypeId);

                if (newEntity.Type == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro! Esse tipo de entidade não existe."));
                }

                newEntity.Fields = new List<AppEntityDataField>();
                bool modelErrors = false;
                foreach (var field in model.Fields)
                {
                    if (field.FieldType < 0 || field.FieldType > DataFieldTypeEnum.Boolean || string.IsNullOrWhiteSpace(field.Name))
                    {
                        return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro! Verifique o modelo e tente novamente."));
                    }

                    if (field.FieldType == DataFieldTypeEnum.Text)
                    {
                        if(string.IsNullOrWhiteSpace(field.TextValue)) { modelErrors = true; break; }
                        newEntity.Fields.Add(new AppEntityDataFieldText()
                        {
                            Entity = newEntity,
                            Name = field.Name,
                            Value = field.TextValue
                        });
                    }
                    else if (field.FieldType == DataFieldTypeEnum.Number)
                    {
                        if (field.NumberValue == decimal.MinValue) { modelErrors = true; break; }
                        newEntity.Fields.Add(new AppEntityDataFieldNumber()
                        {
                            Entity = newEntity,
                            Name = field.Name,
                            Value = field.NumberValue
                        });
                    }
                    else if (field.FieldType == DataFieldTypeEnum.Date)
                    {
                        if (field.DateValue == null) { modelErrors = true; break; }
                        newEntity.Fields.Add(new AppEntityDataFieldDate()
                        {
                            Entity = newEntity,
                            Name = field.Name,
                            Value = field.DateValue
                        });
                    }
                    else if (field.FieldType == DataFieldTypeEnum.Combobox)
                    {
                        var newField = new AppEntityDataFieldCombobox()
                        {
                            Entity = newEntity,
                            Name = field.Name,
                            Options = new List<AppEntityDataFieldComboboxOption>(),
                            SelectedOption = field.ComboboxSelected
                        };

                        if(field.ComboboxOptions == null || field.ComboboxOptions.Length == 0 || field.ComboboxSelected < 0) { modelErrors = true; break; }
                        for (int i = 0; i < field.ComboboxOptions.Length; i++)
                        {
                            var newOption = new AppEntityDataFieldComboboxOption()
                            {
                                Name = field.ComboboxOptions[i],
                                Combobox = newField
                            };

                            newField.Options.Add(newOption);
                        }

                        newEntity.Fields.Add(newField);
                    }
                    else if (field.FieldType == DataFieldTypeEnum.Boolean)
                    {
                        newEntity.Fields.Add(new AppEntityDataFieldBoolean()
                        {
                            Entity = newEntity,
                            Name = field.Name,
                            Value = field.BooleanValue
                        });
                    }
                }

                if (modelErrors)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro! Verifique o modelo e tente novamente."));
                }

                _repo.AddEntity(newEntity);

                bool saved = await _repo.SaveChangesAsync();
                if (!saved)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro ao tentar criar uma nova entidade. Verifique o modelo e tente novamente."));
                }

                return Created("", new ApiResponse(StatusCodes.Status201Created, "Sucesso! Nova entidade criada com sucesso."));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteEntity(int id)
        {
            try
            {
                var entity = await _repo.GetEntityByIdAsync(id, true);
                if (entity == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro! Essa entidade não existe."));
                }

                foreach (var item in entity.Fields)
                {
                    if (item.GetType().Name == "AppEntityDataFieldCombobox")
                    {
                        _repo.RemoveDataFieldComboboxOptionsRange(await _repo.GetDataFieldComboboxOptionsAsync(item.Id));
                    }
                }

                _repo.RemoveEntity(entity);

                bool saved = await _repo.SaveChangesAsync();
                if (!saved)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro ao tentar eliminar a entidade. Verifique o modelo e tente novamente."));
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Sucesso! Entidade foi eliminada."));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        //[HttpPatch("{id:int}")]
        //public async Task<ActionResult<ApiResponse>> PatchEntityType(int id, NewAppEntityModel model)
        //{
        //    try
        //    {
        //        var entity = await _repo.GetEntityByIdAsync(id, false);
        //        if (entity == null)
        //        {
        //            return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro! Essa entidade não existe."));
        //        }

        //        if (entity.Type.Id == model.TypeId)
        //        {
        //            return Conflict(new ApiResponse(StatusCodes.Status409Conflict, "Erro! Esta entidade já era desse tipo."));
        //        }

        //        entity.Type = await _repo.GetEntityTypeByIdAsync(model.TypeId);
        //        if (entity.Type == null)
        //        {
        //            return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro! Essa tipo de entidade não existe."));
        //        }

        //        bool saved = await _repo.SaveChangesAsync();
        //        if (!saved)
        //        {
        //            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro ao tentar atualizar a entidade. Verifique o modelo e tente novamente."));
        //        }

        //        return Ok(new ApiResponse(StatusCodes.Status200OK, $"Sucesso! Entidade agora é do tipo '{entity.Type.Name}'."));
        //    }
        //    catch (Exception)
        //    {
        //        return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
        //    }
        //}

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateEntity(int id, NewAppEntityModel model)
        {
            try
            {
                var entity = await _repo.GetEntityByIdAsync(id, true);
                if (entity == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro! Essa entidade não existe."));
                }

                entity.Type = await _repo.GetEntityTypeByIdAsync(model.TypeId);
                if (entity.Type == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro! Essa tipo de entidade não existe."));
                }

                if (model.Fields == null || model.Fields.Count == 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro! Entidade precisa de pelo menos um campo."));
                }

                foreach (var item in entity.Fields)
                {
                    if (item.GetType().Name == "AppEntityDataFieldCombobox")
                    {
                        _repo.RemoveDataFieldComboboxOptionsRange(await _repo.GetDataFieldComboboxOptionsAsync(item.Id));
                    }
                }

                bool modelErrors = false;
                entity.Fields = new List<AppEntityDataField>();
                foreach (var field in model.Fields)
                {
                    if (field.FieldType < 0 || field.FieldType > DataFieldTypeEnum.Boolean || string.IsNullOrWhiteSpace(field.Name))
                    {
                        return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro! Verifique o modelo e tente novamente."));
                    }

                    if (field.FieldType == DataFieldTypeEnum.Text)
                    {
                        if (string.IsNullOrWhiteSpace(field.TextValue)) { modelErrors = true; break; }
                        entity.Fields.Add(new AppEntityDataFieldText()
                        {
                            Entity = entity,
                            Name = field.Name,
                            Value = field.TextValue
                        });
                    }
                    else if (field.FieldType == DataFieldTypeEnum.Number)
                    {
                        if (field.NumberValue == decimal.MinValue) { modelErrors = true; break; }
                        entity.Fields.Add(new AppEntityDataFieldNumber()
                        {
                            Entity = entity,
                            Name = field.Name,
                            Value = field.NumberValue
                        });
                    }
                    else if (field.FieldType == DataFieldTypeEnum.Date)
                    {
                        if (field.DateValue == null) { modelErrors = true; break; }
                        entity.Fields.Add(new AppEntityDataFieldDate()
                        {
                            Entity = entity,
                            Name = field.Name,
                            Value = field.DateValue
                        });
                    }
                    else if (field.FieldType == DataFieldTypeEnum.Combobox)
                    {
                        var newField = new AppEntityDataFieldCombobox()
                        {
                            Entity = entity,
                            Name = field.Name,
                            Options = new List<AppEntityDataFieldComboboxOption>(),
                            SelectedOption = field.ComboboxSelected
                        };

                        if (field.ComboboxOptions == null || field.ComboboxOptions.Length == 0 || field.ComboboxSelected < 0) { modelErrors = true; break; }
                        for (int i = 0; i < field.ComboboxOptions.Length; i++)
                        {
                            var newOption = new AppEntityDataFieldComboboxOption()
                            {
                                Name = field.ComboboxOptions[i],
                                Combobox = newField
                            };

                            newField.Options.Add(newOption);
                        }

                        entity.Fields.Add(newField);
                    }
                    else if (field.FieldType == DataFieldTypeEnum.Boolean)
                    {
                        entity.Fields.Add(new AppEntityDataFieldBoolean()
                        {
                            Entity = entity,
                            Name = field.Name,
                            Value = field.BooleanValue
                        });
                    }
                }

                if (modelErrors)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro! Verifique o modelo e tente novamente."));
                }

                _repo.UpdateEntity(entity);

                bool saved = await _repo.SaveChangesAsync();
                if (!saved)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro ao tentar atualizar entidade. Verifique o modelo e tente novamente."));
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Sucesso! Entidade foi atualizada."));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }
    }
}