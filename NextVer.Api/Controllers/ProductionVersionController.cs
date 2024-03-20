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
    public class ProductionVersionController : ControllerBase
    {
        private readonly IBaseRepository<ProductionVersion> _productionVersionRepository;
        private readonly IMapper _mapper;

        public ProductionVersionController(IBaseRepository<ProductionVersion> productionVersionRepository, IMapper mapper)
        {
            _productionVersionRepository = productionVersionRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(ProductionVersionForAddDto productionVersionForAdd)
        {
            if (!AuthorizationHelper.IsAuthorizedForAdding(User, out var userId))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            if (productionVersionForAdd == null)
                return BadRequest("Production Version data is missing.");

            var productionVersion = _mapper.Map<ProductionVersion>(productionVersionForAdd);

            productionVersion.UserId = userId;
            productionVersion.CreatedAt = DateTime.UtcNow;
            productionVersion.UpdatedAt = DateTime.UtcNow;

            var result = await _productionVersionRepository.Add(productionVersion);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            if (productionVersionForAdd.TechnologyIds != null && productionVersionForAdd.TechnologyIds.Any())
            {
                foreach (var technologyId in productionVersionForAdd.TechnologyIds)
                {
                    var productionVersionTechnologyToAdd = new ProductionTechnology
                    {
                        ProductionVersionId = productionVersion.Id,
                        TechnologyId = technologyId
                    };
                    _productionVersionRepository.AddLinkEntity(productionVersionTechnologyToAdd);
                }

                var technologiesAdded = await _productionVersionRepository.SaveChangesAsync();
                if (!technologiesAdded)
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(ProductionVersionForEditDto productionVersionForEdit)
        {
            if (!AuthorizationHelper.IsAuthorizedForEditing(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var productionTechnologies = _productionVersionRepository.GetLinkEntitiesFor<ProductionVersion, ProductionTechnology>(
                linkEntity => linkEntity.ProductionVersionId == productionVersionForEdit.Id
            ).ToList();

            var existingTechnologyIds = productionTechnologies.Select(linkEntity => linkEntity.TechnologyId).ToList();

            var updatedTechnologyIds = productionVersionForEdit.TechnologyIds.ToList();

            var newTechnologyIds = updatedTechnologyIds.Except(existingTechnologyIds).ToList();

            var technologysToRemove = productionTechnologies.Where(linkEntity => !updatedTechnologyIds.Contains(linkEntity.TechnologyId)).ToList();

            foreach (var technologyToRemove in technologysToRemove)
            {
                _productionVersionRepository.RemoveLinkEntity(technologyToRemove);
            }

            foreach (var newTechnologyId in newTechnologyIds)
            {
                var productionVersionTechnologyToAdd = new ProductionTechnology
                {
                    ProductionVersionId = productionVersionForEdit.Id,
                    TechnologyId = newTechnologyId
                };
                _productionVersionRepository.AddLinkEntity(productionVersionTechnologyToAdd);
            }

            var result = await _productionVersionRepository.Edit(productionVersionForEdit.Id, productionVersionForEdit);

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

            var result = await _productionVersionRepository.Delete(id);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [AllowAnonymous]
        [HttpGet("{productionVersionId}/details")]
        public async Task<IActionResult> GetTechnologiesByMovieVersion(int productionVersionId)
        {
            var technologies = await _productionVersionRepository.GetEntitiesBy<ProductionVersion>(p => p.ProductionTechnologies.Any(pv => pv.ProductionVersionId == productionVersionId));
            return Ok(technologies);
        }
    }
}
