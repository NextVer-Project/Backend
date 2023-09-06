using System.Security.Claims;

namespace NextVerBackend.Helpers
{
    public static class AuthorizationHelper
    {
        private static readonly List<int> _userTypesAllowedToAdd = new List<int> { 1, 2, 3, 4 };
        private static readonly List<int> _userTypesAllowedToEdit = new List<int> { 1, 2, 3, 4 };
        private static readonly List<int> _userTypesAllowedToDelete = new List<int> { 1, 2, 3, 4 };

        public static bool IsAuthorizedForAdding(ClaimsPrincipal user, out int userId)
        {
            if (!int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty, out userId))
                return false;

            var userTypeIdClaim = user.FindFirstValue(ClaimTypes.Role);

            if (userTypeIdClaim == null || !int.TryParse(userTypeIdClaim, out var userTypeId))
                return false;

            return _userTypesAllowedToAdd.Contains(userTypeId);
        }

        public static bool IsAuthorizedForEditing(ClaimsPrincipal user)
        {
            if (!int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty, out var userId))
                return false;

            var userTypeIdClaim = user.FindFirstValue(ClaimTypes.Role);

            if (userTypeIdClaim == null || !int.TryParse(userTypeIdClaim, out var userTypeId))
                return false;

            return _userTypesAllowedToEdit.Contains(userTypeId);
        }
        public static bool IsAuthorizedForDeletion(ClaimsPrincipal user)
        {
            if (!int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty, out var userId))
                return false;

            var userTypeIdClaim = user.FindFirstValue(ClaimTypes.Role);

            if (userTypeIdClaim == null || !int.TryParse(userTypeIdClaim, out var userTypeId))
                return false;

            return _userTypesAllowedToDelete.Contains(userTypeId);
        }
    }
}