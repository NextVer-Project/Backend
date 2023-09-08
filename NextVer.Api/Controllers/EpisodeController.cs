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
    public class EpisodeController : ControllerBase
    {
        private readonly IBaseRepository<TvShow> _tvShowRepository;
        private readonly IBaseRepository<Episode> _episodeRepository;
        private readonly IMapper _mapper;

        public EpisodeController(IBaseRepository<TvShow> tvShowRepository, IBaseRepository<Episode> episodeRepository, IMapper mapper)
        {
            _tvShowRepository = tvShowRepository;
            _episodeRepository = episodeRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddEpisode(EpisodeForAddDto episodeForAddDto)
        {
            if (!AuthorizationHelper.IsAuthorizedForAdding(User, out var userId))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            if (episodeForAddDto == null)
                return BadRequest("Episode data is missing.");

            var tvShow = await _tvShowRepository.GetById(episodeForAddDto.TvShowId);
            if (tvShow == null)
                return BadRequest("TvShow not found.");

            var episode = _mapper.Map<Episode>(episodeForAddDto);
            episode.UserId = userId;
            episode.ViewCounter = 0;
            episode.IsApproved = false;

            var result = await _episodeRepository.Add(episode);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }


        [HttpPatch("edit")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(EpisodeForEditDto episodeForEditDto)
        {
            if (!AuthorizationHelper.IsAuthorizedForEditing(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var existingTvShow = await _episodeRepository.GetById(episodeForEditDto.Id);
            if (existingTvShow == null)
                return BadRequest("TvShow not found.");

            var result = await _episodeRepository.Edit(episodeForEditDto.Id, episodeForEditDto);

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

            var episode = await _episodeRepository.GetById(id);
            if (episode == null)
                return BadRequest("Episode not found.");

            var result = await _episodeRepository.Delete(id);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
