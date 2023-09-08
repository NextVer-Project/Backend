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
    public class UserCollectionController : ControllerBase
    {
        private readonly IBaseRepository<UserCollection> _userCollectionRepository;
        private readonly IMapper _mapper;

        public UserCollectionController(IBaseRepository<UserCollection> userCollectionRepository, IMapper mapper)
        {
            _userCollectionRepository = userCollectionRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(UserCollectionForAddDto userCollectionForAdd)
        {
            if (!AuthorizationHelper.IsAuthorizedForAdding(User, out var userId))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            if (userCollectionForAdd == null)
                return BadRequest("User Collection data is missing.");

            var userCollection = _mapper.Map<UserCollection>(userCollectionForAdd);

            userCollection.UserId = userId;
            userCollection.CreatedAt = DateTime.UtcNow;
            userCollection.UpdatedAt = DateTime.UtcNow;

            var result = await _userCollectionRepository.Add(userCollection);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            if (userCollectionForAdd.ProductionVersionIds != null && userCollectionForAdd.ProductionVersionIds.Any())
            {
                foreach (var productionVersionId in userCollectionForAdd.ProductionVersionIds)
                {
                    var userCollectionProductionVersionToAdd = new UserCollectionProduction
                    {
                        UserCollectionId = userCollection.Id,
                        ProductionVersionId = productionVersionId
                    };
                    _userCollectionRepository.AddLinkEntity(userCollectionProductionVersionToAdd);
                }

                var productionsAdded = await _userCollectionRepository.SaveChangesAsync();
                if (!productionsAdded)
                    return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(UserCollectionForEditDto userCollectionForEdit)
        {
            if (!AuthorizationHelper.IsAuthorizedForEditing(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var productionProductions = _userCollectionRepository.GetLinkEntitiesFor<UserCollection, UserCollectionProduction>(
                linkEntity => linkEntity.UserCollectionId == userCollectionForEdit.Id
            ).ToList();

            var existingProductionIds = productionProductions.Select(linkEntity => linkEntity.ProductionVersionId).ToList();

            var updatedProductionIds = userCollectionForEdit.ProductionVersionIds.ToList();

            var newProductionIds = updatedProductionIds.Except(existingProductionIds).ToList();

            var productionsToRemove = productionProductions.Where(linkEntity => !updatedProductionIds.Contains(linkEntity.ProductionVersionId)).ToList();

            foreach (var productionToRemove in productionsToRemove)
            {
                _userCollectionRepository.RemoveLinkEntity(productionToRemove);
            }

            foreach (var newProductionId in newProductionIds)
            {
                var userCollectionProductionToAdd = new UserCollectionProduction
                {
                    UserCollectionId = userCollectionForEdit.Id,
                    ProductionVersionId = newProductionId
                };
                _userCollectionRepository.AddLinkEntity(userCollectionProductionToAdd);
            }

            var result = await _userCollectionRepository.Edit(userCollectionForEdit.Id, userCollectionForEdit);

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

            var result = await _userCollectionRepository.Delete(id);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
