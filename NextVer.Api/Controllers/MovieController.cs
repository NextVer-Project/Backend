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
    public class MovieController : ControllerBase
    {

        private readonly IBaseRepository<Movie> _movieRepository;
        private readonly IMovieRepository _specificMovieRepository;
        private readonly IMapper _mapper;

        public MovieController(IBaseRepository<Movie> movieRepository, IMovieRepository specificTvShowRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _specificMovieRepository = specificTvShowRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(MovieForAddDto movieForAdd)
        {
            if (!AuthorizationHelper.IsAuthorizedForAdding(User, out var userId))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var movie = _mapper.Map<Movie>(movieForAdd);

            movie.UserId = userId;
            movie.CreatedAt = DateTime.UtcNow;
            movie.UpdatedAt = DateTime.UtcNow;
            movie.TrailerUrl = VideoLinkHelper.ConvertVideoLink(movie.TrailerUrl);

            var result = await _movieRepository.Add(movie);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            if (movieForAdd.GenreIds != null && movieForAdd.GenreIds.Any())
            {
                foreach (var genreId in movieForAdd.GenreIds)
                {
                    var movieGenreToAdd = new MovieGenre
                    {
                        MovieId = movie.Id,
                        GenreId = genreId
                    };
                    _movieRepository.AddLinkEntity(movieGenreToAdd);
                }

                var genresAdded = await _movieRepository.SaveChangesAsync();
                if (!genresAdded)
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            if (movieForAdd.UniverseIds != null && movieForAdd.UniverseIds.Any())
            {
                foreach (var universeId in movieForAdd.UniverseIds)
                {
                    var movieUniverseToAdd = new MovieUniverse
                    {
                        MovieId = movie.Id,
                        UniverseId = universeId
                    };
                    _movieRepository.AddLinkEntity(movieUniverseToAdd);
                }

                var universesAdded = await _movieRepository.SaveChangesAsync();
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
        public async Task<IActionResult> Update(MovieForEditDto movieForEditDto)
        {
            if (!AuthorizationHelper.IsAuthorizedForEditing(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var existingMovie = await _movieRepository.GetById(movieForEditDto.Id);
            if (existingMovie == null)
                return BadRequest("Movie not found.");

            var movieGenres = _movieRepository.GetLinkEntitiesFor<Movie, MovieGenre>(
                linkEntity => linkEntity.MovieId == movieForEditDto.Id
            ).ToList();


            var existingGenreIds = movieGenres.Select(linkEntity => linkEntity.GenreId).ToList();

            var updatedGenreIds = movieForEditDto.GenreIds.ToList();

            var newGenreIds = updatedGenreIds.Except(existingGenreIds).ToList();

            var genresToRemove = movieGenres.Where(linkEntity => !updatedGenreIds.Contains(linkEntity.GenreId)).ToList();

            foreach (var genreToRemove in genresToRemove)
            {
                _movieRepository.RemoveLinkEntity(genreToRemove);
            }

            foreach (var newGenreId in newGenreIds)
            {
                var movieGenreToAdd = new MovieGenre
                {
                    MovieId = movieForEditDto.Id,
                    GenreId = newGenreId
                };
                _movieRepository.AddLinkEntity(movieGenreToAdd);
            }

            var movieUniverses = _movieRepository.GetLinkEntitiesFor<Movie, MovieUniverse>(
                linkEntity => linkEntity.MovieId == movieForEditDto.Id
            ).ToList();


            var existingUniverseIds = movieUniverses.Select(linkEntity => linkEntity.UniverseId).ToList();

            var updatedUniverseIds = movieForEditDto.UniverseIds.ToList();

            var newUniverseIds = updatedUniverseIds.Except(existingUniverseIds).ToList();

            var universesToRemove = movieUniverses.Where(linkEntity => !updatedUniverseIds.Contains(linkEntity.UniverseId)).ToList();

            foreach (var universeToRemove in universesToRemove)
            {
                _movieRepository.RemoveLinkEntity(universeToRemove);
            }

            foreach (var newUniverseId in newUniverseIds)
            {
                var movieUniverseToAdd = new MovieUniverse
                {
                    MovieId = movieForEditDto.Id,
                    UniverseId = newUniverseId
                };
                _movieRepository.AddLinkEntity(movieUniverseToAdd);
            }

            existingMovie.TrailerUrl = VideoLinkHelper.ConvertVideoLink(existingMovie.TrailerUrl);

            var result = await _movieRepository.Edit(movieForEditDto.Id, movieForEditDto);

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

            var existingMovie = await _movieRepository.GetById(id);
            if (existingMovie == null)
                return BadRequest("Movie not found.");

            var movieGenres = _movieRepository.GetLinkEntitiesFor<Movie, MovieGenre>(
                linkEntity => linkEntity.MovieId == id
            ).ToList();

            foreach (var movieGenre in movieGenres)
            {
                _movieRepository.RemoveLinkEntity(movieGenre);
            }

            var movieUniverses = _movieRepository.GetLinkEntitiesFor<Movie, MovieUniverse>(
                linkEntity => linkEntity.MovieId == id
            ).ToList();

            foreach (var movieUniverse in movieUniverses)
            {
                _movieRepository.RemoveLinkEntity(movieUniverse);
            }

            var result = await _movieRepository.Delete(id);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("count")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MovieCountDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetNumberOfMovies()
        {
            var result = await _movieRepository.GetNumberOfEntities<Movie>();

            return result switch
            {
                < 0 => StatusCode((int)HttpStatusCode.InternalServerError),
                0 => BadRequest("Could not find any movies"),
                _ => Ok(new MovieCountDto { NumberOfMovies = result })
            };
        }

        [HttpGet("genre/{genreId}")]
        public async Task<IActionResult> GetMoviesByGenre(int genreId)
        {
            var movies = await _movieRepository.GetEntitiesBy<Movie>(m => m.MovieGenres.Any(mg => mg.GenreId == genreId));
            return Ok(movies);
        }

        [AllowAnonymous]
        [HttpGet("{movieId}/genres")]
        public async Task<IActionResult> GetGenresByMovie(int movieId)
        {
            var genres = await _movieRepository.GetEntitiesBy<Genre>(m => m.MovieGenres.Any(mu => mu.MovieId == movieId));
            return Ok(genres);
        }

        [HttpGet("universe/{universeId}")]
        public async Task<IActionResult> GetMoviesByUniverse(int universeId)
        {
            var movies = await _movieRepository.GetEntitiesBy<Movie>(m => m.MovieUniverses.Any(mu => mu.UniverseId == universeId));
            return Ok(movies);
        }

        [AllowAnonymous]
        [HttpGet("{movieId}/universes")]
        public async Task<IActionResult> GetUniverseByMovie(int movieId)
        {
            var universes = await _movieRepository.GetEntitiesBy<Universe>(m => m.MovieUniverses.Any(mu => mu.MovieId == movieId));
            return Ok(universes);
        }

        [AllowAnonymous]
        [HttpGet("{movieId}/releasePlaces")]
        public async Task<IActionResult> GetMovieVodByMovieId(int movieId)
        {
            var releasePlaces = await _specificMovieRepository.GetProductionVersionsVodByMovieId(movieId);
            return Ok(releasePlaces);
        }

        [HttpGet("{movieId}/details")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMovieDetailsById(int movieId)
        {
            var productions = await _specificMovieRepository.GetProductionVersionsWithDetailsByMovieId(movieId);

            if (productions == null || productions.Count == 0)
                return BadRequest("Could not find any movie versions with this ID");

            foreach (var production in productions)
            {
                var technologiesForProduction = await _specificMovieRepository.GetTechnologiesByProductionVersionId(production.Id);

                if (technologiesForProduction == null)
                    return BadRequest("Technologies not found for this production");

                production.Technologies = technologiesForProduction.ToList();
            }

            return productions != null ? Ok(productions) : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [AllowAnonymous]
        [HttpGet("{movieId}/trailer")]
        public async Task<IActionResult> GetTrailerUrlByMovie(int movieId)
        {
            var movie = await _movieRepository.GetById(movieId);

            if (movie != null)
            {
                return Content(movie.TrailerUrl, "text/plain");
            }
            else
            {
                return NotFound($"Movie with ID {movieId} not found.");
            }
        }

        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<Movie>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchMovies(string title)
        {
            var result = await _movieRepository.GetEntitiesBy<Movie>(m => m.Title.Contains(title));
            return result != null ? Ok(result) : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("latest")]
        [ProducesResponseType(typeof(IEnumerable<Movie>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetLatestMovies(int limit, string sortBy)
        {
            var result = await _movieRepository.GetEntitiesBy<Movie>(m => true);

            switch (sortBy.ToLower())
            {
                case "releasedate":
                    result = result.OrderByDescending(m => m.ReleaseDate);
                    break;
                case "newest":
                default:
                    result = result.OrderByDescending(m => m.CreatedAt);
                    break;
            }

            var latestMovies = result.Take(limit);
            return latestMovies != null ? Ok(latestMovies) : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<Movie>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMoviesByUser(int userId)
        {
            var result = await _movieRepository.GetEntitiesBy<Movie>(m => m.UserId == userId);
            return result != null ? Ok(result) : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedList<MovieForListDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMovies([FromQuery] ProductionParameters parameters)
        {
            var movies = await _specificMovieRepository.GetMovies(parameters);

            if (movies == null || !movies.Any())
                return BadRequest("Could not find any movies");

            Response.AddPaginationHeader(movies.CurrentPage, movies.PageSize, movies.TotalCount, movies.TotalPages);

            return Ok(movies);
        }

        [AllowAnonymous]
        [HttpGet("{movieId}")]
        public async Task<IActionResult> GetMovieById(int movieId)
        {
            var movie = await _movieRepository.GetById(movieId);
            return Ok(movie);
        }
    }
}