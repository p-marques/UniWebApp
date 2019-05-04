using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [HttpPost("new")]
        public async Task<ApiResponse> AddEntityType(NewAppEntityTypeModel model)
        {
            try
            {
                var newType = _mapper.Map<AppEntityType>(model);
                _repo.AddEntityType(newType);

                bool saved = await _repo.SaveChangesAsync();
                if (!saved)
                {
                    return new ApiResponse(StatusCodes.Status400BadRequest, "Erro ao tentar criar um novo tipo de entidade. Verifique o modelo e tente novamente.");
                }

                Response.StatusCode = StatusCodes.Status201Created;
                return new ApiResponse(StatusCodes.Status201Created, $"Tipo de entidade '{model.Name}' criado com sucesso.");
            }
            catch (Exception)
            {
                return new ApiResponse<string>(StatusCodes.Status500InternalServerError, "Erro inesperado ao tentar adicionar item.", "");
            }
        }
    }
}
