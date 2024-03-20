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
    public class RatingController : ControllerBase
    {
        private readonly IBaseRepository<Rating> _ratingRepository;
        private readonly IMapper _mapper;

        public RatingController(IBaseRepository<Rating> ratingRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(RatingForAddDto ratingForAdd)
        {
            if (!AuthorizationHelper.IsAuthorizedForAdding(User, out var userId))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            if (ratingForAdd == null)
                return BadRequest("Rating data is missing.");

            var rating = _mapper.Map<Rating>(ratingForAdd);

            rating.UserId = userId;
            rating.CreatedAt = DateTime.UtcNow;

            var result = await _ratingRepository.Add(rating);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(RatingForEditDto ratingForEdit)
        {
            if (!AuthorizationHelper.IsAuthorizedForEditing(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var result = await _ratingRepository.Edit(ratingForEdit.Id, ratingForEdit);

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

            var result = await _ratingRepository.Delete(id);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("{ratingId}/details")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetRatingById(int ratingId)
        {
            var rating = await _ratingRepository.GetById(ratingId);
            return Ok(rating);
        }
    }
}
