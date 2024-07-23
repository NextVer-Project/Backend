using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextVer.Domain.DTOs;
using NextVer.Domain.Models;
using NextVer.Infrastructure.Interfaces;
using NextVerBackend.Enums;
using NextVerBackend.Helpers;
using System.Net;

namespace NextVerBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IBaseRepository<Rating> _ratingBaseRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public RatingController(IRatingRepository ratingRepository, IMapper mapper)
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

            var result = await _ratingBaseRepository.Add(rating);

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

            var result = await _ratingBaseRepository.Edit(ratingForEdit.Id, ratingForEdit);

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

            var result = await _ratingBaseRepository.Delete(id);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("{ratingId}/details")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetRatingById(int ratingId)
        {
            var rating = await _ratingBaseRepository.GetById(ratingId);
            return Ok(rating);
        }

        [HttpGet("averageMovie/{movieId}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAverageMovieRating(int movieId)
        {
            var averageRating = await _ratingRepository.GetAverageRating(movieId, (int)ProductionTypes.Movie);

            return Ok(averageRating);
        }

        [HttpGet("ratingCountMovie/{movieId}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMovieRatingCount(int movieId)
        {
            var ratingCount = await _ratingRepository.GetRatingCount(movieId, (int)ProductionTypes.Movie);

            return Ok(ratingCount);
        }

        [HttpGet("averageTvShow/{tvShowId}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAverageTvShowRating(int tvShowId)
        {
            var averageRating = await _ratingRepository.GetAverageRating(tvShowId, (int)ProductionTypes.TvShow);

            return Ok(averageRating);
        }

        [HttpGet("ratingCountTvShow/{tvShowId}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetTvShowRatingCount(int tvShowId)
        {
            var ratingCount = await _ratingRepository.GetRatingCount(tvShowId, (int)ProductionTypes.TvShow);

            return Ok(ratingCount);
        }
    }
}
