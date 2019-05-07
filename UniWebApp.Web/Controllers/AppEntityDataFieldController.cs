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
    [Route("api/entities/fields")]
    [ApiController]
    public class AppEntityDataFieldController : ControllerBase
    {
        private readonly IUniWebAppRepository _repo;

        public AppEntityDataFieldController(IUniWebAppRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddDataField(NewAppEntityDataFieldModel model)
        {
            try
            {
                var entity = await _repo.GetEntityByIdAsync(model.EntityId, includeFields: true);
                if (entity == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Erro. Essa entidade não existe."));
                }

                if(entity.Fields.Where(x => x.Name == model.Name).Count() > 0)
                {
                    return Conflict(new ApiResponse(StatusCodes.Status409Conflict, "Erro. Um campo com esse nome já existe nesta entidade."));
                }

                // WIP

                return Ok();

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor.");
            }
        }
    }
}
