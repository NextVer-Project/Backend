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
        private readonly ITvShowRepository _specificTvShowRepository;
        private readonly IMapper _mapper;

        public EpisodeController(IBaseRepository<TvShow> tvShowRepository, IBaseRepository<Episode> episodeRepository, ITvShowRepository specificTvShowRepository, IMapper mapper)
        {
            _tvShowRepository = tvShowRepository;
            _episodeRepository = episodeRepository;
            _specificTvShowRepository = specificTvShowRepository;
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

            var existingEpisode = await _specificTvShowRepository.GetEpisodeByNumberAndSeason(episodeForAddDto.TvShowId, episodeForAddDto.SeasonNumber, episodeForAddDto.EpisodeNumber);
            if (existingEpisode != null)
            {
                return BadRequest($"Episode with number {episodeForAddDto.EpisodeNumber} already exists in season {episodeForAddDto.SeasonNumber} of {tvShow.Title} series.");
            }

            var episode = _mapper.Map<Episode>(episodeForAddDto);
            episode.UserId = userId;
            episode.ViewCounter = 0;
            episode.IsApproved = false;

            var tvShowId = episode.TvShowId;
            var result = await _episodeRepository.Add(episode);

            if (result)
            {
                await UpdateTvShowRuntime(tvShowId);
            }

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

            var tvShowId = existingTvShow.TvShowId;
            var result = await _episodeRepository.Edit(episodeForEditDto.Id, episodeForEditDto);

            if (result)
            {
                await UpdateTvShowRuntime(tvShowId);
            }

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

            if (episode.EpisodeNumber == 1 && episode.SeasonNumber == 1)
            {
                return BadRequest("Cannot delete the first episode of the first season.");
            }

            var tvShowId = episode.TvShowId;
            var result = await _episodeRepository.Delete(id);

            if (result)
            {
                await UpdateTvShowRuntime(tvShowId);
            }

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        private async Task UpdateTvShowRuntime(int tvShowId)
        {
            var duration = await _specificTvShowRepository.GetTotalDurationForTvShowInSeconds(tvShowId);

            var tvShow = await _tvShowRepository.GetById(tvShowId);
            if (tvShow != null)
            {
                string totalDurationString = TimeSpan.FromSeconds(duration).ToString(@"d\:hh\:mm\:ss");
                tvShow.Runtime = totalDurationString;
                await _tvShowRepository.Update(tvShow);
            }
        }

        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<Episode>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchEpisodes(string name)
        {
            var result = await _episodeRepository.GetEntitiesBy<Episode>(m => m.Title.Contains(name));
            return result != null ? Ok(result) : StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
