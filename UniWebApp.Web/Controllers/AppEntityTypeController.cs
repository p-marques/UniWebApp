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
                            a.TemplateFields.Add(new DataFieldTemplateModel()
                            {
                                Id = field.Id,
                                Name = field.Name,
                                FieldType = field.FieldType,
                                MustHave = field.MustHave,
                                ComboboxOptions = _mapper.Map<List<DataFieldTemplateComboboxOptionModel>>(await _repo.GetDataFieldTemplateComboboxOptionsAsync(field.Id))
                            });
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
                AppEntityTypeModel typeModel = new AppEntityTypeModel()
                {
                    Id = type.Id,
                    Name = type.Name
                };

                if (template)
                {
                    typeModel.TemplateFields = new List<DataFieldTemplateModel>();
                    foreach (var item in type.TemplateFields)
                    {
                        typeModel.TemplateFields.Add(new DataFieldTemplateModel()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            FieldType = item.FieldType,
                            MustHave = item.MustHave,
                            ComboboxOptions = _mapper.Map<List<DataFieldTemplateComboboxOptionModel>>(await _repo.GetDataFieldTemplateComboboxOptionsAsync(item.Id))
                        });
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
                if (await _repo.GetEntityTypeByNameAsync(model.Name.Trim()) != null)
                {
                    return Conflict(new ApiResponse(StatusCodes.Status409Conflict, "Erro. Tipo de entidade já existe."));
                }

                if (model.Name == null || model.Name.Length < 2 || model.Name.Length > 50)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Tamanho do nome não aceite."));
                }

                if (model.TemplateFields == null || model.TemplateFields.Count < 1 || model.TemplateFields.Count > 5)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest,
                        "Erro. Para criar um novo tipo de entidade é preciso criar pelo menos 1 campo e no máximo 5."));
                }

                if (model.TemplateFields.Where(x => x.MustHave == true).Count() == 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest,
                        "Erro. Para criar um novo tipo de entidade é preciso criar pelo menos 1 campo obrigatório e no máximo 5."));
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
                    var newField = new DataFieldTemplate();
                    newField.Name = field.Name;
                    newField.EntityType = newType;
                    newField.FieldType = field.FieldType;
                    newField.MustHave = field.MustHave;

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

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateEntityTypeName(int id, AppEntityTypeModel model)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Id do objeto é obrigatório."));
                }

                model.Name = model.Name.Trim();
                var typeToUpdate = await _repo.GetEntityTypeByIdAsync(id);
                if (typeToUpdate == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Id não encontrado."));
                }

                if (typeToUpdate.Name == model.Name.Trim())
                {
                    return Conflict(new ApiResponse(StatusCodes.Status409Conflict, $"Recurso não foi atualizado. {model.Name.Trim()} = {typeToUpdate.Name}"));
                }

                typeToUpdate.Name = model.Name;

                bool saved = await _repo.SaveChangesAsync();
                if (!saved)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro ao tentar atualizar o tipo de entidade. Verifique o modelo e tente novamente."));
                }

                return Ok(new ApiResponse<AppEntityTypeModel>(StatusCodes.Status200OK,
                    $"Tipo de entidade atualizado com sucesso.", _mapper.Map<AppEntityTypeModel>(typeToUpdate)));
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
                if (id <= 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Id do objeto é obrigatório."));
                }

                var typeToDelete = await _repo.GetEntityTypeByIdAsync(id);
                if (typeToDelete == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Id não encontrado."));
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