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
    [Route("api/entities/types/templates/fields")]
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
        public async Task<ActionResult<ApiResponse>> AddFieldToTemplate(NewDataFieldTemplateModel model)
        {
            try
            {
                if (model.FieldType < 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Modelo incorreto. Verifique os dados inseridos."));
                }

                if (model.FieldType == DataFieldTypeEnum.Combobox && (model.ComboboxOptions == null || model.ComboboxOptions.Length == 0))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Campo de escolha múltipa requer opções. Verifique os dados inseridos."));
                }

                DataFieldTemplate foundFieldTemplate = await _repo.GetDataFieldTemplateByNameAsync(model.EntityTypeId, model.Name);
                if (foundFieldTemplate != null)
                {
                    return Conflict(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Um campo com esse nome já existe."));
                }

                var newFieldTemplate = _mapper.Map<DataFieldTemplate>(model);
                newFieldTemplate.EntityType = await _repo.GetEntityTypeByIdAsync(model.EntityTypeId);

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

                return Created("", new ApiResponse<DataFieldTemplateModel>(StatusCodes.Status201Created, "Sucesso! Novo campo adicionado.", _mapper.Map<DataFieldTemplateModel>(newFieldTemplate)));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }
    }
}
