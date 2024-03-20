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
    public class MediaGalleryTypeController : ControllerBase
    {
        private readonly IBaseRepository<MediaGalleryType> _mediaGalleryTypeRepository;
        private readonly IMapper _mapper;

        public MediaGalleryTypeController(IBaseRepository<MediaGalleryType> mediaGalleryTypeRepository, IMapper mapper)
        {
            _mediaGalleryTypeRepository = mediaGalleryTypeRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(MediaGalleryTypeForAddDto mediaGalleryTypeForAdd)
        {
            if (!AuthorizationHelper.IsAuthorizedForAdding(User, out var userId))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            if (mediaGalleryTypeForAdd == null)
                return BadRequest("Media Gallery Type data is missing.");

            var mediaGalleryType = _mapper.Map<MediaGalleryType>(mediaGalleryTypeForAdd);

            var result = await _mediaGalleryTypeRepository.Add(mediaGalleryType);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(MediaGalleryTypeForEditDto mediaGalleryTypeForEdit)
        {
            if (!AuthorizationHelper.IsAuthorizedForEditing(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var result = await _mediaGalleryTypeRepository.Edit(mediaGalleryTypeForEdit.Id, mediaGalleryTypeForEdit);

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

            var result = await _mediaGalleryTypeRepository.Delete(id);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("{mediaGalleryTypeId}/details")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMediaGalleryTypeById(int mediaGalleryTypeId)
        {
            var mediaGalleryType = await _mediaGalleryTypeRepository.GetById(mediaGalleryTypeId);
            return Ok(mediaGalleryType);
        }
    }
}
