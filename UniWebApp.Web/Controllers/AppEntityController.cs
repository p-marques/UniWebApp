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
    }
}
