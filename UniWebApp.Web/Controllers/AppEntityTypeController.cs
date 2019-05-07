using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public async Task<ActionResult<ApiResponse>> GetEntityTypes()
        {
            try
            {
                var types = await _repo.GetAllEntityTypesAsync();

                return Ok(new ApiResponse<List<AppEntityTypeModel>>(StatusCodes.Status200OK, "OK", _mapper.Map<List<AppEntityTypeModel>>(types)));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new ApiResponse(StatusCodes.Status500InternalServerError, "Erro inesperado. Tente mais tarde."));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddEntityType(AppEntityTypeModel model)
        {
            try
            {
                if (await _repo.GetEntityTypeByNameAsync(model.Name.Trim()) != null)
                {
                    return Conflict(new ApiResponse(StatusCodes.Status409Conflict, "Erro. Tipo de entidade já existe."));
                }

                var newType = _mapper.Map<AppEntityType>(model);
                _repo.AddEntityType(newType);

                bool saved = await _repo.SaveChangesAsync();
                if (!saved)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro ao tentar criar um novo tipo de entidade. Verifique o modelo e tente novamente."));
                }

                return Created("", new ApiResponse<AppEntityTypeModel>(StatusCodes.Status201Created, 
                    $"Tipo de entidade '{model.Name}' criado com sucesso.", _mapper.Map<AppEntityTypeModel>(newType)));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Erro inesperado ao tentar adicionar item."));
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> UpdateEntityTypeName(AppEntityTypeModel model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Id do objeto é obrigatório."));
                }

                model.Name = model.Name.Trim();
                var typeToUpdate = await _repo.GetEntityTypeByIdAsync(model.Id);
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

        [HttpDelete]
        public async Task<ActionResult<ApiResponse>> DeleteEntityType(AppEntityTypeModel model)
        {
            try
            {
                if (model.Id <= 0)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Erro. Id do objeto é obrigatório."));
                }

                var typeToDelete = await _repo.GetEntityTypeByIdAsync(model.Id);
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

                return Ok(new ApiResponse(StatusCodes.Status200OK, $"Tipo de entidade eliminado com sucesso."));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Erro inesperado ao tentar eliminar item."));
            }
        }
    }
}
