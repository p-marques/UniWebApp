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
        public async Task<ActionResult<ApiResponse>> GetAppEntities()
        {
            try
            {
                var results = await _repo.GetAllEntitiesAsync();

                return Ok(new ApiResponse<List<AppEntity>>(StatusCodes.Status200OK, "OK", results));
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
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }
    }
}