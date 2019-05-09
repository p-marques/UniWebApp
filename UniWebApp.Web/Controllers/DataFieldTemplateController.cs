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
    [Route("api/entities/types/{entityTypeId:int}/fields")]
    [ApiController]
    public class DataFieldTemplateController : ControllerBase
    {
        private readonly IUniWebAppRepository _repo;
        private readonly IMapper _mapper;

        public DataFieldTemplateController(IUniWebAppRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddTemplateField(int entityTypeId, NewDataFieldTemplateModel model)
        {
            try
            {
                if (entityTypeId <= 0 || model.FieldType < 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Modelo incorreto. Verifique os dados inseridos."));
                }

                if (model.FieldType == DataFieldTypeEnum.Combobox && (model.ComboboxOptions == null || model.ComboboxOptions.Length == 0))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Campo de escolha múltipa requer opções. Verifique os dados inseridos."));
                }

                DataFieldTemplate foundFieldTemplate = await _repo.GetDataFieldTemplateByNameAsync(entityTypeId, model.Name);
                if (foundFieldTemplate != null)
                {
                    return Conflict(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Um campo com esse nome já existe."));
                }

                var newFieldTemplate = _mapper.Map<DataFieldTemplate>(model);
                newFieldTemplate.EntityType = await _repo.GetEntityTypeByIdAsync(entityTypeId);
                if (newFieldTemplate.EntityType == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro. Tipo de entidade não encontrado."));
                }

                if (model.FieldType == DataFieldTypeEnum.Combobox)
                {
                    newFieldTemplate.ComboboxOptions = new List<DataFieldTemplateComboboxOption>();
                    foreach (var item in model.ComboboxOptions)
                    {
                        newFieldTemplate.ComboboxOptions.Add(new DataFieldTemplateComboboxOption() { Name = item, DataFieldTemplate = newFieldTemplate });
                    }
                }

                _repo.AddDataFieldTemplate(newFieldTemplate);
                bool result = await _repo.SaveChangesAsync();
                if (!result)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro. Algo correu mal ao adicionar o novo campo.");
                }

                return Created("", new ApiResponse(StatusCodes.Status201Created, "Sucesso! Novo campo adicionado."));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpPut("{templateFieldId:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateTemplateField(int entityTypeId, int templateFieldId, NewDataFieldTemplateModel model)
        {
            try
            {
                if (entityTypeId <= 0 || model.FieldType < 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Modelo incorreto. Verifique os dados inseridos."));
                }

                if (model.FieldType == DataFieldTypeEnum.Combobox && (model.ComboboxOptions == null || model.ComboboxOptions.Length == 0))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Campo de escolha múltipa requer opções. Verifique os dados inseridos."));
                }

                DataFieldTemplate foundFieldTemplate = await _repo.GetDataFieldTemplateByNameAsync(entityTypeId, model.Name);
                if (foundFieldTemplate != null)
                {
                    return Conflict(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Um campo com esse nome já existe."));
                }

                var field = await _repo.GetDataFieldTemplateByIdAsync(templateFieldId);
                if (field == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro. Campo não encontrado."));
                }

                if (model.FieldType == DataFieldTypeEnum.Combobox && field.FieldType != DataFieldTypeEnum.Combobox)
                {
                    field.ComboboxOptions = new List<DataFieldTemplateComboboxOption>();
                    foreach (var item in model.ComboboxOptions)
                    {
                        field.ComboboxOptions.Add(new DataFieldTemplateComboboxOption() { Name = item, DataFieldTemplate = field });
                    }
                }
                else if (field.FieldType == DataFieldTypeEnum.Combobox && model.FieldType != DataFieldTypeEnum.Combobox)
                {
                    _repo.RemoveDataFieldTemplateComboboxOptions(await _repo.GetDataFieldTemplateComboboxOptionsAsync(field.Id));
                }
                else if (field.FieldType == DataFieldTypeEnum.Combobox && model.FieldType == DataFieldTypeEnum.Combobox)
                {
                    var options = await _repo.GetDataFieldTemplateComboboxOptionsAsync(field.Id);
                    if (options.Count == model.ComboboxOptions.Count())
                    {
                        for (int i = 0; i < options.Count; i++)
                        {
                            options[i].Name = model.ComboboxOptions[i];
                        }
                    }
                    else
                    {
                        _repo.RemoveDataFieldTemplateComboboxOptions(options);
                        field.ComboboxOptions = new List<DataFieldTemplateComboboxOption>();
                        foreach (var item in model.ComboboxOptions)
                        {
                            field.ComboboxOptions.Add(new DataFieldTemplateComboboxOption() { Name = item, DataFieldTemplate = field });
                        }
                    }
                }

                field.Name = model.Name;
                field.MustHave = model.MustHave;
                field.FieldType = model.FieldType;

                bool result = await _repo.SaveChangesAsync();
                if (!result)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro. Algo correu mal ao atualizar o campo.");
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Sucesso! Campo atualizado com sucesso."));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{templateFieldId:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteTemplateField(int entityTypeId, int templateFieldId)
        {
            try
            {
                if (await _repo.GetEntityTypeByIdAsync(entityTypeId) == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro. Tipo de entidade não encontrado."));
                }

                var field = await _repo.GetDataFieldTemplateByIdAsync(templateFieldId);
                if (field == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro. Campo não encontrado."));
                }

                _repo.RemoveDataFieldTemplate(field);
                bool result = await _repo.SaveChangesAsync();
                if (!result)
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro. Algo correu mal ao eliminar o campo.");
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Sucesso! Campo eliminado com sucesso."));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }
    }
}