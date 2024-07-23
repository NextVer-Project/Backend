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
    public class ReleasePlaceController : ControllerBase
    {
        private readonly IBaseRepository<ReleasePlace> _releasePlaceRepository;
        private readonly IMapper _mapper;

        public ReleasePlaceController(IBaseRepository<ReleasePlace> releasePlaceRepository, IMapper mapper)
        {
            _releasePlaceRepository = releasePlaceRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(ReleasePlaceForAddDto releasePlaceForAdd)
        {
            if (!AuthorizationHelper.IsAuthorizedForAdding(User, out var userId))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            if (releasePlaceForAdd == null)
                return BadRequest("Release Place data is missing.");

            var releasePlace = _mapper.Map<ReleasePlace>(releasePlaceForAdd);

            var result = await _releasePlaceRepository.Add(releasePlace);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(ReleasePlaceForEditDto releasePlaceForEdit)
        {
            if (!AuthorizationHelper.IsAuthorizedForEditing(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var result = await _releasePlaceRepository.Edit(releasePlaceForEdit.Id, releasePlaceForEdit);

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

            var result = await _releasePlaceRepository.Delete(id);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GeReleasePlaces()
        {
            var releasePlaces = await _releasePlaceRepository.GetAll();

            if (releasePlaces == null || !releasePlaces.Any())
                return BadRequest("Could not find any genres");

            return Ok(releasePlaces);
        }

        [HttpGet("{releasePlaceId}/details")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetReleasePlaceById(int releasePlaceId)
        {
            var releasePlace = await _releasePlaceRepository.GetById(releasePlaceId);
            return Ok(releasePlace);
        }

        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<ReleasePlace>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchReleasePlaces(string name)
        {
            var result = await _releasePlaceRepository.GetEntitiesBy<ReleasePlace>(m => m.Name.Contains(name));
            return result != null ? Ok(result) : StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
