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
    public class NotificationController : ControllerBase
    {
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationController(IBaseRepository<Notification> notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Add(NotificationForAddDto notificationForAdd)
        {
            if (!AuthorizationHelper.IsAuthorizedForAdding(User, out var userId))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            if (notificationForAdd == null)
                return BadRequest("Notification data is missing.");

            var notification = _mapper.Map<Notification>(notificationForAdd);

            notification.UserId = userId;
            notification.CreatedAt = DateTime.UtcNow;

            var result = await _notificationRepository.Add(notification);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(NotificationForEditDto notificationForEdit)
        {
            if (!AuthorizationHelper.IsAuthorizedForEditing(User))
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var result = await _notificationRepository.Edit(notificationForEdit.Id, notificationForEdit);

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

            var result = await _notificationRepository.Delete(id);

            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
