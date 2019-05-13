using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniWebApp.Core;
using UniWebApp.Data;
using UniWebApp.Web.Models;

namespace UniWebApp.Web.Controllers
{
    [Route("api/entities/types")]
    [ApiController]
    public class AppEntityTypeController : ControllerBase
    {
        private readonly IUniWebAppRepository _repo;
        private readonly IMapper _mapper;

        public AppEntityTypeController(IUniWebAppRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetEntityTypes(bool template = false)
        {
            try
            {
                var types = await _repo.GetAllEntityTypesAsync(template);
                var returnObj = new List<AppEntityTypeModel>();
                foreach (var item in types)
                {
                    var a = new AppEntityTypeModel()
                    {
                        Id = item.Id,
                        Name = item.Name
                    };

                    if (template)
                    {
                        a.TemplateFields = new List<DataFieldTemplateModel>();

                        foreach (var field in item.TemplateFields)
                        {
                            var fieldModel = new DataFieldTemplateModel()
                            {
                                Id = field.Id,
                                Name = field.Name,
                                FieldType = field.FieldType
                            };

                            if(field.FieldType == DataFieldTypeEnum.Combobox)
                            {
                                fieldModel.ComboboxOptions = _mapper.Map<List<DataFieldTemplateComboboxOptionModel>>(await _repo.GetDataFieldTemplateComboboxOptionsAsync(field.Id));
                            }

                            a.TemplateFields.Add(fieldModel);
                        }
                    }

                    returnObj.Add(a);
                }

                return Ok(new ApiResponse<List<AppEntityTypeModel>>(StatusCodes.Status200OK, "OK", returnObj));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse(StatusCodes.Status500InternalServerError, "Erro inesperado. Tente mais tarde."));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetEntityType(int id, bool template = false)
        {
            try
            {
                AppEntityType type = await _repo.GetEntityTypeByIdAsync(id);
                if (type == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro. Tipo de entidade não encontrado."));
                }

                AppEntityTypeModel typeModel = new AppEntityTypeModel()
                {
                    Id = type.Id,
                    Name = type.Name
                };

                if (template)
                {
                    type.TemplateFields = await _repo.GetEntityTypeTemplateFieldsAsync(id);
                    typeModel.TemplateFields = new List<DataFieldTemplateModel>();
                    foreach (var field in type.TemplateFields)
                    {
                        var fieldModel = new DataFieldTemplateModel()
                        {
                            Id = field.Id,
                            Name = field.Name,
                            FieldType = field.FieldType
                        };

                        if(field.FieldType == DataFieldTypeEnum.Combobox)
                        {
                            fieldModel.ComboboxOptions = _mapper.Map<List<DataFieldTemplateComboboxOptionModel>>(field.ComboboxOptions);
                        }

                        typeModel.TemplateFields.Add(fieldModel);
                    }
                }

                return Ok(new ApiResponse<AppEntityTypeModel>(StatusCodes.Status200OK, "OK", typeModel));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse(StatusCodes.Status500InternalServerError, "Erro inesperado. Tente mais tarde."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddEntityType(NewAppEntityTypeModel model)
        {
            try
            {
                if (model.Name == null || model.Name.Length < 2 || model.Name.Length > 50)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Tamanho do nome não aceite."));
                }

                if (await _repo.GetEntityTypeByNameAsync(model.Name.Trim()) != null)
                {
                    return Conflict(new ApiResponse(StatusCodes.Status409Conflict, "Erro. Tipo de entidade já existe."));
                }

                if (model.TemplateFields == null || model.TemplateFields.Count < 1 || model.TemplateFields.Count > 5)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest,
                        "Erro. Para criar um novo tipo de entidade é preciso criar pelo menos 1 campo e no máximo 5."));
                }

                if (model.TemplateFields.Where(x => x.FieldType == DataFieldTypeEnum.Combobox && (x.ComboboxOptions == null || x.ComboboxOptions.Count() == 0)).Count() > 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest,
                        "Erro. Campo de escolha múltipa requer opções. Verifique os dados inseridos."));
                }

                AppEntityType newType = new AppEntityType()
                {
                    Name = model.Name.Trim(),
                    TemplateFields = new List<DataFieldTemplate>()
                };

                foreach (var field in model.TemplateFields)
                {
                    var newField = new DataFieldTemplate()
                    {
                        Name = field.Name,
                        EntityType = newType,
                        FieldType = field.FieldType
                    };

                    if (newField.FieldType == DataFieldTypeEnum.Combobox)
                    {
                        newField.ComboboxOptions = new List<DataFieldTemplateComboboxOption>();
                        foreach (var option in field.ComboboxOptions)
                        {
                            newField.ComboboxOptions.Add(new DataFieldTemplateComboboxOption() { DataFieldTemplate = newField, Name = option });
                        }
                    }

                    newType.TemplateFields.Add(newField);
                }

                _repo.AddEntityType(newType);

                bool saved = await _repo.SaveChangesAsync();
                if (!saved)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro ao tentar criar um novo tipo de entidade. Verifique o modelo e tente novamente."));
                }

                return Created("", new ApiResponse(StatusCodes.Status201Created, $"Sucesso! Tipo de entidade '{model.Name}' foi criado."));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Erro inesperado ao tentar adicionar item."));
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateEntityTypeName(int id, PatchAppEntityTypeModel model)
        {
            try
            {
                if (model.Name == null || model.Name.Length < 2 || model.Name.Length > 50)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Tamanho do nome não aceite."));
                }

                model.Name = model.Name.Trim();
                var typeToUpdate = await _repo.GetEntityTypeByIdAsync(id);
                if (typeToUpdate == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro. Tipo de entidade não encontrado."));
                }

                if (typeToUpdate.Name == model.Name)
                {
                    return Conflict(new ApiResponse(StatusCodes.Status409Conflict, $"Erro. Recurso não foi atualizado. {model.Name} = {typeToUpdate.Name}"));
                }

                string oldName = typeToUpdate.Name;
                typeToUpdate.Name = model.Name;

                bool saved = await _repo.SaveChangesAsync();
                if (!saved)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro ao tentar atualizar o tipo de entidade. Verifique o modelo e tente novamente."));
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, $"Sucesso! Tipo de entidade atualizado com sucesso. '{oldName}' atualizado para '{model.Name}'"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Erro inesperado ao tentar atualizar item."));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteEntityType(int id)
        {
            try
            {
                var typeToDelete = await _repo.GetEntityTypeByIdAsync(id);
                if (typeToDelete == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Tipo de entidade não encontrado."));
                }

                _repo.RemoveEntityType(typeToDelete);
                bool saved = await _repo.SaveChangesAsync();
                if (!saved)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro ao tentar eliminar o tipo de entidade. Verifique o modelo e tente novamente."));
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Tipo de entidade eliminado com sucesso."));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Erro inesperado ao tentar eliminar item."));
            }
        }
    }
}