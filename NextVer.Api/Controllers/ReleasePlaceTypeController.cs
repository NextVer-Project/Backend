using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextVer.Domain.DTOs;
using NextVer.Domain.Models;
using NextVer.Infrastructure.Interfaces;
using NextVerBackend.Helpers;
using System.Net;

namespace NextVerBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReleasePlaceTypeController : ControllerBase
    {
        private readonly IBaseRepository<ReleasePlaceType> _releasePlaceTypeRepository;
        private readonly IMapper _mapper;

        public ReleasePlaceTypeController(IBaseRepository<ReleasePlaceType> releasePlaceTypeRepository, IMapper mapper)
        {
            _releasePlaceTypeRepository = releasePlaceTypeRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(ReleasePlaceTypeForAddDto releasePlaceTypeForAdd)
        {
            if (!AuthorizationHelper.IsAuthorizedForAdding(User, out var userId))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            if (releasePlaceTypeForAdd == null)
                return BadRequest("Release Place Type data is missing.");

            var releasePlaceType = _mapper.Map<ReleasePlaceType>(releasePlaceTypeForAdd);

            var result = await _releasePlaceTypeRepository.Add(releasePlaceType);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(ReleasePlaceTypeForEditDto releasePlaceTypeForEdit)
        {
            if (!AuthorizationHelper.IsAuthorizedForEditing(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var result = await _releasePlaceTypeRepository.Edit(releasePlaceTypeForEdit.Id, releasePlaceTypeForEdit);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!AuthorizationHelper.IsAuthorizedForDeletion(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var result = await _releasePlaceTypeRepository.Delete(id);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("{releasePlaceTypeId}/details")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetReleasePlaceTypeById(int releasePlaceTypeId)
        {
            var releasePlaceType = await _releasePlaceTypeRepository.GetById(releasePlaceTypeId);
            return Ok(releasePlaceType);
        }
    }
}
