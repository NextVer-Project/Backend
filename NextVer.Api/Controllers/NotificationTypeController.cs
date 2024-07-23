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
    public class NotificationTypeController : ControllerBase
    {
        private readonly IBaseRepository<NotificationType> _notificationTypeRepository;
        private readonly IMapper _mapper;

        public NotificationTypeController(IBaseRepository<NotificationType> notificationTypeRepository, IMapper mapper)
        {
            _notificationTypeRepository = notificationTypeRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(NotificationTypeForAddDto notificationTypeForAdd)
        {
            if (!AuthorizationHelper.IsAuthorizedForAdding(User, out var userId))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            if (notificationTypeForAdd == null)
                return BadRequest("Notification Type data is missing.");

            var notificationType = _mapper.Map<NotificationType>(notificationTypeForAdd);

            var result = await _notificationTypeRepository.Add(notificationType);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(NotificationTypeForEditDto notificationTypeForEdit)
        {
            if (!AuthorizationHelper.IsAuthorizedForEditing(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var result = await _notificationTypeRepository.Edit(notificationTypeForEdit.Id, notificationTypeForEdit);

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

            var result = await _notificationTypeRepository.Delete(id);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("{notificationTypeId}/details")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetNotificationTypeById(int notificationTypeId)
        {
            var notificationType = await _notificationTypeRepository.GetById(notificationTypeId);
            return Ok(notificationType);
        }

        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<NotificationType>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchNotificationTypes(string name)
        {
            var result = await _notificationTypeRepository.GetEntitiesBy<NotificationType>(m => m.Name.Contains(name));
            return result != null ? Ok(result) : StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
