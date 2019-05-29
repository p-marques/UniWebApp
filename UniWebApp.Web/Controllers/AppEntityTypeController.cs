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
        public async Task<ActionResult<ApiResponse>> GetEntityTypes()
        {
            try
            {
                var types = await _repo.GetAllEntityTypesAsync();
                var returnObj = new List<AppEntityTypeModel>();
                foreach (var item in types)
                {
                    var a = new AppEntityTypeModel()
                    {
                        Id = item.Id,
                        Name = item.Name
                    };

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
        public async Task<ActionResult<ApiResponse>> GetEntityType(int id)
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

                return Ok(new ApiResponse<AppEntityTypeModel>(StatusCodes.Status200OK, "OK", typeModel));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse(StatusCodes.Status500InternalServerError, "Erro inesperado. Tente mais tarde."));
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