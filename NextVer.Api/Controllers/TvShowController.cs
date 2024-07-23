using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextVer.Domain.DTOs;
using NextVer.Domain.Models;
using NextVer.Infrastructure.Helpers;
using NextVer.Infrastructure.Helpers.PaginationParameters;
using NextVer.Infrastructure.Interfaces;
using NextVerBackend.Extensions;
using NextVerBackend.Helpers;
using System.Net;

namespace NextVerBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TvShowController : ControllerBase
    {
        private readonly IBaseRepository<TvShow> _tvShowRepository;
        private readonly ITvShowRepository _specificTvShowRepository;
        private readonly IMapper _mapper;

        public TvShowController(IBaseRepository<TvShow> tvShowRepository, ITvShowRepository specificTvShowRepository, IMapper mapper)
        {
            _tvShowRepository = tvShowRepository;
            _specificTvShowRepository = specificTvShowRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(TvShowForAddDto tvShowForAdd)
        {
            if (!AuthorizationHelper.IsAuthorizedForAdding(User, out var userId))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var tvShow = _mapper.Map<TvShow>(tvShowForAdd);
            tvShow.UserId = userId;
            tvShow.CreatedAt = DateTime.UtcNow;
            tvShow.UpdatedAt = DateTime.UtcNow;
            tvShow.TrailerUrl = VideoLinkHelper.ConvertVideoLink(tvShow.TrailerUrl);

            var result = await _tvShowRepository.Add(tvShow);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            if (tvShowForAdd.GenreIds != null && tvShowForAdd.GenreIds.Any())
            {
                foreach (var genreId in tvShowForAdd.GenreIds)
                {
                    var tvShowGenreToAdd = new TvShowGenre
                    {
                        TvShowId = tvShow.Id,
                        GenreId = genreId
                    };
                    _tvShowRepository.AddLinkEntity(tvShowGenreToAdd);
                }

                var genresAdded = await _tvShowRepository.SaveChangesAsync();
                if (!genresAdded)
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            if (tvShowForAdd.UniverseIds != null && tvShowForAdd.UniverseIds.Any())
            {
                foreach (var universeId in tvShowForAdd.UniverseIds)
                {
                    var tvShowUniverseToAdd = new TvShowUniverse
                    {
                        TvShowId = tvShow.Id,
                        UniverseId = universeId
                    };
                    _tvShowRepository.AddLinkEntity(tvShowUniverseToAdd);
                }

                var universesAdded = await _tvShowRepository.SaveChangesAsync();
                if (!universesAdded)
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Ok();
        }

        [HttpPatch("edit")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(TvShowForEditDto tvShowForEditDto)
        {
            if (!AuthorizationHelper.IsAuthorizedForEditing(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var existingTvShow = await _tvShowRepository.GetById(tvShowForEditDto.Id);
            if (existingTvShow == null)
                return BadRequest("TvShow not found.");

            var tvShowGenres = _tvShowRepository.GetLinkEntitiesFor<TvShow, TvShowGenre>(
                linkEntity => linkEntity.TvShowId == tvShowForEditDto.Id
            ).ToList();

            var existingGenreIds = tvShowGenres.Select(linkEntity => linkEntity.GenreId).ToList();

            var updatedGenreIds = tvShowForEditDto.GenreIds.ToList();

            var newGenreIds = updatedGenreIds.Except(existingGenreIds).ToList();

            var genresToRemove = tvShowGenres.Where(linkEntity => !updatedGenreIds.Contains(linkEntity.GenreId)).ToList();

            foreach (var genreToRemove in genresToRemove)
            {
                _tvShowRepository.RemoveLinkEntity(genreToRemove);
            }

            foreach (var newGenreId in newGenreIds)
            {
                var tvShowGenreToAdd = new TvShowGenre
                {
                    TvShowId = tvShowForEditDto.Id,
                    GenreId = newGenreId
                };
                _tvShowRepository.AddLinkEntity(tvShowGenreToAdd);
            }

            var tvShowUniverses = _tvShowRepository.GetLinkEntitiesFor<TvShow, TvShowUniverse>(
                linkEntity => linkEntity.TvShowId == tvShowForEditDto.Id
            ).ToList();

            var existingUniverseIds = tvShowUniverses.Select(linkEntity => linkEntity.UniverseId).ToList();

            var updatedUniverseIds = tvShowForEditDto.UniverseIds.ToList();

            var newUniverseIds = updatedUniverseIds.Except(existingUniverseIds).ToList();

            var universesToRemove = tvShowUniverses.Where(linkEntity => !updatedUniverseIds.Contains(linkEntity.UniverseId)).ToList();

            foreach (var universeToRemove in universesToRemove)
            {
                _tvShowRepository.RemoveLinkEntity(universeToRemove);
            }

            foreach (var newUniverseId in newUniverseIds)
            {
                var tvShowUniverseToAdd = new TvShowUniverse
                {
                    TvShowId = tvShowForEditDto.Id,
                    UniverseId = newUniverseId
                };
                _tvShowRepository.AddLinkEntity(tvShowUniverseToAdd);
            }

            existingTvShow.TrailerUrl = VideoLinkHelper.ConvertVideoLink(existingTvShow.TrailerUrl);

            var result = await _tvShowRepository.Edit(tvShowForEditDto.Id, tvShowForEditDto);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!AuthorizationHelper.IsAuthorizedForDeletion(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var existingTvShow = await _tvShowRepository.GetById(id);
            if (existingTvShow == null)
                return BadRequest("TvShow not found.");

            var tvShowGenres = _tvShowRepository.GetLinkEntitiesFor<TvShow, TvShowGenre>(
                linkEntity => linkEntity.TvShowId == id
            ).ToList();

            foreach (var tvShowGenre in tvShowGenres)
            {
                _tvShowRepository.RemoveLinkEntity(tvShowGenre);
            }

            var tvShowUniverses = _tvShowRepository.GetLinkEntitiesFor<TvShow, TvShowUniverse>(
                linkEntity => linkEntity.TvShowId == id
            ).ToList();

            foreach (var tvShowUniverse in tvShowUniverses)
            {
                _tvShowRepository.RemoveLinkEntity(tvShowUniverse);
            }

            var result = await _tvShowRepository.Delete(id);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("count")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TvShowCountDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetNumberOfTvShows()
        {
            var result = await _tvShowRepository.GetNumberOfEntities<TvShow>();

            return result switch
            {
                < 0 => StatusCode((int)HttpStatusCode.InternalServerError),
                0 => BadRequest("Could not find any tvShows"),
                _ => Ok(new TvShowCountDto { NumberOfTvShows = result })
            };
        }

        [HttpGet("season/duration/{tvShowId}/{seasonNum}/minutes")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetSeasonDurationForTvShowInMinutes(int tvShowId, int seasonNum)
        {
            var tvShows = await _specificTvShowRepository.GetSeasonDurationForTvShowInMinutes(tvShowId, seasonNum);
            return Ok(tvShows);
        }

        [HttpGet("season/duration/{tvShowId}/minutes")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetTotalDurationForTvShowInMinutes(int tvShowId)
        {
            var tvShows = await _specificTvShowRepository.GetTotalDurationForTvShowInMinutes(tvShowId);
            return Ok(tvShows);
        }

        [HttpGet("season/duration/{tvShowId}/{seasonNum}/seconds")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetSeasonDurationForTvShowInSeconds(int tvShowId, int seasonNum)
        {
            var tvShows = await _specificTvShowRepository.GetSeasonDurationForTvShowInSeconds(tvShowId, seasonNum);
            return Ok(tvShows);
        }

        [HttpGet("season/duration/{tvShowId}/seconds")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetTotalDurationForTvShowInSeconds(int tvShowId)
        {
            var tvShows = await _specificTvShowRepository.GetTotalDurationForTvShowInSeconds(tvShowId);
            return Ok(tvShows);
        }

        [AllowAnonymous]
        [HttpGet("genre/{genreId}")]
        public async Task<IActionResult> GetTvShowsByGenre(int genreId)
        {
            var tvShows = await _tvShowRepository.GetEntitiesBy<TvShow>(m => m.TvShowGenres.Any(mg => mg.GenreId == genreId));
            return Ok(tvShows);
        }

        [AllowAnonymous]
        [HttpGet("{tvShowId}/genres")]
        public async Task<IActionResult> GetGenresByTvShow(int tvShowId)
        {
            var genres = await _tvShowRepository.GetEntitiesBy<Genre>(m => m.TvShowGenres.Any(mu => mu.TvShowId == tvShowId));
            return Ok(genres);
        }

        [AllowAnonymous]
        [HttpGet("universe/{universeId}")]
        public async Task<IActionResult> GetTvShowsByUniverse(int universeId)
        {
            var tvShows = await _tvShowRepository.GetEntitiesBy<TvShow>(m => m.TvShowUniverses.Any(mu => mu.UniverseId == universeId));
            return Ok(tvShows);
        }

        [AllowAnonymous]
        [HttpGet("{tvShowId}/universes")]
        public async Task<IActionResult> GetUniversesByTvShow(int tvShowId)
        {
            var universes = await _tvShowRepository.GetEntitiesBy<Universe>(m => m.TvShowUniverses.Any(mu => mu.TvShowId == tvShowId));
            return Ok(universes);
        }

        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<TvShow>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchTvShows(string title)
        {
            var result = await _tvShowRepository.GetEntitiesBy<TvShow>(m => m.Title.Contains(title));
            return result != null ? Ok(result) : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("latest")]
        [ProducesResponseType(typeof(IEnumerable<TvShow>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetLatestTvShows(int limit, string sortBy)
        {
            var result = await _tvShowRepository.GetEntitiesBy<TvShow>(m => true);

            switch (sortBy.ToLower())
            {
                case "newest":
                default:
                    result = result.OrderByDescending(m => m.CreatedAt);
                    break;
            }

            var latestTvShows = result.Take(limit);
            return latestTvShows != null ? Ok(latestTvShows) : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<TvShow>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetTvShowsByUser(int userId)
        {
            var result = await _tvShowRepository.GetEntitiesBy<TvShow>(m => m.UserId == userId);
            return result != null ? Ok(result) : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedList<TvShowForListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetTvShows([FromQuery] ProductionParameters parameters)
        {
            var tvShows = await _specificTvShowRepository.GetTvShows(parameters);

            if (tvShows == null || !tvShows.Any())
                return BadRequest("Could not find any tv shows");

            Response.AddPaginationHeader(tvShows.CurrentPage, tvShows.PageSize, tvShows.TotalCount, tvShows.TotalPages);

            return Ok(tvShows);
        }

        [AllowAnonymous]
        [HttpGet("{tvShowId}")]
        public async Task<IActionResult> GetTvShowById(int tvShowId)
        {
            var tvShow = await _tvShowRepository.GetById(tvShowId);
            return Ok(tvShow);
        }
    }
}